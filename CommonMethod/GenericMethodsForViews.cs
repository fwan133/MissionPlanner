using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaptopRevitCommands.CommonMethod
{
    public static class GenericMethodsForViews
    {
        // Method 1: Read Camera Data from the Local
        public static List<DigitalCameraModel> ReadCameraData()
        {
            CheckAndCreateCameraModelFile();

            List<DigitalCameraModel> outputCameraList = new List<DigitalCameraModel>();

            string filepath = @"C:\ProgramData\Autodesk\RVT 2020\Libraries\Custom\DigitalCameraModels.xlsx";
            ExcelPackage excelPkg = new ExcelPackage(new FileInfo(filepath));
            ExcelWorksheet excelWorksheet = excelPkg.Workbook.Worksheets["CameraModels"];

            int i = 1;
            while (excelWorksheet.Cells[i, 1].Value!=null)
            {
                string name = excelWorksheet.Cells[i, 1].Value.ToString();
                double sensorSizeU=Convert.ToDouble(excelWorksheet.Cells[i, 2].Value);
                double sensorSizeV = Convert.ToDouble(excelWorksheet.Cells[i, 3].Value);
                int pixelNumU = Convert.ToInt32(excelWorksheet.Cells[i, 4].Value);
                int pixelNumV = Convert.ToInt32(excelWorksheet.Cells[i, 5].Value);
                bool fixedFocalLens = Convert.ToBoolean(excelWorksheet.Cells[i, 6].Value);
                double focalLength = Convert.ToDouble(excelWorksheet.Cells[i, 7].Value);

                outputCameraList.Add(new DigitalCameraModel(name, fixedFocalLens, focalLength, sensorSizeU, sensorSizeV, pixelNumU, pixelNumV));
                i++;
            }

            return outputCameraList;
        }

        // Method 2: Write Camera Data
        public static void WriteCameraData(List<DigitalCameraModel> mDigitalCameraModelList)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string filepath = @"C:\ProgramData\Autodesk\RVT 2020\Libraries\Custom\DigitalCameraModels.xlsx";
            ExcelPackage excelPkg = new ExcelPackage();
            ExcelWorksheet excelWorksheet = excelPkg.Workbook.Worksheets.Add("CameraModels");
            int i = 1;
            foreach(DigitalCameraModel digitalCameraModel in mDigitalCameraModelList)
            {
                excelWorksheet.Cells[i, 1].Value = digitalCameraModel.Name;
                excelWorksheet.Cells[i, 2].Value = digitalCameraModel.SensorSizeU;
                excelWorksheet.Cells[i, 3].Value = digitalCameraModel.SensorSizeV;
                excelWorksheet.Cells[i, 4].Value = digitalCameraModel.PixelNumU;
                excelWorksheet.Cells[i, 5].Value = digitalCameraModel.PixelNumV;
                excelWorksheet.Cells[i, 6].Value = digitalCameraModel.FixedFocalLens.ToString();
                excelWorksheet.Cells[i, 7].Value = digitalCameraModel.FocalLength;

                i++;
            }
            excelPkg.SaveAs(new FileInfo(filepath));
        }

        // Method 3:Check if the camera model file exists
        public static void CheckAndCreateCameraModelFile()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string filepath = @"C:\ProgramData\Autodesk\RVT 2020\Libraries\Custom\DigitalCameraModels.xlsx";
            FileInfo fileinfo = new FileInfo(filepath);
            if (!fileinfo.Exists)
            {
                ExcelPackage excelPkg = new ExcelPackage();
                ExcelWorksheet excelWorksheet = excelPkg.Workbook.Worksheets.Add("CameraModels");
                excelPkg.SaveAs(fileinfo);
            }
        }

        // Method 4: The restrictions of textbox
        public static void LimitDicimalForTextBox(TextBox textbox, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
            {
                e.Handled = true;
            }

            // "." is not allowed at the first digit
            if (e.KeyChar == (char)('.') && textbox.Text == "")
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == (char)('.') && textbox.Text.Contains('.'))
            {
                e.Handled = true;
            }

            // The first digit is 0, and the second digit has to be the decimal point
            if (e.KeyChar != (char)('.') && e.KeyChar != 8 && textbox.Text == "0")
            {
                e.Handled = true;
            }
        }
        public static void LimitDigitsForTextBox(TextBox textbox, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }

            // "." is not allowed at the first digit
            if (e.KeyChar == 48 && textbox.Text == "")
            {
                e.Handled = true;
            }
        }

    }
}
