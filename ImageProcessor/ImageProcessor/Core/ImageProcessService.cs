using ImageProcessor.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace ImageProcessor.Core
{
    /// <summary>
    /// 图片处理服务
    /// </summary>
    public class ImageProcessService
    {
        private static PixelFormat[] indexedPixelFormats = new PixelFormat[]{
            PixelFormat.Undefined,
            PixelFormat.DontCare,
            PixelFormat.Format16bppArgb1555,
            PixelFormat.Format1bppIndexed,
            PixelFormat.Format4bppIndexed,
            PixelFormat.Format8bppIndexed
        };

        public virtual MemoryStream GetImagePress(Image image, ImageParam imageParam)
        { 

            //处理jpeg图片
            if (image.PixelFormat.ToString() == "8207"
                || IsPixelFormatIndexed(image.PixelFormat))
            {
                image = ConvertCmykToRgb(image);
            }

            //转换 size
            int width = image.Width;
            int height = image.Height;
            if (imageParam.ChangeDimension)
            {
                width = imageParam.Width;
                height = imageParam.Height;
            }

            // 获取图片比率 ,等比例压缩， width和height需要进行换算
            var aspectRatio = Convert.ToDouble(image.Width) / Convert.ToDouble(image.Height);
            int tempHeight = Convert.ToInt32(width / aspectRatio);
            if (tempHeight > height)
            {
                width = Convert.ToInt32(height * aspectRatio);
            }
            else
            {
                height = tempHeight;
            }

            //根据指定大小创建Bitmap实例  
            Bitmap bt = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bt))
            {
                g.Clear(Color.White);
                //设置画布的描绘质量  
                Rectangle rect = new Rectangle((imageParam.Width - width) / 2, (imageParam.Height - height) / 2, width, height);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, rect);
                g.Dispose();
            }

            EncoderParameters EncoderParams = new EncoderParameters(); //取得内置的编码器  
            long[] Quality = new long[1];
            Quality[0] = 100;
            EncoderParameter EncoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Quality);
            EncoderParams.Param[0] = EncoderParam;

            try
            {
                //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象  
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICI = null;
                for (int i = 0; i < arrayICI.Length; i++)
                {
                    if (arrayICI[i].FormatDescription.Equals("JPEG"))
                    {
                        jpegICI = arrayICI[i]; //设置为JPEG编码方式  
                        break;
                    }
                }

                bt = Sharpen(bt, 0.5f);


                if (imageParam.Xdpi > 0 && imageParam.Ydpi > 0)
                {
                    //100分辨率
                    bt.SetResolution(imageParam.Xdpi, imageParam.Ydpi);
                }

                MemoryStream newStream = new MemoryStream();
                if (jpegICI != null) //保存缩略图  
                {
                    bt.Save(newStream, jpegICI, EncoderParams);
                }
                else
                {
                    bt.Save(newStream, ImageFormat.Jpeg);
                }
                return newStream;
            }
            catch { return null; }
        }


        #region private
        private bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            foreach (PixelFormat pf in indexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }

            return false;
        }

        /// <summary>
        /// 处理jpeg图片
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private Bitmap ConvertCmykToRgb(Image image)
        {
            Image tmpImage = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);

            Graphics g = Graphics.FromImage(tmpImage);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            g.DrawImage(image, rect);

            Bitmap returnBmp = new Bitmap(tmpImage);

            g.Dispose();
            tmpImage.Dispose();
            image.Dispose();
            return returnBmp;
        }

        private Bitmap Sharpen(Bitmap bitmap, float sharpenValue)
        {
            if (bitmap == null)
            {
                return null;
            }

            int width = bitmap.Width;
            int height = bitmap.Height;

            try
            {

                Bitmap bmpRtn = new Bitmap(width, height, PixelFormat.Format24bppRgb);

                BitmapData srcBits = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                BitmapData targetBits = bmpRtn.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* pSrcBits = (byte*)srcBits.Scan0.ToPointer();
                    byte* pTargetBits = (byte*)targetBits.Scan0.ToPointer();
                    int stride = srcBits.Stride;
                    byte* pTemp;
                    for (int h = 0; h < height; h++)
                    {
                        for (int w = 0; w < width; w++)
                        {
                            if (w == 0 || w == width - 1 || h == 0 || h == height - 1)
                            {
                                //最边上的像素不处理
                                pTargetBits[0] = pSrcBits[0];
                                pTargetBits[1] = pSrcBits[1];
                                pTargetBits[2] = pSrcBits[2];
                            }
                            else
                            {
                                //取周围9点的值。位于边缘上的点不做改变。
                                int r1, r2, r3, r4, r5, r6, r7, r8, r0;
                                int g1, g2, g3, g4, g5, g6, g7, g8, g0;
                                int b1, b2, b3, b4, b5, b6, b7, b8, b0;

                                float fR, fG, fB;

                                //左上
                                pTemp = pSrcBits - stride - 3;
                                r1 = pTemp[2];
                                g1 = pTemp[1];
                                b1 = pTemp[0];

                                //正上
                                pTemp = pSrcBits - stride;
                                r2 = pTemp[2];
                                g2 = pTemp[1];
                                b2 = pTemp[0];

                                //右上
                                pTemp = pSrcBits - stride + 3;
                                r3 = pTemp[2];
                                g3 = pTemp[1];
                                b3 = pTemp[0];

                                //左侧
                                pTemp = pSrcBits - 3;
                                r4 = pTemp[2];
                                g4 = pTemp[1];
                                b4 = pTemp[0];

                                //右侧
                                pTemp = pSrcBits + 3;
                                r5 = pTemp[2];
                                g5 = pTemp[1];
                                b5 = pTemp[0];

                                //右下
                                pTemp = pSrcBits + stride - 3;
                                r6 = pTemp[2];
                                g6 = pTemp[1];
                                b6 = pTemp[0];

                                //正下
                                pTemp = pSrcBits + stride;
                                r7 = pTemp[2];
                                g7 = pTemp[1];
                                b7 = pTemp[0];

                                //右下
                                pTemp = pSrcBits + stride + 3;
                                r8 = pTemp[2];
                                g8 = pTemp[1];
                                b8 = pTemp[0];

                                //自己
                                pTemp = pSrcBits;
                                r0 = pTemp[2];
                                g0 = pTemp[1];
                                b0 = pTemp[0];

                                fR = (float)r0 - (float)(r1 + r2 + r3 + r4 + r5 + r6 + r7 + r8) / 8;
                                fG = (float)g0 - (float)(g1 + g2 + g3 + g4 + g5 + g6 + g7 + g8) / 8;
                                fB = (float)b0 - (float)(b1 + b2 + b3 + b4 + b5 + b6 + b7 + b8) / 8;

                                fR = r0 + fR * sharpenValue;
                                fG = g0 + fG * sharpenValue;
                                fB = b0 + fB * sharpenValue;

                                if (fR > 0)
                                {
                                    fR = Math.Min(255, fR);
                                }
                                else
                                {
                                    fR = Math.Max(0, fR);
                                }

                                if (fG > 0)
                                {
                                    fG = Math.Min(255, fG);
                                }
                                else
                                {
                                    fG = Math.Max(0, fG);
                                }

                                if (fB > 0)
                                {
                                    fB = Math.Min(255, fB);
                                }
                                else
                                {
                                    fB = Math.Max(0, fB);
                                }

                                pTargetBits[0] = (byte)fB;
                                pTargetBits[1] = (byte)fG;
                                pTargetBits[2] = (byte)fR;

                            }
                            pSrcBits += 3;
                            pTargetBits += 3;
                        }
                        pSrcBits += srcBits.Stride - width * 3;
                        pTargetBits += srcBits.Stride - width * 3;
                    }
                }

                bitmap.UnlockBits(srcBits);
                bmpRtn.UnlockBits(targetBits);

                return bmpRtn;
            }
            catch
            {
                return null;
            }

        }

        #endregion

    }
}
