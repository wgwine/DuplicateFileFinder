namespace FileFilter
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
            this.pathsListBox = new System.Windows.Forms.ListBox();
            this.addPathDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.targetPathDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scanButton1 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialRaisedButton2 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.outputPathTextField1 = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.materialRaisedButton3 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.deleteCheckBox1 = new MaterialSkin.Controls.MaterialCheckBox();
            this.materialRaisedButton4 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pathsListBox
            // 
            this.pathsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathsListBox.FormattingEnabled = true;
            this.pathsListBox.Location = new System.Drawing.Point(4, 4);
            this.pathsListBox.Name = "pathsListBox";
            this.pathsListBox.Size = new System.Drawing.Size(405, 472);
            this.pathsListBox.TabIndex = 0;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(7, 524);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(401, 25);
            this.progressBar1.TabIndex = 11;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(16, 113);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.comboBox1);
            this.splitContainer1.Panel1.Controls.Add(this.pathsListBox);
            this.splitContainer1.Panel1.Controls.Add(this.scanButton1);
            this.splitContainer1.Panel1.Controls.Add(this.progressBar1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.treeView1);
            this.splitContainer1.Size = new System.Drawing.Size(1242, 552);
            this.splitContainer1.SplitterDistance = 411;
            this.splitContainer1.TabIndex = 12;
            // 
            // scanButton1
            // 
            this.scanButton1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scanButton1.Depth = 0;
            this.scanButton1.Enabled = false;
            this.scanButton1.Location = new System.Drawing.Point(7, 484);
            this.scanButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.scanButton1.Name = "scanButton1";
            this.scanButton1.Primary = true;
            this.scanButton1.Size = new System.Drawing.Size(75, 36);
            this.scanButton1.TabIndex = 15;
            this.scanButton1.Text = "SCAN";
            this.scanButton1.UseVisualStyleBackColor = true;
            this.scanButton1.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.treeView1.Font = new System.Drawing.Font("Courier New", 10.25F, System.Drawing.FontStyle.Bold);
            this.treeView1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.treeView1.LineColor = System.Drawing.Color.GreenYellow;
            this.treeView1.Location = new System.Drawing.Point(4, 4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(820, 545);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(314, 83);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(115, 19);
            this.materialLabel1.TabIndex = 14;
            this.materialLabel1.Text = "Paths to Search";
            // 
            // materialRaisedButton2
            // 
            this.materialRaisedButton2.Depth = 0;
            this.materialRaisedButton2.Location = new System.Drawing.Point(16, 75);
            this.materialRaisedButton2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButton2.Name = "materialRaisedButton2";
            this.materialRaisedButton2.Primary = true;
            this.materialRaisedButton2.Size = new System.Drawing.Size(48, 27);
            this.materialRaisedButton2.TabIndex = 16;
            this.materialRaisedButton2.Text = "+";
            this.materialRaisedButton2.UseVisualStyleBackColor = true;
            this.materialRaisedButton2.Click += new System.EventHandler(this.addPathBox_Click);
            // 
            // outputPathTextField1
            // 
            this.outputPathTextField1.Depth = 0;
            this.outputPathTextField1.Hint = "";
            this.outputPathTextField1.Location = new System.Drawing.Point(435, 79);
            this.outputPathTextField1.MouseState = MaterialSkin.MouseState.HOVER;
            this.outputPathTextField1.Name = "outputPathTextField1";
            this.outputPathTextField1.PasswordChar = '\0';
            this.outputPathTextField1.SelectedText = "";
            this.outputPathTextField1.SelectionLength = 0;
            this.outputPathTextField1.SelectionStart = 0;
            this.outputPathTextField1.Size = new System.Drawing.Size(289, 23);
            this.outputPathTextField1.TabIndex = 17;
            this.outputPathTextField1.Text = "Output folder path";
            this.outputPathTextField1.UseSystemPasswordChar = false;
            this.outputPathTextField1.Click += new System.EventHandler(this.textBox1_Click);
            // 
            // materialRaisedButton3
            // 
            this.materialRaisedButton3.Depth = 0;
            this.materialRaisedButton3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.materialRaisedButton3.Location = new System.Drawing.Point(730, 74);
            this.materialRaisedButton3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButton3.Name = "materialRaisedButton3";
            this.materialRaisedButton3.Primary = true;
            this.materialRaisedButton3.Size = new System.Drawing.Size(121, 27);
            this.materialRaisedButton3.TabIndex = 18;
            this.materialRaisedButton3.Text = "Merge All Files";
            this.materialRaisedButton3.UseVisualStyleBackColor = true;
            this.materialRaisedButton3.Click += new System.EventHandler(this.combineButton_Click);
            // 
            // deleteCheckBox1
            // 
            this.deleteCheckBox1.AutoSize = true;
            this.deleteCheckBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.deleteCheckBox1.Depth = 0;
            this.deleteCheckBox1.Enabled = false;
            this.deleteCheckBox1.Font = new System.Drawing.Font("Roboto", 10F);
            this.deleteCheckBox1.Location = new System.Drawing.Point(1107, 73);
            this.deleteCheckBox1.Margin = new System.Windows.Forms.Padding(0);
            this.deleteCheckBox1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.deleteCheckBox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.deleteCheckBox1.Name = "deleteCheckBox1";
            this.deleteCheckBox1.Ripple = true;
            this.deleteCheckBox1.Size = new System.Drawing.Size(148, 30);
            this.deleteCheckBox1.TabIndex = 19;
            this.deleteCheckBox1.Text = "Delete Source Files";
            this.deleteCheckBox1.UseVisualStyleBackColor = false;
            // 
            // materialRaisedButton4
            // 
            this.materialRaisedButton4.Depth = 0;
            this.materialRaisedButton4.Location = new System.Drawing.Point(70, 75);
            this.materialRaisedButton4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButton4.Name = "materialRaisedButton4";
            this.materialRaisedButton4.Primary = true;
            this.materialRaisedButton4.Size = new System.Drawing.Size(136, 27);
            this.materialRaisedButton4.TabIndex = 20;
            this.materialRaisedButton4.Text = "Remove Selected";
            this.materialRaisedButton4.UseVisualStyleBackColor = true;
            this.materialRaisedButton4.Click += new System.EventHandler(this.removePathButton4_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.ItemHeight = 13;
            this.comboBox1.Location = new System.Drawing.Point(88, 493);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(319, 21);
            this.comboBox1.TabIndex = 16;
            this.comboBox1.Text = "File Scan Method";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1270, 677);
            this.Controls.Add(this.materialRaisedButton4);
            this.Controls.Add(this.deleteCheckBox1);
            this.Controls.Add(this.materialRaisedButton3);
            this.Controls.Add(this.outputPathTextField1);
            this.Controls.Add(this.materialRaisedButton2);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Duplicate File Finder";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox pathsListBox;
        private System.Windows.Forms.FolderBrowserDialog addPathDialog;
        private System.Windows.Forms.FolderBrowserDialog targetPathDialog;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialRaisedButton scanButton1;
        private MaterialSkin.Controls.MaterialRaisedButton materialRaisedButton2;
        private MaterialSkin.Controls.MaterialSingleLineTextField outputPathTextField1;
        private MaterialSkin.Controls.MaterialRaisedButton materialRaisedButton3;
        private MaterialSkin.Controls.MaterialCheckBox deleteCheckBox1;
        private MaterialSkin.Controls.MaterialRaisedButton materialRaisedButton4;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

