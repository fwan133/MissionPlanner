using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using LaptopRevitCommands.Commands.CVA;
using LaptopRevitCommands.Views;

namespace LaptopRevitCommands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    class AdjustViewpoints : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Foundamental Parameters
            double latSafeDis;
            double verSafeDis;

            UAVSafetyZoneDialog safetyZoneDialog = new UAVSafetyZoneDialog();
            DialogResult safetyZoneResult=safetyZoneDialog.ShowDialog();
            if (safetyZoneResult == DialogResult.OK)
            {
                latSafeDis = double.Parse(safetyZoneDialog.label_Lh.Text) * 1000;
                verSafeDis = double.Parse(safetyZoneDialog.label_Lv.Text) * 1000;
            }
            else
            {
                return Result.Failed;
            }

            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            TaskDialogResult dialogResult = TaskDialog.Show("Revit", "Please select the main structure for interference check.", TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok);
            if (dialogResult == TaskDialogResult.Cancel)
            {
                return Result.Succeeded;
            }

            Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

            try
            {
                // Obtain the collector of all camera instances
                string camFamName = "AdaptiveCameraModel";
                IList<Element> mCameraInstances = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).Where(q => q.Name == camFamName).ToList();
                ICollection<ElementId> mAdjustCameraInstance =new List<ElementId>();

                // Pop-up a progress dialog
                ProgressDialog progressDialog = new ProgressDialog();
                progressDialog.Show();
                int index = 1;

                // Check intersection and adjust the position
                foreach (FamilyInstance camIns  in mCameraInstances)
                {
                    LocationPoint locPoint = (LocationPoint) camIns.Location;
                    XYZ position = locPoint.Point;
                    if (CVA_GenericMethods.CheckIfInaccessible(doc, pickedObj, position, latSafeDis, verSafeDis))
                    {
                        // Create available position lists
                        IList<XYZ> mXYZList1 = CreateList1ofAlternativeXYZs(camIns);
                        XYZ initialPosition = CVA_GenericMethods.ObtainCameraPosition(camIns);
                        XYZ targetPoint = CVA_GenericMethods.ObtainTargetPoint(camIns);
                        XYZ alterPoint=ObtainOptimalAlternative(doc, pickedObj, initialPosition, mXYZList1, latSafeDis, verSafeDis);
                        if (alterPoint.GetLength()==0)
                        {
                            IList<XYZ> mXYZList2 = CreateList2ofAlternativeXYZs(camIns);
                            alterPoint = ObtainOptimalAlternative(doc, pickedObj, targetPoint, mXYZList2, latSafeDis, verSafeDis);
                        }

                        // Adjust the camera instance
                        if (alterPoint.GetLength() != 0)
                        {
                            AdjustViewpoint(doc, camIns, alterPoint);
                        }
                        
                        mAdjustCameraInstance.Add(camIns.Id);
                    }

                    // Update progress
                    double progressValue = (double)index / mCameraInstances.Count()*100;
                    progressDialog.UpdateProgress((int) progressValue);
                    index++;
                }
                if (mAdjustCameraInstance.Count() == 0)
                {
                    TaskDialog.Show("Revit", "No intersection of the camera viewpoints was found with the selected structure !");
                }
                else
                {
                    uidoc.Selection.SetElementIds(mAdjustCameraInstance);
                    uidoc.ShowElements(mAdjustCameraInstance);
                    uidoc.RefreshActiveView();
                    TaskDialog.Show("Revit", "The influenced camera viewpoints by the structure has been adjusted.");
                }
                

                return Result.Succeeded;
            }
            catch(Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }

        }

        // Method 1: Obtain optimal alternation position
        private XYZ ObtainOptimalAlternative(Document doc, Reference targetEleRef, XYZ initialXYZ, IList<XYZ> mListXYZ, double latDis, double verDis)
        {
            string familyName = "DroneSafeZone";
            XYZ finalXYZ=new XYZ();
            double minDistance=0;
            GenericMethods.CheckAndLoadFamily(doc, familyName);

            // Create family instances at input points
            using (Transaction trans = new Transaction(doc, "create temporary family instances"))
            {
                trans.Start();

                // Create a temporary family instance
                FamilySymbol famSym = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).Where(q => q.Name == familyName).First() as FamilySymbol;
                famSym.Activate();
                foreach (XYZ xyz in mListXYZ)
                {
                    FamilyInstance mFamIns;
                    if (doc.IsFamilyDocument)
                    {
                        mFamIns = doc.FamilyCreate.NewFamilyInstance(xyz, famSym, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    }
                    else
                    {
                        mFamIns = doc.Create.NewFamilyInstance(xyz, famSym, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    }
                    mFamIns.LookupParameter("LatSafeDistance").Set(GenericMethods.MM2Ft(latDis));
                    mFamIns.LookupParameter("VerSafeDistance").Set(GenericMethods.MM2Ft(verDis));
                }

                trans.Commit();
            }

            // Check interference with targetElement
            ElementIntersectsElementFilter filter= new ElementIntersectsElementFilter(doc.GetElement(targetEleRef.ElementId), true);

            IList<Element> unInsectedElements = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).WherePasses(filter).Where(e => e.Name == familyName).ToList();
            foreach (Element e in unInsectedElements)
            {
                XYZ location = CVA_GenericMethods.ObtainCameraPosition(e as FamilyInstance);
                
                if(minDistance==0 || minDistance > initialXYZ.DistanceTo(location))
                {
                    minDistance = initialXYZ.DistanceTo(location);
                    finalXYZ = location;
                }
            }

            // Delete the temperarily created family instance
            using (Transaction trans = new Transaction(doc, "delete temporary family instances"))
            {
                trans.Start();

                IList<Element> mSafeZoneInstances = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).Where(q => q.Name == familyName).ToList();
                ICollection<ElementId> mSafeZoneInstanceIds = new List<ElementId>();
                foreach (Element ele in mSafeZoneInstances)
                {
                    mSafeZoneInstanceIds.Add(ele.Id);
                }
                doc.Delete(mSafeZoneInstanceIds);

                trans.Commit();
            }

            return finalXYZ;
        }



        // Method 2: Create group 1 of alternative positions for the affected cameraviewpoints
        private IList<XYZ> CreateList1ofAlternativeXYZs(FamilyInstance cameraInstance)
        {
            // Obtain camera-related parameters
            double sensorSizeL = cameraInstance.LookupParameter("SensorSizeLength").AsDouble();
            double sensorSizeW = cameraInstance.LookupParameter("SensorSizeWidth").AsDouble();
            double focalLength = cameraInstance.LookupParameter("FocalLength").AsDouble();

            // Obtain location-related parameters
            double workingDistance = cameraInstance.LookupParameter("WorkingDistance").AsDouble();
            Transform camTransform=cameraInstance.GetTransform();
            XYZ position = camTransform.Origin;
            XYZ orientation = camTransform.BasisX;
            XYZ lengthDir = -camTransform.BasisY;
            XYZ widthDir = camTransform.BasisZ;

            // Calculate the List<XYZ>
            IList<XYZ> alternativeXYZs = new List<XYZ>();
            double safetyDistance = GenericMethods.MM2Ft(2000);

            int layerQuantity1 = 4;
            int number = 13;
            double interval_X = workingDistance * (sensorSizeL / focalLength) / (number-1);
            double interval_Y = workingDistance * (sensorSizeW / focalLength) / (number-1);
            double interval_Z = (workingDistance - safetyDistance) / (layerQuantity1 - 1);

            for(int i = 0; i < layerQuantity1; i++)
            {
                double coefficientZ = interval_Z*i;
                // CenterPoint
                XYZ centerPoint = position.Add(coefficientZ * orientation);
                alternativeXYZs.Add(centerPoint);
                for(int j = 1; j <= (number - 1) / 2; j++)
                {
                    double coefficientX = j * interval_X;
                    double coefficientY = j * interval_Y;

                    XYZ Point1 = position.Add(coefficientX * lengthDir).Add(coefficientZ * orientation);
                    alternativeXYZs.Add(Point1);
                    XYZ Point2 = position.Add(coefficientY * widthDir).Add(coefficientZ*orientation);
                    alternativeXYZs.Add(Point2);
                    XYZ Point3 = position.Add(coefficientX * lengthDir.Negate()).Add(coefficientZ*orientation);
                    alternativeXYZs.Add(Point3);
                    XYZ Point4 = position.Add(coefficientY * widthDir.Negate()).Add(coefficientZ * orientation);
                    alternativeXYZs.Add(Point4);
                }
            }

            return alternativeXYZs;
        }

        // Method 3: Create group 2 of alternative positions for the affected cameraviewpoints
        private IList<XYZ> CreateList2ofAlternativeXYZs(FamilyInstance cameraInstance)
        {
            // Obtain camera-related parameters
            double sensorSizeL = cameraInstance.LookupParameter("SensorSizeLength").AsDouble();
            double sensorSizeW = cameraInstance.LookupParameter("SensorSizeWidth").AsDouble();
            double focalLength = cameraInstance.LookupParameter("FocalLength").AsDouble();

            // Obtain location-related parameters
            double workingDistance = cameraInstance.LookupParameter("WorkingDistance").AsDouble();
            Transform camTransform = cameraInstance.GetTransform();
            XYZ position = camTransform.Origin;
            XYZ orientation = camTransform.BasisX;
            XYZ lengthDir = -camTransform.BasisY;
            XYZ widthDir = camTransform.BasisZ;

            // Calculate the List<XYZ>
            IList<XYZ> alternativeXYZs = new List<XYZ>();

            int layerQuantity1 = 3;
            int number = 13;
            double interval_X = workingDistance * sensorSizeL / focalLength / (number - 1);
            double interval_Y = workingDistance * sensorSizeW / focalLength / (number - 1);
            double interval_Z = workingDistance / layerQuantity1;

            for (int i = 1; i <= layerQuantity1; i++)
            {
                double coefficientZ = interval_Z * i;
                // CenterPoint
                XYZ centerPoint = position.Add(coefficientZ * orientation.Negate());
                alternativeXYZs.Add(centerPoint);
                for (int j = 1; j <= (number - 1) / 2; j++)
                {
                    double coefficientX = j * interval_X;
                    double coefficientY = j * interval_Y;

                    XYZ Point1 = position.Add(coefficientX * lengthDir).Add(coefficientZ * orientation.Negate());
                    alternativeXYZs.Add(Point1);
                    XYZ Point2 = position.Add(coefficientY * widthDir).Add(coefficientZ * orientation.Negate());
                    alternativeXYZs.Add(Point2);
                    XYZ Point3 = position.Add(coefficientX * lengthDir.Negate()).Add(coefficientZ * orientation.Negate());
                    alternativeXYZs.Add(Point3);
                    XYZ Point4 = position.Add(coefficientY * widthDir.Negate()).Add(coefficientZ * orientation.Negate());
                    alternativeXYZs.Add(Point4);
                }
            }

            return alternativeXYZs;
        }

        // Method 4: Adjust the viewpoint to a new position
        private void AdjustViewpoint(Document doc, FamilyInstance mCamIns, XYZ alterPoint)
        {
            XYZ targetPoint = CVA_GenericMethods.ObtainTargetPoint(mCamIns);
            XYZ initialPosition = CVA_GenericMethods.ObtainCameraPosition(mCamIns);
            XYZ dirInitialToTarget = (targetPoint - initialPosition).Normalize();
            XYZ dirAdjustToTarget = (targetPoint - alterPoint).Normalize();

            double angle = 0;
            double adjustWorkingDistance = 0;
            XYZ CrossProduct;
            if (dirInitialToTarget.DistanceTo(dirAdjustToTarget) < 0.00001)
            {
                CrossProduct = dirAdjustToTarget;
                angle = 0;
                adjustWorkingDistance = (targetPoint - alterPoint).GetLength();
            }
            else
            {
                CrossProduct = dirInitialToTarget.CrossProduct(dirAdjustToTarget).Normalize();
                angle = dirInitialToTarget.AngleOnPlaneTo(dirAdjustToTarget, CrossProduct);
                adjustWorkingDistance = (targetPoint - alterPoint).GetLength();
            }
            
            using (Transaction trans = new Transaction(doc, "Adjust the position and orientation of a camera instance."))
            {
                trans.Start();

                mCamIns.Location.Move(alterPoint - initialPosition);
                ElementTransformUtils.RotateElement(doc, mCamIns.Id, Line.CreateUnbound(alterPoint, CrossProduct), angle);
                mCamIns.LookupParameter("WorkingDistance").Set(adjustWorkingDistance);

                trans.Commit();
            }
        }


    }
}
