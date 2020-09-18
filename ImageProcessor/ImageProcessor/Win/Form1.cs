using ImageProcessor.Core;
using ImageProcessor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessor
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 模板目录
        /// </summary>
        const string targetPath = "target";
        /// <summary>
        /// 备份目录
        /// </summary>
        const string bakpath = "bak";


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ImageParam imageParam = this.PrepareData();
            if (!this.ValidateData(imageParam))
            {
                return;
            }

            //开始去水印:获取文件 ，只取第一层级
            var files = Directory.GetFiles(imageParam.PicPath, "*.jpg");

            if (files?.Length == 0)
            {
                MessageBox.Show("路径下不存在任何图片");
                return;
            }

            //多线程处理图片， 默认使用本机处理器数 * 2;
            ParallelOptions parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount * 2
                //MaxDegreeOfParallelism = 1
            };

            ImageProcessService service = new ImageProcessService();

            string newtargetpath = Path.Combine(imageParam.PicPath, targetPath);
            string newbakpath = Path.Combine(imageParam.PicPath, bakpath);
            if (!Directory.Exists(newtargetpath))
            {
                Directory.CreateDirectory(newtargetpath);
            }
            if (!Directory.Exists(newbakpath))
            {
                Directory.CreateDirectory(newbakpath);
            }

            //开始处理图片

            //显示进度条
            progressBar1.Visible = true;
            progressBar1.Value = 0;  //清空进度条
            int len = files.Length, flag = 1;
            Parallel.ForEach(files, parallelOptions, (file) =>
            {
                string fileName = Path.GetFileName(file);

                //压缩图片，返回图片流
                using (Image fileStream = Image.FromFile(file))
                {
                    MemoryStream memoryStream = service.GetImagePress(fileStream, imageParam);
                    //保存图片到新目录
                    Image.FromStream(memoryStream).Save(Path.Combine(newtargetpath, fileName));
                }

                //移动旧图到备份目录  
                new FileInfo(file).MoveTo(Path.Combine(newbakpath, fileName));

                //展示进度
                int newbar = Convert.ToInt32(Math.Ceiling(flag++ * 100.0 / len));
                if (progressBar1.Value < newbar)
                {
                    progressBar1.Value = newbar;
                }

                //Thread.Sleep(1000);
                Application.DoEvents();
            });

            MessageBox.Show("工作完成！");
        }

        private ImageParam PrepareData()
        {
            int.TryParse(this.tbxWidth.Text, out int width);
            int.TryParse(this.tbxXdpi.Text, out int xdpi);
            int.TryParse(this.tbxYDpi.Text, out int ydpi);

            return new ImageParam
            {
                PicPath = this.tbximgpath.Text,
                ChangeDimension = this.cbxChangeDimension.Checked,
                Width = width, 
                ChangeDpi = this.cbxChangeDpi.Checked,
                Xdpi = xdpi,
                Ydpi = ydpi
            };
        }


        private bool ValidateData(ImageParam imageParam)
        {
            //图片路径
            if (!Directory.Exists(imageParam.PicPath))
            {
                MessageBox.Show("图片路径不存在");
                return false;
            }

            if (imageParam.ChangeDimension)
            {
                if (imageParam.Width < 0)
                {
                    MessageBox.Show($"图片宽度 {imageParam.Width} 不合法");
                    return false;
                } 
            }

            if (imageParam.ChangeDpi)
            {
                if (imageParam.Xdpi < 0)
                {
                    MessageBox.Show($"图片 XDPI {imageParam.Xdpi} 不合法");
                    return false;
                }

                if (imageParam.Ydpi < 0)
                {
                    MessageBox.Show($"图片 YDPI {imageParam.Ydpi} 不合法");
                    return false;
                }
            }

            if (!imageParam.ChangeDimension && !imageParam.ChangeDpi)
            {
                MessageBox.Show($"至少要选择一个操作选项");
                return false;
            }
            return true;
        }

    }
}
