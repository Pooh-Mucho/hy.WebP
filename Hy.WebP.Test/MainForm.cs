using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using hy.WebP;

namespace Hy.WebP.Test
{
    public partial class MainForm : Form
    {
        private Image _originImage;
        private byte[] _originBytes;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            if (of_Image.ShowDialog(this) == DialogResult.OK) {
                _originBytes = File.ReadAllBytes(of_Image.FileName);
                _originImage = Image.FromStream(new MemoryStream(_originBytes), false, false);
                pb_Image.Image = _originImage;
                pb_Image.Width = _originImage.Width + 10;
                pb_Image.Height = _originImage.Height + 10;
                lb_Message.Text = string.Format(
                    "origin: {0:N0} bytes", _originBytes.Length
                    );
            }
        }

        private void btn_Origin_Click(object sender, EventArgs e)
        {
            if (_originImage != null) {
                pb_Image.Image = _originImage;
                lb_Message.Text = string.Format(
                    "origin: {0:N0} bytes", _originBytes.Length
                    );
            }
        }

        private void btn_Encode_Lossless_Click(object sender, EventArgs e)
        {
            if (_originImage == null)
                return;
            Stopwatch watch = new Stopwatch();
            string nearLosslessText = ((Button)sender).Text.Replace("%", "");
            int nearLossless = int.Parse(nearLosslessText);
            watch.Start();
            WebPConfig config;
            config = WebPCodec.CreateConfigLossless(9);
            config.Method = 6;
            config.UseSharpYuv = true;
            config.NearLossless = nearLossless;
            config.FilterStrength = 100;
            config.Pass = 1;
            byte[] webpBytes;
            if (nearLossless % 2 == 0) {
                webpBytes = WebPCodec.Encode(config, _originImage);
            }
            else {
                ImageArgbData argbData = ImageArgbData.FromImage(_originImage);
                webpBytes = WebPCodec.Encode(config, argbData);
            }
            watch.Stop();
            TimeSpan ts1 = watch.Elapsed;
            watch.Reset();
            watch.Start();
            Image image = WebPCodec.Decode(webpBytes);
            watch.Stop();
            TimeSpan ts2 = watch.Elapsed;
            pb_Image.Image = image;
            double psnr = WebPCodec.PSNR(_originImage, image);
            lb_Message.Text = string.Format(
                "origin: {0:N0} bytes, webp: {1:N0} bytes, encode: {2:N2} seconds, decode: {3:N2} seconds, psnr: {4:N2} dB",
                _originBytes.Length, webpBytes.Length, ts1.TotalSeconds, ts2.TotalSeconds, psnr
                );
        }

        private void btn_Encode_Click(object sender, EventArgs e)
        {
            if (_originImage == null)
                return;
            Stopwatch watch = new Stopwatch();
            string qualityText = ((Button)sender).Text.Replace("%", "");
            int quality = int.Parse(qualityText);
            watch.Start();
            WebPConfig config;
            config = WebPCodec.CreateConfig(quality);
            config.Method = 6;
            config.UseSharpYuv = true;
            config.FilterSharpness = 7;
            config.Pass = 5;
            /*
            config.AutoFilter = true;
            config.FilterType = 1;
            */
            config.FilterStrength = 100;
            byte[] webpBytes;
            if (quality % 2 == 0) {
                webpBytes = WebPCodec.Encode(config, _originImage);
            }
            else {
                ImageArgbData argbData = ImageArgbData.FromImage(_originImage);
                webpBytes = WebPCodec.Encode(config, argbData);
            }
            watch.Stop();
            TimeSpan ts1 = watch.Elapsed;
            watch.Reset();
            watch.Start();
            Image image = WebPCodec.Decode(webpBytes);
            watch.Stop();
            TimeSpan ts2 = watch.Elapsed;
            pb_Image.Image = image;
            double psnr = WebPCodec.PSNR(_originImage, image);
            lb_Message.Text = string.Format(
                "origin: {0:N0} bytes, webp: {1:N0} bytes, encode: {2:N2} seconds, decode: {3:N2} seconds, psnr: {4:N2} dB",
                _originBytes.Length, webpBytes.Length, ts1.TotalSeconds, ts2.TotalSeconds, psnr
                );
        }

        private void btn_Lossless_Click(object sender, EventArgs e)
        {
            if (_originImage == null) return;
            using (var form = new BatchForm()) {
                form.Type = "Lossless";
                form.OriginBytes = _originBytes;
                form.OriginImage = _originImage;
                form.ShowDialog();
            }
        }

        private void btn_Lossy_Click(object sender, EventArgs e)
        {
            if (_originImage == null) return;
            using (var form = new BatchForm()) {
                form.Type = "Lossy";
                form.OriginBytes = _originBytes;
                form.OriginImage = _originImage;
                form.ShowDialog();
            }
        }

    }
}
