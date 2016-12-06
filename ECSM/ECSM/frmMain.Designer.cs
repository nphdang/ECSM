namespace ECSM
{
    partial class frmMain
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.btBrowseData = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAlpha = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMinDev = new System.Windows.Forms.TextBox();
            this.btMining = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.treeResult = new System.Windows.Forms.TreeView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBrowseDict = new System.Windows.Forms.Button();
            this.txtDict = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.chkSignificant = new System.Windows.Forms.CheckBox();
            this.chkOutput = new System.Windows.Forms.CheckBox();
            this.chkPruneChi = new System.Windows.Forms.CheckBox();
            this.chkExceptional = new System.Windows.Forms.CheckBox();
            this.chkClassification = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Database";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(73, 12);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.ReadOnly = true;
            this.txtDatabase.Size = new System.Drawing.Size(255, 20);
            this.txtDatabase.TabIndex = 1;
            // 
            // btBrowseData
            // 
            this.btBrowseData.Location = new System.Drawing.Point(334, 10);
            this.btBrowseData.Name = "btBrowseData";
            this.btBrowseData.Size = new System.Drawing.Size(75, 23);
            this.btBrowseData.TabIndex = 2;
            this.btBrowseData.Text = "Browse";
            this.btBrowseData.UseVisualStyleBackColor = true;
            this.btBrowseData.Click += new System.EventHandler(this.btBrowseData_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "alpha";
            // 
            // txtAlpha
            // 
            this.txtAlpha.Location = new System.Drawing.Point(53, 71);
            this.txtAlpha.Name = "txtAlpha";
            this.txtAlpha.Size = new System.Drawing.Size(37, 20);
            this.txtAlpha.TabIndex = 5;
            this.txtAlpha.Text = "0.05";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(96, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "minDev";
            // 
            // txtMinDev
            // 
            this.txtMinDev.Location = new System.Drawing.Point(145, 71);
            this.txtMinDev.Name = "txtMinDev";
            this.txtMinDev.Size = new System.Drawing.Size(37, 20);
            this.txtMinDev.TabIndex = 7;
            this.txtMinDev.Text = "5";
            // 
            // btMining
            // 
            this.btMining.Location = new System.Drawing.Point(17, 106);
            this.btMining.Name = "btMining";
            this.btMining.Size = new System.Drawing.Size(92, 41);
            this.btMining.TabIndex = 8;
            this.btMining.Text = "Mining";
            this.btMining.UseVisualStyleBackColor = true;
            this.btMining.Click += new System.EventHandler(this.btMining_Click);
            // 
            // btClose
            // 
            this.btClose.Location = new System.Drawing.Point(127, 106);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(92, 41);
            this.btClose.TabIndex = 9;
            this.btClose.Text = "Close";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // treeResult
            // 
            this.treeResult.Location = new System.Drawing.Point(17, 153);
            this.treeResult.Name = "treeResult";
            this.treeResult.Size = new System.Drawing.Size(474, 298);
            this.treeResult.TabIndex = 10;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(188, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "%";
            // 
            // txtBrowseDict
            // 
            this.txtBrowseDict.Location = new System.Drawing.Point(334, 39);
            this.txtBrowseDict.Name = "txtBrowseDict";
            this.txtBrowseDict.Size = new System.Drawing.Size(75, 23);
            this.txtBrowseDict.TabIndex = 20;
            this.txtBrowseDict.Text = "Browse";
            this.txtBrowseDict.UseVisualStyleBackColor = true;
            this.txtBrowseDict.Click += new System.EventHandler(this.txtBrowseDict_Click);
            // 
            // txtDict
            // 
            this.txtDict.Location = new System.Drawing.Point(73, 41);
            this.txtDict.Name = "txtDict";
            this.txtDict.ReadOnly = true;
            this.txtDict.Size = new System.Drawing.Size(255, 20);
            this.txtDict.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Dictionary";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // chkSignificant
            // 
            this.chkSignificant.AutoSize = true;
            this.chkSignificant.Checked = true;
            this.chkSignificant.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSignificant.Location = new System.Drawing.Point(239, 101);
            this.chkSignificant.Name = "chkSignificant";
            this.chkSignificant.Size = new System.Drawing.Size(75, 17);
            this.chkSignificant.TabIndex = 22;
            this.chkSignificant.Text = "Significant";
            this.chkSignificant.UseVisualStyleBackColor = true;
            this.chkSignificant.CheckedChanged += new System.EventHandler(this.chkSignificant_CheckedChanged);
            // 
            // chkOutput
            // 
            this.chkOutput.AutoSize = true;
            this.chkOutput.Location = new System.Drawing.Point(239, 73);
            this.chkOutput.Name = "chkOutput";
            this.chkOutput.Size = new System.Drawing.Size(84, 17);
            this.chkOutput.TabIndex = 23;
            this.chkOutput.Text = "Save results";
            this.chkOutput.UseVisualStyleBackColor = true;
            // 
            // chkPruneChi
            // 
            this.chkPruneChi.AutoSize = true;
            this.chkPruneChi.Checked = true;
            this.chkPruneChi.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPruneChi.Location = new System.Drawing.Point(334, 74);
            this.chkPruneChi.Name = "chkPruneChi";
            this.chkPruneChi.Size = new System.Drawing.Size(107, 17);
            this.chkPruneChi.TabIndex = 24;
            this.chkPruneChi.Text = "Prune Chi-square";
            this.chkPruneChi.UseVisualStyleBackColor = true;
            // 
            // chkExceptional
            // 
            this.chkExceptional.AutoSize = true;
            this.chkExceptional.Checked = true;
            this.chkExceptional.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExceptional.Location = new System.Drawing.Point(334, 101);
            this.chkExceptional.Name = "chkExceptional";
            this.chkExceptional.Size = new System.Drawing.Size(81, 17);
            this.chkExceptional.TabIndex = 25;
            this.chkExceptional.Text = "Exceptional";
            this.chkExceptional.UseVisualStyleBackColor = true;
            // 
            // chkClassification
            // 
            this.chkClassification.AutoSize = true;
            this.chkClassification.Location = new System.Drawing.Point(239, 129);
            this.chkClassification.Name = "chkClassification";
            this.chkClassification.Size = new System.Drawing.Size(87, 17);
            this.chkClassification.TabIndex = 26;
            this.chkClassification.Text = "Classification";
            this.chkClassification.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 462);
            this.Controls.Add(this.chkClassification);
            this.Controls.Add(this.chkExceptional);
            this.Controls.Add(this.chkPruneChi);
            this.Controls.Add(this.chkOutput);
            this.Controls.Add(this.chkSignificant);
            this.Controls.Add(this.txtBrowseDict);
            this.Controls.Add(this.txtDict);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.treeResult);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btMining);
            this.Controls.Add(this.txtMinDev);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAlpha);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btBrowseData);
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Exceptional Contrast Set Mining (ECSM)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Button btBrowseData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAlpha;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMinDev;
        private System.Windows.Forms.Button btMining;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.TreeView treeResult;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button txtBrowseDict;
        private System.Windows.Forms.TextBox txtDict;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.CheckBox chkSignificant;
        private System.Windows.Forms.CheckBox chkOutput;
        private System.Windows.Forms.CheckBox chkPruneChi;
        private System.Windows.Forms.CheckBox chkExceptional;
        private System.Windows.Forms.CheckBox chkClassification;
    }
}

