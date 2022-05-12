using System.Windows;
using System.Xml;
using ClashesManager.Models;

namespace ClashesManager.Core
{
    internal static class XmlUtils
    {
        private static List<ClashTestModel> _clashTestModelList = new();
        private static XmlElement xmlDocument { get; set; }
        private static string newFilePath { get; set; }

        /// <summary>
        /// Creates a list of clash tests and return it
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static List<ClashTestModel> CreateClashTestsList(string filePath)
        {
            _clashTestModelList = new(); // Setting new collection before parsing
            GetXmlElement(filePath);
            if (xmlDocument is null) return null;

            newFilePath = CreateNewPath(filePath);
            ParseXml(xmlDocument);

            return _clashTestModelList;
        }

        private static void GetXmlElement(string filePath)
        {
            var xDoc = new XmlDocument();

            xDoc.Load(filePath);
            xmlDocument = xDoc.DocumentElement;
        }

        /// <summary>
        /// Parse xml and fill clash tests list
        /// </summary>
        /// <param name="element"></param>
        private static void ParseXml(XmlElement element)
        {
            var clashTests = element.GetElementsByTagName("clashtest");
            if (clashTests.Count > 0)
            {
                foreach (XmlElement clashTest in clashTests)
                {
                    var clashTestModel = new ClashTestModel();
                    clashTestModel.XmlFilePath = newFilePath;
                    clashTestModel.TestName = clashTest.GetAttribute("name");
                    clashTestModel.TestType = clashTest.GetAttribute("test_type");
                    clashTestModel.Tolerance = clashTest.GetAttribute("tolerance");

                    var counter = 0;

                    var clashResults = clashTest.GetElementsByTagName("clashresult");
                    if (clashResults.Count > 0)
                        foreach (XmlElement clashResult in clashResults)
                        {
                            var clashModel = new ClashModel();

                            counter++;
                            clashModel.Number = counter.ToString();

                            clashModel.ClashName = clashResult.GetAttribute("name");

                            clashModel.ImgPath = CreateImgPath(newFilePath, clashResult.GetAttribute("href"));

                            var gridLocation = clashResult.GetElementsByTagName("gridlocation").OfType<XmlElement>()
                                .FirstOrDefault();
                            clashModel.ClashGridLocation = gridLocation?.InnerText;

                            var firstElementId = clashResult.GetElementsByTagName("objectattribute")
                                .OfType<XmlElement>()
                                .First()
                                .GetElementsByTagName("value")[0]
                                .InnerText;
                            clashModel.FirstElementId = firstElementId;

                            var secondElementId = clashResult.GetElementsByTagName("objectattribute")
                                .OfType<XmlElement>()
                                .Last()
                                .GetElementsByTagName("value")[0]
                                .InnerText;
                            clashModel.SecondElementId = secondElementId;

                            var firstElementDoc = clashResult.GetElementsByTagName("node")
                                .OfType<XmlElement>()
                                .FirstOrDefault(i => char.IsDigit(i.InnerText[0]))?
                                .InnerText;
                            clashModel.FirstElementDoc = firstElementDoc;

                            var secondElementDoc = clashResult.GetElementsByTagName("pathlink")
                                .OfType<XmlElement>()
                                .LastOrDefault()?
                                .GetElementsByTagName("node")
                                .OfType<XmlElement>()
                                .Where(i => char.IsDigit(i.InnerText[0]))
                                .OrderByDescending(i=>i.InnerText.Length)
                                .FirstOrDefault()?
                                .InnerText;
                            clashModel.SecondElementDoc = secondElementDoc;

                            var firstElementName = clashResult.GetElementsByTagName("smarttags")
                                .OfType<XmlElement>()
                                .First() // element name
                                .GetElementsByTagName("value")[0]
                                .InnerText;
                            clashModel.FirstElementName = firstElementName;

                            var secondElementName = clashResult.GetElementsByTagName("smarttags")
                                .OfType<XmlElement>()
                                .Last() // second element
                                .GetElementsByTagName("value")[0] // element name
                                .InnerText;
                            clashModel.SecondElementName = secondElementName;

                            var firstElementType = clashResult.GetElementsByTagName("smarttags")
                                .OfType<XmlElement>()
                                .First() // first element
                                .GetElementsByTagName("value")[1] // element type
                                .InnerText;
                            clashModel.FirstElementType = firstElementType;

                            var secondElementType = clashResult.GetElementsByTagName("smarttags")
                                .OfType<XmlElement>()
                                .Last() // first element
                                .GetElementsByTagName("value")[1] // element type
                                .InnerText;
                            clashModel.SecondElementType = secondElementType;

                            clashTestModel.ClashModels.Add(clashModel);
                        }
                    _clashTestModelList.Add(clashTestModel);
                }
            }
        }

        [CanBeNull]
        public static string CreateNewPath(string filePath)
        {
            try
            {
                var newFilePath = "";

                char splittedChar = '\\';

                var fileName = filePath.Split(splittedChar).Last().Replace(".xml", "");
                var newRelativePath = fileName + "_files" + "\\REPORT-" + fileName;

                newFilePath = filePath.Replace(fileName, newRelativePath);

                return newFilePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while creating a new sml document path\n{ex.Message}", "Error");
                return null;
            }
        }

        [CanBeNull]
        public static string CreateImgPath(string filePath, string relativeImgPath)
        {
            try
            {
                var newFilePath = "";
                char splittedChar = '\\';

                var croppedPath = filePath.Replace(filePath.Split(splittedChar).Last(),"");
                var imgName = relativeImgPath.Split(splittedChar).Last();
                newFilePath = croppedPath + "\\" + imgName;

                return newFilePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while creating a new sml document path\n{ex.Message}", "Error");
                return null;
            }
        }
    }
}
