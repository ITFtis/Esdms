using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.ProjectFold
{
    [Dou.Misc.Attr.MenuDef(Id = "ActivityCategory", Name = "會議", MenuPath = "專案資料", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class ActivityCategoryController : APaginationModelController<ActivityCategory>
    {        
        // GET: ActivityCategory
        public ActionResult Index()
        {
            return View();
        }

        protected override IQueryable<ActivityCategory> BeforeIQueryToPagedList(IModelEntity<ActivityCategory> dbEntity, IQueryable<ActivityCategory> iquery, params KeyValueParams[] paras)
        {
            KeyValueParams ksort = paras.FirstOrDefault((KeyValueParams s) => s.key == "sort");
            KeyValueParams korder = paras.FirstOrDefault((KeyValueParams s) => s.key == "order");
            //分頁排序
            if (ksort.value != null && korder.value != null)
            {
            }
            else
            {
                //預設排序                
                iquery = iquery.OrderBy(a => a.Type).ThenBy(a => a.Sort);
            }

            return base.BeforeIQueryToPagedList(dbEntity, iquery, paras);
        }

        protected override void AddDBObject(IModelEntity<ActivityCategory> dbEntity, IEnumerable<ActivityCategory> objs)
        {
            var f = objs.First();

            f.BDate = DateTime.Now;
            f.BFno = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);
            ActivityCategorySelectItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<ActivityCategory> dbEntity, IEnumerable<ActivityCategory> objs)
        {
            var f = objs.First();

            f.UDate = DateTime.Now;
            f.UFno = Dou.Context.CurrentUserBase.Id;
            f.UName = Dou.Context.CurrentUserBase.Name;

            base.UpdateDBObject(dbEntity, objs);
            ActivityCategorySelectItems.Reset();
        }

        protected override void DeleteDBObject(IModelEntity<ActivityCategory> dbEntity, IEnumerable<ActivityCategory> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            ActivityCategorySelectItems.Reset();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            opts.GetFiled("Type").align = "left";
            opts.GetFiled("Sort").visible = true;
            opts.GetFiled("Sort").align = "left";
            opts.GetFiled("IsCount").colsize = 6;

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<ActivityCategory> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<ActivityCategory>(new EsdmsModelContextExt());
        }
    }
}