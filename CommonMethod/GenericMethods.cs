using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using LaptopRevitCommands.Views;

namespace LaptopRevitCommands
{
    public static class GenericMethods
    {
        // Method 1: Calculate Camera Position from reference point and direction and distance
        public static XYZ CameraPosition(XYZ RefPos, XYZ RefOri, double Distance)
        {
            double X = RefPos.X + RefOri.X * MM2Ft(Distance);
            double Y = RefPos.Y + RefOri.Y * MM2Ft(Distance);
            double Z = RefPos.Z + RefOri.Z * MM2Ft(Distance);
            XYZ mCamPos = new XYZ(X,Y,Z);
            return mCamPos;
        }

        // Method 2: Calculate Camera Rotation from CenterDirection and U Direction
        public static Orientation CameraRotation(XYZ CamDirection, XYZ Udirection)
        {
            XYZ IniDir = new XYZ(1,0,0);

            // Yaw
            XYZ YawAxis = new XYZ(0, 0, 1);
            XYZ PostYawDir;
            XYZ PitchAxis;
            XYZ CamLenDir;
            double yaw;
            if (CamDirection.X< 1/Math.Pow(10,10) & CamDirection.Y < 1 / Math.Pow(10, 10))
            {
                yaw = new XYZ(0,-1,0).AngleOnPlaneTo(Udirection, YawAxis.Normalize());
                PostYawDir = new XYZ(-Udirection.Y, Udirection.X, 0);
                PitchAxis = new XYZ(Udirection.X, Udirection.Y, 0);
                CamLenDir = PitchAxis;
            }
            else
            {
                yaw = IniDir.AngleOnPlaneTo(CamDirection, YawAxis.Normalize());
                PostYawDir = new XYZ(CamDirection.X, CamDirection.Y, 0);
                PitchAxis = new XYZ(CamDirection.Y, -CamDirection.X, 0);
                CamLenDir = PitchAxis;
            }

            // Pitch
            double pitch = PostYawDir.AngleOnPlaneTo(CamDirection, PitchAxis.Normalize());

            XYZ RollAxis = CamDirection;       
            double roll = CamLenDir.AngleOnPlaneTo(Udirection, RollAxis.Normalize());

            Orientation mCamRot = new Orientation(Math.Round(yaw,3), Math.Round(pitch,3), Math.Round(roll,3));
            return mCamRot;
        }

        // Method 3: Insert Adaptive Camera Model Instance at desired postion
        public static void InsertCameraModel(Document doc, XYZ CamPos, Orientation CamRot, double workingDistance)
        {
            // Check and load the adaptive family (AdaptiveCameraModel)
            String targetName = "AdaptiveCameraModel";
            CheckAndLoadFamily(doc, targetName);

            // Create the camera instance
            FamilySymbol famSym = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).Where(q => q.Name == targetName).First() as FamilySymbol;
            FamilyInstance AdaptiveCameraInstance;

            using (Transaction trans = new Transaction(doc, "Create a camera instance"))
            {
                trans.Start();

                if (!famSym.IsActive)
                {
                    famSym.Activate();
                }

                AdaptiveCameraInstance = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, famSym);
                workingDistance = GenericMethods.MM2Ft(workingDistance);
                AdaptiveCameraInstance.LookupParameter("WorkingDistance").Set(workingDistance);

                IList<ElementId> adptPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(AdaptiveCameraInstance);
                ReferencePoint refPoint = (ReferencePoint)doc.GetElement(adptPoints[0]);

                XYZ transform = CamPos.Subtract(refPoint.Position);
                ElementTransformUtils.MoveElement(doc, adptPoints[0], transform);

                refPoint = (ReferencePoint)doc.GetElement(adptPoints[0]);
                XYZ adaptPointPosition = refPoint.Position;
                XYZ YawAxis = AdaptiveCameraInstance.GetTransform().BasisZ; ;
                ElementTransformUtils.RotateElement(doc, adptPoints[0], Line.CreateUnbound(adaptPointPosition, YawAxis), CamRot.Yaw);

                XYZ PitchAxis = -AdaptiveCameraInstance.GetTransform().BasisY;
                ElementTransformUtils.RotateElement(doc, adptPoints[0], Line.CreateUnbound(adaptPointPosition, PitchAxis), CamRot.Pitch);

                XYZ RollAxis = AdaptiveCameraInstance.GetTransform().BasisX;
                ElementTransformUtils.RotateElement(doc, adptPoints[0], Line.CreateUnbound(adaptPointPosition, RollAxis), CamRot.Roll);

