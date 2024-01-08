using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;

namespace LaptopRevitCommands
{
    public static class ExtensibleStorage
    {
        // Method 1: Judge if the target scheme exists
        public static Schema SchemaExist(String schemaName)
        {
            Schema schema = null;
            IList<Schema> schemas = Schema.ListSchemas();

            if (schemas != null && schemas.Count > 0)
            {
                // get schema
                foreach (Schema s in schemas)
                {
                    if (s.SchemaName == schemaName)
                    {
                        schema = s;
                        break;
                    }
                }
            }
            return schema;
        }

        // Method 2 Schema for DividedSurface
        public static Schema CreateSchemaForDividedSurface(Document doc)
        {
            using (Transaction trans = new Transaction(doc, "Create a Schema for divided surface."))
            {
                trans.Start();

                Guid schemaGUID = Guid.NewGuid();

                SchemaBuilder schemaBuilder = new SchemaBuilder(schemaGUID);
                schemaBuilder.SetReadAccessLevel(AccessLevel.Public);
                schemaBuilder.SetWriteAccessLevel(AccessLevel.Public);
                schemaBuilder.SetSchemaName("DividedSurfaceSchema");
                schemaBuilder.SetDocumentation("Storage of extensible data of a divided face");

                FieldBuilder fieldBuilder1 = schemaBuilder.AddSimpleField("WorkingDistance", typeof(double));
                fieldBuilder1.SetUnitType(UnitType.UT_Length);
                FieldBuilder fieldBuilder2 = schemaBuilder.AddSimpleField("CameraModelName", typeof(string));

                Schema mSchema = schemaBuilder.Finish();

                trans.Commit();

                return mSchema;
            }

        }

        public static void AddEntityForDividedSurfaceSchema(Document doc, DividedSurface mDividedSurface, Schema mSchema, double workingDistance, string cameraModelName)
        {
            using (Transaction trans = new Transaction(doc, "Add an entity to the divided surface schema."))
            {
                trans.Start();

                Entity entity = new Entity(mSchema);

                Field fieldWorkingDistance = mSchema.GetField("WorkingDistance");
                entity.Set<double>(fieldWorkingDistance, workingDistance, DisplayUnitType.DUT_DECIMAL_FEET);

                Field fieldCameraModelName = mSchema.GetField("CameraModelName");
                entity.Set<string>(fieldCameraModelName, cameraModelName);

                mDividedSurface.SetEntity(entity);

                trans.Commit();
            }
        }

        // Method 3: Schema for divided path
        public static Schema CreateSchemaForDividedPath(Document doc)
        {
            using (Transaction trans = new Transaction(doc, "Create a Schema for divided path."))
            {
                trans.Start();

                Guid schemaGUID = Guid.NewGuid();

                SchemaBuilder schemaBuilder = new SchemaBuilder(schemaGUID);
                schemaBuilder.SetReadAccessLevel(AccessLevel.Public);
                schemaBuilder.SetWriteAccessLevel(AccessLevel.Public);
                schemaBuilder.SetSchemaName("DividedPathSchema");
                schemaBuilder.SetDocumentation("Storage of extensible data of a divided path");

                FieldBuilder fieldBuilder1 = schemaBuilder.AddSimpleField("WorkingDistance", typeof(double));
                fieldBuilder1.SetUnitType(UnitType.UT_Length);
                FieldBuilder fieldBuilder2 = schemaBuilder.AddSimpleField("CameraModelName", typeof(string));

                Schema mSchema = schemaBuilder.Finish();

                trans.Commit();

                return mSchema;
            }
        }

        public static void AddEntityForDividedPathSchema(Document doc, DividedPath mDividedPath, Schema mSchema, double workingDistance, string cameraModelName)
        {
            using (Transaction trans = new Transaction(doc, "Add an entity to the divided surface schema."))
            {
                trans.Start();

                Entity entity = new Entity(mSchema);

                Field fieldWorkingDistance = mSchema.GetField("WorkingDistance");
                entity.Set<double>(fieldWorkingDistance, workingDistance, DisplayUnitType.DUT_DECIMAL_FEET);

                Field fieldCameraModelName = mSchema.GetField("CameraModelName");
                entity.Set<string>(fieldCameraModelName, cameraModelName);

                mDividedPath.SetEntity(entity);

                trans.Commit();
            }
        }

        // Method 4: Schema for Camera Instance
        public static Schema CreateSchemaForCameraInstance(Document doc)
        {
            using (Transaction trans = new Transaction(doc, "Create a Schema for camera instance."))
            {
                trans.Start();

                Guid schemaGUID = Guid.NewGuid();

                SchemaBuilder schemaBuilder = new SchemaBuilder(schemaGUID);
                schemaBuilder.SetReadAccessLevel(AccessLevel.Public);
                schemaBuilder.SetWriteAccessLevel(AccessLevel.Public);
                schemaBuilder.SetSchemaName("CameraInstanceSchema");
                schemaBuilder.SetDocumentation("Storage of extensible data of an instance of a camera");

                FieldBuilder fieldBuilder1 = schemaBuilder.AddSimpleField("CameraPosition", typeof(XYZ));
                fieldBuilder1.SetUnitType(UnitType.UT_Length);
                FieldBuilder fieldBuilder2 = schemaBuilder.AddSimpleField("CameraRotation", typeof(XYZ));
                fieldBuilder2.SetUnitType(UnitType.UT_Angle);
                FieldBuilder fieldBuilder3 = schemaBuilder.AddSimpleField("CameraModelName", typeof(string));

                Schema mSchema = schemaBuilder.Finish();

                trans.Commit();

                return mSchema;
            }

        }

        public static void AddEntityForCameraInstance(Document doc, FamilyInstance mCameraInstance, Schema mSchema, XYZ camPos, XYZ camRot, string cameraModelName)
        {
            using (Transaction trans = new Transaction(doc, "Add an entity to the divided surface schema."))
            {
                trans.Start();

                Entity entity = new Entity(mSchema);

                Field fieldCameraPosition = mSchema.GetField("CameraPosition");
                entity.Set<XYZ>(fieldCameraPosition, camPos, DisplayUnitType.DUT_DECIMAL_FEET);

                Field fieldCameraRotation = mSchema.GetField("CameraRotation");
                entity.Set<XYZ>(fieldCameraRotation, camRot, DisplayUnitType.DUT_DECIMAL_DEGREES);

                Field fieldCameraModelName = mSchema.GetField("CameraModelName");
                entity.Set<string>(fieldCameraModelName, cameraModelName);

                mCameraInstance.SetEntity(entity);

                trans.Commit();
            }
        }

    }
}
