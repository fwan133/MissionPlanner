
using System.Collections.Generic;

namespace LaptopRevitCommands.Views
{
    partial class ImageConfiParaDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageConfiParaDialog));
            this.tB_ForwardOverlap = new System.Windows.Forms.TextBox();
            this.cB_CameraList = new System.Windows.Forms.ComboBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_NewCamera = new System.Windows.Forms.Button();
            this.cB_MaintainOverlap = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_CheckCamera = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tB_SideOverlap = new System.Windows.Forms.TextBox();
            this.tB_GSD = new System.Windows.Forms.TextBox();
            this.label_WorkingDistance = new System.Windows.Forms.Label();
            this.pictureBox_Image = new System.Windows.Forms.PictureBox();
            this.textBox_Note = new System.Windows.Forms.TextBox();
            this.label_RDlength = new System.Windows.Forms.Label();
            this.label_RDwidth = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Image)).BeginInit();
            this.SuspendLayout();
            // 
            // tB_ForwardOverlap
            // 
            this.tB_ForwardOverlap.Location = new System.Drawing.Point(128, 93);
            this.tB_ForwardOverlap.Name = "tB_ForwardOverlap";
            this.tB_ForwardOverlap.Size = new System.Drawing.Size(106, 22);
            this.tB_ForwardOverlap.TabIndex = 2;
            this.tB_ForwardOverlap.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tB_ForwardOverlap.TextChanged += new System.EventHandler(this.tB_ForwardOverlap_TextChanged);
            this.tB_ForwardOverlap.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tB_ForwardOverlap_KeyPress);
            // 
            // cB_CameraList
            // 
            this.cB_CameraList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cB_CameraList.FormattingEnabled = true;
            this.cB_CameraList.Location = new System.Drawing.Point(18, 27);
            this.cB_CameraList.Name = "cB_CameraList";
            this.cB_CameraList.Size = new System.Drawing.Size(273, 23);
            this.cB_CameraList.TabIndex = 2;
            this.cB_CameraList.TextChanged += new System.EventHandler(this.cB_CameraList_TextChanged);
            // 
            // button_OK
            // 
            this.button_OK.Font = new System.Drawing.Font("SimSun", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_OK.Location = new System.Drawing.Point(322, 313);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 26);
            this.button_OK.TabIndex = 2;
            this.button_OK.Text = "Ok";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Font = new System.Drawing.Font("SimSun", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Cancel.Location = new System.Drawing.Point(405, 313);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 26);
            this.button_Cancel.TabIndex = 3;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_NewCamera
            // 
            this.button_NewCamera.Location = new System.Drawing.Point(380, 25);
            this.button_NewCamera.Name = "button_NewCamera";
            this.button_NewCamera.Size = new System.Drawing.Size(75, 26);
            this.button_NewCamera.TabIndex = 1;
            this.button_NewCamera.Text = "New";
            this.button_NewCamera.UseVisualStyleBackColor = true;
            this.button_NewCamera.Click += new System.EventHandler(this.button_NewCamera_Click);
            // 
            // cB_MaintainOverlap
            // 
            this.cB_MaintainOverlap.AutoSize = true;
            this.cB_MaintainOverlap.Checked = true;
            this.cB_MaintainOverlap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cB_MaintainOverlap.Location = new System.Drawing.Point(109, 70);
            this.cB_MaintainOverlap.Name = "cB_MaintainOverlap";
            this.cB_MaintainOverlap.Size = new System.Drawing.Size(124, 19);
            this.cB_MaintainOverlap.TabIndex = 1;
            this.cB_MaintainOverlap.Text = "Maintain the same";
            this.cB_MaintainOverlap.UseVisualStyleBackColor = true;
            this.cB_MaintainOverlap.CheckedChanged += new System.EventHandler(this.cB_MaintainOverlap_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_CheckCamera);
            this.groupBox1.Controls.Add(this.cB_CameraList);
            this.groupBox1.Controls.Add(this.button_NewCamera);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(18, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(462, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Camera";
            // 
            // button_CheckCamera
            // 
            this.button_CheckCamera.Location = new System.Drawing.Point(298, 25);
            this.button_CheckCamera.Name = "button_CheckCamera";
            this.button_CheckCamera.Size = new System.Drawing.Size(75, 26);
            this.button_CheckCamera.TabIndex = 0;
            this.button_CheckCamera.Text = "View";
            this.button_CheckCamera.UseVisualStyleBackColor = true;
            this.button_CheckCamera.Click += new System.EventHandler(this.button_CheckCamera_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.tB_SideOverlap);
            this.groupBox2.Controls.Add(this.tB_GSD);
            this.groupBox2.Controls.Add(this.cB_MaintainOverlap);
            this.groupBox2.Controls.Add(this.tB_ForwardOverlap);
            this.groupBox2.Location = new System.Drawing.Point(18, 93);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(255, 160);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Image Configuration Requirements";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.Control;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox4.Location = new System.Drawing.Point(61, 124);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(62, 15);
            this.textBox4.TabIndex = 25;
            this.textBox4.Text = "Side:";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Control;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox3.Location = new System.Drawing.Point(61, 97);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(62, 15);
            this.textBox3.TabIndex = 24;
            this.textBox3.Text = "Forward:";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Control;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox2.Location = new System.Drawing.Point(18, 73);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(84, 16);
            this.textBox2.TabIndex = 23;
            this.textBox2.Text = "Overlap(%):";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox1.Location = new System.Drawing.Point(18, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(96, 16);
            this.textBox1.TabIndex = 22;
            this.textBox1.Text = "GSD (mm/pixel):";
            // 
            // tB_SideOverlap
            // 
            this.tB_SideOverlap.Enabled = false;
            this.tB_SideOverlap.Location = new System.Drawing.Point(128, 120);
            this.tB_SideOverlap.Name = "tB_SideOverlap";
            this.tB_SideOverlap.Size = new System.Drawing.Size(106, 22);
            this.tB_SideOverlap.TabIndex = 3;
            this.tB_SideOverlap.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tB_SideOverlap.TextChanged += new System.EventHandler(this.tB_SideOverlap_TextChanged);
            this.tB_SideOverlap.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tB_SideOverlap_KeyPress);
            // 
            // tB_GSD
            // 
            this.tB_GSD.Location = new System.Drawing.Point(130, 35);
            this.tB_GSD.Name = "tB_GSD";
            this.tB_GSD.Size = new System.Drawing.Size(106, 22);
            this.tB_GSD.TabIndex = 0;
            this.tB_GSD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tB_GSD.TextChanged += new System.EventHandler(this.tB_GSD_TextChanged);
            this.tB_GSD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tB_GSD_KeyPress);
            // 
            // label_WorkingDistance
            // 
            this.label_WorkingDistance.AutoSize = true;
            this.label_WorkingDistance.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_WorkingDistance.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label_WorkingDistance.Location = new System.Drawing.Point(233, 257);
            this.label_WorkingDistance.Name = "label_WorkingDistance";
            this.label_WorkingDistance.Size = new System.Drawing.Size(40, 17);
            this.label_WorkingDistance.TabIndex = 15;
            this.label_WorkingDistance.Text = "0.000";
            this.label_WorkingDistance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox_Image
            // 
            this.pictureBox_Image.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Image.Image")));
            this.pictureBox_Image.InitialImage = global::LaptopRevitCommands.Properties.Resources.ImageConfigurationDiagram;
            this.pictureBox_Image.Location = new System.Drawing.Point(280, 93);
            this.pictureBox_Image.Name = "pictureBox_Image";
            this.pictureBox_Image.Size = new System.Drawing.Size(200, 160);
            this.pictureBox_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Image.TabIndex = 17;
            this.pictureBox_Image.TabStop = false;
            // 
            // textBox_Note
            // 
            this.textBox_Note.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_Note.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_Note.Location = new System.Drawing.Point(17, 260);
            this.textBox_Note.Multiline = true;
            this.textBox_Note.Name = "textBox_Note";
            this.textBox_Note.Size = new System.Drawing.Size(461, 51);
            this.textBox_Note.TabIndex = 19;
            this.textBox_Note.Text = "The working distance is predicted to be               m. \r\nThe relative distances" +
    " between consecutive images are predicted to be               m in length and   " +
    "            m in width, respectively.";
            // 
            // label_RDlength
            // 
            this.label_RDlength.AutoSize = true;
            this.label_RDlength.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_RDlength.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label_RDlength.Location = new System.Drawing.Point(398, 273);
            this.label_RDlength.Name = "label_RDlength";
            this.label_RDlength.Size = new System.Drawing.Size(40, 17);
            this.label_RDlength.TabIndex = 20;
            this.label_RDlength.Text = "0.000";
            this.label_RDlength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_RDwidth
            // 
            this.label_RDwidth.AutoSize = true;
            this.label_RDwidth.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_RDwidth.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label_RDwidth.Location = new System.Drawing.Point(80, 288);
            this.label_RDwidth.Name = "label_RDwidth";
            this.label_RDwidth.Size = new System.Drawing.Size(40, 17);
            this.label_RDwidth.TabIndex = 21;
            this.label_RDwidth.Text = "0.000";
            this.label_RDwidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ImageConfiParaDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(502, 360);
            this.Controls.Add(this.label_RDwidth);
            this.Controls.Add(this.label_RDlength);
            this.Controls.Add(this.label_WorkingDistance);
            this.Controls.Add(this.textBox_Note);
            this.Controls.Add(this.pictureBox_Image);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImageConfiParaDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Image Configuration Parameters";
            this.Load += new System.EventHandler(this.ImageConfigurationDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Image)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.TextBox tB_ForwardOverlap;
        public System.Windows.Forms.ComboBox cB_CameraList;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_NewCamera;
        private System.Windows.Forms.CheckBox cB_MaintainOverlap;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.TextBox tB_GSD;
        private System.Windows.Forms.Button button_CheckCamera;
        public System.Windows.Forms.TextBox tB_SideOverlap;
        private System.Windows.Forms.Label label_WorkingDistance;
        private System.Windows.Forms.PictureBox pictureBox_Image;
        private System.Windows.Forms.TextBox textBox_Note;
        private System.Windows.Forms.Label label_RDlength;
        private System.Windows.Forms.Label label_RDwidth;

        public List<DigitalCameraModel> mDigitalCameraModels;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
    }
}