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
    class SearchInaccessibleViewpoints: IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Foundamental Parameters
            double latSafeDis;
            double verSafeDis;

            UAVSafetyZoneDialog safetyZoneDialog = new UAVSafetyZoneDialog();
            DialogResult safetyZoneResult = safetyZoneDialog.ShowDialog();
            if (safetyZoneResult == DialogResult.OK)
            {
                latSafeDis = double.Parse(safetyZoneDialog.label_Lh.Text) * 1000;
                verSafeDis = double.Parse(safetyZoneDialog.label_Lv.Text) * 1000;
            }
            else
            {
                return Result.Failed;
            }

            // Obtain the reference of the main structure
            TaskDialogResult dialogResult=TaskDialog.Show("Revit", "Please select the main structure for interference check.", TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok);
            if (dialogResult == TaskDialogResult.Cancel)
            {
                return Result.Succeeded;
            }
            Reference pickedMainStructure = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

            try
            {
                // Obtain all the camera instances
                string camFamName = "AdaptiveCameraModel";
                IList<Element> mCamInstances = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).Where(q => q.Name == camFamName).ToList();
                ICollection<ElementId> mCamInstanceIds = new List<ElementId>();

                // Popup a progress dialog
                ProgressDialog progressDialog = new ProgressDialog();
                progressDialog.Show();
                int index = 1;

                foreach(Element ele in mCamInstances)
                {
                    LocationPoint locationPoint=ele.Location as LocationPoint; ;
                    if (CVA_GenericMethods.CheckIfInaccessible(doc, pickedMainStructure, locationPoint.Point, latSafeDis, verSafeDis))
                    {
                        mCamInstanceIds.Add(ele.Id);
                    }

                    int progress = 100 * index / mCamInstances.Count();
                    index++;
                    progressDialog.UpdateProgress(progress);
                }

                if (mCamInstanceIds.Count() == 0)
                {
                    TaskDialog.Show("Revit","No intersection of the camera viewpoints was found with the selected structure !");
                }
                else
                {
                    uidoc.Selection.SetElementIds(mCamInstanceIds);
                    uidoc.ShowElements(mCamInstanceIds);
                    uidoc.RefreshActiveView();

                    int quantity = mCamInstanceIds.Count();
                    string output= "A total number of " + quantity + " camera viewpoints are detected intersection with the selected element";
                    TaskDialog.Show("Revit", output);
                }

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }

        }
    }
}
