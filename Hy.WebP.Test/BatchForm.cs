using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using hy.WebP;

namespace Hy.WebP.Test
{
    public partial class BatchForm : Form
    {
        private DataTable _images;

        public byte[] OriginBytes;
        public Image OriginImage;
        public ImageArgbData OriginArgb;
        public string Type;

        public BatchForm()
        {
            InitializeComponent();
        }

        private void initializeTable()
        {
            _images = new DataTable();
            _images.Columns.Add("Id", typeof(int));
            _images.Columns.Add("Type", typeof(string));
            _images.Columns.Add("Quality", typeof(int));
            _images.Columns.Add("Level", typeof(int));
            _images.Columns.Add("NearLossless", typeof(int));
            _images.Columns.Add("Method", typeof(int));
            _images.Columns.Add("Pass", typeof(int));
            _images.Columns.Add("FilterSharpness", typeof(int));
            _images.Columns.Add("AutoFilter", typeof(int));
            _images.Columns.Add("FilterType", typeof(int));
            _images.Columns.Add("SharpYUV", typeof(int));
            _images.Columns.Add("QMin", typeof(int));
            _images.Columns.Add("QMax", typeof(int));
            _images.Columns.Add("TargetPSNR", typeof(int));
            _images.Columns.Add("Size", typeof(int));
            _images.Columns.Add("PSNR", typeof(double));
            _images.Columns.Add("Time", typeof(double));
            gv_Images.AutoGenerateColumns = false;
            _images.DefaultView.Sort = "Id";
            gv_Images.DataSource = _images.DefaultView;
        }

        private List<Task> CreateTasks()
        {

            List<Task> tasks = new List<Task>();
            int id = 1;
            if (Type == "Lossless") {
                int[] nearLosslessValues = new int[] { 0, 25, 50, 75, 90, 100 };
                for (int level = 5; level <= 9; level++) {
                    for (int method = 1; method <= 6; method++) {
                        foreach (int nearLossless in nearLosslessValues) {
                            for (int pass = 1; pass <= 1; pass++) {
                                for (int sharpYuv = 1; sharpYuv <= 1; sharpYuv++) {
                                    Task t = new Task();
                                    t.Id = id++;
                                    t.Type = "Lossless";
                                    t.Level = level;
                                    t.NearLossless = nearLossless;
                                    t.Method = method;
                                    t.Pass = pass;
                                    t.SharpYUV = sharpYuv;
                                    tasks.Add(t);
                                }
                            }
                        }
                    }
                }
            }
            else {
                int[] qualities = new int[] { 100, 99, 98, 96, 94, 90, 85, 80, 75 };
                foreach (int quality in qualities) {
                    for (int method = 1; method <= 6; method++) {
                        for (int pass = 1; pass <= 10; pass++) {
                            for (int filterSharpness = 7; filterSharpness <= 7; filterSharpness++) {
                                //for (int autoFilter = 0; autoFilter <= 1; autoFilter++) {
                                //    int[] filterTypes = autoFilter == 1 ? new int[] { 0, 1 } : new int[] { 0 };
                                //    foreach (int filterType in filterTypes) {
                                        for (int sharpYuv = 1; sharpYuv <= 1; sharpYuv++) {
                                            Task t = new Task();
                                            t.Id = id++;
                                            t.Type = "Lossy";
                                            t.Quality = quality;
                                            t.Method = method;
                                            t.Pass = pass;
                                            t.FilterSharpness = filterSharpness;
                                            //t.AutoFilter = autoFilter;
                                            //t.FilterType = filterType;
                                            t.SharpYUV = sharpYuv;
                                            t.QMin = 0;
                                            t.QMax = 100;
                                            tasks.Add(t);
                                        }
                                 //   }
                                //}
                            }
                        }
                    }
                }
                float[] psnrs = new float[] { 80, 50, 48, 45, 43, 42 };
                foreach (float psnr in psnrs) {
                    foreach (int quality in qualities) {
                        Task t = new Task();
                        t.Id = id++;
                        t.Type = "Lossy";
                        t.Quality = quality;
                        t.Method = 6;
                        t.Pass = 10;
                        t.FilterSharpness = 7;
                        t.SharpYUV = 1;
                        t.TargetPSNR = psnr;
                        tasks.Add(t);
                    }
                }
            }
            return tasks;
        }

