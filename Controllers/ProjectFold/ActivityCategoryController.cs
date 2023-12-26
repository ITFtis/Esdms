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
    [Dou.Misc.Attr.MenuDef(Id = "ActivityCategory", Name = "專案類型", MenuPath = "專案資料", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class ActivityCategoryController : APaginationModelController<ActivityCategory>
    {        
        // GET: ActivityCategory
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<ActivityCategory> dbEntity, IEnumerable<ActivityCategory> objs)
        {
            base.AddDBObject(dbEntity, objs);
            ActivityCategorySelectItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<ActivityCategory> dbEntity, IEnumerable<ActivityCategory> objs)
        {
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

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<ActivityCategory> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<ActivityCategory>(new EsdmsModelContextExt());
        }
    }
}