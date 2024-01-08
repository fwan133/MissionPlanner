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
    class CreateTakeoffPoint: IExternalCommand
    {
        
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            string famName_LandingSign = "LandingSign";
            GenericMethods.CheckAndLoadFamily(doc, famName_LandingSign);
            FamilySymbol famSym_TakeoffPoint = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).Where(q => q.Name == famName_LandingSign).First() as FamilySymbol;
            try
            {
                uidoc.PromptForFamilyInstancePlacement(famSym_TakeoffPoint);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Succeeded;
            }
 
            return Result.Succeeded;

        }
    }
}
