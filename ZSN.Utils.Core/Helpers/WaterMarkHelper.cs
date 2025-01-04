using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ZSN.Utils.Core.Util
{
    /// <summary>
    /// 给图片添加水印得类得描述
    /// </summary>
    public class WaterMarkHelper
    {
        private readonly int[] _sizes = { 48, 32, 16, 8, 6, 4 };
        private float _transparency = 1;

        /// <summary>
        /// 实例化一个水印类
        /// </summary>
        public WaterMarkHelper()
        {
        }

        /// <summary>
        ///　初始化一个只添加文字水印得实例
        /// </summary>
        /// <param name="text">水印文字</param>
        /// <param name="fontFamily">文字字体</param>
        /// <param name="bold">是否粗体</param>
        /// <param name="color">字体颜色</param>
        /// <param name="markX">标记位置横坐标</param>
        /// <param name="markY">标记位置纵坐标</param>
        public WaterMarkHelper(string text, string fontFamily, bool bold, Color color, int markX, int markY)
        {
            ImageMark = false;
            TextMark = true;
            Text = text;
            TextFontFamily = fontFamily;
            Bold = bold;
            TextColor = color;
            MarkX = markX;
            MarkY = MarkY;
        }

        /// <summary>
        /// 实例化一个只添加图片水印得实例
        /// </summary>
        /// <param name="imagePath">水印图片路径</param>
        /// <param name="tranparence">透明度</param>
        /// <param name="markX">标记位置横坐标</param>
        /// <param name="markY">标记位置纵坐标</param>
        public WaterMarkHelper(string imagePath, float tranparence, int markX, int markY)
        {
            TextMark = false;
            ImageMark = true;
            ImagePath = imagePath;
            MarkX = markX;
            MarkY = MarkY;
            _transparency = tranparence;
        }

        public static Image DefaultWaterMark
        {
            get
            {
                //                return Resources.ResourceManager.GetObject("watermark") as Bitmap;
                return null;
            }
        }

        /// <summary>
        /// 是否添加文字水印
        /// </summary>
        public bool TextMark { get; set; }

        /// <summary>
        /// 是否添加图片水印
        /// </summary>
        public bool ImageMark { get; set; }

        /// <summary>
        /// 文字水印得内容
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// 图片水印得图片地址
        /// </summary>
        public string ImagePath { get; set; } = "";

        /// <summary>
        /// 添加水印位置得横坐标
        /// </summary>
        public int MarkX { get; set; }

        /// <summary>
        /// 添加水印位置得纵坐标
        /// </summary>
        public int MarkY { get; set; }

        /// <summary>
        /// 水印得透明度
        /// </summary>
        public float Transparency
        {
            get
            {
                if (_transparency > 1.0f) _transparency = 1.0f;
                return _transparency;
            }
            set => _transparency = value;
        }

        /// <summary>
        /// 水印文字得颜色
        /// </summary>
        public Color TextColor { get; set; } = Color.Black;

        /// <summary>
        /// 水印文字得字体
        /// </summary>
        public string TextFontFamily { get; set; } = "宋体";

        /// <summary>
        /// 水印文字是否加粗
        /// </summary>
        public bool Bold { get; set; }

        /// <summary>
        /// 添加水印，此方法适用于gif格式得图片
        /// </summary>
        /// <param name="img">需要添加水印得图片</param>
        /// <returns>添加水印之后得图片</returns>
        public Image Mark(Image img)
        {
            try
            {
                //添加文字水印
                if (TextMark)
                {
                    //根据源图片生成新的Bitmap对象作为作图区，为了给gif图片添加水印，才有此周折
                    var newBitmap = new Bitmap(img.Width, img.Height, PixelFormat.Format24bppRgb);
                    //设置新建位图得分辨率
                    newBitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                    //创建Graphics对象，以对该位图进行操作
                    var g = Graphics.FromImage(newBitmap);
                    //消除锯齿
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    //将原图拷贝到作图区
                    g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                    //声明字体对象
                    Font cFont = null;
                    //用来测试水印文本长度得尺子
                    var size = new SizeF();
                    //探测出一个适合图片大小得字体大小，以适应水印文字大小得自适应
                    for (var i = 0; i < 6; i++)
                    {
                        //创建一个字体对象
                        cFont = new Font(TextFontFamily, _sizes[i]);
                        //是否加粗
                        if (!Bold)
                            cFont = new Font(TextFontFamily, _sizes[i], FontStyle.Regular);
                        else
                            cFont = new Font(TextFontFamily, _sizes[i], FontStyle.Bold);
                        //测量文本大小
                        size = g.MeasureString(Text, cFont);
                        //匹配第一个符合要求得字体大小
                        if ((ushort)size.Width < (ushort)img.Width) break;
                    }
                    //创建刷子对象，准备给图片写上文字
                    Brush brush = new SolidBrush(TextColor);
                    //在指定得位置写上文字
                    g.DrawString(Text, cFont, brush, MarkX, MarkY);
                    //释放Graphics对象
                    g.Dispose();
                    //将生成得图片读入MemoryStream
                    var ms = new MemoryStream();
                    newBitmap.Save(ms, ImageFormat.Jpeg);
                    //重新生成Image对象
                    img = Image.FromStream(ms);
                    //返回新的Image对象
                    return img;
                }
                //添加图像水印
                if (ImageMark)
                {
                    //获得水印图像
                    Image markImg;
                    if (string.IsNullOrEmpty(ImagePath))
                        markImg = DefaultWaterMark;
                    else
                        markImg = Image.FromFile(ImagePath);
                    //创建颜色矩阵
                    float[][] ptsArray ={
                          new float[] {1, 0, 0, 0, 0},
                          new float[] {0, 1, 0, 0, 0},
                          new float[] {0, 0, 1, 0, 0},
                          new[] {0, 0, 0, Transparency, 0}, //注意：此处为0.0f为完全透明，1.0f为完全不透明
                          new float[] {0, 0, 0, 0, 1}};
                    var colorMatrix = new ColorMatrix(ptsArray);
                    //新建一个Image属性
                    var imageAttributes = new ImageAttributes();
                    //将颜色矩阵添加到属性
                    imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default,
                      ColorAdjustType.Default);
                    //生成位图作图区
                    var newBitmap = new Bitmap(img.Width, img.Height, PixelFormat.Format24bppRgb);
                    //设置分辨率
                    newBitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                    //创建Graphics
                    var g = Graphics.FromImage(newBitmap);
                    //消除锯齿
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    //拷贝原图到作图区
                    g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                    //如果原图过小
                    if (markImg.Width > img.Width || markImg.Height > img.Height)
                    {
                        Image.GetThumbnailImageAbort callb = null;
                        //对水印图片生成缩略图,缩小到原图得1/4
                        var new_img = markImg.GetThumbnailImage(img.Width / 4, markImg.Height * img.Width / markImg.Width, callb, new IntPtr());
                        //添加水印
                        g.DrawImage(new_img, new Rectangle(MarkX, MarkY, new_img.Width, new_img.Height), 0, 0, new_img.Width, new_img.Height, GraphicsUnit.Pixel, imageAttributes);
                        //释放缩略图
                        new_img.Dispose();
                        //释放Graphics
                        g.Dispose();
                        //将生成得图片读入MemoryStream
                        var ms = new MemoryStream();
                        newBitmap.Save(ms, ImageFormat.Jpeg);
                        //返回新的Image对象
                        img = Image.FromStream(ms);
                        return img;
                    }
                    //原图足够大
                    else
                    {
                        //添加水印
                        g.DrawImage(markImg, new Rectangle(MarkX, MarkY, markImg.Width, markImg.Height), 0, 0, markImg.Width, markImg.Height, GraphicsUnit.Pixel, imageAttributes);
                        //释放Graphics
                        g.Dispose();
                        //将生成得图片读入MemoryStream
                        var ms = new MemoryStream();
                        newBitmap.Save(ms, ImageFormat.Jpeg);
                        //返回新的Image对象
                        img = Image.FromStream(ms);
                        return img;
                    }
                }
                return img;
            }
            catch
            {
                return img;
            }
        }
    }
}
