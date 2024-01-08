using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace LaptopRevitCommands.Commands.CVA
{
    static class CVA_GenericMethods
    {
        public static bool CheckIfInaccessible(Document doc, Reference reference, XYZ position, double latDis, double verDis)
        {
            string familyName = "DroneSafeZone";
            GenericMethods.CheckAndLoadFamily(doc, familyName);
            bool result = false;
            FamilyInstance mFamIns;

            using (Transaction trans = new Transaction(doc, "create a temporary family instance"))
            {
                trans.Start();

                // Create a temporary family instance
                FamilySymbol famSym = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).Where(q => q.Name == familyName).First() as FamilySymbol;
                famSym.Activate();
                if (doc.IsFamilyDocument)
                {
                    mFamIns = doc.FamilyCreate.NewFamilyInstance(position, famSym, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                }
                else
                {
                    mFamIns = doc.Create.NewFamilyInstance(position, famSym, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                }
                mFamIns.LookupParameter("LatSafeDistance").Set(GenericMethods.MM2Ft(latDis));
                mFamIns.LookupParameter("VerSafeDistance").Set(GenericMethods.MM2Ft(verDis));

                trans.Commit();
            }

            using (Transaction trans = new Transaction(doc, "create a temporary family instance"))
            {
                trans.Start();

                // Check the clash                
                FilteredElementCollector insectElements = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).WherePasses(new ElementIntersectsElementFilter(mFamIns));
                int a = insectElements.Count();
                if (insectElements.Count() != 0)
                {
                    foreach (Element ele in insectElements)
                    {
                        if (ele.Id == reference.ElementId)
                        {
                            result = true;
                            break;
                        }
                    }
                }

                // Delete the temporary element
                doc.Delete(mFamIns.Id);

                trans.Commit();
            }

            return result;
        }

        // Method 2: Obtain position of a camera instance
        public static XYZ ObtainCameraPosition(FamilyInstance mCamIns)
        {
            LocationPoint locPoint = (LocationPoint)mCamIns.Location;
            return locPoint.Point;
        }

        // Method 3: Obtain the target point of the camera instance
        public static XYZ ObtainTargetPoint(FamilyInstance mCamIns)
        {
            double workingDistance = mCamIns.LookupParameter("WorkingDistance").AsDouble();
            Transform camTransform = mCamIns.GetTransform();
            XYZ position = camTransform.Origin;
            XYZ orientation = camTransform.BasisX;
            XYZ targetPoint = position.Add(orientation * workingDistance);

            return targetPoint;
        }
    }
}
