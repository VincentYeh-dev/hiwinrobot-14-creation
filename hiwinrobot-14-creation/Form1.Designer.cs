﻿namespace hiwinrobot_14_creation
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listViewActions = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonActionRunAll = new System.Windows.Forms.Button();
            this.buttonActionRunSelected = new System.Windows.Forms.Button();
            this.buttonActionDoOnce = new System.Windows.Forms.Button();
            this.roiSelectUserControl1 = new hiwinrobot_14_creation.ui.components.ROISelectUserControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(7, 29);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(115, 28);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(7, 64);
            this.buttonDisconnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(115, 28);
            this.buttonDisconnect.TabIndex = 0;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonConnect);
            this.groupBox1.Controls.Add(this.buttonDisconnect);
            this.groupBox1.Location = new System.Drawing.Point(14, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(129, 102);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            // 
            // listViewActions
            // 
            this.listViewActions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewActions.FullRowSelect = true;
            this.listViewActions.GridLines = true;
            this.listViewActions.HideSelection = false;
            this.listViewActions.Location = new System.Drawing.Point(7, 29);
            this.listViewActions.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listViewActions.Name = "listViewActions";
            this.listViewActions.Size = new System.Drawing.Size(401, 327);
            this.listViewActions.TabIndex = 2;
            this.listViewActions.UseCompatibleStateImageBehavior = false;
            this.listViewActions.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "No.";
            this.columnHeader1.Width = 48;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 230;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonActionRunAll);
            this.groupBox2.Controls.Add(this.buttonActionRunSelected);
            this.groupBox2.Controls.Add(this.buttonActionDoOnce);
            this.groupBox2.Controls.Add(this.listViewActions);
            this.groupBox2.Location = new System.Drawing.Point(150, 14);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(418, 413);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Action";
            // 
            // buttonActionRunAll
            // 
            this.buttonActionRunAll.Location = new System.Drawing.Point(7, 364);
            this.buttonActionRunAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonActionRunAll.Name = "buttonActionRunAll";
            this.buttonActionRunAll.Size = new System.Drawing.Size(129, 36);
            this.buttonActionRunAll.TabIndex = 3;
            this.buttonActionRunAll.Text = "Run All";
            this.buttonActionRunAll.UseVisualStyleBackColor = true;
            this.buttonActionRunAll.Click += new System.EventHandler(this.buttonActionRunAll_Click);
            // 
            // buttonActionRunSelected
            // 
            this.buttonActionRunSelected.Location = new System.Drawing.Point(143, 364);
            this.buttonActionRunSelected.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonActionRunSelected.Name = "buttonActionRunSelected";
            this.buttonActionRunSelected.Size = new System.Drawing.Size(129, 36);
            this.buttonActionRunSelected.TabIndex = 3;
            this.buttonActionRunSelected.Text = "Run Selected";
            this.buttonActionRunSelected.UseVisualStyleBackColor = true;
            this.buttonActionRunSelected.Click += new System.EventHandler(this.buttonActionRunSelected_Click);
            // 
            // buttonActionDoOnce
            // 
            this.buttonActionDoOnce.Location = new System.Drawing.Point(279, 364);
            this.buttonActionDoOnce.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonActionDoOnce.Name = "buttonActionDoOnce";
            this.buttonActionDoOnce.Size = new System.Drawing.Size(129, 36);
            this.buttonActionDoOnce.TabIndex = 3;
            this.buttonActionDoOnce.Text = "Do Once";
            this.buttonActionDoOnce.UseVisualStyleBackColor = true;
            this.buttonActionDoOnce.Click += new System.EventHandler(this.buttonActionDoOnce_Click);
            // 
            // roiSelectUserControl1
            // 
            this.roiSelectUserControl1.Location = new System.Drawing.Point(584, 43);
            this.roiSelectUserControl1.Name = "roiSelectUserControl1";
            this.roiSelectUserControl1.Size = new System.Drawing.Size(793, 586);
            this.roiSelectUserControl1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1501, 820);
            this.Controls.Add(this.roiSelectUserControl1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "HIWIN-14屆：智慧創作";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listViewActions;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonActionDoOnce;
        private System.Windows.Forms.Button buttonActionRunAll;
        private System.Windows.Forms.Button buttonActionRunSelected;
        private ui.components.ROISelectUserControl roiSelectUserControl1;
    }
}

