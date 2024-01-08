using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace LaptopRevitCommands.Commands.FRP
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class AvoidObstacles : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Foundamental Parameters
            double latSafeDis = 2500;
            double verSafeDis = 1500;
            double ratio = 0.25;

            try
            {
                // Check if there is flight route
                string famName_FlightRoute = "AdaptiveRedArrow";
                IList<Element> flightPathList = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).Where(q => q.Name == famName_FlightRoute).ToList();
                if (flightPathList.Count() == 0)
                {
                    TaskDialog.Show("Revit", "Please generate the flight route firstly.");
                    return Result.Succeeded;
                }

                // Obtain main structure
                TaskDialogResult dialogResult = TaskDialog.Show("Revit", "Please select the main structure for interference check.", TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok);
                if (dialogResult == TaskDialogResult.Cancel)
                {
                    return Result.Succeeded;
                }
                Reference pickedMainStructure = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                // Check if there is clash of flight route with the main structure
                IList<ElementId> intersectedFlightPathIds = new List<ElementId>();
                foreach(Element ele in flightPathList)
                {
                    FamilyInstance flightPathInstance = ele as FamilyInstance;
                    if (CheckInterferenceForFlightRoute(doc, pickedMainStructure, flightPathInstance, latSafeDis, verSafeDis, ratio))
                    {
                        intersectedFlightPathIds.Add(flightPathInstance.Id);
                    }
                }

                // Present
                uidoc.Selection.SetElementIds(intersectedFlightPathIds);
                uidoc.ShowElements(intersectedFlightPathIds);
                uidoc.RefreshActiveView();

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
        }

        // Method 1: Check if the selected flight path will crash with the main structure
        private Boolean CheckInterferenceForFlightRoute(Document doc, Reference reference, FamilyInstance mFlightPash, double latDis, double verDis, double ratio)
        {
            bool result = false;

            // Obtain the XYZ list to create dronesafetyzone instances
            IList<ElementId> adptPointIdList = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(mFlightPash);
            IList<XYZ> xyzList = new List<XYZ>();

            for (int i=0; i < (adptPointIdList.Count() - 1); i++)
            {
                ReferencePoint refStartPoint = doc.GetElement(adptPointIdList[i]) as ReferencePoint;
                XYZ startPoint = refStartPoint.Position;
                ReferencePoint refEndPoint = doc.GetElement(adptPointIdList[i+1]) as ReferencePoint;
                XYZ endPoint = refEndPoint.Position;
                
                IList<XYZ> xyzs = CalculateXYZList(startPoint, endPoint, latDis, verDis, ratio);
                foreach (XYZ xyz in xyzs)
                {
                    xyzList.Add(xyz);
                }
            }

            // Create temporary safetyzone family instances
            string familyName = "DroneSafeZone";
            GenericMethods.CheckAndLoadFamily(doc, familyName);
            
            using (Transaction trans = new Transaction(doc, "create temporary family instances"))
            {
                trans.Start();

                FamilySymbol famSym = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).Where(q => q.Name == familyName).First() as FamilySymbol;
                famSym.Activate();

                foreach (XYZ xyz in xyzList)
                {
                    FamilyInstance mFamIns = doc.FamilyCreate.NewFamilyInstance(xyz, famSym, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    mFamIns.LookupParameter("LatSafeDistance").Set(GenericMethods.MM2Ft(latDis));
                    mFamIns.LookupParameter("VerSafeDistance").Set(GenericMethods.MM2Ft(verDis));
                }

                trans.Commit();
            }

            // Check the clash
            List<Element> insectElements = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).WherePasses(new ElementIntersectsElementFilter(doc.GetElement(reference))).Where(q => q.Name==familyName).ToList();
            int a = insectElements.Count();
            if (insectElements.Count() != 0)
            {
                result = true;
            }

            // Delete the temporary element
            using (Transaction trans = new Transaction(doc, "create temporary family instances"))
            {
                trans.Start();

                List<Element> safetyZoneElements = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).Where(q => q.Name == familyName).ToList();

                IList<ElementId> safetyZoneElementIdList = new List<ElementId>();
                foreach (Element ele in safetyZoneElements)
                {
                    safetyZoneElementIdList.Add(ele.Id);
                }

                doc.Delete(safetyZoneElementIdList);

                trans.Commit();
            }

            return result;
        }

        private IList<XYZ> CalculateXYZList(XYZ startPoint, XYZ endPoint, double latDis, double verDis, double ratio)
        {
            latDis = GenericMethods.MM2Ft(latDis);
            verDis = GenericMethods.MM2Ft(verDis);
            // Calculate the interval distance
            double angleOfSafeZone = Math.Atan(verDis/latDis);
            double angleOfFlightPath = Math.Asin(Math.Abs((endPoint - startPoint).Z) /(endPoint - startPoint).GetLength());

            double intervalDis;
            if (angleOfSafeZone < angleOfFlightPath)
            {
                intervalDis = verDis / Math.Sin(angleOfFlightPath);
            }
            else
            {
                intervalDis = latDis / Math.Cos(angleOfFlightPath);
            }

            // Calculate the output Ilist<XYZ>
            intervalDis = intervalDis * 2 * ratio;
            IList<XYZ> outputXYZs = new List<XYZ>();
            XYZ dir = (endPoint - startPoint).Normalize();
            double length = (endPoint - startPoint).GetLength();
            int i = 0;
            while (i * intervalDis < length)
            {
                outputXYZs.Add(startPoint + i *intervalDis* dir);
                i++;
            }

            return outputXYZs;
        }

    }
}
