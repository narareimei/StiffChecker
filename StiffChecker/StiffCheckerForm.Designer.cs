namespace Stiff
{
    partial class StiffCheckerForm
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

            if (disposing && (stiffer != null))
            {
                stiffer.Dispose();
            }
            return;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bookGrid = new System.Windows.Forms.DataGridView();
            this.zoom = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gridOff = new System.Windows.Forms.RadioButton();
            this.gridOn = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.view = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btRun = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bookGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bookGrid
            // 
            this.bookGrid.AllowUserToAddRows = false;
            this.bookGrid.AllowUserToDeleteRows = false;
            this.bookGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bookGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bookGrid.Location = new System.Drawing.Point(11, 62);
            this.bookGrid.MultiSelect = false;
            this.bookGrid.Name = "bookGrid";
            this.bookGrid.ReadOnly = true;
            this.bookGrid.RowTemplate.Height = 21;
            this.bookGrid.Size = new System.Drawing.Size(1145, 304);
            this.bookGrid.TabIndex = 1;
            // 
            // zoom
            // 
            this.zoom.Font = new System.Drawing.Font("HG丸ｺﾞｼｯｸM-PRO", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.zoom.FormattingEnabled = true;
            this.zoom.Items.AddRange(new object[] {
            "80",
            "85",
            "90",
            "100"});
            this.zoom.Location = new System.Drawing.Point(107, 18);
            this.zoom.Name = "zoom";
            this.zoom.Size = new System.Drawing.Size(72, 27);
            this.zoom.TabIndex = 2;
            this.zoom.Text = "100";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("HG丸ｺﾞｼｯｸM-PRO", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "表示倍率";
            // 
            // gridOff
            // 
            this.gridOff.AutoSize = true;
            this.gridOff.Checked = true;
            this.gridOff.Font = new System.Drawing.Font("HG丸ｺﾞｼｯｸM-PRO", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gridOff.Location = new System.Drawing.Point(17, 18);
            this.gridOff.Name = "gridOff";
            this.gridOff.Size = new System.Drawing.Size(67, 23);
            this.gridOff.TabIndex = 6;
            this.gridOff.TabStop = true;
            this.gridOff.Text = "なし";
            this.gridOff.UseVisualStyleBackColor = true;
            // 
            // gridOn
            // 
            this.gridOn.AutoSize = true;
            this.gridOn.Font = new System.Drawing.Font("HG丸ｺﾞｼｯｸM-PRO", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gridOn.Location = new System.Drawing.Point(102, 18);
            this.gridOn.Name = "gridOn";
            this.gridOn.Size = new System.Drawing.Size(67, 23);
            this.gridOn.TabIndex = 6;
            this.gridOn.Text = "あり";
            this.gridOn.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("HG丸ｺﾞｼｯｸM-PRO", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(421, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 19);
            this.label2.TabIndex = 7;
            this.label2.Text = "表示モード";
            // 
            // view
            // 
            this.view.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.view.Font = new System.Drawing.Font("HG丸ｺﾞｼｯｸM-PRO", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.view.FormattingEnabled = true;
            this.view.Items.AddRange(new object[] {
            "標準モード",
            "改ページモード"});
            this.view.Location = new System.Drawing.Point(536, 18);
            this.view.Name = "view";
            this.view.Size = new System.Drawing.Size(191, 27);
            this.view.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gridOff);
            this.groupBox1.Controls.Add(this.gridOn);
            this.groupBox1.Font = new System.Drawing.Font("HG丸ｺﾞｼｯｸM-PRO", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(214, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 50);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "枠線";
            // 
            // btRun
            // 
            this.btRun.Font = new System.Drawing.Font("HG丸ｺﾞｼｯｸM-PRO", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btRun.Location = new System.Drawing.Point(754, 14);
            this.btRun.Name = "btRun";
            this.btRun.Size = new System.Drawing.Size(75, 32);
            this.btRun.TabIndex = 10;
            this.btRun.Text = "確認";
            this.btRun.UseVisualStyleBackColor = true;
            this.btRun.Click += new System.EventHandler(this.btRun_Click);
            // 
            // StiffCheckerForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 378);
            this.Controls.Add(this.btRun);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.view);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.zoom);
            this.Controls.Add(this.bookGrid);
            this.Name = "StiffCheckerForm";
            this.Text = "StiffForm";
            this.Load += new System.EventHandler(this.StiffForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.StiffForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.StiffForm_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.bookGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView bookGrid;
        private System.Windows.Forms.ComboBox zoom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton gridOff;
        private System.Windows.Forms.RadioButton gridOn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox view;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btRun;
    }
}