        private void BatchForm_Load(object sender, EventArgs e)
        {
            lb_Origin.Text = string.Format( "origin: {0:N0} bytes", OriginBytes.Length);
            initializeTable();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            _images.Rows.Clear();
            _images.AcceptChanges();
            bg_Worker.RunWorkerAsync();
            btn_Start.Enabled = false;
            btn_Stop.Enabled = true;
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            bg_Worker.CancelAsync();
            btn_Stop.Enabled = false;
        }

        private void bg_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try {
                List<Task> tasks = CreateTasks();
                foreach (Task t in tasks) {
                    if (bg_Worker.CancellationPending)
                        break;
                    if (OriginArgb == null)
                        OriginArgb = WebPCodec.DumpArgbData(OriginImage);
                    t.Execute(OriginArgb);
                    bg_Worker.ReportProgress(0, t);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bg_Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Task t = e.UserState as Task;
            if (t == null) return;
            try {
                DataRow row = _images.NewRow();
                row["Id"] = t.Id;
                row["Type"] = t.Type;
                if (t.Quality != null) row["Quality"] = t.Quality.Value;
                if (t.Level != null) row["Level"] = t.Level.Value;
                if (t.NearLossless != null) row["NearLossless"] = t.NearLossless.Value;
                if (t.Method != null) row["Method"] = t.Method.Value;
                if (t.Pass != null) row["Pass"] = t.Pass.Value;
                if (t.FilterSharpness != null) row["FilterSharpness"] = t.FilterSharpness.Value;
                if (t.AutoFilter != null) row["AutoFilter"] = t.AutoFilter.Value;
                if (t.FilterType != null) row["FilterType"] = t.FilterType.Value;
                if (t.SharpYUV != null) row["SharpYUV"] = t.SharpYUV.Value;
                if (t.QMin != null) row["QMin"] = t.QMin.Value;
                if (t.QMax != null) row["QMax"] = t.QMax.Value;
                if (t.TargetPSNR != null) row["TargetPSNR"] = t.TargetPSNR.Value;
                row["Size"] = t.Size;
                row["PSNR"] = t.PSNR;
                row["Time"] = t.Time;
                _images.Rows.Add(row);
                _images.AcceptChanges();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bg_Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btn_Start.Enabled = true;
            btn_Stop.Enabled = false;
        }

        private class Task
        {
            public int Id;
            public string Type;
            public int? Method;
            public int? Level;
            public int? NearLossless;
            public int? Quality;
            public int? Pass;
            public int? FilterSharpness;
            public int? AutoFilter;
            public int? FilterType;
            public int? SharpYUV;
            public int? QMin;
            public int? QMax;
            public float? TargetPSNR;
            public int Size;
            public double PSNR;
            public double Time;

            public void Execute(ImageArgbData image)
            {
                WebPConfig config;
                if (Type == "Lossless") {
                    config = WebPCodec.CreateConfigLossless(Level.Value);
                }
                else {
                    config = WebPCodec.CreateConfig(Quality.Value);
                }
                if (Method != null) config.Method = Method.Value;
                if (NearLossless != null) config.NearLossless = NearLossless.Value;
                if (Pass != null) config.Pass = Pass.Value;
                if (FilterSharpness != null)
                    config.FilterSharpness = FilterSharpness.Value;
                else
                    config.FilterSharpness = 7;
                config.FilterStrength = 100;
                if (AutoFilter != null) config.AutoFilter = AutoFilter.Value == 1;
                if (FilterType != null) config.FilterType = FilterType.Value;
                if (SharpYUV != null) config.UseSharpYuv = SharpYUV.Value == 1;
                if (SharpYUV != null) config.UseSharpYuv = SharpYUV.Value == 1;
                if (QMin != null) config.QMin = QMin.Value;
                if (QMax != null) config.QMax = QMax.Value;
                if (TargetPSNR != null) config.TargetPSNR = TargetPSNR.Value;

                Stopwatch sw = Stopwatch.StartNew();
                byte[] webpBytes = WebPCodec.Encode(config, image);
                sw.Stop();
                Size = webpBytes.Length;
                Time = sw.Elapsed.TotalSeconds;
                ImageArgbData webpImage = WebPCodec.DecodeToArgb(webpBytes);
                PSNR = WebPCodec.PSNR(image, webpImage);
            }

        }

    }
}
