using System;
using System.IO;

namespace Yannyo.Common
{
    /// <summary>
    /// 文件元数据。
    /// 可以使用以下几种构造方法：
    /// 本地路径：new FileItem("C:/temp.jpg");
    /// 本地文件：new FileItem(new FileInfo("C:/temp.jpg"));
    /// 字节流：new FileItem("abc.jpg", bytes);
    /// </summary>
    public class FileItem
    {
        private string fileName;
        private string mimeType;
        private byte[] content;
        private FileInfo fileInfo;

        /// <summary>
        /// 基于本地文件的构造器。
        /// </summary>
        /// <param name="fileInfo">本地文件</param>
        public FileItem(FileInfo fileInfo)
        {
            if (fileInfo == null || !fileInfo.Exists)
            {
                throw new ArgumentException("fileInfo is null or not exists!");
            }
            this.fileInfo = fileInfo;
        }

        /// <summary>
        /// 基于本地文件全路径的构造器。
        /// </summary>
        /// <param name="filePath">本地文件全路径</param>
        public FileItem(string filePath)
            : this(new FileInfo(filePath))
        { }

        /// <summary>
        /// 基于文件名和字节流的构造器。
        /// </summary>
        /// <param name="fileName">文件名称（服务端持久化字节流到磁盘时的文件名）</param>
        /// <param name="content">文件字节流</param>
        public FileItem(string fileName, byte[] content)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");
            if (content == null || content.Length == 0) throw new ArgumentNullException("content");

            this.fileName = fileName;
            this.content = content;
        }

        /// <summary>
        /// 基于文件名、字节流和媒体类型的构造器。
        /// </summary>
        /// <param name="fileName">文件名（服务端持久化字节流到磁盘时的文件名）</param>
        /// <param name="content">文件字节流</param>
        /// <param name="mimeType">媒体类型</param>
        public FileItem(String fileName, byte[] content, String mimeType)
            : this(fileName, content)
        {
            if (string.IsNullOrEmpty(mimeType)) throw new ArgumentNullException("mimeType");
            this.mimeType = mimeType;
        }

        public string GetFileName()
        {
            if (this.fileName == null && this.fileInfo != null && this.fileInfo.Exists)
            {
                this.fileName = this.fileInfo.FullName;
            }
            return this.fileName;
        }

        public string GetMimeType()
        {
            if (this.mimeType == null)
            {
                this.mimeType = GetMimeType(GetContent());
            }
            return this.mimeType;
        }

        public byte[] GetContent()
        {
            if (this.content == null && this.fileInfo != null && this.fileInfo.Exists)
            {
                using (Stream fileStream = this.fileInfo.OpenRead())
                {
                    this.content = new byte[fileStream.Length];
                    fileStream.Read(content, 0, content.Length);
                }
            }

            return this.content;
        }
        /// <summary>
        /// 获取文件的真实媒体类型。目前只支持JPG, GIF, PNG, BMP四种图片文件。
        /// </summary>
        /// <param name="fileData">文件字节流</param>
        /// <returns>媒体类型</returns>
        public static string GetMimeType(byte[] fileData)
        {
            string suffix = GetFileSuffix(fileData);
            string mimeType;

            switch (suffix)
            {
                case "JPG": mimeType = "image/jpeg"; break;
                case "GIF": mimeType = "image/gif"; break;
                case "PNG": mimeType = "image/png"; break;
                case "BMP": mimeType = "image/bmp"; break;
                default: mimeType = "application/octet-stream"; break;
            }

            return mimeType;
        }
        /// <summary>
        /// 获取文件的真实后缀名。目前只支持JPG, GIF, PNG, BMP四种图片文件。
        /// </summary>
        /// <param name="fileData">文件字节流</param>
        /// <returns>JPG, GIF, PNG or null</returns>
        public static string GetFileSuffix(byte[] fileData)
        {
            if (fileData == null || fileData.Length < 10)
            {
                return null;
            }

            if (fileData[0] == 'G' && fileData[1] == 'I' && fileData[2] == 'F')
            {
                return "GIF";
            }
            else if (fileData[1] == 'P' && fileData[2] == 'N' && fileData[3] == 'G')
            {
                return "PNG";
            }
            else if (fileData[6] == 'J' && fileData[7] == 'F' && fileData[8] == 'I' && fileData[9] == 'F')
            {
                return "JPG";
            }
            else if (fileData[0] == 'B' && fileData[1] == 'M')
            {
                return "BMP";
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据文件后缀名获取文件的媒体类型。
        /// </summary>
        /// <param name="fileName">带后缀的文件名或文件全名</param>
        /// <returns>媒体类型</returns>
        public static string GetMimeType(string fileName)
        {
            string mimeType;
            fileName = fileName.ToLower();

            if (fileName.EndsWith(".bmp", StringComparison.CurrentCulture))
            {
                mimeType = "image/bmp";
            }
            else if (fileName.EndsWith(".gif", StringComparison.CurrentCulture))
            {
                mimeType = "image/gif";
            }
            else if (fileName.EndsWith(".jpg", StringComparison.CurrentCulture) || fileName.EndsWith(".jpeg", StringComparison.CurrentCulture))
            {
                mimeType = "image/jpeg";
            }
            else if (fileName.EndsWith(".png", StringComparison.CurrentCulture))
            {
                mimeType = "image/png";
            }
            else
            {
                mimeType = "application/octet-stream";
            }

            return mimeType;
        }
    }
}
