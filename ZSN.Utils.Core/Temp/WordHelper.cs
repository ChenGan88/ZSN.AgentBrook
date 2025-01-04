using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office;
using Microsoft.Office.Interop.Word;

namespace Yannyo.Common
{
    public class WordDocFileHelper
    {
        private Microsoft.Office.Interop.Word.ApplicationClass wordApp = null;
        private Microsoft.Office.Interop.Word.Document wordDoc = null;
        public Microsoft.Office.Interop.Word.ApplicationClass Application
        {
          
            set { wordApp = value; }
            get { return wordApp; }
        }
        /// <summary>
        /// 文档对象
        /// </summary>
        public Microsoft.Office.Interop.Word.Document Document
        {
            set { wordDoc = value; }
            get { return wordDoc; }
        }

        /// <summary>
        /// 通过模板创建新文档
        /// </summary>
        /// <param name="filePath">模板文件路径</param>
        public void CreateNewDocument(string filePath)
        {
            killWinWordProcess();
            wordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
            wordApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            wordApp.Visible = false;
            object missing = System.Reflection.Missing.Value;
            object templateName = filePath;
            wordDoc = wordApp.Documents.Open(ref templateName, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing);
        }

        /// <summary>
        /// 保存新文件
        /// </summary>
        /// <param name="filePath">保存文件的路径</param>
        public void SaveDocument(string filePath)
        {
            object fileName = filePath;
            object format = WdSaveFormat.wdFormatDocument;//保存格式
            object miss = System.Reflection.Missing.Value;
            wordDoc.SaveAs(ref fileName, ref format, ref miss,
                ref miss, ref miss, ref miss, ref miss,
                ref miss, ref miss, ref miss, ref miss,
                ref miss, ref miss, ref miss, ref miss,
                ref miss);
            //关闭wordDoc，wordApp对象
            object SaveChanges = WdSaveOptions.wdSaveChanges;
            object OriginalFormat = WdOriginalFormat.wdOriginalDocumentFormat;
            object RouteDocument = false;
            wordDoc.Close(ref SaveChanges, ref OriginalFormat, ref RouteDocument);
            wordApp.Quit(ref SaveChanges, ref OriginalFormat, ref RouteDocument);
        }

