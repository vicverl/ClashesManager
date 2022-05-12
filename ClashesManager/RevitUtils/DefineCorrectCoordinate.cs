using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClashesManager.RevitUtils
{
    internal static class DefineCorrectCoordinate
    {
        public static XYZ CorrectCoordinatePointToEngineer(XYZ pointLocation, Element linkElement, Document doc)
        {
            XYZ location = pointLocation;
            XYZ orohonaInternal = checkInternalOrigin(doc);
            double internalZeroValueX = Math.Abs(orohonaInternal.X);
            double internalZeroValueY = Math.Abs(orohonaInternal.Y);
            double internalZeroValueZ = Math.Abs(orohonaInternal.Z);

            Document linkDoc = linkElement.Document;
            XYZ orohonaInternalLink = checkInternalOrigin(linkDoc);
            double internalZeroLinkValueX = Math.Abs(orohonaInternalLink.X);
            double internalZeroLinkValueY = Math.Abs(orohonaInternalLink.Y);
            double internalZeroLinkValueZ = Math.Abs(orohonaInternalLink.Z);

            if (Math.Round(internalZeroLinkValueX, 4) != Math.Round(internalZeroValueX, 4))
            {
                internalZeroLinkValueX = (internalZeroValueX - internalZeroLinkValueX) < 0 ?
                            location.X + (internalZeroValueX - internalZeroLinkValueX) :
                            location.X + (-1) * (internalZeroValueX - internalZeroLinkValueX);
                //locationCorrct = new XYZ(internalZeroValueX, location.Y, location.Z);
            }
            else internalZeroLinkValueX = location.X;

            if (Math.Round(internalZeroLinkValueY, 4) != Math.Round(internalZeroValueY, 4))
            {
                internalZeroLinkValueY = (internalZeroValueY - internalZeroLinkValueY) < 0 ?
                            location.Y + (internalZeroValueY - internalZeroLinkValueY) :
                            location.Y + (-1) * (internalZeroValueY - internalZeroLinkValueY);
                //locationCorrct = new XYZ(location.X, internalZeroLinkValueY, location.Z);
            }
            else internalZeroLinkValueY = location.Y;

            if (Math.Round(internalZeroLinkValueZ, 4) != Math.Round(internalZeroValueZ, 4))
            {
                internalZeroLinkValueZ = (internalZeroLinkValueZ - internalZeroValueZ) < 0 ?
                            location.Z + (-1) * (internalZeroLinkValueZ - internalZeroValueZ) :
                            location.Z + (internalZeroLinkValueZ - internalZeroValueZ);
                //locationCorrct = new XYZ(location.X, location.Y, internalZeroLinkValueZ);
            }
            else internalZeroLinkValueZ = location.Z;

            XYZ newCorerectPoint = new XYZ(internalZeroLinkValueX, internalZeroLinkValueY, internalZeroLinkValueZ);
            return newCorerectPoint;
        }

        private static XYZ checkInternalOrigin(Document doc)//method to predict division between internal base point and base point in project
        {
            XYZ newInternal = null;
            ProjectLocation projectLocation = doc.ActiveProjectLocation;
            var points = (new FilteredElementCollector(doc)).OfClass(typeof(BasePoint)).ToElements();
            foreach (Element bp in points)
            {
                BasePoint bpp = bp as BasePoint;
                if (bpp.IsShared == true)
                {
                    double elevation = bp.get_Parameter(BuiltInParameter.BASEPOINT_ELEVATION_PARAM).AsDouble();
                    BoundingBoxXYZ bb = bp.get_BoundingBox(null);
                    newInternal = bb.Min;//find base points29
                }
            }
            return newInternal;
        }

    }
}
