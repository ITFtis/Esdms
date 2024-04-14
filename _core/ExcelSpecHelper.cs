using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// <param name="autoSizeColumn">"Y":自動調整長度(效能差:資料量多),"N":字串長度調整width,"default":不調整width</param>
        /// <returns>Excel檔名</returns>
        public static string GenerateExcelByLinqF1(string fileTitle, List<string> titles, List<dynamic> list, string savePath, string autoSizeColumn = "default")
        {
            string fileName = "";

            if (list.Count == 0)
            {
                return "ExcelListCount_0";
            }

            HSSFWorkbook workbook = new HSSFWorkbook();

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

                HSSFSheet mySheet1 = (HSSFSheet)workbook.CreateSheet(sheetName);
                //mySheet1.DefaultRowHeight = 15 * 20;

                //建立 Header                
                int hNum = 0;  //目前第幾個資料列
                int countHigh = 0;  //高度(merge..等)

                //(a)條件標頭
                if (titles != null && titles.Count > 0)
                {
                    //range (merge cell 起始行號，終止行號， 起始列號，終止列號)
                    int sMergeX = 0; int sMergeY = 0; int eMergeX = 0; int eMergeY = 0;

                    HSSFRow rowHeader1 = (HSSFRow)mySheet1.CreateRow(hNum);
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
                    mySheet1.SetEnclosedBorderOfRegion(region, BorderStyle.Thin, NPOI.HSSF.Util.HSSFColor.Black.Index);
                    countHigh = sMergeY - sMergeX + 1;
                }

                //(b)欄位說明文字
                hNum = countHigh;
                HSSFRow rowHeader2 = (HSSFRow)mySheet1.CreateRow(hNum);
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
                foreach (var row in sheet)
                {
                    int index = mySheet1.LastRowNum;

                    HSSFRow rowItem = (HSSFRow)mySheet1.CreateRow(index + 1);

                    foreach (var v in row)
                    {
                        string key = v.Key.ToString();
                        if (key == "SheetName")
                            continue;

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

                        rowItem.GetCell(l).CellStyle = contentStyle;
                    }


                }

                //欄寬調整
                if (mySheet1.LastRowNum > 0)
                {
                    //有資料列
                    int columnCount = ((ICollection<KeyValuePair<string, Object>>)sheet.First()).Count;

                    if (autoSizeColumn == "Y")
                    {
                        //*********自動調整長度(效能差:資料量多)********
                        for (int j = 0; j < columnCount; j++)
                        {
                            mySheet1.AutoSizeColumn(j);
                        }
                    }
                    else if (autoSizeColumn == "N")
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
                    else if (autoSizeColumn == "default")
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

            fileName = fileTitle + "_" + DateTime.Now.ToString("yyyy-MM-dd_") + Guid.NewGuid() + ".xls";
            filePathName = savePath + @"\" + fileName;

            FileStream file = new FileStream(filePathName, FileMode.Create);
            workbook.Write(file);
            file.Close();
            workbook = null;
            return fileName;
        }

        /// <summary>
        /// 產生Excel F2 (一般+標頭合併儲存格)
        /// </summary>
        /// <param name="fileTitle">檔名開頭(報表統計_......)</param>
        /// <param name="titles">表頭文字:機車加油站基本資料欄位清單,條件1,條件2..等</param>
        /// <param name="dicsHeaderMerge">表頭合併儲存格({ "缺失家數(家)", 2 })</param>
        /// <param name="list">多個Sheet資料</param>
        /// <param name="savePath">儲存路徑</param>
        /// <param name="autoSizeColumn">"Y":自動調整長度(效能差:資料量多),"N":字串長度調整width,"default":不調整width</param>
        /// <returns>Excel檔名</returns>
        public static string GenerateExcelByLinqF2(string fileTitle, List<string> titles, Dictionary<string, int> dicsHeaderMerge,
                                            List<dynamic> list, string savePath, string autoSizeColumn = "default")
        {
            string fileName = "";

            if (list.Count == 0)
            {
                return "ExcelListCount_0";
            }

            HSSFWorkbook workbook = new HSSFWorkbook();

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

                HSSFSheet mySheet1 = (HSSFSheet)workbook.CreateSheet(sheetName);
                //mySheet1.DefaultRowHeight = 15 * 20;

                //建立 Header                
                int hNum = 0;  //目前第幾個資料列
                int countHigh = 0;  //高度(merge..等)

                //(a)條件標頭
                if (titles != null && titles.Count > 0)
                {
                    //range (merge cell 起始行號，終止行號， 起始列號，終止列號)
                    int sMergeX = 0; int sMergeY = 0; int eMergeX = 0; int eMergeY = 0;

                    HSSFRow rowHeader1 = (HSSFRow)mySheet1.CreateRow(hNum);
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
                    mySheet1.SetEnclosedBorderOfRegion(region, BorderStyle.Thin, NPOI.HSSF.Util.HSSFColor.Black.Index);
                    countHigh = sMergeY - sMergeX + 1;
                }

                //(b)標題合併儲存格(高度1:countHigh = countHigh + 1;)
                var titleStyle = GetTitleStyle(workbook);
                if (dicsHeaderMerge != null && dicsHeaderMerge.Count > 0)
                {
                    HSSFRow rowHeader1_2 = (HSSFRow)mySheet1.CreateRow(countHigh);
                    rowHeader1_2.CreateCell(0).SetCellValue("");

                    //range
                    int firstRow = 0; int lastRow = 0; int firstCol = 0; int lastCol = 0;
                    firstCol = 1;
                    foreach (var dic in dicsHeaderMerge)
                    {
                        rowHeader1_2.CreateCell(firstCol).SetCellValue(dic.Key);
                        rowHeader1_2.GetCell(firstCol).CellStyle = titleStyle;

                        //range
                        firstRow = lastRow = countHigh;
                        lastCol = firstCol + dic.Value - 1;
                        CellRangeAddress region = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
                        mySheet1.AddMergedRegion(region);
                        mySheet1.SetEnclosedBorderOfRegion(region, BorderStyle.Thin, NPOI.HSSF.Util.HSSFColor.Black.Index);
                        firstCol += dic.Value;
                    }

                    countHigh = 1;
                }

                //(c)欄位說明文字
                hNum = mySheet1.LastRowNum + countHigh;
                HSSFRow rowHeader2 = (HSSFRow)mySheet1.CreateRow(hNum);
                for (int j = 0; j < headerName.Count; j++)
                {
                    rowHeader2.CreateCell(j).SetCellValue(headerName[j]);
                    rowHeader2.GetCell(j).CellStyle = titleStyle;
                }

                //資料長度(index, length)
                Dictionary<int, int> colsLength = new Dictionary<int, int>();

                //建立內容
                var contentStyle = GetContentStyle(workbook);
                foreach (var row in sheet)
                {
                    int index = mySheet1.LastRowNum;

                    HSSFRow rowItem = (HSSFRow)mySheet1.CreateRow(index + 1);

                    foreach (var v in row)
                    {
                        string key = v.Key.ToString();
                        if (key == "SheetName")
                            continue;

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

                        rowItem.GetCell(l).CellStyle = contentStyle;
                    }


                }

                //欄寬調整
                if (mySheet1.LastRowNum > 0)
                {
                    //有資料列
                    int columnCount = ((ICollection<KeyValuePair<string, Object>>)sheet.First()).Count;

                    if (autoSizeColumn == "Y")
                    {
                        //*********自動調整長度(效能差:資料量多)********
                        for (int j = 0; j < columnCount; j++)
                        {
                            mySheet1.AutoSizeColumn(j);
                        }
                    }
                    else if (autoSizeColumn == "N")
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
                                    //columnWidth = columnWidth + ((len - 12) / 2);
                                    columnWidth = columnWidth + (int)Math.Round((double)(len - 12) / 0.4, 0);

                                    //欄寬上限
                                    int up = 50;
                                    if (columnWidth > up)
                                        columnWidth = up;
                                }
                            }

                            //excel儲存格實際寬度轉換公式
                            columnWidth = (int)((columnWidth + 0.71) * 256);
                            mySheet1.SetColumnWidth(j, columnWidth);
                        }
                    }
                    else if (autoSizeColumn == "default")
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

            fileName = fileTitle + "_" + DateTime.Now.ToString("yyyy-MM-dd_") + Guid.NewGuid() + ".xls";
            filePathName = savePath + @"\" + fileName;

            FileStream file = new FileStream(filePathName, FileMode.Create);
            workbook.Write(file);
            file.Close();
            workbook = null;
            return fileName;
        }

        /// <summary>
        /// 產生Excel Grow
        /// </summary>
        /// <param name="fileTitle">檔名開頭(報表統計_......)</param>
        /// <param name="savePath">儲存路徑</param>
        /// <param name="autoSizeColumn">"Y":自動調整長度(效能差:資料量多),"N":字串長度調整width,"default":不調整width</param>
        /// <returns>Excel檔名</returns>
        public static string GenerateExcelGrow(string fileTitle, string savePath, string strhtml, string autoSizeColumn = "default")
        {
            string fileName = "";

            //匯出
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            string filePathName = "";
            int i = 0;
            bool exist = false;

            fileName = fileTitle + "_" + DateTime.Now.ToString("yyyy-MM-dd_") + Guid.NewGuid() + ".xls";
            filePathName = savePath + @"\" + fileName;

            System.IO.File.WriteAllText(filePathName, strhtml);
            return fileName;
        }

        /// <summary>
        /// Npoi Style:條件標頭
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private static HSSFCellStyle GetConditionStyle(HSSFWorkbook workbook)
        {
            //將目前欄位的CellStyle設定為自動換行

            HSSFCellStyle oStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            //多行文字
            oStyle.WrapText = true;
            //文字置中
            oStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            HSSFFont font1 = (HSSFFont)workbook.CreateFont();
            //字體顏色
            font1.Color = NPOI.HSSF.Util.HSSFColor.Blue.Index;
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
        private static HSSFCellStyle GetTitleStyle(HSSFWorkbook workbook)
        {
            HSSFCellStyle oStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            //多行文字
            oStyle.WrapText = true;
            //水平對齊
            oStyle.Alignment = HorizontalAlignment.Center;
            //文字置中
            oStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            HSSFFont font1 = (HSSFFont)workbook.CreateFont();
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
        private static HSSFCellStyle GetContentStyle(HSSFWorkbook workbook)
        {
            HSSFCellStyle oStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            //多行文字
            oStyle.WrapText = true;
            //文字置中
            oStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            HSSFFont font1 = (HSSFFont)workbook.CreateFont();
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
    }
}