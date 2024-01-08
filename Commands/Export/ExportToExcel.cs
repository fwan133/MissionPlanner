using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;
using OfficeOpenXml;

namespace LaptopRevitCommands.Commands.Export
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class ExportToExcel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Obtain the flight route
            string famName_FlightRoute = "AdaptiveRedArrow";
            IList<Element> flightPathList = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).Where(q => q.Name == famName_FlightRoute).ToList();

            // Obtain the camera viewpoints
            List<CameraViewpoint> cameraViewpointList = GenericMethods.ObtainCameraViewpointsFromDoc(doc);

            if (flightPathList.Count()==0 | cameraViewpointList.Count() == 0)
            {
                TaskDialog.Show("Revit", "Please plan the flight route first.");
                return Result.Cancelled;
            }

            // Obtain positions from flight path
            List<XYZ> waypointList = new List<XYZ>();
            foreach (Element flightPathInstance in flightPathList)
            {
                FamilyInstance flightPathIns = flightPathInstance as FamilyInstance;
                List<XYZ> xyzList = ObtainWaypointFromPath(doc,flightPathIns);
                foreach(XYZ xyz in xyzList)
                {
                    waypointList.Add(xyz);
                }
            }

            try
            {
                // Generate an ordered cameraviewpoint list
                List<CameraViewpoint> finalWayPoints = new List<CameraViewpoint>();
                foreach(XYZ xyz in waypointList)
                {
                    finalWayPoints.Add(ObtainCameraViewpointFromInputWaypoint(xyz,cameraViewpointList));
                }

                // Obtain the camera viewpoint for export
                List<CameraViewpoint> exportList = GenericMethods.ConvertUnitOfViewpoints(finalWayPoints);

                // Check if two waypoints are too close
                for(int i = 1; i < exportList.Count(); i++)
                {
                    if (exportList.ElementAt(i).position.DistanceTo(exportList.ElementAt(i - 1).position) < 1)
                    {
                        XYZ xyz_CurrentViewpoint = exportList.ElementAt(i).position;
                        CameraViewpoint insertViewpoint=new CameraViewpoint(new XYZ(xyz_CurrentViewpoint.X, xyz_CurrentViewpoint.Y, xyz_CurrentViewpoint.Z+3),new Orientation(1000,1000,1000));
                        exportList.Insert(i, insertViewpoint);
                    }
                }

                // Export to Excel
                SaveToExcel(exportList);
                TaskDialog.Show("Revit","The flight route has been saved successfully.");

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                TaskDialog.Show("Revit Error", e.Message);
                return Result.Failed;
            }

        }

        // Method 1: Obtain waypoints from flight path
        private List<XYZ> ObtainWaypointFromPath(Document doc, FamilyInstance mFlightPathInstance)
        {
            IList<ElementId> adptPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(mFlightPathInstance);
            List<XYZ> outputXYZlist = new List<XYZ>();

            for (int i = 0; i <= (adptPoints.Count() - 3); i++)
            {
                ReferencePoint refPoint1 = doc.GetElement(adptPoints[i]) as ReferencePoint;
                XYZ point1 = GenericMethods.RoundXYZ(refPoint1.Position);
                ReferencePoint refPoint2 = doc.GetElement(adptPoints[i + 1]) as ReferencePoint;
                XYZ point2 = GenericMethods.RoundXYZ(refPoint2.Position);
                ReferencePoint refPoint3 = doc.GetElement(adptPoints[i + 2]) as ReferencePoint;
                XYZ point3 = GenericMethods.RoundXYZ(refPoint3.Position);

                XYZ line1 = point2 - point1;
                XYZ line2 = point3 - point1;
                if (line1.AngleTo(line2) > 0.1)
                {
                    outputXYZlist.Add(point2);
                }

                if (i == ((adptPoints.Count() - 3)))
                {
                    outputXYZlist.Add(point3);
                }
            }
            return outputXYZlist;
        }

        // Method 2: Obtain the cameraviewpoint according to the reference waypoint
        private CameraViewpoint ObtainCameraViewpointFromInputWaypoint(XYZ waypoint, List<CameraViewpoint> mCameraViewpointList)
        {
            CameraViewpoint outputCamView;
            CameraViewpoint camViewpoint = (CameraViewpoint) mCameraViewpointList.OrderBy(q => q.position.DistanceTo(waypoint)).ToList().First();

            if (camViewpoint.position.DistanceTo(waypoint) < 0.001)
            {
                outputCamView = camViewpoint;
            }
            else
            {
                outputCamView = new CameraViewpoint(waypoint, new Orientation(1000,1000,1000));
            }

            return outputCamView;
        }

        // Method 3: Export WaypointList to Excel
        private void SaveToExcel(List<CameraViewpoint> finalWayPoints)
        {
            // Obtain the directory and filename
            SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();
            saveDlg.InitialDirectory = @"C:\";
            saveDlg.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveDlg.FilterIndex = 0;
            saveDlg.RestoreDirectory = true;
            saveDlg.Title = "Export Excel File To";
            if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = saveDlg.FileName;
                FileInfo file = new FileInfo(path);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                DeleteIfExists(file);

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Sheet1");

                    int rowIndex = 1;
                    ws.Cells[rowIndex, 1].Value = "X";
                    ws.Cells[rowIndex, 2].Value = "Y";
                    ws.Cells[rowIndex, 3].Value = "Z";
                    ws.Cells[rowIndex, 4].Value = "Yaw";
                    ws.Cells[rowIndex, 5].Value = "Pitch";
                    ws.Cells[rowIndex, 6].Value = "Roll";

                    foreach (CameraViewpoint camViewpoint in finalWayPoints)
                    {
                        rowIndex++;
                        ws.Cells[rowIndex, 1].Value = camViewpoint.position.X;
                        ws.Cells[rowIndex, 2].Value = camViewpoint.position.Y;
                        ws.Cells[rowIndex, 3].Value = camViewpoint.position.Z;
                        ws.Cells[rowIndex, 4].Value = camViewpoint.orientation.Yaw;
                        ws.Cells[rowIndex, 5].Value = camViewpoint.orientation.Pitch;
                        ws.Cells[rowIndex, 6].Value = camViewpoint.orientation.Roll;
                    }

                    package.Save();
                }
            }            
        }

        private void DeleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }
    }
}
