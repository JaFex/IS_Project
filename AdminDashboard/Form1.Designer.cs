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
            this.textBoxDate1 = new System.Windows.Forms.TextBox();
            this.comboBoxInformation = new System.Windows.Forms.ComboBox();
            this.buttonInformation = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxParkId = new System.Windows.Forms.TextBox();
            this.textBoxSpotId = new System.Windows.Forms.TextBox();
            this.textBoxDate2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
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
            this.showText.Location = new System.Drawing.Point(48, 206);
            this.showText.Name = "showText";
            this.showText.Size = new System.Drawing.Size(552, 367);
            this.showText.TabIndex = 6;
            this.showText.Text = "";
            this.showText.TextChanged += new System.EventHandler(this.showText_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(44, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 24);
            this.label1.TabIndex = 7;
            this.label1.Text = "Information:";
            // 
            // textBoxDate1
            // 
            this.textBoxDate1.Location = new System.Drawing.Point(411, 125);
            this.textBoxDate1.Name = "textBoxDate1";
            this.textBoxDate1.Size = new System.Drawing.Size(188, 20);
            this.textBoxDate1.TabIndex = 9;
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
            this.comboBoxInformation.Location = new System.Drawing.Point(12, 69);
            this.comboBoxInformation.Name = "comboBoxInformation";
            this.comboBoxInformation.Size = new System.Drawing.Size(560, 24);
            this.comboBoxInformation.TabIndex = 11;
            this.comboBoxInformation.Tag = "";
            this.comboBoxInformation.SelectedIndexChanged += new System.EventHandler(this.comboBoxInformation_SelectedIndexChanged);
            // 
            // buttonInformation
            // 
            this.buttonInformation.BackColor = System.Drawing.SystemColors.Highlight;
            this.buttonInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonInformation.Location = new System.Drawing.Point(578, 59);
            this.buttonInformation.Name = "buttonInformation";
            this.buttonInformation.Size = new System.Drawing.Size(117, 41);
            this.buttonInformation.TabIndex = 12;
            this.buttonInformation.Text = "Information";
            this.buttonInformation.UseVisualStyleBackColor = false;
            this.buttonInformation.Click += new System.EventHandler(this.buttonInformation_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(83, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Park ID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(276, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Spot ID";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(687, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Date2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(483, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Date1";
            // 
            // textBoxParkId
            // 
            this.textBoxParkId.Location = new System.Drawing.Point(12, 125);
            this.textBoxParkId.Name = "textBoxParkId";
            this.textBoxParkId.Size = new System.Drawing.Size(188, 20);
            this.textBoxParkId.TabIndex = 18;
            // 
            // textBoxSpotId
            // 
            this.textBoxSpotId.Location = new System.Drawing.Point(206, 125);
            this.textBoxSpotId.Name = "textBoxSpotId";
            this.textBoxSpotId.Size = new System.Drawing.Size(188, 20);
            this.textBoxSpotId.TabIndex = 19;
            // 
            // textBoxDate2
            // 
            this.textBoxDate2.Location = new System.Drawing.Point(605, 125);
            this.textBoxDate2.Name = "textBoxDate2";
            this.textBoxDate2.Size = new System.Drawing.Size(188, 20);
            this.textBoxDate2.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 148);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Format: Campus_2_A_Park1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(425, 149);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(163, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Format: 11-27-2018 6_04_01 PM";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(203, 149);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Format: A-8";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ClientSize = new System.Drawing.Size(794, 646);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxDate2);
            this.Controls.Add(this.textBoxSpotId);
            this.Controls.Add(this.textBoxParkId);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonInformation);
            this.Controls.Add(this.comboBoxInformation);
            this.Controls.Add(this.textBoxDate1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.showText);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.Text = "ADMIN DASHBOARD";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox showText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDate1;
        private System.Windows.Forms.ComboBox comboBoxInformation;
        private System.Windows.Forms.Button buttonInformation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxParkId;
        private System.Windows.Forms.TextBox textBoxSpotId;
        private System.Windows.Forms.TextBox textBoxDate2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}

