namespace Hy.WebP.Test
{
    partial class BatchForm
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
            if (disposing && (components != null)) {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Start = new System.Windows.Forms.Button();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.gv_Images = new System.Windows.Forms.DataGridView();
            this.lb_Origin = new System.Windows.Forms.Label();
            this.bg_Worker = new System.ComponentModel.BackgroundWorker();
            this.ic_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_Quality = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_Level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_NearLossless = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_Method = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_Pass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_FilterSharpness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_AutoFilter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_FilterType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_SharpYUV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_QMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_QMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_TargetPSNR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_PSNR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ic_Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Images)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btn_Start, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_Stop, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.gv_Images, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lb_Origin, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 361);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btn_Start
            // 
            this.btn_Start.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Start.Location = new System.Drawing.Point(3, 3);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(74, 22);
            this.btn_Start.TabIndex = 0;
            this.btn_Start.Text = "Start";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // btn_Stop
            // 
            this.btn_Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Stop.Enabled = false;
            this.btn_Stop.Location = new System.Drawing.Point(83, 3);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(74, 22);
            this.btn_Stop.TabIndex = 1;
            this.btn_Stop.Text = "Stop";
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // gv_Images
            // 
            this.gv_Images.AllowUserToAddRows = false;
            this.gv_Images.AllowUserToDeleteRows = false;
            this.gv_Images.AllowUserToResizeRows = false;
            this.gv_Images.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gv_Images.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ic_Id,
            this.ic_Type,
            this.ic_Quality,
            this.ic_Level,
            this.ic_NearLossless,
            this.ic_Method,
            this.ic_Pass,
            this.ic_FilterSharpness,
            this.ic_AutoFilter,
            this.ic_FilterType,
            this.ic_SharpYUV,
            this.ic_QMin,
            this.ic_QMax,
            this.ic_TargetPSNR,
            this.ic_Size,
            this.ic_PSNR,
            this.ic_Time});
            this.tableLayoutPanel1.SetColumnSpan(this.gv_Images, 3);
            this.gv_Images.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gv_Images.Location = new System.Drawing.Point(3, 31);
            this.gv_Images.Name = "gv_Images";
            this.gv_Images.ReadOnly = true;
            this.gv_Images.RowHeadersVisible = false;
            this.gv_Images.RowTemplate.Height = 23;
            this.gv_Images.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gv_Images.Size = new System.Drawing.Size(778, 327);
            this.gv_Images.TabIndex = 2;
            // 
            // lb_Origin
            // 
            this.lb_Origin.AutoSize = true;
            this.lb_Origin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Origin.Location = new System.Drawing.Point(163, 0);
            this.lb_Origin.Name = "lb_Origin";
            this.lb_Origin.Size = new System.Drawing.Size(618, 28);
            this.lb_Origin.TabIndex = 3;
            this.lb_Origin.Text = "Origin";
            this.lb_Origin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bg_Worker
            // 
            this.bg_Worker.WorkerReportsProgress = true;
            this.bg_Worker.WorkerSupportsCancellation = true;
            this.bg_Worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bg_Worker_DoWork);
            this.bg_Worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bg_Worker_ProgressChanged);
            this.bg_Worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bg_Worker_RunWorkerCompleted);
            // 
            // ic_Id
            // 
            this.ic_Id.DataPropertyName = "Id";
            this.ic_Id.HeaderText = "Id";
            this.ic_Id.Name = "ic_Id";
            this.ic_Id.ReadOnly = true;
            this.ic_Id.Width = 70;
            // 
            // ic_Type
            // 
            this.ic_Type.DataPropertyName = "Type";
            this.ic_Type.HeaderText = "Type";
            this.ic_Type.Name = "ic_Type";
            this.ic_Type.ReadOnly = true;
            this.ic_Type.Width = 70;
            // 
            // ic_Quality
            // 
            this.ic_Quality.DataPropertyName = "Quality";
            this.ic_Quality.HeaderText = "Quality";
            this.ic_Quality.Name = "ic_Quality";
            this.ic_Quality.ReadOnly = true;
            this.ic_Quality.Width = 80;
            // 
            // ic_Level
            // 
            this.ic_Level.DataPropertyName = "Level";
            this.ic_Level.HeaderText = "Level";
            this.ic_Level.Name = "ic_Level";
            this.ic_Level.ReadOnly = true;
            // 
            // ic_NearLossless
            // 
            this.ic_NearLossless.DataPropertyName = "NearLossless";
            this.ic_NearLossless.HeaderText = "NearLossless";
            this.ic_NearLossless.Name = "ic_NearLossless";
            this.ic_NearLossless.ReadOnly = true;
            this.ic_NearLossless.Width = 80;
            // 
            // ic_Method
            // 
            this.ic_Method.DataPropertyName = "Method";
            this.ic_Method.HeaderText = "Method";
            this.ic_Method.Name = "ic_Method";
            this.ic_Method.ReadOnly = true;
            this.ic_Method.Width = 80;
            // 
            // ic_Pass
            // 
            this.ic_Pass.DataPropertyName = "Pass";
            this.ic_Pass.HeaderText = "Pass";
            this.ic_Pass.Name = "ic_Pass";
            this.ic_Pass.ReadOnly = true;
            this.ic_Pass.Width = 80;
            // 
            // ic_FilterSharpness
            // 
            this.ic_FilterSharpness.DataPropertyName = "FilterSharpness";
            this.ic_FilterSharpness.HeaderText = "FilterSharpness";
            this.ic_FilterSharpness.Name = "ic_FilterSharpness";
            this.ic_FilterSharpness.ReadOnly = true;
            // 
            // ic_AutoFilter
            // 
            this.ic_AutoFilter.DataPropertyName = "AutoFilter";
            this.ic_AutoFilter.HeaderText = "AutoFilter";
            this.ic_AutoFilter.Name = "ic_AutoFilter";
            this.ic_AutoFilter.ReadOnly = true;
            this.ic_AutoFilter.Width = 80;
            // 
            // ic_FilterType
            // 
            this.ic_FilterType.DataPropertyName = "FilterType";
            this.ic_FilterType.HeaderText = "FilterType";
            this.ic_FilterType.Name = "ic_FilterType";
            this.ic_FilterType.ReadOnly = true;
            this.ic_FilterType.Width = 80;
            // 
            // ic_SharpYUV
            // 
            this.ic_SharpYUV.DataPropertyName = "SharpYUV";
            this.ic_SharpYUV.HeaderText = "SharpYUV";
            this.ic_SharpYUV.Name = "ic_SharpYUV";
            this.ic_SharpYUV.ReadOnly = true;
            this.ic_SharpYUV.Width = 80;
            // 
            // ic_QMin
            // 
            this.ic_QMin.DataPropertyName = "QMin";
            this.ic_QMin.HeaderText = "QMin";
            this.ic_QMin.Name = "ic_QMin";
            this.ic_QMin.ReadOnly = true;
            this.ic_QMin.Width = 60;
            // 
            // ic_QMax
            // 
            this.ic_QMax.DataPropertyName = "QMax";
            this.ic_QMax.HeaderText = "QMax";
            this.ic_QMax.Name = "ic_QMax";
            this.ic_QMax.ReadOnly = true;
            this.ic_QMax.Width = 60;
            // 
            // ic_TargetPSNR
            // 
            this.ic_TargetPSNR.DataPropertyName = "TargetPSNR";
            this.ic_TargetPSNR.HeaderText = "TargetPSNR";
            this.ic_TargetPSNR.Name = "ic_TargetPSNR";
            this.ic_TargetPSNR.ReadOnly = true;
            this.ic_TargetPSNR.Width = 80;
            // 
            // ic_Size
            // 
            this.ic_Size.DataPropertyName = "Size";
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.ic_Size.DefaultCellStyle = dataGridViewCellStyle1;
            this.ic_Size.HeaderText = "Size";
            this.ic_Size.Name = "ic_Size";
            this.ic_Size.ReadOnly = true;
            // 
            // ic_PSNR
            // 
            this.ic_PSNR.DataPropertyName = "PSNR";
            dataGridViewCellStyle2.Format = "F2";
            dataGridViewCellStyle2.NullValue = null;
            this.ic_PSNR.DefaultCellStyle = dataGridViewCellStyle2;
            this.ic_PSNR.HeaderText = "PSNR";
            this.ic_PSNR.Name = "ic_PSNR";
            this.ic_PSNR.ReadOnly = true;
            // 
            // ic_Time
            // 
            this.ic_Time.DataPropertyName = "Time";
            dataGridViewCellStyle3.Format = "F2";
            dataGridViewCellStyle3.NullValue = null;
            this.ic_Time.DefaultCellStyle = dataGridViewCellStyle3;
            this.ic_Time.HeaderText = "Time";
            this.ic_Time.Name = "ic_Time";
            this.ic_Time.ReadOnly = true;
            // 
            // BatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BatchForm";
            this.ShowInTaskbar = false;
            this.Text = "BatchForm";
            this.Load += new System.EventHandler(this.BatchForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Images)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.DataGridView gv_Images;
        private System.Windows.Forms.Label lb_Origin;
        private System.ComponentModel.BackgroundWorker bg_Worker;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_Quality;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_Level;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_NearLossless;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_Method;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_Pass;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_FilterSharpness;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_AutoFilter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_FilterType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_SharpYUV;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_QMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_QMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_TargetPSNR;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_Size;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_PSNR;
        private System.Windows.Forms.DataGridViewTextBoxColumn ic_Time;
    }
}