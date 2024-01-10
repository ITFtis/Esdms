using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using FtisHelperV2.DB.Helpe;
using FtisHelperV2.DB.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.Es
{
    [Dou.Misc.Attr.MenuDef(Id = "BasicUser", Name = "專家基本資料", MenuPath = "專家資料", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class BasicUserController : APaginationModelController<BasicUser>
    {
        // GET: BasicUser
        public ActionResult Index()
        {
            return View();
        }

        protected override IQueryable<BasicUser> BeforeIQueryToPagedList(IQueryable<BasicUser> iquery, params KeyValueParams[] paras)
        {
            var DuplicateName = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "DuplicateName");
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

            options.GetFiled("PId").visible = true;
            options.GetFiled("Name").visible = true;
            options.GetFiled("Position").visible = true;
            //options.GetFiled("BDate").visible = true;
            options.GetFiled("BName").visible = true;
            options.GetFiled("strExpertises").visible = true;

            options.ctrlFieldAlign = "left";
            options.editformWindowStyle = "modal";
            options.editformWindowClasses = "modal-xl";
            options.editformSize.height = "fixed";
            options.editformSize.width = "auto";
            
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

            if (obj.BasicUser_Private != null)
            {
                Dou.Models.DB.IModelEntity<BasicUser_Private> basicUser_Private = new Dou.Models.DB.ModelEntity<BasicUser_Private>(dbContext);
                basicUser_Private.Delete(obj.BasicUser_Private);                
            }
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