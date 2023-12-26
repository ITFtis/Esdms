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
    [Dou.Misc.Attr.MenuDef(Id = "ProjectUnit", Name = "專案單位", MenuPath = "專案資料", Action = "Index", Index = 3, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class ProjectUnitController : APaginationModelController<ProjectUnit>
    {
        // GET: ProjectUnit
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<ProjectUnit> dbEntity, IEnumerable<ProjectUnit> objs)
        {
            base.AddDBObject(dbEntity, objs);
            ProjectUnitSelectItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<ProjectUnit> dbEntity, IEnumerable<ProjectUnit> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            ProjectUnitSelectItems.Reset();
        }

        protected override void DeleteDBObject(IModelEntity<ProjectUnit> dbEntity, IEnumerable<ProjectUnit> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            ProjectUnitSelectItems.Reset();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<ProjectUnit> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<ProjectUnit>(new EsdmsModelContextExt());
        }
    }
}