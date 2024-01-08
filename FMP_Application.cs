using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media;

namespace LaptopRevitCommands
{
    class FMP_Application : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string ribbonTabName = "UAV-FMP";
            application.CreateRibbonTab(ribbonTabName);
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            string path = myAssembly.Location;

            // RibbonPanel 1: Camera Viewpoints Planning
            RibbonPanel mRibbonPanel_ICVP = application.CreateRibbonPanel(ribbonTabName, "Camera Viewpoints Planning");
            // Button 1-PushButton: CameraModel
            PushButtonData mPushButtonDataNewCamera= new PushButtonData("Button_NewCamera", "New Camera\nModel", path, "LaptopRevitCommands.CreateDigitalCamera");
            PushButton mPushButtonNewCamera= mRibbonPanel_ICVP.AddItem(mPushButtonDataNewCamera) as PushButton;
            mPushButtonNewCamera.LargeImage= GetEmbeddedImage(myAssembly, "LaptopRevitCommands.Resources.CameraModel.png");
            // Button 2-PushButton: CameraViewPointsForEdge
            PushButtonData mPushButtonDataDivideEdge = new PushButtonData("Button_DivideEdge", "For Edge", path, "LaptopRevitCommands.DivideEdge");
            PushButton mPushButtonCameraViewEdge = mRibbonPanel_ICVP.AddItem(mPushButtonDataDivideEdge) as PushButton;
            mPushButtonCameraViewEdge.LargeImage = GetEmbeddedImage(myAssembly, "LaptopRevitCommands.Resources.ViewpointsForEdge.png");
            //Button 3-PushButton: CameraViewPointsForSurfaces
            PushButtonData mPushButtonDataDivideSur = new PushButtonData("Button_DividedSurf", "For Surface", path, "LaptopRevitCommands.DivideSurface");
            PushButton mPushButtonDivideSur = mRibbonPanel_ICVP.AddItem(mPushButtonDataDivideSur) as PushButton;
            mPushButtonDivideSur.LargeImage = GetEmbeddedImage(myAssembly, "LaptopRevitCommands.Resources.ViewpointsForSurface.png");

            // RibbonPanel 2: Camera Viewpoints Adjustment
            RibbonPanel mRibbonPanel_CVA = application.CreateRibbonPanel(ribbonTabName, "Camera Viewpoints Adjustment");
            // Button 1-PushButton: Search inaccessible viewpoints
            PushButtonData mPushButtonDataSearch = new PushButtonData("Button_Search", "Interference\nCheck", path, "LaptopRevitCommands.SearchInaccessibleViewpoints");
            PushButton mPushButtonSearch = mRibbonPanel_CVA.AddItem(mPushButtonDataSearch) as PushButton;
            mPushButtonSearch.LargeImage = GetEmbeddedImage(myAssembly, "LaptopRevitCommands.Resources.Search.png");
            // Button 2-PushButton: Automatically adjust the viewpoints
            PushButtonData mPushButtonDataAutomatedAdjustment = new PushButtonData("Button_AutomatedAdjustment", " Adjust ", path, "LaptopRevitCommands.AdjustViewpoints");
            PushButton mPushButtonAutomatedAdjustment = mRibbonPanel_CVA.AddItem(mPushButtonDataAutomatedAdjustment) as PushButton;
            mPushButtonAutomatedAdjustment.LargeImage = GetEmbeddedImage(myAssembly, "LaptopRevitCommands.Resources.AutomatedAdjust.png");
            // Button 3-PushButton: Manually adjust the viewpoints
            PushButtonData mPushButtonDataManualAdjustment = new PushButtonData("Button_ManualAdjustment", "Move/Rotate", path, "LaptopRevitCommands.ManuallyAdjustViewpoints");
            PushButton mPushButtonManualAdjustment = mRibbonPanel_CVA.AddItem(mPushButtonDataManualAdjustment) as PushButton;
            mPushButtonManualAdjustment.LargeImage = GetEmbeddedImage(myAssembly, "LaptopRevitCommands.Resources.ManualAdjust.png");

