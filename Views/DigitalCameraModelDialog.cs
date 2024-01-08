using LaptopRevitCommands.CommonMethod;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.UI;

namespace LaptopRevitCommands.Views
{
    public partial class DigitalCameraModelDialog : Form
    {
        public DigitalCameraModelDialog()
        {
            InitializeComponent();
            UpdateCameraList();
        }

        public DigitalCameraModelDialog(int index)
        {
            InitializeComponent();
            UpdateCameraList();
            this.comboBox_CameraList.SelectedIndex = index;
        }

        private void comboBox_CameraList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCameraName = this.comboBox_CameraList.Text;
            UpdateCameraParameters(selectedCameraName);
        }

        private void textBox_SensorLength_TextChanged(object sender, EventArgs e)
        {
            UpdateWidthPixelNumber();
        }

        private void textBox_SensorWidth_TextChanged(object sender, EventArgs e)
        {
            UpdateWidthPixelNumber();
        }
        private void textBox_LengthPixelNumber_TextChanged(object sender, EventArgs e)
        {
            UpdateWidthPixelNumber();
        }

        private void checkBox_FixedFocalLens_CheckStateChanged(object sender, EventArgs e)
        {
            // update the status of the checkBox
            UpdateFocalLength();
        }
        private void button_New_Click(object sender, EventArgs e)
        {
            // Clear up all the camera parameters in current interface
            this.comboBox_CameraList.ResetText();
            this.textBox_SensorLength.ResetText();
            this.textBox_SensorWidth.ResetText();
            this.textBox_LengthPixelNumber.ResetText();
            this.textBox_WidthPixelNumber.ResetText();
            this.checkBox_FixedFocalLens.Checked=true;
            this.textBox_FocalLength.ResetText();
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            // Check if there is an existing model with the same Name
            string currentName = this.comboBox_CameraList.Text;
            if (mDigitalCameraModels.Where(q => q.Name == currentName).Count() == 1)
            {
                TaskDialog.Show("Digital Camera Model", "The name of the camera model has already existed!");
                return;
            }
            
            // Check the parameters and Save the camera models
            if (CheckValidityOfParameters())
            {
                string name = this.comboBox_CameraList.Text;
                double sensorSizeU = Convert.ToDouble(this.textBox_SensorLength.Text);
                double sensorSizeV = Convert.ToDouble(this.textBox_SensorWidth.Text);
                int pixelNumU = Convert.ToInt32(this.textBox_LengthPixelNumber.Text);
                int pixelNumV = Convert.ToInt32(this.textBox_WidthPixelNumber.Text);
                bool fixedFocalLens = Convert.ToBoolean(this.checkBox_FixedFocalLens.Checked);
                double focalLength = 0;
                if (fixedFocalLens)
                {
                    focalLength = Convert.ToDouble(this.textBox_FocalLength.Text);
                }

                this.mDigitalCameraModels.Add(new DigitalCameraModel(name, fixedFocalLens, focalLength, sensorSizeU, sensorSizeV, pixelNumU, pixelNumV));
                SaveAndUpdateCameraList(mDigitalCameraModels);
                TaskDialog.Show("Digital Camera Model", "Add Successfully!");
            }
        }

        private void button_Modify_Click(object sender, EventArgs e)
        {
            if (CheckValidityOfParameters())
            {
                this.mDigitalCameraModels.ElementAt((this.comboBox_CameraList.SelectedIndex)).Name = this.comboBox_CameraList.Text;
                this.mDigitalCameraModels.ElementAt((this.comboBox_CameraList.SelectedIndex)).SensorSizeU = Convert.ToDouble(this.textBox_SensorLength.Text);
                this.mDigitalCameraModels.ElementAt((this.comboBox_CameraList.SelectedIndex)).SensorSizeV = Convert.ToDouble(this.textBox_SensorWidth.Text);
                this.mDigitalCameraModels.ElementAt((this.comboBox_CameraList.SelectedIndex)).PixelNumU = Convert.ToInt32(this.textBox_LengthPixelNumber.Text);
                this.mDigitalCameraModels.ElementAt((this.comboBox_CameraList.SelectedIndex)).PixelNumV = Convert.ToInt32(this.textBox_WidthPixelNumber.Text);
                this.mDigitalCameraModels.ElementAt((this.comboBox_CameraList.SelectedIndex)).FixedFocalLens = Convert.ToBoolean(this.checkBox_FixedFocalLens.Checked);
                if (this.checkBox_FixedFocalLens.Checked)
                {
                    this.mDigitalCameraModels.ElementAt((this.comboBox_CameraList.SelectedIndex)).FocalLength = Convert.ToDouble(this.textBox_FocalLength.Text);
                }
                else
                {
                    this.mDigitalCameraModels.ElementAt((this.comboBox_CameraList.SelectedIndex)).FocalLength = 0;
                }
            }

            SaveAndUpdateCameraList(mDigitalCameraModels);
            TaskDialog.Show("Digital Camera Model","Modify Successfully!");
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            // Delete the selected camera model
            this.mDigitalCameraModels.RemoveAt(this.comboBox_CameraList.SelectedIndex);
            SaveAndUpdateCameraList(mDigitalCameraModels);
            TaskDialog.Show("Digital Camera Model","Delete Successfully!");        
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            // Cancel the modification of database of the camera models
            this.Close();
        }