        /// <summary>
        /// 在书签处插入值
        /// </summary>
        /// <param name="bookmark">模板中定义的书签名</param>
        /// <param name="value">书签处插入的内容</param>
        /// <returns></returns>
        public bool InsertValue(string bookmark, string value)
        {
            object bkObj = bookmark;
            if (wordApp.ActiveDocument.Bookmarks.Exists(bookmark))
            {
                wordApp.ActiveDocument.Bookmarks.get_Item(ref bkObj).Select();
                wordApp.Selection.TypeText(value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 插入表格,bookmark书签
        /// </summary>
        /// <param name="bookmark">模板中定义的书签名</param>
        /// <param name="rows">插入表格的行数</param>
        /// <param name="columns">插入表格的列数</param>
        /// <param name="width"></param>
        /// <returns></returns>
        public Table InsertTable(string bookmark, int rows, int columns, float width)
        {
            object miss = System.Reflection.Missing.Value;
            object oStart = bookmark;
            Range range = wordDoc.Bookmarks.get_Item(ref oStart).Range;//表格插入位置
            Table newTable = wordDoc.Tables.Add(range, rows, columns, ref miss, ref miss);
            //设置表的格式
            newTable.Borders.Enable = 1;  //允许有边框，默认没有边框(为0时报错，1为实线边框，2、3为虚线边框，以后的数字没试过)
            newTable.Borders.OutsideLineWidth = WdLineWidth.wdLineWidth050pt;//边框宽度
            if (width != 0)
            {
                newTable.PreferredWidth = width;//表格宽度
            }
            newTable.AllowPageBreaks = false;
            return newTable;
        }

        /// <summary>
        /// 合并单元格 表id,开始行号,开始列号,结束行号,结束列号
        /// </summary>
        /// <param name="n">Table表格 索引从1开始</param>
        /// <param name="row1">开始行号</param>
        /// <param name="column1">开始列号</param>
        /// <param name="row2">结束行号</param>
        /// <param name="column2">结束列号</param>
        public void MergeCell(int n, int row1, int column1, int row2, int column2)
        {
            wordDoc.Content.Tables[n].Cell(row1, column1).Merge(wordDoc.Content.Tables[n].Cell(row2, column2));
        }

        /// <summary>
        /// 合并单元格 表名,开始行号,开始列号,结束行号,结束列号
        /// </summary>
        /// <param name="table">Microsoft.Office.Interop.Word.Table 对象</param>
        /// <param name="row1">开始行号</param>
        /// <param name="column1">开始列号</param>
        /// <param name="row2">结束行号</param>
        /// <param name="column2">结束列号</param>
        public void MergeCell(Microsoft.Office.Interop.Word.Table table, int row1, int column1, int row2, int column2)
        {
            table.Cell(row1, column1).Merge(table.Cell(row2, column2));
        }

        /// <summary>
        /// 设置表格内容对齐方式 Align水平方向，Vertical垂直方向(左对齐，居中对齐，右对齐分别对应Align和Vertical的值为-1,0,1)
        /// Microsoft.Office.Interop.Word.Table table
        /// </summary>
        /// <param name="n">Table表格 索引从1开始</param>
        /// <param name="Align">平方向：左对齐（-1），居中对齐（0），右对齐（1）</param>
        /// <param name="Vertical">垂直方向：左对齐（-1），居中对齐（0），右对齐（1）</param>
        public void SetParagraph_Table(int n, int Align, int Vertical)
        {
            switch (Align)
            {
                case -1: wordDoc.Content.Tables[n].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft; break;//左对齐
                case 0: wordDoc.Content.Tables[n].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; break;//水平居中
                case 1: wordDoc.Content.Tables[n].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight; break;//右对齐
            }
            switch (Vertical)
            {
                case -1: wordDoc.Content.Tables[n].Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalTop; break;//顶端对齐
                case 0: wordDoc.Content.Tables[n].Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter; break;//垂直居中
                case 1: wordDoc.Content.Tables[n].Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalBottom; break;//底端对齐
            }
        }

        /// <summary>
        /// 设置单元格内容对齐方式 Align水平方向，Vertical垂直方向(左对齐，居中对齐，右对齐分别对应Align和Vertical的值为-1,0,1)
        /// Microsoft.Office.Interop.Word.Table table
        /// </summary>
        /// <param name="n">Table表格 索引从1开始</param>
        /// <param name="row">行号 索引从1开始</param>
        /// <param name="column">列号 索引从1开始</param>
        /// <param name="Align">平方向：左对齐（-1），居中对齐（0），右对齐（1）</param>
        /// <param name="Vertical">垂直方向：左对齐（-1），居中对齐（0），右对齐（1）</param>
        public void SetParagraph_Table(int n, int row, int column, int Align, int Vertical)
        {
            switch (Align)
            {
                case -1: wordDoc.Content.Tables[n].Cell(row, column).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft; break;//左对齐
                case 0: wordDoc.Content.Tables[n].Cell(row, column).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; break;//水平居中
                case 1: wordDoc.Content.Tables[n].Cell(row, column).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight; break;//右对齐
            }
            switch (Vertical)
            {
                case -1: wordDoc.Content.Tables[n].Cell(row, column).Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalTop; break;//顶端对齐
                case 0: wordDoc.Content.Tables[n].Cell(row, column).Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter; break;//垂直居中
                case 1: wordDoc.Content.Tables[n].Cell(row, column).Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalBottom; break;//底端对齐
            }

        }

        /// <summary>
        /// 设置表格字体
        /// </summary>
        /// <param name="table">Microsoft.Office.Interop.Word.Table 对象</param>
        /// <param name="fontName">字体 如：宋体</param>
        /// <param name="size">字体大小 如：9 （磅）</param>
        public void SetFont_Table(Table table, string fontName, double size)
        {
            if (size != 0)
            {
                table.Range.Font.Size = Convert.ToSingle(size);
            }
            if (fontName != "")
            {
                table.Range.Font.Name = fontName;
            }
        }

        /// <summary>
        /// 设置单元格字体 Microsoft.Office.Interop.Word.Table table
        /// </summary>
        /// <param name="n">Table表格 索引从1开始</param>
        /// <param name="row">行号 索引从1开始</param>
        /// <param name="column">列号 索引从1开始</param>
        /// <param name="fontName">字体 如：宋体</param>
        /// <param name="size">字体大小 如：9 （磅）</param>
        /// <param name="bold">字体加粗 </param>
        public void SetFont_Table(int n, int row, int column, string fontName, double size, int bold)
        {
            if (size != 0)
            {
                wordDoc.Content.Tables[n].Cell(row, column).Range.Font.Size = Convert.ToSingle(size);
            }
            if (fontName != "")
            {
                wordDoc.Content.Tables[n].Cell(row, column).Range.Font.Name = fontName;
            }
            wordDoc.Content.Tables[n].Cell(row, column).Range.Font.Bold = bold;// 0 表示不是粗体，其他值都是
        }

        /// <summary>
        /// 是否使用边框,n表格的序号,use是或否
        /// 该处边框参数可以用int代替bool可以让方法更全面
        /// 具体值方法中介绍
        /// </summary>
        /// <param name="n">Table表格 索引从1开始</param>
        /// <param name="use">0:无边框；1:实线边框；2、3为虚线边框；以后的数字没试过</param>
        public void UseBorder(int n, int use)
        {
            wordDoc.Content.Tables[n].Borders.Enable = use;
        }

        /// <summary>
        /// 给表格插入一行,n表格的序号从1开始记
        /// </summary>
        /// <param name="n">Table表格 索引从1开始</param>
        public void AddRow(int n)
        {
            object miss = System.Reflection.Missing.Value;
            wordDoc.Content.Tables[n].Rows.Add(ref miss);
        }

        /// <summary>
        /// 给表格添加一行
        /// </summary>
        /// <param name="table">Microsoft.Office.Interop.Word.Table 对象</param>
        public void AddRow(Microsoft.Office.Interop.Word.Table table)
        {
            object miss = System.Reflection.Missing.Value;
            table.Rows.Add(ref miss);
        }

        /// <summary>
        /// 给表格插入rows行,n为表格的序号
        /// </summary>
        /// <param name="n">Table表格 索引从1开始</param>
        /// <param name="rows">添加位置的行号 行号索引从1开始</param>
        public void AddRow(int n, int rows)
        {
            object miss = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Table table = wordDoc.Content.Tables[n];
            for (int i = 0; i < rows; i++)
            {
                table.Rows.Add(ref miss);
            }
        }

        /// <summary>
        /// 删除表格第rows行,n为表格的序号
        /// </summary>
        /// <param name="n">Table表格 索引从1开始</param>
        /// <param name="rows">删除位置的行号 行号索引从1开始</param>
        public void DeleteRow(int n, int row)
        {
            Microsoft.Office.Interop.Word.Table table = wordDoc.Content.Tables[n];
            table.Rows[row].Delete();
        }

        /// <summary>
        /// 给表格中单元格插入元素，table所在表格，row行号，column列号，value插入的元素
        /// </summary>
        /// <param name="table">Microsoft.Office.Interop.Word.Table 对象</param>
        /// <param name="row">行号 索引从1开始</param>
        /// <param name="column">列号 索引从1开始</param>
        /// <param name="value">给单元格中添加的值</param>
        public void InsertCell(Microsoft.Office.Interop.Word.Table table, int row, int column, string value)
        {
            table.Cell(row, column).Range.Text = value;
        }

        /// <summary>
        /// 给表格中单元格插入元素，n表格的序号从1开始记，row行号，column列号，value插入的元素
        /// </summary>
        /// <param name="n">Table表格 索引从1开始</param>
        /// <param name="row">行号 索引从1开始</param>
        /// <param name="column">列号 索引从1开始</param>
        /// <param name="value">给单元格中添加的值</param>
        public void InsertCell(int n, int row, int column, string value)
        {
            wordDoc.Content.Tables[n].Cell(row, column).Range.Text = value;
        }

        /// <summary>
        /// 给表格插入一行数据，n为表格的序号，row行号，columns列数，values插入的值
        /// </summary>
        /// <param name="n">Table表格 索引从1开始</param>
        /// <param name="row">行号 索引从1开始</param>
        /// <param name="column">列号 索引从1开始</param>
        /// <param name="values">行单元格中每一列数据合集</param>
        public void InsertCell(int n, int row, int columns, string[] values)
        {
            Table table = wordDoc.Content.Tables[n];
            for (int i = 0; i < columns; i++)
            {
                table.Cell(row, i + 1).Range.Text = values[i];
            }
        }

        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="bookmark">模板中定义的书签名</param>
        /// <param name="picturePath">图片的露肩</param>
        /// <param name="width">图片宽</param>
        /// <param name="hight">图片高</param>
        public void InsertPicture(string bookmark, string picturePath, float width, float hight)
        {
            object miss = System.Reflection.Missing.Value;
            object oStart = bookmark;
            Object linkToFile = false;       //图片是否为外部链接
            Object saveWithDocument = true;  //图片是否随文档一起保存 
            object range = wordDoc.Bookmarks.get_Item(ref oStart).Range;//图片插入位置
            wordDoc.InlineShapes.AddPicture(picturePath, ref linkToFile, ref saveWithDocument, ref range);
            wordDoc.Application.ActiveDocument.InlineShapes[1].Width = width;   //设置图片宽度
            wordDoc.Application.ActiveDocument.InlineShapes[1].Height = hight;  //设置图片高度
        }

        /// <summary>
        /// 插入一段文字,text为文字内容
        /// </summary>
        /// <param name="bookmark">模板中定义的书签名</param>
        /// <param name="text">插入的文本对象</param>
        public void InsertText(string bookmark, string text)
        {
            object oStart = bookmark;
            object range = wordDoc.Bookmarks.get_Item(ref oStart).Range;
            Paragraph wp = wordDoc.Content.Paragraphs.Add(ref range);
            wp.Format.SpaceBefore = 6;
            wp.Range.Text = text;
            wp.Format.SpaceAfter = 24;
            wp.Range.InsertParagraphAfter();
            wordDoc.Paragraphs.Last.Range.Text = "\n";
        }

        /// <summary>
        /// 杀掉winword.exe进程
        /// </summary>
        public void killWinWordProcess()
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("WINWORD");
            foreach (System.Diagnostics.Process process in processes)
            {
                bool b = process.MainWindowTitle == "";
                if (process.MainWindowTitle == "")
                {
                    process.Kill();
                }
            }
        }

    }
}
