using Esdms.Controllers.Es;
using Esdms.Models;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.Streaming.Values;
using NPOI.XSSF.UserModel;
using Spire.Xls;
using Spire.Xls.Core.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Esdms
{
    public class Rpt_ProjectInvoice : ReportClass
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Export(int id)
        {
            string url = "";

            try
            {
                Dou.Models.DB.IModelEntity<ProjectInvoice> m_ProjectInvoice = new Dou.Models.DB.ModelEntity<ProjectInvoice>(new EsdmsModelContextExt());
                var invoice = m_ProjectInvoice.GetAll().Where(a => a.Id == id).FirstOrDefault();

                ////Dou.Models.DB.IModelEntity<Project> m_Project = new Dou.Models.DB.ModelEntity<Project>(new EsdmsModelContextExt());
                ////var project = m_Project.GetAll().Where(a => a.PrjId == invoice.PrjId).FirstOrDefault();

                ////if (project == null)
                ////{
                ////    _errorMessage = "查無此專案編號：" + project.PrjId;
                ////    return "";
                ////}

                //複製範本
                string sourcePath = HttpContext.Current.Server.MapPath("~/DocsWeb/Template/") + "(範本)114年專家學者請款明細.xlsx";

                string fileName = invoice.PrjId + "_" + invoice.PrjName + "_" 
                                    + DateFormat.ToDate2_1(DateTime.Now) + ".xlsx";
                string toFolder = FileHelper.GetFileFolder(Code.TempUploadFile.匯出專家請款單);

                if (!Directory.Exists(toFolder))
                {
                    Directory.CreateDirectory(toFolder);
                }

                string toPath = toFolder + fileName;

                using (FileStream rFile = System.IO.File.OpenRead(sourcePath))
                {
                    //編輯範本檔
                    XSSFWorkbook workbook = null;
                    XSSFSheet sheet = null;                    
                    workbook = new XSSFWorkbook(rFile);
                    rFile.Close();

                    //Sheet
                    sheet = (XSSFSheet)workbook.GetSheetAt(0);
                    workbook.SetSheetName(workbook.GetSheetIndex(sheet), invoice.WorkItem);
                    
                    //1.專案內容
                    var costCode = ProjectCostCode.GetAllDatas().Where(a => a.Code == invoice.CostCode).FirstOrDefault();                    
                    Dictionary<string, string> doc = new Dictionary<string, string>()
                    {
                        {"[$Year$]", invoice.PrjYear.ToString()},
                        {"[$WorkItem$]", invoice.WorkItem},
                        {"[$PjNoM$]", invoice.PrjPjNoM },
                        {"[$PrjName$]", invoice.PrjName },
                        {"[$CostName$]", costCode != null ? costCode.Name : "" },
                        {"[$CommissionedUnit$]", invoice.PrjCommissionedUnit },
                        {"[$Fee$]", invoice.Fee.ToString() },
                        {"[$PrjStartDate$]", DateFormat.ToTwDate5((DateTime)invoice.PrjStartDate) },
                        {"[$PrjEndDate$]", DateFormat.ToTwDate5((DateTime)invoice.PrjEndDate) },
                    };

                    // 獲取行數和列數
                    int rowCount = sheet.LastRowNum;
                    int columnCount = sheet.GetRow(0).LastCellNum - 1;

                    // 循環遍歷所有行和列
                    for (int i = 0; i <= rowCount; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        for (int j = 0; j <= columnCount; j++)
                        {
                            // 獲取單元格值
                            string cellValue = row.GetCell(j).ToString();

                            var vs = doc.Where(a => cellValue.Contains(a.Key));

                            if (vs.Count() > 0)
                            {
                                //1個儲存格可能有2個取代變數
                                foreach (var v in vs)
                                {
                                    if (v.Key == "[$Fee$]")
                                    {
                                        row.GetCell(j).SetCellValue(int.Parse(v.Value));
                                    }
                                    else
                                    {
                                        cellValue = cellValue.Replace(v.Key, v.Value);
                                        row.GetCell(j).SetCellValue(cellValue);
                                    }
                                }
                            }
                        }
                    }

                    //2.專家學者明細 第5列學者資料                    
                    int refn = 5;                    
                    IRow refRow = sheet.GetRow(refn);

                    int count = 0;
                    var basics = ProjectInvoiceBasic.GetAllDatas().Where(a => a.MId == id).ToList();
                    foreach (var basic in basics)
                    {                        
                        Dictionary<string, string> dicBasic = new Dictionary<string, string>()
                        {
                            {"[$ApplyDate$]", DateFormat.ToDate12_1(basic.ApplyDate.ToString())},
                            {"[$CopName$]", basic.CopName},
                            {"[$BasicName$]", basic.BasicName },
                            {"[$Amount$]", basic.Amount.ToString()},
                            {"[$Note$]", basic.Note },
                        };

                        int index = refn + count;  //實際新增row位置
                        var rowInsert = sheet.CreateRow(index);
                        
                        //Copy row style
                        for (int i = 0; i <= refRow.LastCellNum - 1; i++)
                        {
                            var cell = rowInsert.CreateCell(i);
                            cell.CellStyle = refRow.Cells[i].CellStyle;
                        }

                        //Merge Cell
                        //0：已Merge，使用原範本
                        if (count > 0)
                        {
                            //Merge the cell(起行、止行，起列，止列)                        
                            sheet.AddMergedRegion(new CellRangeAddress(index, index, 2, 5));
                            sheet.AddMergedRegion(new CellRangeAddress(index, index, 6, 7));
                            sheet.AddMergedRegion(new CellRangeAddress(index, index, 9, 10));
                        }

                        for (int j = 0; j <= columnCount; j++)
                        {
                            //序次
                            if (j == 0)
                            {
                                rowInsert.GetCell(j).SetCellValue(count + 1);
                                continue;
                            }

                            // 獲取單元格值
                            string cellValue = refRow.GetCell(j).ToString();

                            if (!string.IsNullOrEmpty(cellValue))
                            {                                
                                var vs = dicBasic.Where(a => cellValue.Contains(a.Key));

                                if (vs.Count() > 0)
                                {
                                    foreach (var v in vs)
                                    {
                                        //1個儲存格可能有2個取代變數
                                        if (v.Key == "[$Amount$]")
                                        {
                                            rowInsert.GetCell(j).SetCellValue(int.Parse(v.Value));
                                        }
                                        else
                                        {
                                            cellValue = cellValue.Replace(v.Key, v.Value);
                                            rowInsert.GetCell(j).SetCellValue(cellValue);
                                        }
                                    }
                                }
                            }
                        }

                        count++;
                    }

                    //寫入
                    FileStream s = new FileStream(toPath, FileMode.Create, FileAccess.Write);
                    workbook.Write(s);
                    s.Close();

                    workbook.Close();
                }

                url = Esdms.Cm.PhysicalToUrl(toPath);
            }
            catch (Exception ex)
            {
                _errorMessage = "產製請款單失敗：" + ex.Message;
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                return "";
            }

            return url;
        }

    }
}