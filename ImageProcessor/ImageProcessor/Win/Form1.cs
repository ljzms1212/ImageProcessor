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
using System.Text.RegularExpressions;
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
            List<string> files = new List<string>();
            files.AddRange(Directory.GetFiles(imageParam.PicPath, "*.jpg"));

            foreach (var dir in Directory.GetDirectories(imageParam.PicPath))
            {
                if (dir.EndsWith(targetPath, StringComparison.InvariantCultureIgnoreCase)
                    || dir.EndsWith(bakpath, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                files.AddRange(Directory.GetFiles(dir, "*.jpg", SearchOption.AllDirectories));
            }

            if (!files.Any())
            {
                MessageBox.Show("路径下不存在任何图片");
                return;
            }

            string rootpath = imageParam.PicPath.TrimEnd('\\');
            string filepattern = $"{rootpath}\\(?<subpath>.*)\\.*.jpg";
            filepattern = filepattern.Replace("\\", "\\\\");

            //多线程处理图片， 默认使用本机处理器数 * 2;
            ParallelOptions parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount * 2
                //MaxDegreeOfParallelism = 1
            };

            ImageProcessService service = new ImageProcessService();

            string newtargetpath = Path.Combine(rootpath, targetPath);
            string newbakpath = Path.Combine(rootpath, bakpath);

            //开始处理图片

            //显示进度条
            progressBar1.Visible = true;
            progressBar1.Value = 0;  //清空进度条
            int len = files.Count, flag = 1;
            Parallel.ForEach(files, parallelOptions, (file) =>
            {
                string fileName = Path.GetFileName(file);
                Match match = Regex.Match(file, filepattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string subPath = match.Groups["subpath"].Value;
                    string targetFileName = Path.Combine(rootpath, targetPath, subPath, fileName);
                    string bakfilename = Path.Combine(rootpath, bakpath, subPath, fileName);

                    CreateDirectory(targetFileName);
                    CreateDirectory(bakfilename);

                    //压缩图片，返回图片流
                    using (Image fileStream = Image.FromFile(file))
                    {
                        MemoryStream memoryStream = service.GetImagePress(fileStream, imageParam);
                        //保存图片到新目录
                        Image.FromStream(memoryStream).Save(targetFileName);
                    }

                    //移动旧图到备份目录  
                    new FileInfo(file).MoveTo(bakfilename);

                    //如果目录为空，则删除目录
                    //需要考虑多级为空，依次递归删除至顶层目录
                    

                    //展示进度
                    int newbar = Convert.ToInt32(Math.Ceiling(flag++ * 100.0 / len));
                    if (progressBar1.Value < newbar)
                    {
                        progressBar1.Value = newbar;
                    }
                }

                //Thread.Sleep(1000);
                Application.DoEvents();
            });

            MessageBox.Show("工作完成！");
        }

        private static readonly object cre_locker = new object();
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="filename"></param>
        private static void CreateDirectory(string filename)
        {
            lock (cre_locker)
            {
                string path = Path.GetDirectoryName(filename);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        private ImageParam PrepareData()
        {
            int.TryParse(this.tbxWidth.Text, out int width);
            int.TryParse(this.tbxHeight.Text, out int height);
            int.TryParse(this.tbxXdpi.Text, out int xdpi);
            int.TryParse(this.tbxYDpi.Text, out int ydpi);

            return new ImageParam
            {
                PicPath = this.tbximgpath.Text,
                ChangeDimension = this.cbxChangeDimension.Checked,
                Width = width,
                Height = height,
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

                if (imageParam.Height < 0)
                {
                    MessageBox.Show($"图片高度 {imageParam.Height} 不合法");
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
