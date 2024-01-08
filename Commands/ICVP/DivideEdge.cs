using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.ExtensibleStorage;
using LaptopRevitCommands.Views;
using System.Windows.Forms;

namespace LaptopRevitCommands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    class DivideEdge : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            
            try
            {
                // Pop up a dialog to obtain image configuration parameters
                DigitalCameraModel mDigitalCameraModel = new DigitalCameraModel();
                ImagesConfigurationParameters mImageConfigurationParameters = new ImagesConfigurationParameters();

                ImageConfiParaDialog mImageConfiParaDialog = new ImageConfiParaDialog();
                DialogResult imageConfigurationResult = mImageConfiParaDialog.ShowDialog();
                if (imageConfigurationResult == DialogResult.OK)
                {
                    mDigitalCameraModel = mImageConfiParaDialog.mDigitalCameraModels.Where(q => q.Name == mImageConfiParaDialog.cB_CameraList.Text).First();
                    double GSD = Convert.ToDouble(mImageConfiParaDialog.tB_GSD.Text);
                    double forwardOverlap = (double)Convert.ToDouble(mImageConfiParaDialog.tB_ForwardOverlap.Text) / 100;
                    double sideOverlap = (double)Convert.ToDouble(mImageConfiParaDialog.tB_SideOverlap.Text) / 100;
                    mImageConfigurationParameters = new ImagesConfigurationParameters(GSD, sideOverlap, forwardOverlap);
                }
                else if (imageConfigurationResult == DialogResult.Cancel)
                {
                    return Result.Failed;
                }

                //Pick Edge
                TaskDialogResult dialogResult = TaskDialog.Show("Revit", "Please select edges for camera viewpoints planning.", TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok);
                if (dialogResult == TaskDialogResult.Cancel)
                {
                    return Result.Succeeded;
                }

                IList<Reference> pickedEdges = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Edge);

                foreach (Reference pickedEdge in pickedEdges)
                {
                    Edge edge = doc.GetElement(pickedEdge).GetGeometryObjectFromReference(pickedEdge) as Edge;

                    // Calculate foundamental direction
                    bool isCameraDirectionParallelToEdge = IsCameraDirectionParallelToEdge(edge);
                    double edgeCurvature = CalculateEdgeCurvature(edge);

                    // Calculate the distance
                    double workingDistance = CalculateWorkingDistance(edgeCurvature, mDigitalCameraModel, isCameraDirectionParallelToEdge, mImageConfigurationParameters);
                    double divisionDistance = CalculateDivisionDistance(workingDistance, edgeCurvature, isCameraDirectionParallelToEdge, mDigitalCameraModel, mImageConfigurationParameters);

                    // Divide the path
                    DividedPath mDividedPath;
                    using (Transaction trans = new Transaction(doc, "Divide a path"))
                    {
                        trans.Start();

                        IList<Reference> iListEdge = new List<Reference> { pickedEdge };
                        mDividedPath = DividedPath.Create(doc, iListEdge);
                        int numberPoints = (int)Math.Ceiling(edge.ApproximateLength / GenericMethods.MM2Ft(divisionDistance)) + 1;
                        mDividedPath.SpacingRuleLayout = SpacingRuleLayout.FixedNumber;
                        mDividedPath.FixedNumberOfPoints = numberPoints;

                        trans.Commit();
                    }

                    /*
                    // Store Data to the divided edge
                    Schema mSchema = ExtensibleStorage.SchemaExist("DividedPathSchema");
                    if (mSchema == null)
                    {
                        mSchema = ExtensibleStorage.CreateSchemaForDividedPath(doc);
                    }

                    ExtensibleStorage.AddEntityForDividedPathSchema(doc, mDividedPath, mSchema, workingDistance, "CameraModel1");
                    */

                    // Obtain the camera viewpoints
                    int pointsNumber = mDividedPath.NumberOfPoints;
                    for (int i = 0; i < (pointsNumber - 1); i++)
                    {
                        double gap = (double)1 / (pointsNumber - 1);
                        double para = gap / 2 + i * gap;

                        // Foundamental parameters
                        double angle = CalcuInnerAngle(edge, para);
                        XYZ point = edge.Evaluate(para);
                        Face face1 = edge.GetFace(0);
                        XYZ face1Normal = face1.ComputeNormal(edge.EvaluateOnFace(para, face1));
                        Face face2 = edge.GetFace(1);
                        XYZ face2Normal = face2.ComputeNormal(edge.EvaluateOnFace(para, face2));

                        // Obtain the refDirection of cameraviewpoints
                        List<XYZ> mXYZList = ObtainRefDirection(angle, face1Normal, face2Normal);

                        foreach (XYZ xyz in mXYZList)
                        {
                            XYZ camPos = GenericMethods.CameraPosition(point, xyz, workingDistance);

                            XYZ CameraDir = new XYZ(-xyz.X, -xyz.Y, -xyz.Z);
                            XYZ uDirection;
                            if (isCameraDirectionParallelToEdge)
                            {
                                uDirection = edge.ComputeDerivatives(para).BasisX.Normalize();
                            }
                            else
                            {
                                uDirection = edge.ComputeDerivatives(para).BasisZ.Normalize();
                            }
                            Orientation camRot = GenericMethods.CameraRotation(CameraDir, uDirection);

                            // Insert a camera model at the desired position
                            GenericMethods.InsertCameraModel(doc, camPos, camRot, workingDistance);
                        }
                    }
                } 

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
        }

        // Method 1: Calculate Udirection
        private bool IsCameraDirectionParallelToEdge(Edge edge)
        {
            Transform mTransform = edge.ComputeDerivatives(0.5);
            XYZ edgeDirection = mTransform.BasisX;

            double angle = edgeDirection.AngleTo(new XYZ(edgeDirection.X, edgeDirection.Y, 0));
            
            if (angle < (Math.PI/4))
            {
                return true;
            }
            else
            {
                return false;
            } 
        }

        // Method 2: Calculate Curvature
        private double CalculateEdgeCurvature(Edge edge)
        {
            double radius;
            Arc arc=edge.AsCurve() as Arc;
            if (arc != null)
            {
                radius = arc.Radius;
            }
            else
            {
                radius = 0;
            }

            double edgeRadius = GenericMethods.Ft2MM(radius);
            double edgeCurvature;
            if (edgeRadius < 1 / Math.Pow(10, 10))
            {
                edgeCurvature = 0;
            }
            else
            {
                edgeCurvature = 1 / edgeRadius;
            }
            return edgeCurvature;
        }

        // Method 3: Calculate the Working Distance
        private double CalculateWorkingDistance(double curvature, DigitalCameraModel mDigitalCameraModel, bool isCameraDirectionParallelToEdge, ImagesConfigurationParameters mImageConfiguration)
        {
            double focalLength = mDigitalCameraModel.FocalLength;
            double GSD = mImageConfiguration.GSD;
            double sensorSize;
            double pixelNumber;
            double overlap;

            if (isCameraDirectionParallelToEdge)
            {
                sensorSize= mDigitalCameraModel.SensorSizeU;
                pixelNumber = mDigitalCameraModel.PixelNumU;
                overlap = mImageConfiguration.overlapU;
            }
            else
            {
                sensorSize = mDigitalCameraModel.SensorSizeV;
                pixelNumber = mDigitalCameraModel.PixelNumV;
                overlap = mImageConfiguration.overlapV;
            }

            double workingDistance = mDigitalCameraModel.ObtainWorkingDistance(GSD);

            double GSDmax;

            if (curvature == 0)
            {
                GSDmax = mDigitalCameraModel.ObtainGSD(workingDistance);
            }
            else
            {
                GSDmax = CalculateGSDmaxforCurve(curvature,workingDistance,focalLength,sensorSize,pixelNumber,overlap);

                int ii = 0;
                while (Math.Abs(GSDmax - GSD) / GSD > 0.01 && ii<15)
                {
                    double distance= workingDistance * GSD / GSDmax;
                    workingDistance = (workingDistance+distance)/2;

                    GSDmax= CalculateGSDmaxforCurve(curvature, workingDistance, focalLength, sensorSize, pixelNumber, overlap);

                    ii++;
                }
            }

            return workingDistance;
        }

        private double CalculateGSDmaxforCurve(double curvature, double workingDistance, double focalLength, double sensorSize, double pixelNumber, double overlap)
        {
            double pixelSize = sensorSize / pixelNumber;
            double GSDmax;

            if (curvature == 0)
            {
                GSDmax = workingDistance / focalLength * pixelSize;
            }
            else
            {
                double R = 1 / curvature;

                double c = sensorSize / Math.Sqrt(sensorSize * sensorSize + 4 * focalLength * focalLength);

                double Dlim = (1 / c - 1) * R;
                double alpha = Math.Asin(c);
                double beta;

                if (workingDistance < Dlim)
                {
                    beta = Math.Asin((1 + workingDistance / R) * c) - alpha;
                }
                else
                {
                    beta = Math.Acos(R / (R + workingDistance));
                }

                double betaModified = beta * (1 - overlap);
                GSDmax = ((workingDistance + R * (1 - Math.Cos(betaModified))) / focalLength * pixelSize) / Math.Cos(betaModified);
            }

            return GSDmax;
        }

        private double CalculateDivisionDistance(double workingDistance, double curvature, bool isCameraDirectionParallelToEdge, DigitalCameraModel mDigitalCameraModel, ImagesConfigurationParameters mImageConfiguration)
        {
            double focalLength = mDigitalCameraModel.FocalLength;
            double GSD = mImageConfiguration.GSD;
            double sensorSize;
            double pixelNumber;
            double overlap;

            if (isCameraDirectionParallelToEdge)
            {
                sensorSize = mDigitalCameraModel.SensorSizeU;
                pixelNumber = mDigitalCameraModel.PixelNumU;
                overlap = mImageConfiguration.overlapU;
            }
            else
            {
                sensorSize = mDigitalCameraModel.SensorSizeV;
                pixelNumber = mDigitalCameraModel.PixelNumV;
                overlap = mImageConfiguration.overlapV;
            }

            double divisionDistance;

            if (curvature == 0)
            {
                divisionDistance = (1 - overlap) * sensorSize * workingDistance / focalLength;
            }
            else
            {
                double R = 1 / curvature;

                double c = sensorSize / Math.Sqrt(sensorSize * sensorSize + 4 * focalLength * focalLength);

                double Dlim = (1 / c - 1) * R;
                double alpha = Math.Asin(c);
                double beta;

                if (workingDistance < Dlim)
                {
                    beta = Math.Asin((1 + workingDistance / R) * c) - alpha;
                }
                else
                {
                    beta = Math.Acos(R / (R + workingDistance));
                }

                divisionDistance = (1-overlap)*2 * beta * R;
            }

            return divisionDistance;
        }

        //Method 4: Calculate the innerangle of the edge
        private double CalcuInnerAngle(Edge edge, double param)
        {
            XYZ basePoint = edge.Evaluate(param);

            Face face1 = edge.GetFace(0);
            UV uv1 = edge.EvaluateOnFace(param, face1);
            XYZ face1Normal = face1.ComputeNormal(uv1);

            Face face2 = edge.GetFace(1);
            UV uv2 = edge.EvaluateOnFace(param, face2);
            XYZ face2Normal = face2.ComputeNormal(uv2);

            double angle = face1Normal.AngleTo(face2Normal);

            XYZ newPoint = basePoint.Add(face1Normal).Add(face2Normal);
            IntersectionResult intersectionResult = face1.Project(newPoint);

            if (intersectionResult == null)
            {
                angle = Math.PI - angle;
            }
            else
            {
                angle = Math.PI + angle;
            }

            return angle;
        }

        // Method 6: Calculate the RefDirection of the cameraviewpoints
        private List<XYZ> ObtainRefDirection(double angle, XYZ face1Normal, XYZ face2Normal)
        {
            List<XYZ> mList = new List<XYZ>();

            if(angle<Math.PI*0.25)   // <=45, 5
            {
                XYZ refDire1 = (face1Normal + face2Normal).Normalize();
                XYZ refDire2 = (face1Normal + refDire1).Normalize();
                XYZ refDire3 = (refDire1 + refDire2).Normalize();
                XYZ refDire4 = (face2Normal + refDire1).Normalize();
                XYZ refDire5 = (refDire1 + refDire4).Normalize();
                mList.Add(refDire1);
                mList.Add(refDire2);
                mList.Add(refDire3);
                mList.Add(refDire4);
                mList.Add(refDire5);
            }
            else if (angle < Math.PI *0.49)   // 45~90, 3
            {
                XYZ refDire1 = (face1Normal + face2Normal).Normalize();
                XYZ refDire2 = (face1Normal + refDire1).Normalize();
                XYZ refDire3 = (face2Normal + refDire1).Normalize();
                mList.Add(refDire1);
                mList.Add(refDire2);
                mList.Add(refDire3);
            } 
            else if(angle<Math.PI*0.74) //90~135, 2
            {
                double theta = face1Normal.AngleTo(face2Normal);
                XYZ refDire1 = CalcuDire(face1Normal,face2Normal,theta/3);
                XYZ refDire2 = CalcuDire(face1Normal, face2Normal, 2*theta / 3);
                mList.Add(refDire1);
                mList.Add(refDire2);
            }
            else  // 135~360
            {
                XYZ refDire1 = (face1Normal + face2Normal).Normalize();
                mList.Add(refDire1);
            }

            return mList;
        }

        private XYZ CalcuDire(XYZ face1Normal, XYZ face2Normal, double theta)
        {
            double angle = face1Normal.AngleTo(face2Normal);
            XYZ xyz = (face1Normal + Math.Sin(theta) / Math.Sin(angle - theta) * face2Normal).Normalize();
            return xyz;
        }
    }
}
