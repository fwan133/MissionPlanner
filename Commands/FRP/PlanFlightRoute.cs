using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using LaptopRevitCommands.Views;

namespace LaptopRevitCommands.Commands.FRP
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class PlanFlightRoute : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            XYZ landingPosition = new XYZ();
            IList<Element> mCamInstances;

            // Check LandingPoint 
            string famName_LandingPoint = "LandingSign";
            IList<Element> mLandPoints=new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).Where(q => q.Name == famName_LandingPoint).ToList();
            if (mLandPoints.Count() == 0)
            {
                TaskDialog.Show("Flight Route Planning", "Please specify the take-off/landing position for the drone.");
                return Result.Cancelled;
            }
            else if(mLandPoints.Count()>1)
            {
                TaskDialog.Show("Flight Route Planning", "Please ensure there is only one take-off/landing position for the flight path planning.");
                return Result.Cancelled;
            }
            else
            {
                LocationPoint locPoint=mLandPoints.First().Location as LocationPoint;
                landingPosition = locPoint.Point;
            }

            // Check Camera Viewpoints
            string famName_CameraViewPoint = "AdaptiveCameraModel";
            mCamInstances = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).Where(q => q.Name == famName_CameraViewPoint).ToList();
            if (mCamInstances.Count() == 0)
            {
                TaskDialog.Show("Flight Route Planning", "Please plan the camera viewpoints first.");
                return Result.Cancelled;
            }

            // Obtain BlockBoxes
            BlockBox initialBlockBox = new BlockBox(GenericMethods.ObtainCameraViewpointsFromDoc(doc));
            // Divide in vertical
            List<BlockBox> verticalBlockBoxes= initialBlockBox.DivideBlockInVertical(0.01);
            // Divide in lateral
            List<BlockBox> BlockBoxList = new List<BlockBox>();

            int a = verticalBlockBoxes.Sum(q => q.CameraViewpointList.Count());

            foreach (var verticalBlockBox in verticalBlockBoxes)
            {
                List<BlockBox> lateralBlockBoxList=verticalBlockBox.DivideBlockInLateral(0.01);
                foreach (var lateralBlockBox in lateralBlockBoxList)
                {
                    BlockBoxList.Add(lateralBlockBox);
                }
            }

            int b = BlockBoxList.Sum(q => q.CameraViewpointList.Count());
            // Order the BlockBoxList
            CameraViewpoint startCameraViewpoint = new CameraViewpoint(landingPosition, new Orientation(0, 0, 0));
            IList<BlockBox> finalBlockBoxList=GenericMethods.OrderBlockBoxList(startCameraViewpoint, BlockBoxList);

            int c = finalBlockBoxList.Sum(q => q.CameraViewpointList.Count());

            // Obtain the flight route
            List<CameraViewpoint> flightRoute = new List<CameraViewpoint>();
            flightRoute.Add(startCameraViewpoint);
            foreach(BlockBox blockBox in finalBlockBoxList)
            {
                IList<CameraViewpoint> localCameraViewpointList = flightRoute.Last().ObtainShortestViewpointList(blockBox.CameraViewpointList);
                foreach(var cameraViewpoint in localCameraViewpointList)
                {
                    flightRoute.Add(cameraViewpoint);
                }
            }

            // Generate the flight route
            string famName_FlightPath = "AdaptiveRedArrow";
            CreateFlightPath(doc, famName_FlightPath,flightRoute);

            return Result.Succeeded;
        }

        // Method 1: Obtain the positions of all camera viewpoints
        private IList<XYZ> ObtainPositionsFromElements(IList<Element> mCamInstances)
        {
            IList<XYZ> viewPointsList = new List<XYZ>();
            foreach(Element camIns in mCamInstances)
            {
                viewPointsList.Add(GenericMethods.ObtainPositionFromElement(camIns as FamilyInstance));
            }
            return viewPointsList;
        }

        // Method 2: Obtain the orientation of all camera viewpoints


        // Method 3: Create Flight Path between two points
        private void CreateFlightPath(Document doc, String famName, IList<CameraViewpoint> viewPointsList)
        {
            // Popup ProgressDialog
            ProgressDialog progressDialog = new ProgressDialog();
            progressDialog.Show();

            GenericMethods.CheckAndLoadFamily(doc, famName);
            FamilySymbol famSym_RedArrow = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).Where(q => q.Name == famName).First() as FamilySymbol;

            using (Transaction trans=new Transaction(doc,"Create flight Path between two points."))
            {
                trans.Start();

                for (int i = 0; i < (viewPointsList.Count() - 1); i++)
                {
                    XYZ startPoint = viewPointsList[i].position;
                    XYZ endPoint = viewPointsList[i + 1].position;
                    XYZ midPoint1 =startPoint + 0.33 * (endPoint - startPoint);
                    XYZ midPoint2 =startPoint + 0.66 * (endPoint - startPoint);
                    FamilyInstance AdaptiveFlightPath = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, famSym_RedArrow);

                    IList<ElementId> adptPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(AdaptiveFlightPath);
                    ReferencePoint Start = (ReferencePoint)doc.GetElement(adptPoints[0]);
                    XYZ transform1 = startPoint.Subtract(Start.Position);
                    ElementTransformUtils.MoveElement(doc, adptPoints[0], transform1);

                    ReferencePoint MidPoint1 = (ReferencePoint)doc.GetElement(adptPoints[1]);
                    XYZ transform2 = midPoint1.Subtract(MidPoint1.Position);
                    ElementTransformUtils.MoveElement(doc, adptPoints[1], transform2);

                    ReferencePoint MidPoint2 = (ReferencePoint)doc.GetElement(adptPoints[2]);
                    XYZ transform3 = midPoint2.Subtract(MidPoint2.Position);
                    ElementTransformUtils.MoveElement(doc, adptPoints[2], transform3);

                    ReferencePoint End = (ReferencePoint)doc.GetElement(adptPoints[3]);
                    XYZ transform4 = endPoint.Subtract(End.Position);
                    ElementTransformUtils.MoveElement(doc, adptPoints[3], transform4);

                    AdaptiveFlightPath.LookupParameter("Number").Set(i.ToString());

                    // Update the progress
                    double value = (double) i / ((viewPointsList.Count()-2))*100;
                    int progressValue = (int)value;
                    progressDialog.UpdateProgress(progressValue);
                }

                trans.Commit();
            }
                
        }
    }
}
