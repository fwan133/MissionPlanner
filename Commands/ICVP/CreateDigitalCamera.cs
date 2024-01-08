using LaptopRevitCommands.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace LaptopRevitCommands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CreateDigitalCamera: IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            DigitalCameraModelDialog mCameraDialog = new DigitalCameraModelDialog();
            mCameraDialog.ShowDialog();
            return Result.Succeeded;
        }
    }
}
