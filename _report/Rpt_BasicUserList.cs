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
using Dou.Models;
using System.Dynamic;
using System.Web.Mvc;
using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using FtisHelperV2.DB.Helpe;
using FtisHelperV2.DB.Model;

namespace Esdms
{
    public class Rpt_BasicUserList : ReportClass
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Export(List<BasicUser> datas)
        {
            string url = "";

            ////List<string> titles = new List<string>() { "匯出專家清單，查詢條件:" };
            List<string> titles = new List<string>();

            try
            {
                string fileTitle = "專家清單";
                string folder = FileHelper.GetFileFolder(Code.TempUploadFile.匯出專家清單);

                //產出Dynamic資料 (給Excel)
                List<dynamic> list = new List<dynamic>();

                foreach (var data in datas)
                {
                    dynamic f = new ExpandoObject();
                    f.姓名 = data.Name;   //ooooooooooo                    
                    if (1 == 1)
                    {
                        var v = Code.GetSex().Where(a => int.Parse(a.Key) == data.Sex).FirstOrDefault();
                        f.性別 = v.Value;
                    }
                    if (1 == 1)
                    {
                        var v = Code.GetOnJob().Where(a => a.Key == data.OnJob).FirstOrDefault();
                        f.在職狀況 = v.Value;
                    }
                    f.手機號碼 = data.PrivatePhone;
                    f.辦公室電話 = data.OfficePhone;
                    f.辦公室電話2 = data.OfficePhone2;
                    f.傳真 = data.Fax;
                    f.辦公_Email = data.OfficeEmail;
                    f.私人_Email = data.PrivateEmail;                    
                    if (1 == 1)
                    {
                        var v = CitySelectItems.CITIES.Where(a => a.CityCode == data.CityCode).FirstOrDefault();
                        f.辦公_縣市 = v == null ? "" : v.Name;
                    }                    
                    if (1 == 1)
                    {
                        var v = TownSelectItems.Towns.Where(a => a.ZIP == data.ZIP).FirstOrDefault();
                        f.辦公_鄉鎮市區 = v == null ? "" : v.Name;
                    }
                    f.辦公_地址 = data.OfficeAddress;                    
                    if (1 == 1)
                    {
                        var v = CitySelectItems.CITIES.Where(a => a.CityCode == data.PCityCode).FirstOrDefault();
                        f.住家_縣市 = v == null ? "" : v.Name;
                    }                    
                    if (1 == 1)
                    {
                        var v = TownSelectItems.Towns.Where(a => a.ZIP == data.PZIP).FirstOrDefault();
                        f.住家_鄉鎮市區 = v == null ? "" : v.Name;
                    }
                    f.住家_地址 = data.PAddress;
                    f.備註 = data.Note;                    
                    if (1 == 1)
                    {
                        var v = CategorySelectItems.Categorys.Where(a => a.Id == data.CategoryId).FirstOrDefault();
                        f.人員類別 = v == null ? "" : v.Name;   //ooooooooooo
                    }
                    f.單位系所 = data.UnitName; //ooooooooooo
                    f.職稱 = data.Position;   //ooooooooooo
                    f.專長 = HtmlHelper.RemoveHtmlTag(data.strExpertises.Replace("</br>", "\n"));   //ooooooooooo
                    f.會外評選 = HtmlHelper.RemoveHtmlTag(data.vmOutCount.Replace("</br>", "\n"));  //ooooooooooo
                    f.會內參與 = HtmlHelper.RemoveHtmlTag(data.vmInCount.Replace("</br>", "\n"));   //ooooooooooo

                    f.SheetName = fileTitle;//sheep.名稱;
                    list.Add(f);
                }

                //查無符合資料表數
                if (list.Count == 0)
                {
                    _errorMessage = "查無符合資料表數";
                }

                //特殊儲存格位置Top
                List<string> topContents = new List<string>() { "專長", "會外評選", "會內參與" };

                //產出excel
                string fileName = Esdms.ExcelSpecHelper.GenerateExcelByLinqF1(fileTitle, titles, list, folder, "N", topContents);
                string path = folder + fileName;
                url = Esdms.Cm.PhysicalToUrl(path);
            }
            catch(Exception ex)
            {
                _errorMessage = "匯出專家清單失敗：" + ex.Message;
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                return "";
            }

            return url;
        }

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
                {
                    _errorMessage = "專家清單-無資料匯出";
                    return "";
                }

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
                dr["評選次數"] = HtmlHelper.RemoveHtmlTag(data.vmOutCount.Replace("</br>", "\n"));

                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}