        private void UpdateCameraList()
        {
            this.mDigitalCameraModels = GenericMethodsForViews.ReadCameraData();
            this.comboBox_CameraList.Items.Clear();
            if (mDigitalCameraModels.Count() != 0)
            {
                foreach(DigitalCameraModel digitalCameraModel in mDigitalCameraModels)
                {
                    this.comboBox_CameraList.Items.Add(digitalCameraModel.Name);
                }
            }
        }

        private void SaveAndUpdateCameraList(List<DigitalCameraModel> mDigitalCameraModels)
        {
            GenericMethodsForViews.WriteCameraData(mDigitalCameraModels);

            if (mDigitalCameraModels.Count == 0)
            {
                this.comboBox_CameraList.ResetText();
                this.textBox_SensorLength.ResetText();
                this.textBox_SensorWidth.ResetText();
                this.textBox_LengthPixelNumber.ResetText();
                this.textBox_WidthPixelNumber.ResetText();
                this.checkBox_FixedFocalLens.Checked = true;
                this.textBox_FocalLength.ResetText();
            }
            else
            {
                this.comboBox_CameraList.Items.Clear();
                foreach (DigitalCameraModel digitalCameraModel in mDigitalCameraModels)
                {
                    this.comboBox_CameraList.Items.Add(digitalCameraModel.Name);
                }
                this.comboBox_CameraList.SelectedIndex = 0;
            }
        }

        private void UpdateFocalLength()
        {
            if (this.checkBox_FixedFocalLens.Checked)
            {
                this.textBox_FocalLength.Enabled = true;
            }
            else
            {
                this.textBox_FocalLength.Enabled = false;
                this.textBox_FocalLength.ResetText();
            }
        }

        private void UpdateWidthPixelNumber()
        {
            try
            {
                if ((this.textBox_SensorLength.Text != String.Empty) & (this.textBox_SensorWidth.Text != String.Empty) & (this.textBox_LengthPixelNumber.Text != String.Empty))
                {
                    double sensorSizeU = Convert.ToDouble(this.textBox_SensorLength.Text);
                    double sensorSizeV = Convert.ToDouble(this.textBox_SensorWidth.Text);
                    int pixelNumU = Convert.ToInt32(this.textBox_LengthPixelNumber.Text);

                    int pixelNumV = (int)(pixelNumU * (sensorSizeV / sensorSizeU));
                    this.textBox_WidthPixelNumber.Text = pixelNumV.ToString();
                }
            }
            catch (Exception)
            {

            }
        }

        private void UpdateCameraParameters(string cameraName)
        {
            DigitalCameraModel digitalCameraModel = mDigitalCameraModels.Where(q => q.Name == cameraName).First();

            this.textBox_SensorLength.Text = digitalCameraModel.SensorSizeU.ToString();
            this.textBox_SensorWidth.Text = digitalCameraModel.SensorSizeV.ToString();
            this.textBox_LengthPixelNumber.Text = digitalCameraModel.PixelNumU.ToString();
            this.checkBox_FixedFocalLens.Checked = digitalCameraModel.FixedFocalLens;
            if (digitalCameraModel.FocalLength != 0)
            {
                this.textBox_FocalLength.Text = digitalCameraModel.FocalLength.ToString();
            }
        }

        private bool CheckValidityOfParameters()
        {
            try
            {
                string name = this.comboBox_CameraList.Text;
                double sensorSizeU = Convert.ToDouble(this.textBox_SensorLength.Text);
                double sensorSizeV = Convert.ToDouble(this.textBox_SensorWidth.Text);
                int pixelNumU = Convert.ToInt32(this.textBox_LengthPixelNumber.Text);
                int pixelNumV = Convert.ToInt32(this.textBox_WidthPixelNumber.Text);
                bool fixedFocalLens = Convert.ToBoolean(this.checkBox_FixedFocalLens.Checked);
                if (this.checkBox_FixedFocalLens.Checked)
                {
                    double focalLength = Convert.ToDouble(this.textBox_FocalLength.Text);
                }
                return true;
            }
            catch (Exception)
            {
                TaskDialog.Show("Digital Camera Model", "Please ensure the validity of all the input parameters.");
                return false;
            }
        }

        // Define the restrictions of textboxes
        #region
        private void textBox_SensorLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            GenericMethodsForViews.LimitDicimalForTextBox(sender as System.Windows.Forms.TextBox, e);
        }

        private void textBox_SensorWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            GenericMethodsForViews.LimitDicimalForTextBox(sender as System.Windows.Forms.TextBox, e);
        }

        private void textBox_LengthPixelNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            GenericMethodsForViews.LimitDigitsForTextBox(sender as System.Windows.Forms.TextBox, e);
        }

        private void textBox_FocalLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            GenericMethodsForViews.LimitDicimalForTextBox(sender as System.Windows.Forms.TextBox, e);
        }
        #endregion
    }
}
