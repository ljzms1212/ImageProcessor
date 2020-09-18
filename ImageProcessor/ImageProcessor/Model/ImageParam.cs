using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageProcessor.Model
{
    /// <summary>
    /// 图片参数
    /// </summary>
    public class ImageParam
    {
        /// <summary>
        /// 图片目录
        /// </summary>
        public string PicPath { get; set; }

        /// <summary>
        /// 调整维度
        /// </summary>
        /// <remarks>宽高</remarks>
        public bool ChangeDimension { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 调整 dpi
        /// </summary>
        public bool ChangeDpi { get; set; }

        /// <summary>
        /// x 像素点
        /// </summary>
        public int Xdpi { get; set; }

        /// <summary>
        /// y 像素点
        /// </summary>
        public int Ydpi { get; set; }

    }
}