                trans.Commit();
            }
        }

        public static void InserCameraModels(Document doc, List<CameraViewpoint> mCameraViewpointList, double workingDistance)
        {
            // Popup a progress dialog
            ProgressDialog progressDialog = new ProgressDialog();
            progressDialog.Show();
            int index = 1;

            // Check and load the adaptive family (AdaptiveCameraModel)
            String targetName = "AdaptiveCameraModel";
            CheckAndLoadFamily(doc, targetName);           
            FamilySymbol famSym = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).Where(q => q.Name == targetName).First() as FamilySymbol;

            // Create the camera instances
            using (Transaction trans = new Transaction(doc, "Create a camera instance"))
            {
                trans.Start();

                if (!famSym.IsActive)
                {
                    famSym.Activate();
                }

                foreach (CameraViewpoint cameraViewpoint in mCameraViewpointList)
                {
                    Orientation camOri = cameraViewpoint.orientation;
                    FamilyInstance AdaptiveCameraInstance = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, famSym);
                    workingDistance = GenericMethods.MM2Ft(workingDistance);
                    AdaptiveCameraInstance.LookupParameter("WorkingDistance").Set(workingDistance);

                    IList<ElementId> adptPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(AdaptiveCameraInstance);
                    ReferencePoint refPoint = (ReferencePoint)doc.GetElement(adptPoints[0]);

                    XYZ transform = cameraViewpoint.position.Subtract(refPoint.Position);
                    ElementTransformUtils.MoveElement(doc, adptPoints[0], transform);

                    refPoint = (ReferencePoint)doc.GetElement(adptPoints[0]);
                    XYZ adaptPointPosition = refPoint.Position;
                    XYZ YawAxis = AdaptiveCameraInstance.GetTransform().BasisZ; ;
                    ElementTransformUtils.RotateElement(doc, adptPoints[0], Line.CreateUnbound(adaptPointPosition, YawAxis), camOri.Yaw);

                    XYZ PitchAxis = -AdaptiveCameraInstance.GetTransform().BasisY;
                    ElementTransformUtils.RotateElement(doc, adptPoints[0], Line.CreateUnbound(adaptPointPosition, PitchAxis), camOri.Pitch);

                    XYZ RollAxis = AdaptiveCameraInstance.GetTransform().BasisX;
                    ElementTransformUtils.RotateElement(doc, adptPoints[0], Line.CreateUnbound(adaptPointPosition, RollAxis), camOri.Roll);

                    // Update the progress
                    int progressValue = index * 100 / mCameraViewpointList.Count();
                    progressDialog.UpdateProgress(progressValue);
                    index++;
                }

                trans.Commit();
            }
        }

        // Method 4: Check and load the needed family
        public static void CheckAndLoadFamily(Document doc, String familyName)
        {
            FilteredElementCollector mFilteredElementCollector = new FilteredElementCollector(doc);
            if (!(mFilteredElementCollector.OfClass(typeof(Family)).FirstOrDefault(e => e.Name.Equals(familyName)) is Family family))
            {
                using (Transaction trans = new Transaction(doc, "load a family"))
                {
                    trans.Start();

                    string filepath = @"C:\ProgramData\Autodesk\RVT 2020\Libraries\Custom\" + familyName + ".rfa";
                    doc.LoadFamily(filepath);

                    trans.Commit();
                }
                
            }
        }

        // Method 5: Obtain the position from family instance
        public static XYZ ObtainPositionFromElement(FamilyInstance mCamIns)
        {
            LocationPoint locPoint = (LocationPoint)mCamIns.Location;
            return locPoint.Point;
        }

        // Method 6: Obtain the camera viewpoints from the current document
        public static List<CameraViewpoint> ObtainCameraViewpointsFromDoc(Document doc)
        {
            string camFamName = "AdaptiveCameraModel";
            IList<Element> mCamInstances = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).Where(q => q.Name == camFamName).ToList();

            List<CameraViewpoint> mCameraViewpointList = new List<CameraViewpoint>();

            foreach(Element camModel in mCamInstances )
            {
                FamilyInstance camIns= camModel as FamilyInstance;
                Transform mTransform= camIns.GetTransform();
                XYZ position = new XYZ(Math.Round(mTransform.Origin.X,3), Math.Round(mTransform.Origin.Y, 3), Math.Round(mTransform.Origin.Z, 3));
                Orientation orientation = CameraRotation(mTransform.BasisX, -mTransform.BasisY);

                using (Transaction trans=new Transaction(doc, "Obtain the position and orientation information"))
                {
                    trans.Start();

                    XYZ outPosition = ConvertPositionForOutput(position);
                    Orientation outOrientation = ConvertOrientationForOutput(orientation);
                    camIns.LookupParameter("Position").Set(Math.Round(outPosition.X,3)+","+ Math.Round(outPosition.Y,3) + ","+Math.Round(outPosition.Z,3));
                    camIns.LookupParameter("Orientation").Set(Math.Round(outOrientation.Yaw)+","+ Math.Round(outOrientation.Pitch)+","+ Math.Round(outOrientation.Roll));

                    trans.Commit();
                }

                mCameraViewpointList.Add(new CameraViewpoint(position, orientation));
            }

            return mCameraViewpointList;
        }

        // Method 7: Order the blockbox list
        public static IList<BlockBox> OrderBlockBoxList(CameraViewpoint startCameraViewpoint, IList<BlockBox> inputBlockBoxList)
        {
            IList<BlockBox> originalBlockBoxList = inputBlockBoxList;
            IList<BlockBox> outputBlockBoxList = new List<BlockBox>();


            BlockBox blockBox= originalBlockBoxList.OrderBy(q => q.center.Z).ThenBy(q => Math.Abs(startCameraViewpoint.position.Y - q.center.Y)).ElementAt(0);
            outputBlockBoxList.Add(blockBox);
            int number = originalBlockBoxList.Count();
            for (int i=1; i<number; i++)
            {
                originalBlockBoxList.Remove(blockBox);
                blockBox = outputBlockBoxList.ElementAt(i - 1).ObtainNearestBlockBox(originalBlockBoxList);
                outputBlockBoxList.Add(blockBox);
            }

            return outputBlockBoxList;
        }

        // Method 8: Round the XYZ into 3 digits
        public static XYZ RoundXYZ(XYZ xyz)
        {
            return new XYZ (Math.Round(xyz.X, 4), Math.Round(xyz.Y, 4), Math.Round(xyz.Z, 4));
        }

        // Method 9: Convert the unit of the camera viewpoint
        public static List<CameraViewpoint> ConvertUnitOfViewpoints(List<CameraViewpoint> finalCameraViewpoint)
        {
            List<CameraViewpoint> outCameraViewpoints = new List<CameraViewpoint>();
            foreach(CameraViewpoint camViewpoint in finalCameraViewpoint)
            {
                XYZ position = ConvertPositionForOutput(camViewpoint.position);
                Orientation orientation = ConvertOrientationForOutput(camViewpoint.orientation);

                outCameraViewpoints.Add(new CameraViewpoint(position,orientation));
            }
            return outCameraViewpoints;
        }

        public static XYZ ConvertPositionForOutput(XYZ position)
        {
            double X = Math.Round(GenericMethods.Ft2MM(position.X) / 1000, 3);
            double Y = Math.Round(GenericMethods.Ft2MM(position.Y) / 1000, 3);
            double Z = Math.Round(GenericMethods.Ft2MM(position.Z) / 1000, 3);
            return new XYZ(X,Y,Z);
        }

        public static Orientation ConvertOrientationForOutput(Orientation orientation)
        {
            double Yaw = Math.Round(GenericMethods.Rad2Degree(orientation.Yaw));
            double Pitch = Math.Round(GenericMethods.Rad2Degree(orientation.Pitch));
            double Roll = Math.Round(GenericMethods.Rad2Degree(orientation.Roll));
            if (Pitch > 90 && Pitch <= 270)
            {
                Pitch = 180 - Pitch;
                Yaw = 180 + Yaw;
                if (Yaw >= 360)
                {
                    Yaw = Yaw - 360;
                }
            }
            else if (Pitch > 270)
            {
                Pitch = Pitch - 360;
            }

            if (Roll <= 90)
            {

            }else if (Roll <= 180)
            {
                Roll = Roll - 180;
            }else if (Roll <= 270)
            {
                Roll = Roll - 180;
            }
            else
            {
                Roll = Roll - 360;
            }

            return new Orientation(Yaw, Pitch, Roll);
        }


        // Method : Unit Conversion
        public static double MM2Ft(double millimeter)
        {
            double Ft = millimeter / 304.8;
            return Ft;
        }

        public static double Ft2MM(double Ft)
        {
            double MM = Ft * 304.8;
            return MM;
        }

        public static double Rad2Degree(double rad)
        {
            return rad * 180 / Math.PI;
        }

        public static double Degree2Rad(double degree)
        {
            return degree / 180 * Math.PI;
        }
    }
}
