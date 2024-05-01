using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using FtisHelperV2.DB.Helpe;
using FtisHelperV2.DB.Model;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using DouHelper;
using Dou.Models;
using System.Data.Entity.Infrastructure;
using System.Web.UI.WebControls;

namespace Esdms.Controllers.Es
{
    [Dou.Misc.Attr.MenuDef(Id = "BasicUser", Name = "專家基本資料", MenuPath = "專家資料", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class BasicUserController : APaginationModelController<BasicUser>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: BasicUser
        public ActionResult Index()
        {
            List<RoleUser> roles = Dou.Context.CurrentUser<User>().GetUserRoles();
            ViewBag.Roles = roles;

            //有專家編輯權限
            List<string> editRoles = new List<string>() { "admin", "DataManager", "ftisadmin" };
            ViewBag.IsView = roles.Where(a => editRoles.Any(b => a.RoleId == b)).Count() == 0;

            string path = Server.MapPath("~/Data/vPower.json");
            if (System.IO.File.Exists(path))
            {
                List<Esdms.vPower> items = null;

                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<Esdms.vPower>>(json);
                }

                var v = items.Where(a => a.empno == Dou.Context.CurrentUserBase.Id).FirstOrDefault();
                if (v != null)
                {
                    string ss = v.empno;
                    ViewBag.VPower = v.vpower.Split(',').ToArray();
                }
            }

            return View();
        }

        protected override IQueryable<BasicUser> BeforeIQueryToPagedList(IQueryable<BasicUser> iquery, params KeyValueParams[] paras)
        {
            iquery = GetOutputData(iquery, paras);

            //////////////////////////////
            ////iquery = iquery.Where(a => a.PId == "S202404070002");

            return base.BeforeIQueryToPagedList(iquery, paras);
        }

        //匯出清單
        public ActionResult ExportList(List<string> Names, vwe_ChkExport chks, string sort, string order, params KeyValueParams[] paras)
        {
            if (sort != null)
            {
                KeyValueParams k = new KeyValueParams();
                k.key = "sort"; 
                k.value = sort;
                paras = paras.Concat(new KeyValueParams[1] { k }).ToArray();
            }
            if (order != null)
            {
                KeyValueParams k = new KeyValueParams();
                k.key = "order";
                k.value = order;
                paras = paras.Concat(new KeyValueParams[1] { k }).ToArray();
            }

            var iquery = GetModelEntity().GetAll();

            //沒勾選，預設全部
            if (Names != null && Names.Count>0)
            {
                iquery = iquery.Where(a => Names.Contains(a.Name));
            }

            iquery = GetOutputData(iquery, paras);
            var datas = iquery.ToList();

            Rpt_BasicUserList rep = new Rpt_BasicUserList();
            //string url = rep.Export(datas, ".xlsx");
            string url = rep.Export(chks, datas);

            if (url == "")
            {
                return Json(new { result = false, errorMessage = rep.ErrorMessage }, JsonRequestBehavior.AllowGet);                
            }
            else
            {
                return Json(new { result = true, url = url }, JsonRequestBehavior.AllowGet);
            }            
        }

        private IQueryable<BasicUser> GetOutputData(IQueryable<BasicUser> iquery, params KeyValueParams[] paras)
        {
            //---1.查詢---
            var Names = KeyValue.GetFilterParaValue(paras, "Names");
            var Name = KeyValue.GetFilterParaValue(paras, "Name");
            var DuplicateName = KeyValue.GetFilterParaValue(paras, "DuplicateName");
            var SubjectId = KeyValue.GetFilterParaValue(paras, "SubjectId");
            var SubjectDetailId = KeyValue.GetFilterParaValue(paras, "SubjectDetailId");
            var strExpertises = KeyValue.GetFilterParaValue(paras, "strExpertises");

            if (!string.IsNullOrEmpty(Names))
            {
                var aryNames = Names.Split(',');
                iquery = iquery.Where(a => aryNames.Contains(a.Name));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                iquery = iquery.Where(a => a.Name.IndexOf(Name) > -1);
            }
            //是否重複查詢            
            if (!string.IsNullOrEmpty(DuplicateName))
            {
                var v = iquery.GroupBy(a => a.Name)
                        .Select(a => new
                        {
                            Name = a.Key,
                            Count = a.Count(),
                        });
                if (DuplicateName == "Y")
                {
                    v = v.Where(a => a.Count > 1);
                }
                else
                {
                    v = v.Where(a => a.Count == 1);
                }

                iquery = iquery.Where(a => v.Any(b => b.Name == a.Name));
                iquery = iquery.OrderBy(a => a.Name);
            }

            //專長類別查詢(M 下拉) 虛擬欄位            
            if (!string.IsNullOrEmpty(SubjectId))
            {
                var enumerable = iquery.AsEnumerable();
                enumerable = enumerable.Where(a => a.Expertises.Any(b => b.SubjectId.ToString() == SubjectId));
                iquery = enumerable.AsQueryable();
            }
            //專長領域查詢(D 下拉) 虛擬欄位            
            if (!string.IsNullOrEmpty(SubjectDetailId))
            {
                var enumerable = iquery.AsEnumerable();
                var SubjectDetailIds = SubjectDetailId.Split(',');
                enumerable = enumerable.Where(a => a.Expertises.Any(b => SubjectDetailIds.Any(p => p == b.SubjectDetailId.ToString())));
                iquery = enumerable.AsQueryable();
            }

            //專長領域查詢(文字) 虛擬欄位            
            if (!string.IsNullOrEmpty(strExpertises))
            {
                var enumerable = iquery.AsEnumerable();
                enumerable = enumerable.Where(a => a.strExpertises.Contains(strExpertises));
                iquery = enumerable.AsQueryable();
            }

            //---2.排序---
            KeyValueParams ksort = paras.FirstOrDefault((KeyValueParams s) => s.key == "sort");
            KeyValueParams korder = paras.FirstOrDefault((KeyValueParams s) => s.key == "order");

            if (ksort == null || ksort.value == null)
            {
                //預設排序
                iquery = iquery.OrderByDescending(a => a.PId);
            }
            else if (ksort.value.ToString() == "vmOutCount")
            {
                string sort = ksort.value.ToString();
                string order = korder.value.ToString();
                var enumerable = iquery.AsEnumerable();

                if (order == "asc")
                {
                    enumerable = enumerable.OrderBy(a => a.vmTotalOutCount).ThenBy(a => a.Name);//order by name(清單第一欄位排序)
                }
                else if (order == "desc")
                {
                    enumerable = enumerable.OrderByDescending(a => a.vmTotalOutCount).ThenBy(a => a.Name);//order by name(清單第一欄位排序)
                }
                iquery = enumerable.AsQueryable();
            }
            else if (ksort.value.ToString() == "vmInCount")
            {
                string sort = ksort.value.ToString();
                string order = korder.value.ToString();
                var enumerable = iquery.AsEnumerable();

                if (order == "asc")
                {
                    enumerable = enumerable.OrderBy(a => a.vmTotalInCount).ThenBy(a => a.Name);//order by name(清單第一欄位排序)
                }
                else if (order == "desc")
                {
                    enumerable = enumerable.OrderByDescending(a => a.vmTotalInCount).ThenBy(a => a.Name);//order by name(清單第一欄位排序)
                }
                iquery = enumerable.AsQueryable();
            }

            return iquery;
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var options = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in options.fields)
            {
                field.sortable = true;
                field.visible = false;
            }
            
            options.GetFiled("Name").visible = true;
            options.GetFiled("UnitName").visible = true;
            options.GetFiled("UnitName").title = "單位(系所)";
            options.GetFiled("Position").visible = true;
            //options.GetFiled("BDate").visible = true;
            options.GetFiled("CategoryId").visible = true;            
            options.GetFiled("SubjectId").filter = true;
            options.GetFiled("SubjectDetailId").filter = true;
            options.GetFiled("strExpertises").visible = true;
            options.GetFiled("strExpertises").filter = true;
            options.GetFiled("vmOutCount").visible = true;
            options.GetFiled("vmInCount").visible = true;

            options.ctrlFieldAlign = "left";
            options.editformWindowStyle = "modal";
            options.editformWindowClasses = "modal-xl";
            options.editformSize.height = "fixed";
            options.editformSize.width = "auto";

            options.GetFiled("OfficeEmail").colsize = 6;
            options.GetFiled("PrivateEmail").colsize = 6;
            options.GetFiled("OfficeAddress").colsize = 6;
            options.GetFiled("PAddress").colsize = 6;

            options.GetFiled("Note").colsize = 12;
            options.GetFiled("Note").datatype = "textarea";
            options.GetFiled("Note").textareaheight = 9;

            //共用頁面
            options.editformWindowStyle = "showEditformOnly";

            return options;
        }

