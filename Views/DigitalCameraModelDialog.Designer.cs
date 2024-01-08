
using System.Collections.Generic;

namespace LaptopRevitCommands.Views
{
    partial class DigitalCameraModelDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DigitalCameraModelDialog));
            this.button_Modify = new System.Windows.Forms.Button();
            this.button_Delete = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_WidthPixelNumber = new System.Windows.Forms.TextBox();
            this.textBox_LengthPixelNumber = new System.Windows.Forms.TextBox();
            this.textBox_SensorWidth = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_SensorLength = new System.Windows.Forms.TextBox();
            this.comboBox_CameraList = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_FocalLength = new System.Windows.Forms.TextBox();
            this.checkBox_FixedFocalLens = new System.Windows.Forms.CheckBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button_Add = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_New = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Modify
            // 
            this.button_Modify.Font = new System.Drawing.Font("SimSun", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Modify.Location = new System.Drawing.Point(290, 200);
            this.button_Modify.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_Modify.Name = "button_Modify";
            this.button_Modify.Size = new System.Drawing.Size(70, 29);
            this.button_Modify.TabIndex = 4;
            this.button_Modify.Text = "Modify";
            this.button_Modify.UseVisualStyleBackColor = true;
            this.button_Modify.Click += new System.EventHandler(this.button_Modify_Click);
            // 
            // button_Delete
            // 
            this.button_Delete.Font = new System.Drawing.Font("SimSun", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Delete.Location = new System.Drawing.Point(365, 200);
            this.button_Delete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(70, 29);
            this.button_Delete.TabIndex = 5;
            this.button_Delete.Text = "Delete";
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(253, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "(mm)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sensor Size (mm):";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox_WidthPixelNumber);
            this.groupBox1.Controls.Add(this.textBox_LengthPixelNumber);
            this.groupBox1.Controls.Add(this.textBox_SensorWidth);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_SensorLength);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(211, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 85);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sensor";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(205, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "×";
            // 
            // textBox_WidthPixelNumber
            // 
            this.textBox_WidthPixelNumber.Enabled = false;
            this.textBox_WidthPixelNumber.Location = new System.Drawing.Point(222, 50);
            this.textBox_WidthPixelNumber.Name = "textBox_WidthPixelNumber";
            this.textBox_WidthPixelNumber.Size = new System.Drawing.Size(70, 22);
            this.textBox_WidthPixelNumber.TabIndex = 3;
            this.textBox_WidthPixelNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_LengthPixelNumber
            // 
            this.textBox_LengthPixelNumber.Location = new System.Drawing.Point(129, 50);
            this.textBox_LengthPixelNumber.Name = "textBox_LengthPixelNumber";
            this.textBox_LengthPixelNumber.Size = new System.Drawing.Size(70, 22);
            this.textBox_LengthPixelNumber.TabIndex = 2;
            this.textBox_LengthPixelNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_LengthPixelNumber.TextChanged += new System.EventHandler(this.textBox_LengthPixelNumber_TextChanged);
            this.textBox_LengthPixelNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_LengthPixelNumber_KeyPress);
            // 
            // textBox_SensorWidth
            // 
            this.textBox_SensorWidth.Location = new System.Drawing.Point(222, 22);
            this.textBox_SensorWidth.Name = "textBox_SensorWidth";
            this.textBox_SensorWidth.Size = new System.Drawing.Size(70, 22);
            this.textBox_SensorWidth.TabIndex = 1;
            this.textBox_SensorWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_SensorWidth.TextChanged += new System.EventHandler(this.textBox_SensorWidth_TextChanged);
            this.textBox_SensorWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_SensorWidth_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(205, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "×";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Number of Pixels:";
            // 
            // textBox_SensorLength
            // 
            this.textBox_SensorLength.Location = new System.Drawing.Point(129, 22);
            this.textBox_SensorLength.Name = "textBox_SensorLength";
            this.textBox_SensorLength.Size = new System.Drawing.Size(70, 22);
            this.textBox_SensorLength.TabIndex = 0;
            this.textBox_SensorLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_SensorLength.TextChanged += new System.EventHandler(this.textBox_SensorLength_TextChanged);
            this.textBox_SensorLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_SensorLength_KeyPress);
            // 
            // comboBox_CameraList
            // 
            this.comboBox_CameraList.FormattingEnabled = true;
            this.comboBox_CameraList.Location = new System.Drawing.Point(69, 13);
            this.comboBox_CameraList.Name = "comboBox_CameraList";
            this.comboBox_CameraList.Size = new System.Drawing.Size(365, 23);
            this.comboBox_CameraList.TabIndex = 8;
            this.comboBox_CameraList.SelectedIndexChanged += new System.EventHandler(this.comboBox_CameraList_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_FocalLength);
            this.groupBox2.Controls.Add(this.checkBox_FixedFocalLens);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(211, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(299, 60);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Lens";
            // 
            // textBox_FocalLength
            // 
            this.textBox_FocalLength.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_FocalLength.Location = new System.Drawing.Point(168, 24);
            this.textBox_FocalLength.Name = "textBox_FocalLength";
            this.textBox_FocalLength.Size = new System.Drawing.Size(79, 22);
            this.textBox_FocalLength.TabIndex = 0;
            this.textBox_FocalLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_FocalLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_FocalLength_KeyPress);
            // 
            // checkBox_FixedFocalLens
            // 
            this.checkBox_FixedFocalLens.AutoSize = true;
            this.checkBox_FixedFocalLens.Checked = true;
            this.checkBox_FixedFocalLens.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_FixedFocalLens.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_FixedFocalLens.Location = new System.Drawing.Point(22, 25);
            this.checkBox_FixedFocalLens.Name = "checkBox_FixedFocalLens";
            this.checkBox_FixedFocalLens.Size = new System.Drawing.Size(127, 19);
            this.checkBox_FixedFocalLens.TabIndex = 0;
            this.checkBox_FixedFocalLens.Text = "Fixed Focal Length";
            this.checkBox_FixedFocalLens.UseVisualStyleBackColor = true;
            this.checkBox_FixedFocalLens.CheckStateChanged += new System.EventHandler(this.checkBox_FixedFocalLens_CheckStateChanged);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Font = new System.Drawing.Font("SimSun", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Cancel.Location = new System.Drawing.Point(440, 200);
            this.button_Cancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(70, 29);
            this.button_Cancel.TabIndex = 6;
            this.button_Cancel.Text = "Close";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "Name:";
            // 
            // button_Add
            // 
            this.button_Add.Font = new System.Drawing.Font("SimSun", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Add.Location = new System.Drawing.Point(215, 200);
            this.button_Add.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(70, 29);
            this.button_Add.TabIndex = 3;
            this.button_Add.Text = "Add";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LaptopRevitCommands.Properties.Resources.PinholeCameraModel;
            this.pictureBox1.InitialImage = global::LaptopRevitCommands.Properties.Resources.PinholeCameraModel;
            this.pictureBox1.Location = new System.Drawing.Point(22, 46);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(183, 145);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // button_New
            // 
            this.button_New.Location = new System.Drawing.Point(440, 12);
            this.button_New.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_New.Name = "button_New";
            this.button_New.Size = new System.Drawing.Size(70, 25);
            this.button_New.TabIndex = 0;
            this.button_New.Text = "New";
            this.button_New.UseVisualStyleBackColor = true;
            this.button_New.Click += new System.EventHandler(this.button_New_Click);
            // 
            // DigitalCameraModelDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(536, 246);
            this.Controls.Add(this.button_New);
            this.Controls.Add(this.button_Add);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.comboBox_CameraList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_Delete);
            this.Controls.Add(this.button_Modify);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DigitalCameraModelDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Digital Camera Model";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_Modify;
        private System.Windows.Forms.Button button_Delete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_SensorWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_SensorLength;
        private System.Windows.Forms.ComboBox comboBox_CameraList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_WidthPixelNumber;
        private System.Windows.Forms.TextBox textBox_LengthPixelNumber;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_FocalLength;
        private System.Windows.Forms.CheckBox checkBox_FixedFocalLens;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.Button button_New;

        private List<DigitalCameraModel> mDigitalCameraModels;
    }
}