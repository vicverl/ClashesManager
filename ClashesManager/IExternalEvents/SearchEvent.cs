using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ClashesManager.Utils;

namespace ClashesManager.IExternalEvent
{
    internal class SearchEvent : IExternalEventHandler
    {
        internal int FirstId { get; set; }
        internal int SecondId { get; set; }
        internal string ElelmDocNameFisrt { get; set; }
        internal string ElelmDocNameSecond { get; set; }

        public static string _view3DName = "Clash Detector 3DView";

        private Window mainWindow;

        #region Constructor
        internal SearchEvent(Window windowOwner)
        {
            this.mainWindow = windowOwner;
        }
        #endregion

        public void Execute(UIApplication app)
        {

            var uiDoc = app.ActiveUIDocument;
            var doc = uiDoc.Document;

            bool topmost = mainWindow.Topmost;
            mainWindow.Topmost = false;

            RevitLinkInstance linkIstanseFisrt = null;
            RevitLinkInstance linkIstanseSecond = null;
            Document firstDoc = GetDocumentFromNFCName(doc, ElelmDocNameFisrt, ref linkIstanseFisrt);
            Document secondDoc = GetDocumentFromNFCName(doc, ElelmDocNameSecond, ref linkIstanseSecond);
            if (firstDoc == null && secondDoc == null) return;
            Element firstElem = firstDoc.GetElement(new ElementId(FirstId));
            Element secondElem = secondDoc.GetElement(new ElementId(SecondId));

            try
            {
                View3D view = get3DViewByName(_view3DName, doc);
                if (view == null)
                {
                    view = createView3D(_view3DName, doc);
                }
                if (view != null)
                {
                    using (Transaction transaction = new Transaction(doc, "Isolate Clash on 3D View"))
                    {
                        transaction.Start();

                        ICollection<ElementId> OpenDocElemeId = new List<ElementId>();

                        if (doc.Title == firstElem.Document.Title) OpenDocElemeId.Add(firstElem.Id);
                        else if (doc.Title == secondElem.Document.Title) OpenDocElemeId.Add(secondElem.Id);
                        if (OpenDocElemeId.Count == 0)
                        {
                            MessageBox.Show($"Не удалось найти элементы в открытой модели \n Убедитесь что работаете в одной из моделях \n {firstDoc.Title} \n {secondDoc.Title}", "Ошибка");
                            transaction.RollBack();
                            return;
                        }

                        view.IsSectionBoxActive = true;
                        Element mainOpenDocElement = doc.GetElement(OpenDocElemeId.Where(x => x.IntegerValue == firstElem.Id.IntegerValue 
                        || x.IntegerValue == secondElem.Id.IntegerValue).ToList().First());
                        Solid solid1 = getSolidFromElement(firstElem);
                        Solid solid2 = getSolidFromElement(secondElem);
                        Solid intersectSolid = BooleanOperationsUtils.ExecuteBooleanOperation(
                            solid1,
                            solid2,
                            BooleanOperationsType.Intersect
                            );
                        BoundingBoxXYZ ElementBBox = mainOpenDocElement.get_BoundingBox(view);
                        BoundingBoxXYZ bbox = intersectSolid.GetBoundingBox();
                        if ((bbox.Max - bbox.Min).Z > 1000000) bbox = ElementBBox;
                        bbox.Max = new XYZ(bbox.Max.X + 3, bbox.Max.Y + 3, bbox.Max.Z + 3);
                        bbox.Min = new XYZ(bbox.Min.X - 3, bbox.Min.Y - 3, bbox.Min.Z - 3);

                        XYZ oldm = bbox.Max;
                        XYZ oldM = bbox.Min;
                        
                        XYZ currentBBoxMax = bbox.Max;//DefineCorrectCoordinate.CorrectCoordinatePointToEngineer(bbox.Max, SecondElem, doc);
                        XYZ currentBBoxMin = bbox.Min;//DefineCorrectCoordinate.CorrectCoordinatePointToEngineer(bbox.Min, SecondElem, doc);

                        bbox.Max = currentBBoxMax;
                        bbox.Min = currentBBoxMin;

                        ElementId docToIsolate = null;
                        var isolatedEleme = new List<ElementId>() {
                            mainOpenDocElement.Id };
                            
                        if (linkIstanseFisrt != null) docToIsolate = (linkIstanseFisrt.Id);
                        else if (linkIstanseSecond != null) docToIsolate = (linkIstanseSecond.Id);

                        if (docToIsolate != null) isolatedEleme.Add(docToIsolate);
                      
                        view.IsolateElementsTemporary(isolatedEleme);

                        view.SetSectionBox(bbox);
                        transaction.Commit();
                        uiDoc.ActiveView = view;
                        UIView uiview = uiDoc.GetOpenUIViews().Where(x => x.ViewId == view.Id).FirstOrDefault();

                        uiview.ZoomAndCenterRectangle(
                            new XYZ(ElementBBox.Max.X + 5, ElementBBox.Max.Y + 5, ElementBBox.Max.Z + 5),
                            new XYZ(ElementBBox.Min.X - 5, ElementBBox.Min.Y - 5, ElementBBox.Min.Z - 5)
                            );
                        uiDoc.Selection.SetElementIds(OpenDocElemeId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(mainWindow, "Не удалось отобразить элемент на отдельном 3D виде!" + "\n" + ex.Message, "Ошибка");
                Analytics.SaveExceptionReport(ex, "Не удалось отобразить элемент на отдельном 3D виде!");
            }

            mainWindow.Topmost = topmost;

        }

        private Solid getSolidFromElement(Element intersectDocumentElement)
        {
            Solid solidMainIntersect = null;
            Options geomOptions = new Options();
            geomOptions.ComputeReferences = true;
            geomOptions.IncludeNonVisibleObjects = false;
            GeometryElement elemGeometry = intersectDocumentElement.get_Geometry(geomOptions);

            foreach (GeometryObject elemPrimitive in elemGeometry)
            {
                var solid = elemPrimitive;

                if (solid is Solid)
                {
                    try
                    {//sometimes object may dont have a volume paraemeter, even it had a solid geometry and we just try it
                        Solid solidMain = (Solid)solid;
                        if (solidMain.Volume != 0)
                        {
                            solidMainIntersect = solidMain;
                            return solidMainIntersect;
                        }
                        else
                            continue;//continue if volume equal zero
                    }
                    catch
                    {
                        solidMainIntersect = (Solid)solid;
                        continue;
                    }
                }
                else if (elemPrimitive is GeometryInstance)
                {
                    GeometryInstance geometryIntsnceVar = (GeometryInstance)solid;
                    GeometryElement geomElementInst = geometryIntsnceVar.GetInstanceGeometry();
                    foreach (GeometryObject geomElementSolid in geomElementInst)
                    {
                        if (geomElementSolid is Solid)
                        {
                            try
                            {//sometimes object may dont have a volume paraemeter, even it had a solid geometry and we just try it
                                Solid solidMain = (Solid)geomElementSolid;
                                if (solidMain.Volume != 0)
                                {
                                    solidMainIntersect = solidMain;
                                    return solidMainIntersect;
                                }
                                else
                                    continue;//continue if volume equal zero
                            }
                            catch
                            {
                                solidMainIntersect = (Solid)solid;
                                continue;
                            }
                        }
                    }
                }
            }
            return solidMainIntersect;
        }
        public string GetName()
        {
            return "Show in View 3D";
        }

        private static View3D get3DViewByName(string Name, Document doc)
        {
            View3D view3d;

            using (var collector = new FilteredElementCollector(doc))
            {
                view3d = collector
                                .OfClass(typeof(View3D))
                                .Cast<View3D>()
                                .Where(x => x.Name == Name)
                                .FirstOrDefault();
            }
            return view3d;
        }

        public static View3D createView3D(string Name, Document doc)
        {
            View3D newView3d = null;

            ViewFamilyType viewFamilyType3D = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .Cast<ViewFamilyType>()
                .FirstOrDefault<ViewFamilyType>(x => ViewFamily.ThreeDimensional == x.ViewFamily);

            using (Transaction transaction = new Transaction(doc, "Crate3DView"))
            {
                transaction.Start();
                newView3d = View3D.CreateIsometric(doc, viewFamilyType3D.Id);
                if (newView3d != null)
                {
                    newView3d.Name = Name;
                    newView3d.DetailLevel = ViewDetailLevel.Fine;
                    newView3d.DisplayStyle = DisplayStyle.ShadingWithEdges;
                    newView3d.Discipline = ViewDiscipline.Coordination;
                }
                transaction.Commit();
            }

            return newView3d;
        }


        private static string getClearDocName(string docName)
        {
            if (docName.Contains(".rvt"))
                return docName.Replace("", ".rvt").Replace("SHAR", "WORK");
            else
                return docName;
        }

        public static Document GetDocumentFromNFCName(Document activeDoc, string docNFCName, ref RevitLinkInstance linkIstanse)
        {
            Document document = null;
            var docName = activeDoc.Title;
            string docNameRVT = String.Empty;
            docNameRVT = getClearDocName(docName);
            
            if (docNameRVT.Contains(docNFCName )) return activeDoc;//Logic if doc to find is active document
            else
            {
                List<Element> revitLinkInstanceColletion = (new FilteredElementCollector(activeDoc)).OfClass(typeof(RevitLinkInstance)).ToList();
                foreach (RevitLinkInstance revitLinkInstances in revitLinkInstanceColletion)
                {
                    if (!(revitLinkInstances.IsValidObject)) continue;
                    
                    Document linkDoc = revitLinkInstances.GetLinkDocument();
                    if (linkDoc == null) continue;
                    string linkDocName = linkDoc.Title.Replace("SHAR", "WORK");
                    string linkDocNameRVT = getClearDocName(linkDocName);
                    if (linkDocNameRVT.Contains(docNFCName.Replace("SHAR", "WORK")))
                    {
                        linkIstanse = revitLinkInstances;
                        return linkDoc;
                    }
                }
            }
            MessageBox.Show($"Не удалось найти документ \n Проверьте подключение связанного файла \n {docNFCName} ", "Ошибка");
            return document;
        }

    }
}
