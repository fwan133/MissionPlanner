using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaptopRevitCommands.Views
{
    public partial class ProgressDialog : Form
    {
        public ProgressDialog()
        {
            InitializeComponent();
            this.progressBar_Main.Minimum = 0;
            this.progressBar_Main.Maximum = 100;
        }

        public void UpdateProgress(int value)
        {
            if (value < progressBar_Main.Maximum)
            {
                progressBar_Main.Value = value;
                textBox_Progress.Text = value + " %";
            }
            else
            {
                this.Close();
            }
            Application.DoEvents();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }
    }
}
