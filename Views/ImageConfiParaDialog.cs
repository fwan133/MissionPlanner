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
    public partial class ImageConfiParaDialog : Form
    {
        public ImageConfiParaDialog()
        {
            InitializeComponent();
        }

        private void ImageConfigurationDialog_Load(object sender, EventArgs e)
        {
            // Update the camera model list
            UpdateCameraList();
        }

        private void cB_CameraList_TextChanged(object sender, EventArgs e)
        {
            UpdateDistances();
        }
        private void button_CheckCamera_Click(object sender, EventArgs e)
        {
            // Pop up a new windows to display the parameters of the camera
            int index = this.cB_CameraList.SelectedIndex;
            DigitalCameraModelDialog mDigitalCameraDialog = new DigitalCameraModelDialog(index);
            mDigitalCameraDialog.ShowDialog();
            UpdateCameraList();
        }

        private void button_NewCamera_Click(object sender, EventArgs e)
        {
            // Pop up a new windows to create a new camera
            DigitalCameraModelDialog mDigitalCameraDialog = new DigitalCameraModelDialog();
            mDigitalCameraDialog.ShowDialog();
            UpdateCameraList();
        }

        private void tB_GSD_TextChanged(object sender, EventArgs e)
        {
            // Update the working distance and relative distances to match the inputed GSD
            UpdateDistances();
        }

        private void tB_ForwardOverlap_TextChanged(object sender, EventArgs e)
        {
            if (this.cB_MaintainOverlap.Checked)
            {
                this.tB_SideOverlap.Text = this.tB_ForwardOverlap.Text;
            }
            // Update the working distance and relative distances to match the inputed overlap
            UpdateDistances();
        }

        private void tB_SideOverlap_TextChanged(object sender, EventArgs e)
        {
            // Update the working distance and relative distances to match the inputed overlap
            UpdateDistances();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            // Return the inputed image configuration parameters
            if (CheckParameters())
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                TaskDialog.Show("Image Configuration Parameters","Please input image configutation parameters.");
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            // Cancel the defination of image configuration parameters
            this.DialogResult = DialogResult.Cancel;
        }

        private void UpdateCameraList()
        {
            this.mDigitalCameraModels = GenericMethodsForViews.ReadCameraData();
            this.cB_CameraList.Items.Clear();
            if (mDigitalCameraModels.Count() != 0)
            {
                foreach (DigitalCameraModel digitalCameraModel in mDigitalCameraModels)
                {
                    this.cB_CameraList.Items.Add(digitalCameraModel.Name);
                }
                this.cB_CameraList.SelectedIndex = 0;
            }
        }

        // Restrict the input validation of textbox
        #region
        private void tB_GSD_KeyPress(object sender, KeyPressEventArgs e)
        {
            GenericMethodsForViews.LimitDicimalForTextBox(sender as System.Windows.Forms.TextBox, e);
        }

        private void tB_ForwardOverlap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }

            // "0" is not allowed at the first digit
            if (e.KeyChar == 48 && (sender as System.Windows.Forms.TextBox).Text == "")
            {
                e.Handled = true;
            }

            // two digits are allowed for maximum
            if (e.KeyChar != (char)Keys.Back && (sender as System.Windows.Forms.TextBox).Text.Length==2)
            {
                e.Handled = true;
            }
        }

        private void tB_SideOverlap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }

            // "0" is not allowed at the first digit
            if (e.KeyChar == 48 && (sender as System.Windows.Forms.TextBox).Text == "")
            {
                e.Handled = true;
            }

            // two digits are allowed for maximum
            if (e.KeyChar != (char)Keys.Back && (sender as System.Windows.Forms.TextBox).Text.Length == 2)
            {
                e.Handled = true;
            }
        }
        #endregion // Limit the input of textBox

        private void cB_MaintainOverlap_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cB_MaintainOverlap.Checked)
            {
                this.tB_SideOverlap.Enabled = false;
                this.tB_SideOverlap.Text = this.tB_ForwardOverlap.Text;
            }
            else
            {
                this.tB_SideOverlap.Enabled = true;
            }
        }

        private void UpdateDistances()
        {
            try
            {
                double GSD = Convert.ToDouble(this.tB_GSD.Text);
                double forwardOverlap = (double)Convert.ToDouble(this.tB_ForwardOverlap.Text) / 100;
                double sideOverlap = (double)Convert.ToDouble(this.tB_SideOverlap.Text) / 100;

                DigitalCameraModel digitalCameraModel=mDigitalCameraModels.Where(q => q.Name == this.cB_CameraList.Text).First();

                this.label_WorkingDistance.Text = Math.Round(digitalCameraModel.ObtainWorkingDistance(GSD)/1000,3).ToString();
                this.label_RDlength.Text = Math.Round(digitalCameraModel.ObtainRelativeDistanceLength(GSD, sideOverlap)/1000,3).ToString();
                this.label_RDwidth.Text = Math.Round(digitalCameraModel.ObtainRelativeDistanceWidth(GSD, forwardOverlap)/1000,3).ToString();
            }
            catch(Exception)
            {

            }

        }

        private bool CheckParameters()
        {
            if(this.cB_CameraList.Text.Length!=0 && this.tB_GSD.Text.Length!=0 && this.tB_ForwardOverlap.Text.Length!=0 && this.tB_SideOverlap.Text.Length != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