        protected override void AddDBObject(IModelEntity<BasicUser> dbEntity, IEnumerable<BasicUser> objs)
        {
            if (!ToValidate(objs.First(), "Add"))
                return;

            var f = objs.First();

            f.PId = GenerateHelper.FId(dbEntity.GetAll().ToList());
            f.BDate = DateTime.Now;
            f.BFno = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);
            BasicUserNameSelectItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<BasicUser> dbEntity, IEnumerable<BasicUser> objs)
        {
            var f = objs.First();

            f.UDate = DateTime.Now;
            f.UFno = Dou.Context.CurrentUserBase.Id;
            f.UName = Dou.Context.CurrentUserBase.Name;

            base.UpdateDBObject(dbEntity, objs);
            BasicUserNameSelectItems.Reset();
        }

        protected override void DeleteDBObject(IModelEntity<BasicUser> dbEntity, IEnumerable<BasicUser> objs)
        {
            var obj = objs.FirstOrDefault();

            //DB沒關聯
            var dbContext = new EsdmsModelContextExt();

            if (obj.Expertises != null)
            {
                Dou.Models.DB.IModelEntity<Expertise> expertise = new Dou.Models.DB.ModelEntity<Expertise>(dbContext);
                expertise.Delete(obj.Expertises);
            }
            if (obj.UserHistoryOpinions != null)
            {
                Dou.Models.DB.IModelEntity<UserHistoryOpinion> userHistoryOpinion = new Dou.Models.DB.ModelEntity<UserHistoryOpinion>(dbContext);
                userHistoryOpinion.Delete(obj.UserHistoryOpinions);                
            }
            if (obj.Resumes != null)
            {
                Dou.Models.DB.IModelEntity<Resume> resume = new Dou.Models.DB.ModelEntity<Resume>(dbContext);
                resume.Delete(obj.Resumes);                
            }
            if (obj.FTISUserHistorys != null)
            {
                Dou.Models.DB.IModelEntity<FTISUserHistory> ftisUserHistory = new Dou.Models.DB.ModelEntity<FTISUserHistory>(dbContext);
                ftisUserHistory.Delete(obj.FTISUserHistorys);                
            }
            if (obj.BasicUser_Licenses != null)
            {
                Dou.Models.DB.IModelEntity<BasicUser_License> basicUser_License = new Dou.Models.DB.ModelEntity<BasicUser_License>(dbContext);
                basicUser_License.Delete(obj.BasicUser_Licenses);
            }

            base.DeleteDBObject(dbEntity, objs);
            BasicUserNameSelectItems.Reset();
        }

        //姓名已存在(true:有)
        public ActionResult ExistName(string PId, string Name)
        {
            //原身分 + 姓名沒異動
            int o = GetModelEntity().GetAll().Where(a => a.PId == PId && a.Name == Name).Count();
            if (o > 0)
            {
                return Json(new { exist = false }, JsonRequestBehavior.AllowGet);
            }

            //原身分 + 姓名有異動
            var u = GetModelEntity().GetAll().Where(a => a.PId != PId && a.Name == Name);
            return Json(new { exist = u.Count() > 0, basicuser = u }, JsonRequestBehavior.AllowGet);
        }

        //取得單筆user
        public ActionResult GetBasicUser(string PId)
        {
            var u = GetModelEntity().GetAll().Where(a => a.PId == PId);

            var jstr = JsonConvert.SerializeObject(u, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

        //取得autocomplete user
        public ActionResult GetBasicUserList(string searchKeyword)
        {
            var jquery = BasicUserNameSelectItems.BasicUsers;//.Where(a => a.PId == PId);

            jquery = jquery.Where(a=>a.Name.Contains(searchKeyword));

            var result = jquery.Select(a => new { 
                a.PId, a.Name,
            });

            var jstr = JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

        //上傳檔案(匯入專家資料)
        public ActionResult UpFile()
        {
            try
            {
                //步驟1.上傳檔案
                string folder = FileHelper.GetFileFolder(Code.TempUploadFile.匯入專家資料);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                HttpPostedFileBase file = Request.Files[0];
                //檔名'_'，需拿掉(因資料夾分類使用)
                string fileName = Guid.NewGuid().ToString() + "_" + file.FileName.Replace("_", "-");
                string path = folder + fileName;
                if (file.ContentLength > 0)
                {
                    file.SaveAs(path);
                }

                //步驟2.讀取檔案(excel失敗)
                DataTable dt = null;
                using (var f = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[f.Length];
                    f.Read(bytes, 0, (int)f.Length);
                    f.Position = 0;

                    var workbook = WorkbookFactory.Create(f);
                    //指定的Sheet
                    //var sheet = workbook.GetSheetAt(sheetIndex);
                    //指定為Header的Row
                    int sheetCount = workbook.NumberOfSheets;

                    //讀第一個Sheet
                    var sheet = workbook.GetSheetAt(0);
                    //需要調整讀取每個row->ReadDataTableFromSheet
                    int titleCol = 1;
                    var table = ExcelHelper.ExcelToDt(sheet, titleCol - 1);

                    ////過濾：循環添加標題行(excel沒定義的標題)
                    for (int i = table.Columns.Count; i-- > 0;)
                        if (table.Columns[i].ColumnName.IndexOf("Column") > -1)
                            table.Columns.RemoveAt(i);

                    dt = table;
                }

                if (dt == null)
                {
                    return Json(new { result = false, errorMessage = "步驟2.讀取檔案(excel失敗)" });
                }

                //步驟3.儲存檔案
                //標題
                int outYear = 0;                
                Dictionary<string, int> dic = new Dictionary<string, int>();
                for (int index = 0; index < dt.Columns.Count; index++)
                {
                    string name = dt.Columns[index].ColumnName;
                    if (name.Contains("_Add")) { dic.Add("_Add", index); continue; }
                    if (name.Contains("_Name")) { dic.Add("_Name", index); continue; }
                    if (name.Contains("_Sex")) { dic.Add("_Sex", index); continue; }
                    if (name.Contains("_CategoryId")) { dic.Add("_CategoryId", index); continue; }
                    if (name.Contains("_OnJob")) { dic.Add("_OnJob", index); continue; }
                    if (name.Contains("_UnitName")) { dic.Add("_UnitName", index); continue; }
                    if (name.Contains("_Position")) { dic.Add("_Position", index); continue; }
                    if (name.Contains("_OfficePhone")) { dic.Add("_OfficePhone", index); continue; }
                    if (name.Contains("_2OfficePhone")) { dic.Add("_OfficePhone2", index); continue; }
                    if (name.Contains("_PrivatePhone")) { dic.Add("_PrivatePhone", index); continue; }
                    if (name.Contains("_Fax")) { dic.Add("_Fax", index); continue; }
                    if (name.Contains("_OfficeEmail")) { dic.Add("_OfficeEmail", index); continue; }
                    if (name.Contains("_PrivateEmail")) { dic.Add("_PrivateEmail", index); continue; }
                    if (name.Contains("_CityCode")) { dic.Add("_CityCode", index); continue; }
                    if (name.Contains("_ZIP")) { dic.Add("_ZIP", index); continue; }
                    if (name.Contains("_OfficeAddress")) { dic.Add("_OfficeAddress", index); continue; }
                    if (name.Contains("_PCityCode")) { dic.Add("_PCityCode", index); continue; }
                    if (name.Contains("_PZIP")) { dic.Add("_PZIP", index); continue; }
                    if (name.Contains("_Paddress")) { dic.Add("_Paddress", index); continue; }
                    if (name.Contains("_Note")) { dic.Add("_Note", index); continue; }
                    if (name.Contains("_Env_")) 
                    { 
                        dic.Add("_Env_", index);
                        if (name.Split('_').Length == 3)
                        {
                            int.TryParse(name.Split('_')[2], out outYear);
                        }
                        continue; 
                    }
                    if (name.Contains("_Eng_")) { dic.Add("_Eng_", index); continue; }
                    if (name.Contains("_Ida_")) { dic.Add("_Ida_", index); continue; }
                    if (name.Contains("_SubjectDetailId")) { dic.Add("_SubjectDetailId", index); continue; }
                    if (name.Contains("_FtisActivityCategoryId")) { dic.Add("_FtisActivityCategoryId", index); continue; }
                    if (name.Contains("_FtisDCode")) { dic.Add("_FtisDCode", index); continue; }
                    if (name.Contains("_FtisOwner")) { dic.Add("_FtisOwner", index); continue; }
                    if (name.Contains("_FtisYear")) { dic.Add("_FtisYear", index); continue; }
                    if (name.Contains("_FtisProjectId")) { dic.Add("_FtisProjectId", index); continue; }
                    if (name.Contains("_FtisOutActivityCategoryId")) { dic.Add("_FtisOutActivityCategoryId", index); continue; }
                    if (name.Contains("_SetOutYear")) { dic.Add("_SetOutYear", index); continue; }
                    if (name.Contains("_SetName")) { dic.Add("_SetName", index); continue; }                    
                    if (name.Contains("_SetBidName")) { dic.Add("_SetBidName", index); continue; }
                }

                //int cc = 0;
                var dbContext = new EsdmsModelContextExt();
                Dou.Models.DB.IModelEntity<BasicUser> basicUser = new Dou.Models.DB.ModelEntity<BasicUser>(dbContext);
                Dou.Models.DB.IModelEntity<Expertise> expertise = new Dou.Models.DB.ModelEntity<Expertise>(dbContext);
                Dou.Models.DB.IModelEntity<FTISUserHistory> fTISUserHistory = new Dou.Models.DB.ModelEntity<FTISUserHistory>(dbContext);
                Dou.Models.DB.IModelEntity<SubjectDetail> subjectDetail = new Dou.Models.DB.ModelEntity<SubjectDetail>(dbContext);                
                Dou.Models.DB.IModelEntity<ActivityCategory> activityCategory = new Dou.Models.DB.ModelEntity<ActivityCategory>(dbContext);
                Dou.Models.DB.IModelEntity<Esdms.Models.Project> project = new Dou.Models.DB.ModelEntity<Esdms.Models.Project>(dbContext);
                Dou.Models.DB.IModelEntity<Esdms.Models.UserHistorySet> userHistorySet = new Dou.Models.DB.ModelEntity<Esdms.Models.UserHistorySet>(dbContext);
                Dou.Models.DB.IModelEntity<Esdms.Models.UserHistorySetBid> userHistorySetBid = new Dou.Models.DB.ModelEntity<Esdms.Models.UserHistorySetBid>(dbContext);

                var m_subjectDetail = subjectDetail.GetAll().ToList();
                var m_activityCategory = activityCategory.GetAll().ToList();
                var m_project = project.GetAll().ToList();

                foreach (DataRow row in dt.Rows)
                {
                    //cc++; if (cc > 3) break;

                    //_Env_ooo,_Eng_ooo,_Ida_ooo
                    //_Add,_Name,_Sex,_CategoryId,_OnJob,_UnitName,_Position,_SubjectDetailId
                    string _Add = !dic.ContainsKey("_Add") ? "" : row.ItemArray[dic["_Add"]].ToString();
                    if (_Add.ToUpper() == "N")
                    {
                        continue;
                    }

                    string name = !dic.ContainsKey("_Name") ? "" : row.ItemArray[dic["_Name"]].ToString();
                    string ___sex = !dic.ContainsKey("_Sex") ? "" : row.ItemArray[dic["_Sex"]].ToString();
                    string ___categoryId = !dic.ContainsKey("_CategoryId") ? "" : row.ItemArray[dic["_CategoryId"]].ToString();
                    string ___onJob = !dic.ContainsKey("_OnJob") ? "" : row.ItemArray[dic["_OnJob"]].ToString();
                    string unitName = !dic.ContainsKey("_UnitName") ? "" : row.ItemArray[dic["_UnitName"]].ToString();
                    string position = !dic.ContainsKey("_Position") ? "" : row.ItemArray[dic["_Position"]].ToString();
                    string officePhone = !dic.ContainsKey("_OfficePhone") ? "" : row.ItemArray[dic["_OfficePhone"]].ToString();
                    string officePhone2 = !dic.ContainsKey("_OfficePhone2") ? "" : row.ItemArray[dic["_OfficePhone2"]].ToString();
                    string privatePhone = !dic.ContainsKey("_PrivatePhone") ? "" : row.ItemArray[dic["_PrivatePhone"]].ToString();
                    string fax = !dic.ContainsKey("_Fax") ? "" : row.ItemArray[dic["_Fax"]].ToString();
                    string officeEmail = !dic.ContainsKey("_OfficeEmail") ? "" : row.ItemArray[dic["_OfficeEmail"]].ToString();
                    string privateEmail = !dic.ContainsKey("_PrivateEmail") ? "" : row.ItemArray[dic["_PrivateEmail"]].ToString();                   
                    string ___cityCode = !dic.ContainsKey("_CityCode") ? "" : row.ItemArray[dic["_CityCode"]].ToString();                    
                    string ___zip = !dic.ContainsKey("_ZIP") ? "" : row.ItemArray[dic["_ZIP"]].ToString();
                    string officeAddress = !dic.ContainsKey("_OfficeAddress") ? "" : row.ItemArray[dic["_OfficeAddress"]].ToString();
                    string ___pCityCode = !dic.ContainsKey("_PCityCode") ? "" : row.ItemArray[dic["_PCityCode"]].ToString();
                    string ___pZIP = !dic.ContainsKey("_PZIP") ? "" : row.ItemArray[dic["_PZIP"]].ToString();
                    string paddress = !dic.ContainsKey("_Paddress") ? "" : row.ItemArray[dic["_Paddress"]].ToString();
                    string note = !dic.ContainsKey("_Note") ? "" : row.ItemArray[dic["_Note"]].ToString();

                    string ___env = !dic.ContainsKey("_Env_") ? "" : row.ItemArray[dic["_Env_"]].ToString();
                    string ___eng = !dic.ContainsKey("_Eng_") ? "" : row.ItemArray[dic["_Eng_"]].ToString();
                    string ___ida = !dic.ContainsKey("_Ida_") ? "" : row.ItemArray[dic["_Ida_"]].ToString();
                    string ___subjectDetailId = !dic.ContainsKey("_SubjectDetailId") ? "" : row.ItemArray[dic["_SubjectDetailId"]].ToString();

                    string ___ftisActivityCategoryId = !dic.ContainsKey("_FtisActivityCategoryId") ? "" : row.ItemArray[dic["_FtisActivityCategoryId"]].ToString();
                    string ___ftisDCode = !dic.ContainsKey("_FtisDCode") ? "" : row.ItemArray[dic["_FtisDCode"]].ToString();
                    string ftisOwner = !dic.ContainsKey("_FtisOwner") ? "" : row.ItemArray[dic["_FtisOwner"]].ToString();
                    string ___ftisYear = !dic.ContainsKey("_FtisYear") ? "" : row.ItemArray[dic["_FtisYear"]].ToString();
                    string ___ftisProjectId = !dic.ContainsKey("_FtisProjectId") ? "" : row.ItemArray[dic["_FtisProjectId"]].ToString();                    
                    string ___ftisOutActivityCategoryId = !dic.ContainsKey("_FtisOutActivityCategoryId") ? "" : row.ItemArray[dic["_FtisOutActivityCategoryId"]].ToString();
                    string ___setOutYear = !dic.ContainsKey("_SetOutYear") ? "" : row.ItemArray[dic["_SetOutYear"]].ToString();
                    string setName = !dic.ContainsKey("_SetName") ? "" : row.ItemArray[dic["_SetName"]].ToString();                    
                    string setBidName = !dic.ContainsKey("_SetBidName") ? "" : row.ItemArray[dic["_SetBidName"]].ToString();
                    if (name == "")
                        continue;

                    //(1)儲存basicUser
                    var v1 = Code.GetSex().Where(a => a.Value.ToString() == ___sex);
                    int? sex = v1.Count() == 0 ? (int?)null : int.Parse(v1.FirstOrDefault().Key);

                    var v2 = CategorySelectItems.Categorys.Where(a => a.Name == ___categoryId);
                    int? categoryId = v2.Count() == 0 ? (int?)null : v2.FirstOrDefault().Id;

                    var v3 = Code.GetOnJob().Where(a => a.Value.ToString() == ___onJob);
                    string onJob = v3.Count() == 0 ? null : v3.FirstOrDefault().Key;

                    var v4 = CitySelectItems.CITIES.Where(a => a.Name == ___cityCode);
                    string cityCode = v4.Count() == 0 ? null : v4.FirstOrDefault().CityCode;

                    var v5 = TownSelectItems.Towns.Where(a => a.Name == ___zip);
                    string zip = v5.Count() == 0 ? null : v5.FirstOrDefault().ZIP;

                    var v6 = CitySelectItems.CITIES.Where(a => a.Name == ___pCityCode);
                    string pCityCode = v6.Count() == 0 ? null : v6.FirstOrDefault().CityCode;

                    var v7 = TownSelectItems.Towns.Where(a => a.Name == ___pZIP);
                    string pzip = v7.Count() == 0 ? null : v7.FirstOrDefault().ZIP;

                    //(會外)會議 - 環境部
                    int env_num = 0;                    
                    int.TryParse(___env, out env_num);

                    //(會外)會議 - 能源署
                    int eng_num = 0;
                    int.TryParse(___eng, out eng_num);

                    //(會外)會議 - 產發署
                    int ida_num = 0;
                    int.TryParse(___ida, out ida_num);

                    //(會內)會議
                    var ftisActivityCategoryId = m_activityCategory.Where(a => a.Type == 1)
                                                        .Where(a => ___ftisActivityCategoryId.Split(',').Any(b => b == a.DCode)).ToList();

                    //專案(單筆)
                    var ftisProject = m_project.Where(a => ___ftisProjectId.Split(',').Any(b => b == a.DCode)).ToList();

                    //部門(單筆) 用名稱比對
                    var ftisDCode = Code.GetDepartment().Where(a => ___ftisDCode == a.Value.ToString()).ToList();

                    //專長
                    var subjectDetailId = m_subjectDetail.Where(a => ___subjectDetailId.Split(',').Any(b => b == a.DCode)).ToList();

                    //(會外)會議
                    var ftisOutActivityCategoryId = m_activityCategory.Where(a => a.Type == 2)
                                                        .Where(a => ___ftisOutActivityCategoryId.Split(',').Any(b => b == a.DCode)).ToList();

                    string rPId = "";
                    var data = basicUser.GetAll().Where(a => a.Name == name).FirstOrDefault();
                    if (data == null)
                    {
                        //新增
                        Esdms.Models.BasicUser nuser = new Esdms.Models.BasicUser();
                        nuser.PId = GenerateHelper.FId(basicUser.GetAll().ToList());

                        nuser.Name = name;
                        //excel有欄位顯示才設定
                        if (dic.ContainsKey("_Sex")) nuser.Sex = sex;
                        if (dic.ContainsKey("_CategoryId")) nuser.CategoryId = categoryId;
                        if (dic.ContainsKey("_OnJob")) nuser.OnJob = onJob;
                        if (dic.ContainsKey("_UnitName")) nuser.UnitName = unitName;
                        if (dic.ContainsKey("_Position")) nuser.Position = position;
                        if (dic.ContainsKey("_OfficePhone")) nuser.OfficePhone = officePhone;
                        if (dic.ContainsKey("_OfficePhone2")) nuser.OfficePhone2 = officePhone2;
                        if (dic.ContainsKey("_PrivatePhone")) nuser.PrivatePhone = privatePhone;
                        if (dic.ContainsKey("_Fax")) nuser.Fax = fax;
                        if (dic.ContainsKey("_OfficeEmail")) nuser.OfficeEmail = officeEmail;
                        if (dic.ContainsKey("_PrivateEmail")) nuser.PrivateEmail = privateEmail;
                        if (dic.ContainsKey("_CityCode")) nuser.CityCode = cityCode;
                        if (dic.ContainsKey("_ZIP")) nuser.ZIP = zip;
                        if (dic.ContainsKey("_OfficeAddress")) nuser.OfficeAddress = officeAddress;                                               
                        if (dic.ContainsKey("_PCityCode")) nuser.PCityCode = pCityCode;
                        if (dic.ContainsKey("_PZIP")) nuser.PZIP = pzip;
                        if (dic.ContainsKey("_Paddress")) nuser.PAddress = paddress;
                        if (dic.ContainsKey("_Note")) nuser.Note = note;
                        nuser.BDate = DateTime.Now;
                        nuser.BFno = Dou.Context.CurrentUserBase.Id;
                        nuser.BName = Dou.Context.CurrentUserBase.Name;                                                

                        basicUser.Add(nuser);
                        rPId = nuser.PId;
                                             
                    }
                    else
                    {
                        //修改
                        ////data.Name = name;  (where name)，不需要修改
                        if (dic.ContainsKey("_Sex") && sex != null) data.Sex = sex;
                        if (dic.ContainsKey("_CategoryId") && categoryId != null) data.CategoryId = categoryId;
                        if (dic.ContainsKey("_OnJob") && onJob != null) data.OnJob = onJob;
                        if (dic.ContainsKey("_UnitName") && !string.IsNullOrEmpty(unitName.Trim())) data.UnitName = unitName;
                        if (dic.ContainsKey("_Position") && !string.IsNullOrEmpty(position.Trim())) data.Position = position;

                        if (dic.ContainsKey("_OfficePhone") && !string.IsNullOrEmpty(officePhone.Trim())) data.OfficePhone = officePhone;
                        if (dic.ContainsKey("_OfficePhone2") && !string.IsNullOrEmpty(officePhone2.Trim())) data.OfficePhone2 = officePhone2;
                        if (dic.ContainsKey("_PrivatePhone") && !string.IsNullOrEmpty(privatePhone.Trim())) data.PrivatePhone = privatePhone;
                        if (dic.ContainsKey("_Fax") && !string.IsNullOrEmpty(fax.Trim())) data.Fax = fax;
                        if (dic.ContainsKey("_OfficeEmail") && !string.IsNullOrEmpty(officeEmail.Trim())) data.OfficeEmail = officeEmail;
                        if (dic.ContainsKey("_PrivateEmail") && !string.IsNullOrEmpty(privateEmail.Trim())) data.PrivateEmail = privateEmail;
                        if (dic.ContainsKey("_CityCode") && cityCode != null) data.CityCode = cityCode;
                        if (dic.ContainsKey("_ZIP") && zip != null) data.ZIP = zip;
                        if (dic.ContainsKey("_OfficeAddress") && !string.IsNullOrEmpty(officeAddress.Trim())) data.OfficeAddress = officeAddress;                        
                        if (dic.ContainsKey("_PCityCode") && pCityCode != null) data.PCityCode = pCityCode;                        
                        if (dic.ContainsKey("_PZIP") && pzip != null) data.PZIP = pzip;
                        if (dic.ContainsKey("_Paddress") && !string.IsNullOrEmpty(paddress.Trim())) data.PAddress = paddress;
                        if (dic.ContainsKey("_Note") && !string.IsNullOrEmpty(note.Trim())) data.Note = note;

                        data.UDate = DateTime.Now;
                        data.UFno = Dou.Context.CurrentUserBase.Id;
                        data.UName = Dou.Context.CurrentUserBase.Name;

                        basicUser.Update(data);
                        rPId = data.PId;
                    }

                    if (rPId != "")
                    {
                        #region  更新專家參與紀錄(會外)
                        
                        //(1)環境部
                        if (dic.ContainsKey("_Env_"))
                        {
                            var envId = activityCategory.GetAll().Where(a => a.Type == 2)
                                        .Where(a => a.Name.Contains("環境部")).FirstOrDefault().Id;
                            var h1 = fTISUserHistory.GetAll()
                                    .Where(a => a.PId == rPId
                                        && a.OutYear == outYear && a.ActivityCategoryId == envId).FirstOrDefault();

                            if (h1 == null)
                            {
                                FTISUserHistory addHistory1 = new FTISUserHistory();
                                addHistory1.PId = rPId;
                                addHistory1.ActivityCategoryType = 2;    //會外
                                addHistory1.ActivityCategoryId = envId;
                                addHistory1.ActivityCategoryJoinNum = env_num;
                                addHistory1.OutYear = outYear;
                                fTISUserHistory.Add(addHistory1);
                            }
                            else
                            {
                                h1.ActivityCategoryJoinNum = env_num;
                                fTISUserHistory.Update(h1);
                            }
                        }

                        //(2)能源署
                        if (dic.ContainsKey("_Eng_"))
                        {
                            var engId = activityCategory.GetAll().Where(a => a.Type == 2)
                                        .Where(a => a.Name.Contains("能源署")).FirstOrDefault().Id;
                            var h2 = fTISUserHistory.GetAll()
                                    .Where(a => a.PId == rPId
                                        && a.OutYear == outYear && a.ActivityCategoryId == engId).FirstOrDefault();

                            if (h2 == null)
                            {
                                FTISUserHistory addHistory1 = new FTISUserHistory();
                                addHistory1.PId = rPId;
                                addHistory1.ActivityCategoryType = 2;    //會外
                                addHistory1.ActivityCategoryId = engId;
                                addHistory1.ActivityCategoryJoinNum = eng_num;
                                addHistory1.OutYear = outYear;
                                fTISUserHistory.Add(addHistory1);
                            }
                            else
                            {
                                h2.ActivityCategoryJoinNum = eng_num;
                                fTISUserHistory.Update(h2);
                            }
                        }

                        //(3)產發署
                        if (dic.ContainsKey("_Ida_"))
                        {
                            var idaId = activityCategory.GetAll().Where(a => a.Type == 2)
                                        .Where(a => a.Name.Contains("產發署")).FirstOrDefault().Id;
                            var h3 = fTISUserHistory.GetAll()
                                    .Where(a => a.PId == rPId
                                        && a.OutYear == outYear && a.ActivityCategoryId == idaId).FirstOrDefault();

                            if (h3 == null)
                            {
                                FTISUserHistory addHistory1 = new FTISUserHistory();
                                addHistory1.PId = rPId;
                                addHistory1.ActivityCategoryType = 2;    //會外
                                addHistory1.ActivityCategoryId = idaId;
                                addHistory1.ActivityCategoryJoinNum = ida_num;
                                addHistory1.OutYear = outYear;
                                fTISUserHistory.Add(addHistory1);
                            }
                            else
                            {
                                h3.ActivityCategoryJoinNum = ida_num;
                                fTISUserHistory.Update(h3);
                            }
                        }

                        #endregion

                        #region  更新專家參與紀錄 會內(刪除新增)

                        if (ftisActivityCategoryId.Count > 0)
                        {                            
                            int ftisYear = 0;
                            if (int.TryParse(___ftisYear, out ftisYear) && ftisProject.Count > 0)
                            {
                                //a_1(會內)刪除(條件：年度+專案ID)
                                var delfTISUserHistory = fTISUserHistory.GetAll().Where(a => a.PId == rPId
                                                            && a.Year == ftisYear && a.ActivityCategoryType == 1)
                                                            .AsEnumerable()
                                                            .Where(a => ftisProject.Any(b => b.Id == a.ProjectId));
                                
                                var aa = delfTISUserHistory.ToList();
                                fTISUserHistory.Delete(delfTISUserHistory);

                                //a_2(會內)新增
                                var FTISUserHistorys = ftisActivityCategoryId.Where(a => a.Type == 1).Select(a => new FTISUserHistory
                                {
                                    PId = rPId,
                                    DCode = ftisDCode.Count == 0 ? null : ftisDCode.FirstOrDefault().Key,
                                    Year = ftisYear,
                                    ProjectId = ftisProject == null ? (int?)null : ftisProject.FirstOrDefault().Id,
                                    ActivityCategoryType = 1,  //1會內
                                    ActivityCategoryId = a.Id,
                                    Owner = ftisOwner,
                                    //SubjectId = a.SubjectId,
                                    //SubjectDetailId = a.Id,
                                });
                                fTISUserHistory.Add(FTISUserHistorys);
                            }                            
                        }

                        #endregion

                        #region  更新專家參與紀錄 會外(不存在新增)

                        if (ftisOutActivityCategoryId.Count > 0)
                        {
                            //會外
                            int SetOutYear = 0;
                            if (int.TryParse(___setOutYear, out SetOutYear))
                            {
                                //b_1(會外) 會議名稱不存在就新增(會議名稱)
                                var now = fTISUserHistory.GetAll().Where(a => a.PId == rPId
                                                            && a.OutYear == SetOutYear && a.ActivityCategoryType == 2).ToList();

                                //新增
                                var FTISUserHistorys = ftisOutActivityCategoryId.Where(a => !now.Any(b => b.ActivityCategoryId == a.Id))
                                                        .Select(a => new FTISUserHistory
                                                        {
                                                            PId = rPId,
                                                            OutYear = SetOutYear,
                                                            ActivityCategoryType = 2,  //2會外
                                                            ActivityCategoryId = a.Id,
                                                        });
                                fTISUserHistory.Add(FTISUserHistorys);

                                //b_2(會外) 會議組別
                                //var FtisUHId = 0;
                                var FtisUHId = fTISUserHistory.GetAll().Where(a => a.PId == rPId
                                                            && a.OutYear == SetOutYear && a.ActivityCategoryType == 2)
                                                            .AsEnumerable()
                                                            .Where(a => ftisOutActivityCategoryId.Any(b => b.Id == a.ActivityCategoryId)).FirstOrDefault().Id;

                                //不存在新增(單筆)
                                Int64 UHSetId = 0;

                                var v = userHistorySet.GetAll().Where(a => a.FtisUHId == FtisUHId
                                                                        && a.Year == SetOutYear && a.Name == setName).FirstOrDefault();

                                if (v == null)
                                {
                                    UserHistorySet model = new UserHistorySet();
                                    model.FtisUHId = FtisUHId;
                                    model.Year = SetOutYear;
                                    model.Name = setName;

                                    userHistorySet.Add(model);
                                    UHSetId = model.Id;
                                }
                                else
                                {
                                    UHSetId = v.Id;
                                }

                                //b_3(會外) 會議組別標案
                                var d = userHistorySetBid.GetAll().Where(a => a.UHSetId == UHSetId
                                                                        && a.Name == setBidName).FirstOrDefault();

                                if (d == null)
                                {
                                    UserHistorySetBid model = new UserHistorySetBid();
                                    model.UHSetId = UHSetId;
                                    model.Name = setBidName;

                                    userHistorySetBid.Add(model);
                                }
                                else
                                {
                                    string str = string.Format("評選委員({0}),委辦單位({1}),處室組別({2}),會外年度({3}),標案名稱({4})", name, ftisOutActivityCategoryId.FirstOrDefault().Name, setName, SetOutYear, setBidName);
                                    logger.Info("組別標案已存在：" + str);
                                }
                            }
                        }

                        #endregion

                        #region  更新專長(不存在新增)

                        if (subjectDetailId.Count > 0)
                        {
                            var has = expertise.GetAll().Where(a => a.PId == rPId);
                            var v = subjectDetailId.Where(a => !has.Any(b => b.SubjectDetailId == a.Id));

                            //新增
                            var Expertises = v.Select(a => new Expertise
                            {
                                PId = rPId,
                                SubjectId = a.SubjectId,
                                SubjectDetailId = a.Id,
                            });
                            expertise.Add(Expertises);
                        }

                        #endregion
                    }

                }

                //清除 catch
                BasicUserNameSelectItems.Reset();
                Expertise.ResetGetAllDatas();
                FTISUserHistory.ResetGetAllDatas();
                UserHistorySet.ResetGetAllDatas();
                UserHistorySetBid.ResetGetAllDatas();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, errorMessage = ex.Message });
            }

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        private bool ToValidate(BasicUser f, string type)
        {
            bool result = false;

            var v = GetModelEntity().GetAll().Where(a => a.PId == f.PId);
            if (type == "Add")
            {
                if (v.Count() > 0)
                {
                    string errorMessage = string.Format("身分代碼已存在:" + f.PId);
                    throw new Exception(errorMessage);
                }
            }

            result = true;

            return result;
        }

        protected override Dou.Models.DB.IModelEntity<BasicUser> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<BasicUser>(new EsdmsModelContextExt());
        }
    }

    public class vwe_ChkExport
    {
        //姓名
        public bool ChkName { get; set; }

        //性別
        public bool ChkSex { get; set; }

        //在職狀況
        public bool ChkOnJob { get; set; }

        //手機號碼
        public bool ChkPrivatePhone { get; set; }

        //辦公室電話
        public bool ChkOfficePhone { get; set; }

        //辦公室電話2
        public bool ChkOfficePhone2 { get; set; }

        //傳真
        public bool ChkFax { get; set; }               

        //辦公_Email
        public bool ChkOfficeEmail { get; set; }

        //私人_Email
        public bool ChkPrivateEmail { get; set; }

        //辦公_縣市
        public bool ChkCityCode { get; set; }

        //辦公_鄉鎮市區
        public bool ChkZIP { get; set; }

        //辦公_地址
        public bool ChkOfficeAddress { get; set; }

        //住家_縣市
        public bool ChkPCityCode { get; set; }

        //住家_鄉鎮市區
        public bool ChkPZIP { get; set; }

        //住家_地址
        public bool ChkPAddress { get; set; }

        //備註
        public bool ChkNote { get; set; }

        //人員類別
        public bool ChkCategoryId { get; set; }

        //單位系所
        public bool ChkUnitName { get; set; }

        //職稱
        public bool ChkPosition { get; set; }

        //專長
        public bool ChkstrExpertises { get; set; }

        //會外評選
        public bool ChkvmOutCount { get; set; }

        //會內參與
        public bool ChkvmInCount { get; set; }
    }
}