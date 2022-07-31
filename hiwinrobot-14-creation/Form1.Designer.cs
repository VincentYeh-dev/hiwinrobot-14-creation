namespace hiwinrobot_14_creation
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
            this.buttonActionRunFromHere = new System.Windows.Forms.Button();
            this.buttonActionDoOnce = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(6, 24);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(102, 23);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(6, 53);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(102, 23);
            this.buttonDisconnect.TabIndex = 0;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonConnect);
            this.groupBox1.Controls.Add(this.buttonDisconnect);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(115, 85);
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
            this.listViewActions.Location = new System.Drawing.Point(6, 24);
            this.listViewActions.MultiSelect = false;
            this.listViewActions.Name = "listViewActions";
            this.listViewActions.Size = new System.Drawing.Size(357, 273);
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
            this.groupBox2.Controls.Add(this.buttonActionRunFromHere);
            this.groupBox2.Controls.Add(this.buttonActionDoOnce);
            this.groupBox2.Controls.Add(this.listViewActions);
            this.groupBox2.Location = new System.Drawing.Point(133, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(372, 344);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Action";
            // 
            // buttonActionRunAll
            // 
            this.buttonActionRunAll.Location = new System.Drawing.Point(6, 303);
            this.buttonActionRunAll.Name = "buttonActionRunAll";
            this.buttonActionRunAll.Size = new System.Drawing.Size(115, 30);
            this.buttonActionRunAll.TabIndex = 3;
            this.buttonActionRunAll.Text = "Run All";
            this.buttonActionRunAll.UseVisualStyleBackColor = true;
            this.buttonActionRunAll.Click += new System.EventHandler(this.buttonActionRunAll_Click);
            // 
            // buttonActionRunFromHere
            // 
            this.buttonActionRunFromHere.Location = new System.Drawing.Point(127, 303);
            this.buttonActionRunFromHere.Name = "buttonActionRunFromHere";
            this.buttonActionRunFromHere.Size = new System.Drawing.Size(115, 30);
            this.buttonActionRunFromHere.TabIndex = 3;
            this.buttonActionRunFromHere.Text = "Run from Here";
            this.buttonActionRunFromHere.UseVisualStyleBackColor = true;
            this.buttonActionRunFromHere.Click += new System.EventHandler(this.buttonActionRunFromHere_Click);
            // 
            // buttonActionDoOnce
            // 
            this.buttonActionDoOnce.Location = new System.Drawing.Point(248, 303);
            this.buttonActionDoOnce.Name = "buttonActionDoOnce";
            this.buttonActionDoOnce.Size = new System.Drawing.Size(115, 30);
            this.buttonActionDoOnce.TabIndex = 3;
            this.buttonActionDoOnce.Text = "Do Once";
            this.buttonActionDoOnce.UseVisualStyleBackColor = true;
            this.buttonActionDoOnce.Click += new System.EventHandler(this.buttonActionDoOnce_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 683);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
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
        private System.Windows.Forms.Button buttonActionRunFromHere;
    }
}

