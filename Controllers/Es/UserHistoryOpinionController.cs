using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.Es
{    
    public class UserHistoryOpinionController : Dou.Controllers.AGenericModelController<UserHistoryOpinion>
    {
        // GET: UserHistoryOpinion
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<UserHistoryOpinion> dbEntity, IEnumerable<UserHistoryOpinion> objs)
        {
            var f = objs.First();

            f.BDate = DateTime.Now;
            f.BFno = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);
            UserHistoryOpinion.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<UserHistoryOpinion> dbEntity, IEnumerable<UserHistoryOpinion> objs)
        {
            var f = objs.First();

            f.UDate = DateTime.Now;
            f.UFno = Dou.Context.CurrentUserBase.Id;
            f.UName = Dou.Context.CurrentUserBase.Name;

            base.UpdateDBObject(dbEntity, objs);
            UserHistoryOpinion.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<UserHistoryOpinion> dbEntity, IEnumerable<UserHistoryOpinion> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            UserHistoryOpinion.ResetGetAllDatas();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
            {
                field.filter = false;
                field.sortable = true;
            }

            opts.GetFiled("PId").editable = false;

            opts.GetFiled("PId").colsize = 6;

            opts.GetFiled("Note").colsize = 12;
            opts.GetFiled("Note").datatype = "textarea";
            opts.GetFiled("Note").textareaheight = 9;

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<UserHistoryOpinion> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<UserHistoryOpinion>(new EsdmsModelContextExt());
        }
    }
}