            // RibbonPanel 3: Flight Route Planning
            RibbonPanel mRibbonPanel_FRP = application.CreateRibbonPanel(ribbonTabName, "Flight Route Planning");
            // Button 1-PushButton: Create the takeoff Point for the UAV
            PushButtonData mPushButtonDataTakeoffPoint = new PushButtonData("Button_TakeoffPoint", " Insert \nTake-off Point", path, "LaptopRevitCommands.Commands.FRP.CreateTakeoffPoint");
            PushButton mPushButtonTakeoffPoint = mRibbonPanel_FRP.AddItem(mPushButtonDataTakeoffPoint) as PushButton;
            mPushButtonTakeoffPoint.LargeImage = GetEmbeddedImage(myAssembly, "LaptopRevitCommands.Resources.TakeoffPoint.png");
            // Button 2-PushButton: Plan the shortes UAV flight route
            PushButtonData mPushButtonDataFlightRoute = new PushButtonData("Button_FlightRoute", "Plan the \nFlight Route", path, "LaptopRevitCommands.Commands.FRP.PlanFlightRoute");
            PushButton mPushButtonFlightRoute = mRibbonPanel_FRP.AddItem(mPushButtonDataFlightRoute) as PushButton;
            mPushButtonFlightRoute.LargeImage = GetEmbeddedImage(myAssembly, "LaptopRevitCommands.Resources.RoutePlanning.png");
            // Button 3-PushButton: Adjust the UAV flight route to avoid obstacles
            PushButtonData mPushButtonDataAdjustFlightRoute = new PushButtonData("Button_AdjustFlightRoute", "Avoid\nObstacles", path, "LaptopRevitCommands.Commands.FRP.AvoidObstacles");
            PushButton mPushButtonAdjustFlightRoute = mRibbonPanel_FRP.AddItem(mPushButtonDataAdjustFlightRoute) as PushButton;
            mPushButtonAdjustFlightRoute.LargeImage = GetEmbeddedImage(myAssembly, "LaptopRevitCommands.Resources.AdjustFlightRoute.png");
            // Button 4-PushButton: Delete the generated UAV flight route
            PushButtonData mPushButtonDataDeleteFlightRoute = new PushButtonData("Button_DeleteFlightRoute", " Delete ", path, "LaptopRevitCommands.Commands.FRP.DeleteFlightRoute");
            PushButton mPushButtonDeleteFlightRoute = mRibbonPanel_FRP.AddItem(mPushButtonDataDeleteFlightRoute) as PushButton;
            mPushButtonDeleteFlightRoute.LargeImage = GetEmbeddedImage(myAssembly, "LaptopRevitCommands.Resources.DeleteFlightRoute.png");

            // RibbonPanel 4: Data Export
            RibbonPanel mRibbonPanel_EFR = application.CreateRibbonPanel(ribbonTabName,"Export");
            // Button 1-PushButton: Export the flight route to Excel
            PushButtonData mPushButtonDataExportToExcel = new PushButtonData("Button_ExportToExcel", "Export\nto Excel", path, "LaptopRevitCommands.Commands.Export.ExportToExcel");
            PushButton mPushButtonExportToExcel = mRibbonPanel_EFR.AddItem(mPushButtonDataExportToExcel) as PushButton;
            mPushButtonExportToExcel.LargeImage = GetEmbeddedImage(myAssembly, "LaptopRevitCommands.Resources.ExportToExcel.png");

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private ImageSource GetEmbeddedImage(System.Reflection.Assembly assemb, string imageName)
        {
            System.IO.Stream file = assemb.GetManifestResourceStream(imageName);
            PngBitmapDecoder bd = new PngBitmapDecoder(file, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            return bd.Frames[0];
        }
    }
}
