namespace OLSR.Screens
{
    partial class StartScreen
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblInterList = new System.Windows.Forms.Label();
            this.lstInterfaces = new System.Windows.Forms.ListBox();
            this.bttStart = new System.Windows.Forms.Button();
            this.bttStop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lstNeighbors = new System.Windows.Forms.ListBox();
            this.lst2HopNeighbors = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lstMPR = new System.Windows.Forms.ListBox();
            this.chckDebug = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bttSave = new System.Windows.Forms.Button();
            this.bttRestore = new System.Windows.Forms.Button();
            this.txtRefresh = new System.Windows.Forms.TextBox();
            this.txtTc = new System.Windows.Forms.TextBox();
            this.txtHello = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(98, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(73, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "OLSR";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInterList
            // 
            this.lblInterList.AutoSize = true;
            this.lblInterList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInterList.Location = new System.Drawing.Point(12, 50);
            this.lblInterList.Name = "lblInterList";
            this.lblInterList.Size = new System.Drawing.Size(110, 20);
            this.lblInterList.TabIndex = 1;
            this.lblInterList.Text = "Interfaces List";
            this.lblInterList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstInterfaces
            // 
            this.lstInterfaces.FormattingEnabled = true;
            this.lstInterfaces.Location = new System.Drawing.Point(17, 89);
            this.lstInterfaces.Name = "lstInterfaces";
            this.lstInterfaces.Size = new System.Drawing.Size(104, 134);
            this.lstInterfaces.TabIndex = 2;
            this.lstInterfaces.SelectedIndexChanged += new System.EventHandler(this.lstInterfaces_SelectedIndexChanged);
            // 
            // bttStart
            // 
            this.bttStart.Enabled = false;
            this.bttStart.Location = new System.Drawing.Point(151, 95);
            this.bttStart.Name = "bttStart";
            this.bttStart.Size = new System.Drawing.Size(109, 45);
            this.bttStart.TabIndex = 3;
            this.bttStart.Text = "Start";
            this.bttStart.UseVisualStyleBackColor = true;
            this.bttStart.Click += new System.EventHandler(this.bttStart_Click);
            // 
            // bttStop
            // 
            this.bttStop.Enabled = false;
            this.bttStop.Location = new System.Drawing.Point(151, 173);
            this.bttStop.Name = "bttStop";
            this.bttStop.Size = new System.Drawing.Size(109, 45);
            this.bttStop.TabIndex = 4;
            this.bttStop.Text = "Stop";
            this.bttStop.UseVisualStyleBackColor = true;
            this.bttStop.Click += new System.EventHandler(this.bttStop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(333, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Neighbors";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(462, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "2 Hop Neighbors";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstNeighbors
            // 
            this.lstNeighbors.Enabled = false;
            this.lstNeighbors.FormattingEnabled = true;
            this.lstNeighbors.Location = new System.Drawing.Point(325, 50);
            this.lstNeighbors.Name = "lstNeighbors";
            this.lstNeighbors.Size = new System.Drawing.Size(104, 173);
            this.lstNeighbors.TabIndex = 7;
            // 
            // lst2HopNeighbors
            // 
            this.lst2HopNeighbors.Enabled = false;
            this.lst2HopNeighbors.FormattingEnabled = true;
            this.lst2HopNeighbors.Location = new System.Drawing.Point(478, 50);
            this.lst2HopNeighbors.Name = "lst2HopNeighbors";
            this.lst2HopNeighbors.Size = new System.Drawing.Size(104, 173);
            this.lst2HopNeighbors.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(630, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "MPR SET";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstMPR
            // 
            this.lstMPR.Enabled = false;
            this.lstMPR.FormattingEnabled = true;
            this.lstMPR.Location = new System.Drawing.Point(622, 50);
            this.lstMPR.Name = "lstMPR";
            this.lstMPR.Size = new System.Drawing.Size(104, 173);
            this.lstMPR.TabIndex = 10;
            // 
            // chckDebug
            // 
            this.chckDebug.AutoSize = true;
            this.chckDebug.Location = new System.Drawing.Point(17, 244);
            this.chckDebug.Name = "chckDebug";
            this.chckDebug.Size = new System.Drawing.Size(88, 17);
            this.chckDebug.TabIndex = 11;
            this.chckDebug.Text = "Debug Mode";
            this.chckDebug.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbLanguage);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.bttSave);
            this.groupBox1.Controls.Add(this.bttRestore);
            this.groupBox1.Controls.Add(this.txtRefresh);
            this.groupBox1.Controls.Add(this.txtTc);
            this.groupBox1.Controls.Add(this.txtHello);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Location = new System.Drawing.Point(151, 244);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(575, 90);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuration Parameters";
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Items.AddRange(new object[] {
            "English",
            "Italian"});
            this.cmbLanguage.Location = new System.Drawing.Point(292, 41);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(139, 21);
            this.cmbLanguage.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(290, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 22);
            this.label4.TabIndex = 24;
            this.label4.Text = "Language";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bttSave
            // 
            this.bttSave.Location = new System.Drawing.Point(437, 52);
            this.bttSave.Name = "bttSave";
            this.bttSave.Size = new System.Drawing.Size(121, 27);
            this.bttSave.TabIndex = 20;
            this.bttSave.Text = "Save";
            this.bttSave.Click += new System.EventHandler(this.bttSave_Click);
            // 
            // bttRestore
            // 
            this.bttRestore.Location = new System.Drawing.Point(437, 19);
            this.bttRestore.Name = "bttRestore";
            this.bttRestore.Size = new System.Drawing.Size(121, 27);
            this.bttRestore.TabIndex = 19;
            this.bttRestore.Text = "Restore RFC";
            this.bttRestore.Click += new System.EventHandler(this.bttRestore_Click);
            // 
            // txtRefresh
            // 
            this.txtRefresh.Location = new System.Drawing.Point(174, 63);
            this.txtRefresh.Name = "txtRefresh";
            this.txtRefresh.Size = new System.Drawing.Size(110, 20);
            this.txtRefresh.TabIndex = 18;
            // 
            // txtTc
            // 
            this.txtTc.Location = new System.Drawing.Point(174, 38);
            this.txtTc.Name = "txtTc";
            this.txtTc.Size = new System.Drawing.Size(110, 20);
            this.txtTc.TabIndex = 17;
            // 
            // txtHello
            // 
            this.txtHello.Location = new System.Drawing.Point(174, 14);
            this.txtHello.Name = "txtHello";
            this.txtHello.Size = new System.Drawing.Size(110, 20);
            this.txtHello.TabIndex = 16;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(9, 65);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(159, 22);
            this.label11.TabIndex = 21;
            this.label11.Text = "Refresh Interval";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(9, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(159, 22);
            this.label10.TabIndex = 22;
            this.label10.Text = "TC Interval";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(9, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(159, 22);
            this.label9.TabIndex = 23;
            this.label9.Text = "HELLO Interval";
            // 
            // StartScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 348);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chckDebug);
            this.Controls.Add(this.lstMPR);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lst2HopNeighbors);
            this.Controls.Add(this.lstNeighbors);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bttStop);
            this.Controls.Add(this.bttStart);
            this.Controls.Add(this.lstInterfaces);
            this.Controls.Add(this.lblInterList);
            this.Controls.Add(this.lblTitle);
            this.Name = "StartScreen";
            this.Text = "StartScreen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartScreen_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblInterList;
        private System.Windows.Forms.ListBox lstInterfaces;
        private System.Windows.Forms.Button bttStart;
        private System.Windows.Forms.Button bttStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstNeighbors;
        private System.Windows.Forms.ListBox lst2HopNeighbors;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lstMPR;
        private System.Windows.Forms.CheckBox chckDebug;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bttSave;
        private System.Windows.Forms.Button bttRestore;
        private System.Windows.Forms.TextBox txtRefresh;
        private System.Windows.Forms.TextBox txtTc;
        private System.Windows.Forms.TextBox txtHello;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.Label label4;
    }
}