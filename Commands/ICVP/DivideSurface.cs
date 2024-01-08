using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.ExtensibleStorage;
using LaptopRevitCommands.Views;

namespace LaptopRevitCommands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class DivideSurface : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            List<CameraViewpoint> mCameraViewpointList = new List<CameraViewpoint>();

            try
            {
                // Pop up a dialog to obtain image configuration parameters
                DigitalCameraModel mDigitalCameraModel=new DigitalCameraModel();
                ImagesConfigurationParameters mImageConfigurationParameters=new ImagesConfigurationParameters();

                ImageConfiParaDialog mImageConfiParaDialog = new ImageConfiParaDialog();
                DialogResult imageConfigurationResult= mImageConfiParaDialog.ShowDialog();
                if (imageConfigurationResult == DialogResult.OK)
                {
                    mDigitalCameraModel = mImageConfiParaDialog.mDigitalCameraModels.Where(q => q.Name == mImageConfiParaDialog.cB_CameraList.Text).First();
                    double GSD = Convert.ToDouble(mImageConfiParaDialog.tB_GSD.Text);
                    double forwardOverlap = (double)Convert.ToDouble(mImageConfiParaDialog.tB_ForwardOverlap.Text) / 100;
                    double sideOverlap = (double)Convert.ToDouble(mImageConfiParaDialog.tB_SideOverlap.Text) / 100;
                    mImageConfigurationParameters = new ImagesConfigurationParameters(GSD,sideOverlap,forwardOverlap);
                }
                else if (imageConfigurationResult == DialogResult.Cancel)
                {
                    return Result.Failed;
                }

                //Pick Object
                TaskDialogResult dialogResult = TaskDialog.Show("Revit", "Please select surfaces for camera viewpoints planning.", TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok);
                if (dialogResult == TaskDialogResult.Cancel)
                {
                    return Result.Failed;
                }

                IList<Reference> pickedObjs = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Face);

                // Obtain the information of camera viewpoints
                ProgressDialog progressDialog = new ProgressDialog();
                progressDialog.Show();
                int totalTask = pickedObjs.Count();
                int currentTask = 0;

                foreach(Reference pickedObj in pickedObjs)
                {
                    // Define index for current progress
                    int index = 1;

                    Face face = doc.GetElement(pickedObj).GetGeometryObjectFromReference(pickedObj) as Face;

                    // Judge if the selected face has been divided
                    FilteredElementCollector mDivSurfCollector = new FilteredElementCollector(doc).OfClass(typeof(DividedSurface));
                    if (mDivSurfCollector.Where(q => q.Id == pickedObj.ElementId).Count() != 0)
                    {
                        TaskDialog.Show("Revit", "The selected face has been divided. Please delete the previous one if you want to recreate.");
                        return Result.Failed;
                    }

                    // Calculate the needed rotation
                    double Rotation = CalRotation(face);

                    // Obtain Viewpoint Configuration Parameters
                    ViewpointConfigurationParameters mViewpointConfigurationParameters = CalCamViewConfiParameters(face, Rotation, mDigitalCameraModel, mImageConfigurationParameters);

                    //Divide Surface
                    if (mDivSurfCollector.GetElementCount() == 0 || mDivSurfCollector.Where(q => q.Id == pickedObj.ElementId).Count() == 0)
                    {
                        // Create
                        using (Transaction trans = new Transaction(doc, "Divide Flat Surface"))
                        {
                            trans.Start();

                            DividedSurface ds = DividedSurface.Create(doc, pickedObj);

                            ds.USpacingRule.SetLayoutFixedNumber(100, SpacingRuleJustification.Beginning, Rotation, 0);
                            ds.VSpacingRule.SetLayoutFixedNumber(100, SpacingRuleJustification.Beginning, Rotation, 0);

                            trans.Commit();
                        }

                        // Modification 
                        using (Transaction trans = new Transaction(doc, "Divide Surface"))
                        {
                            trans.Start();

                            DividedSurface ds = DividedSurface.GetDividedSurfaceForReference(doc, pickedObj);

                            // Calculate the USpacingRule_Number
                            SpacingRule srU = ds.USpacingRule;
                            int USpacingRuleNumber = (int)Math.Ceiling((srU.Distance * srU.Number) / GenericMethods.MM2Ft(mViewpointConfigurationParameters.Udistance));
                            ds.USpacingRule.SetLayoutFixedNumber(USpacingRuleNumber, SpacingRuleJustification.Beginning, Rotation, 0);

                            // Calculate the VSpacingRule_Number
                            SpacingRule srV = ds.VSpacingRule;
                            int VSpacingRuleNumber = (int)Math.Ceiling((srV.Distance * srV.Number) / GenericMethods.MM2Ft(mViewpointConfigurationParameters.Vdistance));
                            ds.VSpacingRule.SetLayoutFixedNumber(VSpacingRuleNumber, SpacingRuleJustification.Beginning, Rotation, 0);

                            trans.Commit();
                        }

                        // Obtain Camera Viewpoint List
                        DividedSurface mDividedSurface = DividedSurface.GetDividedSurfaceForReference(doc, pickedObj);
                        for (int i = 0; i < mDividedSurface.NumberOfUGridlines - 1; i++)
                        {
                            for (int j = 0; j < mDividedSurface.NumberOfVGridlines - 1; j++)
                            {
                                // Obtain the uv coordinate of the center point of each pannel
                                UV uv1 = mDividedSurface.GetGridNodeUV(new GridNode(i, j));
                                UV uv2 = mDividedSurface.GetGridNodeUV(new GridNode(i + 1, j));
                                UV uv3 = mDividedSurface.GetGridNodeUV(new GridNode(i, j + 1));
                                UV uv4 = mDividedSurface.GetGridNodeUV(new GridNode(i + 1, j + 1));
                                UV uv = uv1.Add(uv2).Add(uv3).Add(uv4).Divide(4);

                                // Obtain the position and location of the central point
                                Face hostface = face;
                                // Face hostface = mDividedSurface.Host.GetGeometryObjectFromReference(mDividedSurface.HostReference) as Face;

                                if (hostface.IsInside(uv))
                                {
                                    XYZ Pnt = hostface.Evaluate(uv);
                                    XYZ PntNormal = hostface.ComputeNormal(uv);

                                    Transform pretrans = hostface.ComputeDerivatives(uv);
                                    double rotation = mDividedSurface.USpacingRule.GridlinesRotation;
                                    Transform newtrans = Transform.CreateRotation(pretrans.BasisZ, rotation);
                                    XYZ UDir = newtrans.OfPoint(pretrans.BasisX);

                                    // Obtain the position and rotation of the camera viewpoint
                                    XYZ CamPos = GenericMethods.CameraPosition(Pnt, PntNormal, mViewpointConfigurationParameters.workingdistance);
                                    XYZ CameraDir = new XYZ(-PntNormal.X, -PntNormal.Y, -PntNormal.Z);
                                    Orientation CamRot = GenericMethods.CameraRotation(CameraDir, UDir);

                                    GenericMethods.InsertCameraModel(doc, CamPos, CamRot, mViewpointConfigurationParameters.workingdistance);
                                }


                                // Update the progress
                                int totalsubtask = (mDividedSurface.NumberOfVGridlines - 1) * (mDividedSurface.NumberOfUGridlines - 1);
                                double currentprogress = (double)index / totalsubtask;
                                int progress = (int)(100 *(currentTask +currentprogress)/totalTask);
                                index++;
                                progressDialog.UpdateProgress(progress);
                            }
                        }

                    }
                    else
                    {
                        TaskDialog.Show("Revit", "The selected face has been divided. Please delete the previous one if you want to recreate.");
                    }

                    currentTask++;
                }


                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }

        }


        // Method 1: Caluculate rotation of the default gridline
        private double CalRotation(Face face)
        {
            UV centerUV = face.GetBoundingBox().Max.Add(face.GetBoundingBox().Min).Divide(2);
            XYZ Udir = face.ComputeDerivatives(centerUV).BasisX;

            double Rotation = 0;
            if (!(Udir.Z == 0))
            {
                XYZ Zdir = face.ComputeDerivatives(centerUV).BasisZ;
                XYZ RefDir;
                if (Zdir.Y == 0)
                {
                    RefDir = new XYZ(-Zdir.Y / Zdir.X, 1, 0);
                }
                else
                {
                    RefDir = new XYZ(1, -Zdir.X / Zdir.Y, 0);
                }

                Rotation = Udir.AngleTo(RefDir);
                if (Rotation >= Math.PI / 2)
                {
                    Rotation = Rotation - Math.PI;
                }
            }
            Rotation = -Rotation;

            /*
            if ((Rotation-Math.PI/2)<0.001)
            {
                Rotation = Rotation - 0.01;
            }
            if ((Rotation + Math.PI / 2) < 0.001)
            {
                Rotation = Rotation + 0.01;
            }
            */
            return Rotation;
            
        }

        // Method 2: Calculate the image configuration parameters
        private ViewpointConfigurationParameters CalCamViewConfiParameters(Face face, double rotation, DigitalCameraModel mDigitalCameraModel, ImagesConfigurationParameters mImageConfigurationParameters)
        {
            // Retrieve basic parameters
            double sensorSizeU = mDigitalCameraModel.SensorSizeU;
            double sensorSizeV = mDigitalCameraModel.SensorSizeV;
            double focalLength = mDigitalCameraModel.FocalLength;
            int pixelNumberU = mDigitalCameraModel.PixelNumU;
            int pixelNumberV = mDigitalCameraModel.PixelNumV;

            double GSD = mImageConfigurationParameters.GSD;
            double overlapU = mImageConfigurationParameters.overlapU;
            double overlapV = mImageConfigurationParameters.overlapV;

            // Obtain the curvature of Post-rotate axis U and axis V
            UV centerUV = face.GetBoundingBox().Max.Add(face.GetBoundingBox().Min).Divide(2);
            FaceSecondDerivatives mFaceSecondDerivatives=face.ComputeSecondDerivatives(centerUV);
            double UUDerivativeLength=GenericMethods.Ft2MM(mFaceSecondDerivatives.UUDerivative.GetLength());
            double VVDerivativeLength = GenericMethods.Ft2MM(mFaceSecondDerivatives.VVDerivative.GetLength());
            double UUcurvature;
            double VVcurvature;
            if (UUDerivativeLength == 0)
            {
                UUcurvature = 0;
            }
            else
            {
                UUcurvature = 1 / UUDerivativeLength;
            }
            if (VVDerivativeLength == 0)
            {
                VVcurvature = 0;
            }
            else
            {
                VVcurvature = 1 / VVDerivativeLength;
            }

            double curvatureU =UUcurvature*Math.Abs(Math.Cos(rotation))+VVcurvature*Math.Abs(Math.Sin(rotation));
            if (curvatureU < 1/Math.Pow(10,10))
            {
                curvatureU = 0;
            }
            double curvatureV =VVcurvature*Math.Abs(Math.Cos(rotation))+UUcurvature*Math.Abs(Math.Sin(rotation));
            if (curvatureV < 1 / Math.Pow(10, 10))
            {
                curvatureV = 0;
            }

            // Obtain the workingdistance
            double preWorkingDistance = mDigitalCameraModel.ObtainWorkingDistance(GSD);
            double GSDmaxU = CalculateGSDmaxforCurve(curvatureU, preWorkingDistance, focalLength, sensorSizeU, pixelNumberU, overlapU);
            double GSDmaxV = CalculateGSDmaxforCurve(curvatureV, preWorkingDistance, focalLength, sensorSizeV, pixelNumberV, overlapV);
            double GSDmax=Math.Max(GSDmaxU, GSDmaxV);

            int ii = 0;
            while (Math.Abs(GSDmax-GSD)/GSD>0.01 && ii<15)
            {
                double Distance = preWorkingDistance * GSD / GSDmax;
                preWorkingDistance = (Distance+preWorkingDistance) / 2;

                double GSDmaxUadjusted = CalculateGSDmaxforCurve(curvatureU, preWorkingDistance, focalLength, sensorSizeU, pixelNumberU, overlapU);
                double GSDmaxVadjusted = CalculateGSDmaxforCurve(curvatureV, preWorkingDistance, focalLength, sensorSizeV, pixelNumberV, overlapV);

                GSDmax = Math.Max(GSDmaxUadjusted, GSDmaxVadjusted);

                ii++;
            }

            // Obtain viewpoint configuration parameters
            double mWorkingDistance = preWorkingDistance;
            double distanceU = CalRelativeDistances(curvatureV, mWorkingDistance, focalLength, sensorSizeV, pixelNumberV, overlapV);
            double distanceV = CalRelativeDistances(curvatureU, mWorkingDistance, focalLength, sensorSizeU, pixelNumberU, overlapU);

            return new ViewpointConfigurationParameters(distanceU,distanceV,mWorkingDistance);
        }


        private double CalculateGSDmaxforCurve(double curvature, double workingDistance, double focalLength, double sensorSize,double pixelNumber, double overlap)
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

                GSDmax =((workingDistance+R*(1-Math.Cos(betaModified)))/focalLength*pixelSize) / Math.Cos(betaModified);
            }

            return GSDmax;
        }

        private double CalRelativeDistances(double curvature,double workingDistance, double focalLength, double sensorSize, double pixelNumber, double overlap)
        {
            double pixelSize = sensorSize / pixelNumber;
            double relativeDistance;

            if (curvature == 0)
            {
                relativeDistance = sensorSize * workingDistance / focalLength * (1 - overlap);
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

                relativeDistance=2*beta*R*(1-overlap);
            }

            return relativeDistance;
        }
    }
}
