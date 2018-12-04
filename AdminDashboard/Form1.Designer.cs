namespace AdminDashboard
{
    partial class Form1
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
            this.label2 = new System.Windows.Forms.Label();
            this.showText = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxInformation = new System.Windows.Forms.ComboBox();
            this.buttonInformation = new System.Windows.Forms.Button();
            this.parkLabel = new System.Windows.Forms.Label();
            this.spotLabel = new System.Windows.Forms.Label();
            this.parkIdTextBox = new System.Windows.Forms.TextBox();
            this.spotIdTextBox = new System.Windows.Forms.TextBox();
            this.parkLabelFormat = new System.Windows.Forms.Label();
            this.spotLabelFormat = new System.Windows.Forms.Label();
            this.inicialDate = new System.Windows.Forms.DateTimePicker();
            this.inicialDateGroupBox = new System.Windows.Forms.GroupBox();
            this.inicialTime = new System.Windows.Forms.DateTimePicker();
            this.finalDateGroupBox = new System.Windows.Forms.GroupBox();
            this.finalTime = new System.Windows.Forms.DateTimePicker();
            this.finalDate = new System.Windows.Forms.DateTimePicker();
            this.inicialDateGroupBox.SuspendLayout();
            this.finalDateGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(253, 9);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(5);
            this.label2.Size = new System.Drawing.Size(294, 43);
            this.label2.TabIndex = 5;
            this.label2.Text = "ADMIN DASHBOARD";
            // 
            // showText
            // 
            this.showText.BackColor = System.Drawing.SystemColors.MenuBar;
            this.showText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.showText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showText.Location = new System.Drawing.Point(21, 267);
            this.showText.Name = "showText";
            this.showText.Size = new System.Drawing.Size(681, 367);
            this.showText.TabIndex = 6;
            this.showText.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(17, 240);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 24);
            this.label1.TabIndex = 7;
            this.label1.Text = "Information:";
            // 
            // comboBoxInformation
            // 
            this.comboBoxInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxInformation.FormattingEnabled = true;
            this.comboBoxInformation.Items.AddRange(new object[] {
            "1. List of available parks in the platform;",
            "2. Status of all parking spots in a specific park for a given moment;",
            "3. List of status of all parking spots in a specific park for a given time period" +
                ";",
            "4. List of free parking spots from a specific park for a given moment;",
            "5. List of parking spots belonging to a specific park;",
            "6. Detailed information about a specific park;",
            "7. Detailed information about a specific parking spot in a given moment (should a" +
                "lso indicate if the spot is free or occupied);",
            "8. List of parking spots sensors that need to be replaced because of its critical" +
                " battery level, within the overall platform;",
            "9. List of parking spots sensors that need to be replaced for a specific park;",
            "10. Instant occupancy rate in a specific park;"});
            this.comboBoxInformation.Location = new System.Drawing.Point(21, 69);
            this.comboBoxInformation.Name = "comboBoxInformation";
            this.comboBoxInformation.Size = new System.Drawing.Size(558, 24);
            this.comboBoxInformation.TabIndex = 11;
            this.comboBoxInformation.Tag = "";
            this.comboBoxInformation.SelectedIndexChanged += new System.EventHandler(this.comboBoxInformation_SelectedIndexChanged);
            // 
            // buttonInformation
            // 
            this.buttonInformation.BackColor = System.Drawing.SystemColors.Highlight;
            this.buttonInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonInformation.Location = new System.Drawing.Point(585, 59);
            this.buttonInformation.Name = "buttonInformation";
            this.buttonInformation.Size = new System.Drawing.Size(117, 41);
            this.buttonInformation.TabIndex = 12;
            this.buttonInformation.Text = "Information";
            this.buttonInformation.UseVisualStyleBackColor = false;
            this.buttonInformation.Click += new System.EventHandler(this.buttonInformation_Click);
            // 
            // parkLabel
            // 
            this.parkLabel.AutoSize = true;
            this.parkLabel.Location = new System.Drawing.Point(83, 109);
            this.parkLabel.Name = "parkLabel";
            this.parkLabel.Size = new System.Drawing.Size(43, 13);
            this.parkLabel.TabIndex = 13;
            this.parkLabel.Text = "Park ID";
            // 
            // spotLabel
            // 
            this.spotLabel.AutoSize = true;
            this.spotLabel.Location = new System.Drawing.Point(445, 109);
            this.spotLabel.Name = "spotLabel";
            this.spotLabel.Size = new System.Drawing.Size(43, 13);
            this.spotLabel.TabIndex = 14;
            this.spotLabel.Text = "Spot ID";
            // 
            // parkIdTextBox
            // 
            this.parkIdTextBox.Location = new System.Drawing.Point(86, 125);
            this.parkIdTextBox.Name = "parkIdTextBox";
            this.parkIdTextBox.Size = new System.Drawing.Size(188, 20);
            this.parkIdTextBox.TabIndex = 18;
            // 
            // spotIdTextBox
            // 
            this.spotIdTextBox.Location = new System.Drawing.Point(448, 125);
            this.spotIdTextBox.Name = "spotIdTextBox";
            this.spotIdTextBox.Size = new System.Drawing.Size(188, 20);
            this.spotIdTextBox.TabIndex = 19;
            // 
            // parkLabelFormat
            // 
            this.parkLabelFormat.AutoSize = true;
            this.parkLabelFormat.Location = new System.Drawing.Point(83, 148);
            this.parkLabelFormat.Name = "parkLabelFormat";
            this.parkLabelFormat.Size = new System.Drawing.Size(142, 13);
            this.parkLabelFormat.TabIndex = 21;
            this.parkLabelFormat.Text = "Format: Campus_2_A_Park1";
            // 
            // spotLabelFormat
            // 
            this.spotLabelFormat.AutoSize = true;
            this.spotLabelFormat.Location = new System.Drawing.Point(445, 148);
            this.spotLabelFormat.Name = "spotLabelFormat";
            this.spotLabelFormat.Size = new System.Drawing.Size(61, 13);
            this.spotLabelFormat.TabIndex = 23;
            this.spotLabelFormat.Text = "Format: A-8";
            // 
            // inicialDate
            // 
            this.inicialDate.Cursor = System.Windows.Forms.Cursors.No;
            this.inicialDate.CustomFormat = "dd-MM-yyyy";
            this.inicialDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.inicialDate.Location = new System.Drawing.Point(15, 19);
            this.inicialDate.Name = "inicialDate";
            this.inicialDate.Size = new System.Drawing.Size(96, 20);
            this.inicialDate.TabIndex = 24;
            // 
            // inicialDateGroupBox
            // 
            this.inicialDateGroupBox.Controls.Add(this.inicialTime);
            this.inicialDateGroupBox.Controls.Add(this.inicialDate);
            this.inicialDateGroupBox.Location = new System.Drawing.Point(71, 180);
            this.inicialDateGroupBox.Name = "inicialDateGroupBox";
            this.inicialDateGroupBox.Size = new System.Drawing.Size(236, 47);
            this.inicialDateGroupBox.TabIndex = 25;
            this.inicialDateGroupBox.TabStop = false;
            this.inicialDateGroupBox.Text = "Inicial Date";
            // 
            // inicialTime
            // 
            this.inicialTime.CustomFormat = "hh:mm:ss tt";
            this.inicialTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.inicialTime.Location = new System.Drawing.Point(117, 19);
            this.inicialTime.Name = "inicialTime";
            this.inicialTime.ShowUpDown = true;
            this.inicialTime.Size = new System.Drawing.Size(106, 20);
            this.inicialTime.TabIndex = 25;
            // 
            // finalDateGroupBox
            // 
            this.finalDateGroupBox.Controls.Add(this.finalTime);
            this.finalDateGroupBox.Controls.Add(this.finalDate);
            this.finalDateGroupBox.Location = new System.Drawing.Point(433, 180);
            this.finalDateGroupBox.Name = "finalDateGroupBox";
            this.finalDateGroupBox.Size = new System.Drawing.Size(236, 47);
            this.finalDateGroupBox.TabIndex = 26;
            this.finalDateGroupBox.TabStop = false;
            this.finalDateGroupBox.Text = "FinalDate";
            // 
            // finalTime
            // 
            this.finalTime.CustomFormat = "hh:mm:ss tt";
            this.finalTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.finalTime.Location = new System.Drawing.Point(117, 19);
            this.finalTime.Name = "finalTime";
            this.finalTime.ShowUpDown = true;
            this.finalTime.Size = new System.Drawing.Size(106, 20);
            this.finalTime.TabIndex = 25;
            // 
            // finalDate
            // 
            this.finalDate.Cursor = System.Windows.Forms.Cursors.No;
            this.finalDate.CustomFormat = "dd-MM-yyyy";
            this.finalDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.finalDate.Location = new System.Drawing.Point(15, 19);
            this.finalDate.Name = "finalDate";
            this.finalDate.Size = new System.Drawing.Size(96, 20);
            this.finalDate.TabIndex = 24;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ClientSize = new System.Drawing.Size(714, 646);
            this.Controls.Add(this.finalDateGroupBox);
            this.Controls.Add(this.inicialDateGroupBox);
            this.Controls.Add(this.spotLabelFormat);
            this.Controls.Add(this.parkLabelFormat);
            this.Controls.Add(this.spotIdTextBox);
            this.Controls.Add(this.parkIdTextBox);
            this.Controls.Add(this.spotLabel);
            this.Controls.Add(this.parkLabel);
            this.Controls.Add(this.buttonInformation);
            this.Controls.Add(this.comboBoxInformation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.showText);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.Text = "ADMIN DASHBOARD";
            this.inicialDateGroupBox.ResumeLayout(false);
            this.finalDateGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox showText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxInformation;
        private System.Windows.Forms.Button buttonInformation;
        private System.Windows.Forms.Label parkLabel;
        private System.Windows.Forms.Label spotLabel;
        private System.Windows.Forms.TextBox parkIdTextBox;
        private System.Windows.Forms.TextBox spotIdTextBox;
        private System.Windows.Forms.Label parkLabelFormat;
        private System.Windows.Forms.Label spotLabelFormat;
        private System.Windows.Forms.DateTimePicker inicialDate;
        private System.Windows.Forms.GroupBox inicialDateGroupBox;
        private System.Windows.Forms.DateTimePicker inicialTime;
        private System.Windows.Forms.GroupBox finalDateGroupBox;
        private System.Windows.Forms.DateTimePicker finalTime;
        private System.Windows.Forms.DateTimePicker finalDate;
    }
}

