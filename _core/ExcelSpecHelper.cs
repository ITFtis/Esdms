﻿using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Esdms
{
    public class ExcelSpecHelper
    {
        /// <summary>
        /// 產生Excel F1 (一般)
        /// </summary>
        /// <param name="fileTitle">檔名開頭(報表統計_......)</param>
        /// <param name="titles">表頭文字:機車加油站基本資料欄位清單,條件1,條件2..等</param>
        /// <param name="list">多個Sheet資料</param>
        /// <param name="savePath">儲存路徑</param>
        /// <param name="autoSizeColumn">"0":不調整width,"1":自動調整長度(效能差:資料量多),"2":字串長度調整width,"3":字串長度調整width(展開)</param>
        /// <param name="topContents">特殊儲存格位置Top</param>
        /// <param name="strFooter">NPOI內建標記 粗體"&B測試"</param>
        /// <returns>Excel檔名</returns>
        public static string GenerateExcelByLinqF1(string fileTitle, List<string> titles, List<dynamic> list, string savePath,
                                                int autoSizeColumn, List<string> topContents = null, string strFooter = "")
        {
            string fileName = "";

            if (list.Count == 0)
            {
                return "ExcelListCount_0";
            }

            XSSFWorkbook workbook = new XSSFWorkbook();

            //sheet區分：所屬部門代碼
            var sheets = list.GroupBy(x => x.SheetName);
            foreach (var sheet in sheets)
            {
                string sheetName = sheet.Key;

                List<string> headerName = new List<string>();
                foreach (var row in sheet)
                {
                    foreach (var v in row)
                    {
                        string key = v.Key.ToString();
                        if (key != "SheetName")
                            headerName.Add(key);
                    }
                    break;
                }

                XSSFSheet mySheet1 = (XSSFSheet)workbook.CreateSheet(sheetName);

                //設定標腳(footer)
                if (strFooter != "")
                {
                    mySheet1.Footer.Right = strFooter; //粗體"&B測試"
                }

                //mySheet1.DefaultRowHeight = 15 * 20;

                //建立 Header                
                int hNum = 0;  //目前第幾個資料列
                int countHigh = 0;  //高度(merge..等)

                //(a)條件標頭
                if (titles != null && titles.Count > 0)
                {
                    //range (merge cell 起始行號，終止行號， 起始列號，終止列號)
                    int sMergeX = 0; int sMergeY = 0; int eMergeX = 0; int eMergeY = 0;

                    XSSFRow rowHeader1 = (XSSFRow)mySheet1.CreateRow(hNum);
                    string title = string.Join("\n", titles);
                    //產生第一個要用CreateRow 
                    rowHeader1.CreateCell(0).SetCellValue(title);
                    //因為換行所以愈設幫他Row的高度變成3倍
                    //rowHeader1.HeightInPoints = (float)2 * mySheet1.DefaultRowHeight / 20;
                    var conditionStyle = GetConditionStyle(workbook);
                    rowHeader1.GetCell(hNum).CellStyle = conditionStyle;

                    //range
                    sMergeX = 0; sMergeY = sMergeX + titles.Count - 1;
                    eMergeX = 0; eMergeY = eMergeX + headerName.Count - 1;
                    CellRangeAddress region = new CellRangeAddress(sMergeX, sMergeY, eMergeX, eMergeY);
                    mySheet1.AddMergedRegion(region);
                    //mySheet1.SetEnclosedBorderOfRegion(region, BorderStyle.Thin, NPOI.HSSF.Util.HSSFColor.Black.Index);
                    countHigh = sMergeY - sMergeX + 1;
                }

                //(b)欄位說明文字
                hNum = countHigh;
                XSSFRow rowHeader2 = (XSSFRow)mySheet1.CreateRow(hNum);
                var titleStyle = GetTitleStyle(workbook);
                for (int j = 0; j < headerName.Count; j++)
                {
                    rowHeader2.CreateCell(j).SetCellValue(headerName[j]);
                    rowHeader2.GetCell(j).CellStyle = titleStyle;
                }

                //資料長度(index, length)
                Dictionary<int, int> colsLength = new Dictionary<int, int>();

                //建立內容
                var contentStyle = GetContentStyle(workbook);

                XSSFCellStyle hStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                hStyle.CloneStyleFrom(contentStyle);
                hStyle.Alignment = HorizontalAlignment.Left;
                hStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;

                foreach (var row in sheet)
                {
                    int index = mySheet1.LastRowNum;

                    XSSFRow rowItem = (XSSFRow)mySheet1.CreateRow(index + 1);

                    XSSFCellStyle style;
                    foreach (var v in row)
                    {
                        string key = v.Key.ToString();
                        if (key == "SheetName")
                            continue;

                        //特殊儲存格位置Top
                        style = contentStyle;
                        if (topContents != null)
                        {
                            //if (key == "專長")
                            if (topContents.Contains(key))
                            {
                                style = hStyle;
                            }
                        }

                        int l = rowItem.Cells.Count;
                        object value = v.Value;

                        Type t;
                        if (value == null)
                        {
                            value = "";
                            t = typeof(string);
                        }
                        else
                        {
                            t = value.GetType();
                        }

                        if (t == typeof(string))
                        {
                            rowItem.CreateCell(l).SetCellValue(value.ToString());

                            //紀錄字串長度max
                            int len = value.ToString().Length;
                            if (!colsLength.ContainsKey(l) || len > colsLength[l])
                                colsLength[l] = len;
                        }
                        else if (t == typeof(DateTime))
                        {
                            rowItem.CreateCell(l).SetCellValue(value == null ? String.Empty : Esdms.DateFormat.ToDate1(value.ToString()));
                        }
                        else if (t == typeof(int))
                        {
                            rowItem.CreateCell(l).SetCellValue(value == null ? 0 : int.Parse(value.ToString()));
                        }
                        else if (t == typeof(double))
                        {
                            rowItem.CreateCell(l).SetCellValue(value == null ? 0 : double.Parse(value.ToString()));
                        }
                        else
                        {
                            rowItem.CreateCell(l).SetCellValue(value.ToString());
                        }

                        rowItem.GetCell(l).CellStyle = style;
                    }


                }

                //欄寬調整
                if (mySheet1.LastRowNum > 0)
                {
                    //有資料列
                    int columnCount = ((ICollection<KeyValuePair<string, Object>>)sheet.First()).Count;

                    if (autoSizeColumn == 1)
                    {
                        //*********自動調整長度(效能差:資料量多)********
                        for (int j = 0; j < columnCount; j++)
                        {
                            mySheet1.AutoSizeColumn(j);
                        }
                    }
                    else if (autoSizeColumn == 2)
                    {
                        //字串長度調整width
                        for (int j = 0; j < columnCount; j++)
                        {
                            //欄寬預設 12
                            int columnWidth = 12;

                            if (colsLength.ContainsKey(j))
                            {
                                int len = colsLength[j];
                                if (len > columnWidth)
                                {
                                    columnWidth = columnWidth + ((len - 12) / 2);

                                    //欄寬上限
                                    int up = 25;
                                    if (columnWidth > up)
                                        columnWidth = up;
                                }
                            }

                            //excel儲存格實際寬度轉換公式
                            columnWidth = (int)((columnWidth + 0.71) * 256);
                            mySheet1.SetColumnWidth(j, columnWidth);
                        }
                    }
                    else if (autoSizeColumn == 3)
                    {                        
                        //字串長度調整width(展開)
                        for (int j = 0; j < columnCount; j++)
                        {
                            //欄寬預設 11                            
                            int columnWidth = 11;

                            if (colsLength.ContainsKey(j))
                            {
                                int len = colsLength[j];
                                int wordLen = 12;
                                if (len > 6 && len <= columnWidth)
                                {
                                    columnWidth = 18;
                                }
                                else if (len > columnWidth)
                                {                                    
                                    columnWidth = (1 + (len / wordLen)) * 25;

                                    //欄寬上限
                                    int up = 50;
                                    if (columnWidth > up)
                                        columnWidth = up;
                                }

                                //excel儲存格實際寬度轉換公式
                                columnWidth = (int)((columnWidth + 0.71) * 256);
                                mySheet1.SetColumnWidth(j, columnWidth);
                            }
                        }
                    }
                    else if (autoSizeColumn == 0)
                    {
                        //不調整width
                    }
                }
            }

            //匯出
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            string filePathName = "";
            int i = 0;
            bool exist = false;

            fileName = fileTitle + "_" + DateTime.Now.ToString("yyyy-MM-dd_") + Guid.NewGuid() + ".xlsx";
            filePathName = savePath + @"\" + fileName;

            FileStream file = new FileStream(filePathName, FileMode.Create);
            workbook.Write(file);
            file.Close();
            workbook = null;
            return fileName;
        }

        /// <summary>
        /// Npoi Style:條件標頭
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private static XSSFCellStyle GetConditionStyle(XSSFWorkbook workbook)
        {
            //將目前欄位的CellStyle設定為自動換行

            XSSFCellStyle oStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            //多行文字
            oStyle.WrapText = true;
            //文字置中
            oStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            XSSFFont font1 = (XSSFFont)workbook.CreateFont();
            //字體顏色
            font1.Color = IndexedColors.Blue.Index;
            //字體粗體
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            //字型
            font1.FontName = "標楷體";     //新細明體
            oStyle.SetFont(font1);

            //有邊框
            oStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            return oStyle;
        }

        /// <summary>
        /// Npoi Style:標頭
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private static XSSFCellStyle GetTitleStyle(XSSFWorkbook workbook)
        {
            XSSFCellStyle oStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            //多行文字
            oStyle.WrapText = true;
            //水平對齊
            oStyle.Alignment = HorizontalAlignment.Center;
            //文字置中
            oStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            XSSFFont font1 = (XSSFFont)workbook.CreateFont();
            font1.FontHeightInPoints = 12;
            ////字體粗體
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            //字型
            font1.FontName = "標楷體";     //新細明體
            oStyle.SetFont(font1);

            //有邊框
            oStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            return oStyle;
        }

        /// <summary>
        /// Npoi Style:內容
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private static XSSFCellStyle GetContentStyle(XSSFWorkbook workbook)
        {
            XSSFCellStyle oStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            //多行文字
            oStyle.WrapText = true;
            //水平對齊
            oStyle.Alignment = HorizontalAlignment.Center;
            //文字置中
            oStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            XSSFFont font1 = (XSSFFont)workbook.CreateFont();
            //字型
            font1.FontName = "標楷體";     //新細明體
            oStyle.SetFont(font1);

            //有邊框
            oStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            return oStyle;
        }

        /// <summary>
        /// (版本1)1頁(斜印+無縫隙列)浮水印文字 => 董事長
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="waterColor">指定浮水印顏色</param>
        /// <param name="backColor"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="multiple">倍數，預設專家清冊列印1 (範本不同，設定不同)</param>
        /// <returns></returns>
        public static System.Drawing.Image DrawText(String text, System.Drawing.Font font, string waterColor, Color backColor, 
                                                        double height, double width, int multiple = 1)
        {
            //創建一個指定寬度和高度的圖像
            Image img = new Bitmap((int)width, (int)height);
            Graphics drawing = Graphics.FromImage(img);
            //獲取文本大小
            SizeF textSize = drawing.MeasureString(text, font);
            //旋轉圖片
            drawing.TranslateTransform(((int)width - textSize.Width) / 2, ((int)height - textSize.Height) / 2);
            drawing.RotateTransform(-35);  //defalut:-45
            drawing.TranslateTransform(-((int)width - textSize.Width) / 2, -((int)height - textSize.Height) / 2);
            //绘制背景
            drawing.Clear(backColor);
            //创建文本刷

            //浮水印色碼，預設
            string defalultColor = "EsdmsWaterRGB";
            System.Drawing.Color conDrawColor = (System.Drawing.Color)ColorCode.GetWaterColor().Where(a => a.Key == defalultColor).First().Value;           

            //判斷是否有指定浮水印顏色
            var color = ColorCode.GetWaterColor().Where(a => a.Key == waterColor);
            if (color.Count() > 0)
            {
                //Con1
                conDrawColor = (System.Drawing.Color)color.First().Value;
            }

            Brush textBrushCon = new SolidBrush(conDrawColor);

            ////頭
            //drawing.DrawString(text, font, textBrush, 140, 300);
            ////置中
            //drawing.DrawString(text, font, textBrush, 30, ((int)height - textSize.Height) / 2);
            ////尾
            //drawing.DrawString(text, font, textBrush, -90, 900);

            for (int i = 0; i < 5; i++)
                text += text;

            int x = 100 * multiple;
            int y = 20 * multiple;
            int maxY = 1300 * multiple;  //1150 1250 1300
            while (y <= maxY)
            {
                drawing.DrawString(text, font, textBrushCon, x, y);
                x = x - (10 * multiple);

                y += (30 * multiple);
            }

            drawing.Save();
            return img;
        }

        /// <summary>
        /// (版本2)1頁(5列+特殊字體)浮水印文字 => 宇揚協理
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="waterColor">指定浮水印顏色</param>
        /// <param name="backColor"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static System.Drawing.Image DrawText2(String text, System.Drawing.Font font, string waterColor, Color backColor, double height, double width)
        {
            //創建一個指定寬度和高度的圖像
            Image img = new Bitmap((int)width, (int)height);
            Graphics drawing = Graphics.FromImage(img);
            //獲取文本大小
            SizeF textSize = drawing.MeasureString(text, font);
            //旋轉圖片
            drawing.TranslateTransform(((int)width - textSize.Width) / 2, ((int)height - textSize.Height) / 2);
            drawing.RotateTransform(0);  //defalut:-45
            drawing.TranslateTransform(-((int)width - textSize.Width) / 2, -((int)height - textSize.Height) / 2);
            //绘制背景
            drawing.Clear(backColor);
            //创建文本刷

            //浮水印色碼，預設
            string defalultColor = "BurlyWood";
            int alpha = 160;  //透明度(100%=>255)
            System.Drawing.Color con1DrawColor = (System.Drawing.Color)ColorCode.GetWaterColor().Where(a => a.Key == defalultColor).First().Value;
            System.Drawing.Color con2DrawColor = (System.Drawing.Color)ColorCode.GetWaterColor(alpha).Where(a => a.Key == defalultColor).First().Value;

            //判斷是否有指定浮水印顏色
            var color1 = ColorCode.GetWaterColor().Where(a => a.Key == waterColor);
            var color2 = ColorCode.GetWaterColor(alpha).Where(a => a.Key == waterColor);
            if (color1.Count() > 0)
            {
                //Con1
                con1DrawColor = (System.Drawing.Color)color1.First().Value;
                //Con2
                con2DrawColor = (System.Drawing.Color)color2.First().Value;
            }

            Brush textBrushCon1 = new SolidBrush(con1DrawColor);
            Brush textBrushCon2 = new SolidBrush(con2DrawColor);
            
            ////頭
            //drawing.DrawString(text, font, textBrush, 140, 300);
            ////置中
            //drawing.DrawString(text, font, textBrush, 30, ((int)height - textSize.Height) / 2);
            ////尾
            //drawing.DrawString(text, font, textBrush, -90, 900);

            //435, 28
            //int x = 140, y = 300;
            //while (x <= textSize.Width)
            //{
            //    drawing.DrawString(text, font, textBrush, x, y);
            //    x += 50;
            //}

            //int x = 5, y = 50;
            //左右：0 ~ 600
            int x = 0, y = 100;
            int maxY = 1300;

            string content = text + " " + text + " " + text;
            while (y <= maxY)
            {
                string con1 = "";
                string con2 = "";
                for (int i = 0; i < content.Length; i++)
                {
                    string str = content[i].ToString();
                    if (i % 2 == 0)
                    {
                        con1 += content[i];
                        con2 += StringHelper.HasChinese(str) ? "  " : " ";                       
                    }
                    else
                    {
                        con1 += StringHelper.HasChinese(str) ? "  " : " ";
                        con2 += content[i];
                    }
                }

                
                drawing.DrawString(con1, font, textBrushCon1, x, y);
                drawing.DrawString(con2, font, textBrushCon2, x, y);

                y += 220;
            }

            drawing.Save();
            return img;
        }
    }
}