namespace ImageProcessor
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tbximgpath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxWidth = new System.Windows.Forms.TextBox();
            this.tbxYDpi = new System.Windows.Forms.TextBox();
            this.tbxXdpi = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxChangeDpi = new System.Windows.Forms.CheckBox();
            this.cbxChangeDimension = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "请输入图片路径：";
            // 
            // tbximgpath
            // 
            this.tbximgpath.Location = new System.Drawing.Point(230, 57);
            this.tbximgpath.Name = "tbximgpath";
            this.tbximgpath.Size = new System.Drawing.Size(403, 21);
            this.tbximgpath.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(228, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "宽度(px)：";
            // 
            // tbxWidth
            // 
            this.tbxWidth.Location = new System.Drawing.Point(322, 112);
            this.tbxWidth.Name = "tbxWidth";
            this.tbxWidth.Size = new System.Drawing.Size(100, 21);
            this.tbxWidth.TabIndex = 4;
            // 
            // tbxYDpi
            // 
            this.tbxYDpi.Location = new System.Drawing.Point(533, 170);
            this.tbxYDpi.Name = "tbxYDpi";
            this.tbxYDpi.Size = new System.Drawing.Size(100, 21);
            this.tbxYDpi.TabIndex = 9;
            // 
            // tbxXdpi
            // 
            this.tbxXdpi.Location = new System.Drawing.Point(322, 170);
            this.tbxXdpi.Name = "tbxXdpi";
            this.tbxXdpi.Size = new System.Drawing.Size(100, 21);
            this.tbxXdpi.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(482, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "YDPI：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(252, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "XDPI：";
            // 
            // cbxChangeDpi
            // 
            this.cbxChangeDpi.AutoSize = true;
            this.cbxChangeDpi.Location = new System.Drawing.Point(83, 175);
            this.cbxChangeDpi.Name = "cbxChangeDpi";
            this.cbxChangeDpi.Size = new System.Drawing.Size(96, 16);
            this.cbxChangeDpi.TabIndex = 10;
            this.cbxChangeDpi.Text = "是否设置 DPI";
            this.cbxChangeDpi.UseVisualStyleBackColor = true;
            // 
            // cbxChangeDimension
            // 
            this.cbxChangeDimension.AutoSize = true;
            this.cbxChangeDimension.Location = new System.Drawing.Point(83, 114);
            this.cbxChangeDimension.Name = "cbxChangeDimension";
            this.cbxChangeDimension.Size = new System.Drawing.Size(96, 16);
            this.cbxChangeDimension.TabIndex = 11;
            this.cbxChangeDimension.Text = "是否调整大小";
            this.cbxChangeDimension.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(298, 281);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(140, 55);
            this.button1.TabIndex = 12;
            this.button1.Text = "开始执行";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(83, 231);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(550, 23);
            this.progressBar1.TabIndex = 13;
            this.progressBar1.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbxChangeDimension);
            this.Controls.Add(this.cbxChangeDpi);
            this.Controls.Add(this.tbxYDpi);
            this.Controls.Add(this.tbxXdpi);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbxWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbximgpath);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "图片处理小工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbximgpath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxWidth;
        private System.Windows.Forms.TextBox tbxYDpi;
        private System.Windows.Forms.TextBox tbxXdpi;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbxChangeDpi;
        private System.Windows.Forms.CheckBox cbxChangeDimension;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

