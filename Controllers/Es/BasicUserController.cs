using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using FtisHelperV2.DB.Helpe;
using FtisHelperV2.DB.Model;
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
            return base.BeforeIQueryToPagedList(iquery, paras);
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var options = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in options.fields)
                field.sortable = true;

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
            var f = objs.First();

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

            base.DeleteDBObject(dbEntity, objs);
            BasicUserNameSelectItems.Reset();
        }

        protected override Dou.Models.DB.IModelEntity<BasicUser> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<BasicUser>(new EsdmsModelContextExt());
        }
    }
}