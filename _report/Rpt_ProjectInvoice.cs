using Esdms.Controllers.Es;
using Esdms.Models;
using NPOI.XSSF.UserModel;
using Spire.Xls;
using Spire.Xls.Core.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;

namespace Esdms
{
    public class Rpt_ProjectInvoice : ReportClass
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Export(string prjId)
        {
            string url = "";

            try
            {
                //複製範本
                string sourcePath = HttpContext.Current.Server.MapPath("~/DocsWeb/Template/") + "(範本)114年專家學者請款明細.xlsx";

                string fileName = System.IO.Path.GetFileNameWithoutExtension(sourcePath).Replace("(範本)", "") 
                                    + "_" + prjId + "_" 
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
                    workbook.SetSheetName(workbook.GetSheetIndex(sheet), "aaa");

                    //內容

                    //寫入
                    FileStream s = new FileStream(toPath, FileMode.Create, FileAccess.Write);
                    workbook.Write(s);
                    s.Close();

                    workbook.Close();
                }

                url = "abc";
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