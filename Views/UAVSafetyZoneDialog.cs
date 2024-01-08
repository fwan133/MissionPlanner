using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LaptopRevitCommands.CommonMethod;
using Autodesk.Revit.UI;

namespace LaptopRevitCommands.Views
{
    public partial class UAVSafetyZoneDialog : Form
    {
        public UAVSafetyZoneDialog()
        {
            InitializeComponent();
        }

        #region
        private void textBox_Lhp_KeyPress(object sender, KeyPressEventArgs e)
        {
            GenericMethodsForViews.LimitDicimalForTextBox(sender as System.Windows.Forms.TextBox, e);
        }

        private void textBox_Lhw_KeyPress(object sender, KeyPressEventArgs e)
        {
            GenericMethodsForViews.LimitDicimalForTextBox(sender as System.Windows.Forms.TextBox, e);
        }

        private void textBox_Lhm_KeyPress(object sender, KeyPressEventArgs e)
        {
            GenericMethodsForViews.LimitDicimalForTextBox(sender as System.Windows.Forms.TextBox, e);
        }

        private void textBox_Lhe_KeyPress(object sender, KeyPressEventArgs e)
        {
            GenericMethodsForViews.LimitDicimalForTextBox(sender as System.Windows.Forms.TextBox, e);
        }

        private void textBox_Lvp_KeyPress(object sender, KeyPressEventArgs e)
        {
            GenericMethodsForViews.LimitDicimalForTextBox(sender as System.Windows.Forms.TextBox, e);
        }

        private void textBox_Lvw_KeyPress(object sender, KeyPressEventArgs e)
        {
            GenericMethodsForViews.LimitDicimalForTextBox(sender as System.Windows.Forms.TextBox, e);
        }

        private void textBox_Lvm_KeyPress(object sender, KeyPressEventArgs e)
        {
            GenericMethodsForViews.LimitDicimalForTextBox(sender as System.Windows.Forms.TextBox, e);
        }

        private void textBox_Lve_KeyPress(object sender, KeyPressEventArgs e)
        {
            GenericMethodsForViews.LimitDicimalForTextBox(sender as System.Windows.Forms.TextBox, e);
        }

        #endregion

        #region
        private void updateLv()
        {
            try
            {
                double Lvp= Double.Parse(textBox_Lvp.Text);
                double Lvw = Double.Parse(textBox_Lvw.Text);
                double Lvm = Double.Parse(textBox_Lvm.Text);
                double Lve = Double.Parse(textBox_Lve.Text);

                double Lv = Lvp + Lvw + Lvm + Lve;
                label_Lv.Text = Math.Round(Lv, 2).ToString();
            }
            catch(Exception)
            {
                label_Lv.Text = "";
            }
        }

        private void updateLh()
        {
            try
            {
                double Lhp = Double.Parse(textBox_Lhp.Text);
                double Lhw = Double.Parse(textBox_Lhw.Text);
                double Lhm = Double.Parse(textBox_Lhm.Text);
                double Lhe = Double.Parse(textBox_Lhe.Text);

                double Lh = Lhp + Lhw + Lhm + Lhe;
                label_Lh.Text = Math.Round(Lh, 2).ToString();
            }
            catch (Exception)
            {
                label_Lh.Text = "";
            }
        }

        private void textBox_Lhp_TextChanged(object sender, EventArgs e)
        {
            updateLv();
            updateLh();
        }

        private void textBox_Lhw_TextChanged(object sender, EventArgs e)
        {
            updateLv();
            updateLh();
        }

        private void textBox_Lhm_TextChanged(object sender, EventArgs e)
        {
            updateLv();
            updateLh();
        }

        private void textBox_Lhe_TextChanged(object sender, EventArgs e)
        {
            updateLv();
            updateLh();
        }

        private void textBox_Lvp_TextChanged(object sender, EventArgs e)
        {
            updateLv();
            updateLh();
        }

        private void textBox_Lvw_TextChanged(object sender, EventArgs e)
        {
            updateLv();
            updateLh();
        }

        private void textBox_Lvm_TextChanged(object sender, EventArgs e)
        {
            updateLv();
            updateLh();
        }

        private void textBox_Lve_TextChanged(object sender, EventArgs e)
        {
            updateLv();
            updateLh();
        }

        #endregion

        private void button_Ok_Click(object sender, EventArgs e)
        {
            if (label_Lh.Text == "" || label_Lv.Text == "")
            {
                TaskDialog.Show("Error", "Incomplete input.");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
