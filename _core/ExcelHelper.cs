using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data;
using System.Linq;
using System.Web;

using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.POIFS;
using NPOI.Util;
using NPOI.SS.Util;
using NPOI.SS.UserModel;

namespace Esdms
{
    public class ExcelHelper
    {
        /// <summary>
        /// 讀取Excel的Sheet轉換為DataTable：標題行(標題行從0開始)
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="titleCol">標題行</param>
        /// <returns></returns>
        public static DataTable ExcelToDt(ISheet sheet, int titleCol)
        {
            var table = new DataTable();

            int cellCount = 0;

            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                if (sheet.GetRow(i) != null)
                {
                    int num = sheet.GetRow(i).Cells.Count;
                    if (num > cellCount)
                        cellCount = num;
                }
            }

            if (cellCount == 0) return table;

            IRow headerRow = null;
            headerRow = sheet.GetRow(0);

            //標題
            var rowHeader = sheet.GetRow(titleCol);
            for (int j = rowHeader.FirstCellNum; j <= cellCount; j++)
            {
                var cell = rowHeader.GetCell(j);
                if (cell != null)
                {
                    var content = cell.ToString();
                    var column = new DataColumn(content);
                    table.Columns.Add(column);
                }
            }
            for (int i = titleCol + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null)
                    continue;

                // 是否為空白Row
                bool isEmptyRow = true;
                var dataRow = table.NewRow();
                for (int j = row.FirstCellNum; j <= cellCount; j++)
                {
                    //(欄位數量,0起)超過(dataRow數量)，break
                    if (j >= dataRow.ItemArray.Count())
                        break;

                    var cell = row.GetCell(j);
                    if (cell != null)
                    {
                        var content = cell.ToString();
                        dataRow[j] = content;

                        if (!string.IsNullOrWhiteSpace(content))
                            isEmptyRow = false;
                    }
                }

                if (!isEmptyRow)
                    table.Rows.Add(dataRow);
            }
            return table;
        }
    }
}