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
    class DeleteFlightRoute : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            string famName_FlightRoute = "AdaptiveRedArrow";
            IList<Element> flightPathList = new FilteredElementCollector(doc).OfClass(typeof(Family)).Where(q => q.Name == famName_FlightRoute).ToList();

            try
            {
                if (flightPathList.Count() == 0)
                {
                    TaskDialog.Show("Revit", "There is no flight route to delete.");
                    return Result.Succeeded;
                }
                else
                {
                    IList<ElementId> idList = new List<ElementId>();
                    foreach (Element ele in flightPathList)
                    {
                        idList.Add(ele.Id);
                    }

                    TaskDialogResult dialogResult = TaskDialog.Show("Revit", "Are you sure you want to delete the current flight route?", TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok);
                    if (dialogResult == TaskDialogResult.Cancel)
                    {
                        return Result.Failed;
                    }

                    using (Transaction trans = new Transaction(doc, "load a family"))
                    {
                        trans.Start();

                        doc.Delete(idList);

                        trans.Commit();
                    }

                    return Result.Succeeded;
                }

            }
            catch (Exception)
            {
                return Result.Failed;
            }
        }
    }
}
