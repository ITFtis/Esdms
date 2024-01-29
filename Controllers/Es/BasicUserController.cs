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

namespace Esdms.Controllers.Es
{
    [Dou.Misc.Attr.MenuDef(Id = "BasicUser", Name = "專家基本資料", MenuPath = "專家資料", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class BasicUserController : APaginationModelController<BasicUser>
    {
        // GET: BasicUser
        public ActionResult Index()
        {
            List<RoleUser> roles = Dou.Context.CurrentUser<User>().GetUserRoles();
            ViewBag.Roles = roles;

            return View();
        }

        protected override IQueryable<BasicUser> BeforeIQueryToPagedList(IQueryable<BasicUser> iquery, params KeyValueParams[] paras)
        {
            KeyValueParams ksort = paras.FirstOrDefault((KeyValueParams s) => s.key == "sort");
            var DuplicateName = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "DuplicateName");
            var SubjectId = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "SubjectId");
            var SubjectDetailId = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "SubjectDetailId");
            var strExpertises = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "strExpertises");

            //是否重複查詢
            if (DuplicateName != null)
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
            if (SubjectId != null)
            {
                var enumerable = iquery.AsEnumerable();                
                enumerable = enumerable.Where(a => a.Expertises.Any(b => b.SubjectId.ToString() == SubjectId));                
                iquery = enumerable.AsQueryable();                
            }
            //專長領域查詢(D 下拉) 虛擬欄位
            if (SubjectDetailId != null)
            {
                var enumerable = iquery.AsEnumerable();
                enumerable = enumerable.Where(a => a.Expertises.Any(b => b.SubjectDetailId.ToString() == SubjectDetailId));
                iquery = enumerable.AsQueryable();
            }

            //專長領域查詢(文字) 虛擬欄位
            if (strExpertises != null)
            {
                var enumerable = iquery.AsEnumerable();
                enumerable = enumerable.Where(a => a.strExpertises.Contains(strExpertises));
                iquery = enumerable.AsQueryable();
            }

            if (ksort.value == null)
            {
                //預設排序
                iquery = iquery.OrderByDescending(a => a.PId);
            }

            return base.BeforeIQueryToPagedList(iquery, paras);
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
            //options.GetFiled("BName").visible = true;            
            options.GetFiled("SubjectId").filter = true;
            options.GetFiled("SubjectDetailId").filter = true;
            options.GetFiled("strExpertises").visible = true;
            options.GetFiled("strExpertises").filter = true;

            options.ctrlFieldAlign = "left";
            options.editformWindowStyle = "modal";
            options.editformWindowClasses = "modal-xl";
            options.editformSize.height = "fixed";
            options.editformSize.width = "auto";

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
                }
                
                //int cc = 0;
                var dbContext = new EsdmsModelContextExt();
                Dou.Models.DB.IModelEntity<BasicUser> basicUser = new Dou.Models.DB.ModelEntity<BasicUser>(dbContext);
                Dou.Models.DB.IModelEntity<Expertise> expertise = new Dou.Models.DB.ModelEntity<Expertise>(dbContext);
                Dou.Models.DB.IModelEntity<FTISUserHistory> fTISUserHistory = new Dou.Models.DB.ModelEntity<FTISUserHistory>(dbContext);
                Dou.Models.DB.IModelEntity<SubjectDetail> subjectDetail = new Dou.Models.DB.ModelEntity<SubjectDetail>(dbContext);                
                Dou.Models.DB.IModelEntity<ActivityCategory> activityCategory = new Dou.Models.DB.ModelEntity<ActivityCategory>(dbContext);

                var m_subjectDetail = subjectDetail.GetAll().ToList();

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
                    string ___env = !dic.ContainsKey("_Env_") ? "" : row.ItemArray[dic["_Env_"]].ToString();
                    string ___eng = !dic.ContainsKey("_Eng_") ? "" : row.ItemArray[dic["_Eng_"]].ToString();
                    string ___ida = !dic.ContainsKey("_Ida_") ? "" : row.ItemArray[dic["_Ida_"]].ToString();
                    string ___subjectDetailId = !dic.ContainsKey("_SubjectDetailId") ? "" : row.ItemArray[dic["_SubjectDetailId"]].ToString();

                    if (name == "")
                        continue;

                    //(1)儲存basicUser
                    var v1 = Code.GetSex().Where(a => a.Value.ToString() == ___sex);
                    int? sex = v1.Count() == 0 ? (int?)null : int.Parse(v1.FirstOrDefault().Key);

                    var v2 = CategorySelectItems.Categorys.Where(a => a.Name == ___categoryId);
                    int? categoryId = v2.Count() == 0 ? (int?)null : v2.FirstOrDefault().Id;

                    var v3 = Code.GetOnJob().Where(a => a.Value.ToString() == ___onJob);
                    string onJob = v3.Count() == 0 ? null : v3.FirstOrDefault().Key;                    

                    //環境部
                    int env_num = 0;                    
                    int.TryParse(___env, out env_num);

                    //能源署
                    int eng_num = 0;
                    int.TryParse(___eng, out eng_num);

                    //產發署
                    int ida_num = 0;
                    int.TryParse(___ida, out ida_num);

                    var subjectDetailId = m_subjectDetail.Where(a => ___subjectDetailId.Split(',').Any(b => b == a.DCode)).ToList();

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
                        nuser.BDate = DateTime.Now;
                        nuser.BFno = Dou.Context.CurrentUserBase.Id;
                        nuser.BName = Dou.Context.CurrentUserBase.Name;                                                

                        basicUser.Add(nuser);
                        rPId = nuser.PId;

                        //「新增」時，會加入專長                        
                        if (subjectDetailId.Count > 0)
                        {
                            var Expertises = subjectDetailId.Select(a => new Expertise
                            {
                                PId = nuser.PId,
                                SubjectId = a.SubjectId,
                                SubjectDetailId = a.Id,
                            });
                            expertise.Add(Expertises);
                        }                        
                    }
                    else
                    {
                        //修改
                        ////data.Name = name;  (where name)，不需要修改
                        if (dic.ContainsKey("_Sex")) data.Sex = sex;
                        if (dic.ContainsKey("_CategoryId")) data.CategoryId = categoryId;
                        if (dic.ContainsKey("_OnJob")) data.OnJob = onJob;
                        if (dic.ContainsKey("_UnitName")) data.UnitName = unitName;
                        if (dic.ContainsKey("_Position")) data.Position = position;
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
                    }

                }

                //清除 catch
                BasicUserNameSelectItems.Reset();
                Expertise.ResetGetAllDatas();
                FTISUserHistory.ResetGetAllDatas();
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
}