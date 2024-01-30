using Esdms.Models;
using FtisHelperV2.DB.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Text.RegularExpressions;

namespace Esdms
{
    public class Rpt_BasicUserList : ReportClass
    {
        public string Export(List<BasicUser> datas, string ext)
        {
            string resultUrl = "";
            string path = "";

            if (!RType.ContainsKey(ext))
            {
                _errorMessage = "報表格式尚未設定此附檔名：" + ext;
                return "";
            }

            //產出檔案
            try
            {
                _dbContext = FtisHelperV2.DB.Helper.CreateFtisModelContext();

                ReportViewer reportViewer = new ReportViewer();
                reportViewer.ProcessingMode = ProcessingMode.Local;

                // 設定報表 iFrame Full Width
                reportViewer.SizeToReportContent = true;
                reportViewer.Width = Unit.Percentage(100);
                reportViewer.Height = Unit.Percentage(100);

                // Load Report File From Local Path
                reportViewer.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath("~/Report/BasicUserList/Master.rdlc");

                //主表                
                var dtData = GetMasterBasicUser(datas);

                if (dtData.Rows.Count == 0)
                    return "專家清單-無資料匯出";

                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MasterBasicUser", dtData));                               

                Microsoft.Reporting.WebForms.Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = reportViewer.LocalReport.Render(
                   RType[ext], null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

                string folder = FileHelper.GetFileFolder(Code.TempUploadFile.匯出專家清單);


                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName = "專家清單_" + DateFormat.ToDate1(DateTime.Now) + "_" + Guid.NewGuid().ToString().Substring(0, 5) + ext;  //"ext=.docx"
                path = folder + fileName;

                FileStream fs = new FileStream(path,
                   FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            catch (Exception ex)
            {
                _errorMessage = "匯出專家清單失敗" + "\n" + ex.InnerException + ex.Message + "\n" + ex.StackTrace;
                return "";
            }

            //回傳檔案網址
            if (path != "")
            {
                resultUrl = Esdms.Cm.PhysicalToUrl(path);
            }

            return resultUrl;
        }

        //主表
        private DataTable GetMasterBasicUser(List<BasicUser> datas)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("姓名"));
            dt.Columns.Add(new DataColumn("單位系所"));
            dt.Columns.Add(new DataColumn("職稱"));
            dt.Columns.Add(new DataColumn("專長"));
            dt.Columns.Add(new DataColumn("評選次數"));

            DataRow dr = null;
            foreach (var data in datas)
            {
                dr = dt.NewRow();

                dr["姓名"] = data.Name;
                dr["單位系所"] = data.UnitName;
                dr["職稱"] = data.Position;
                dr["專長"] = HtmlHelper.RemoveHtmlTag(data.strExpertises);
                dr["評選次數"] = HtmlHelper.RemoveHtmlTag(data.vmFTISJoinNum);                

                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}