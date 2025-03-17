using Esdms.Controllers.Es;
using Esdms.Models;
using Spire.Xls;
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
                File.Copy(sourcePath, toPath, true);

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