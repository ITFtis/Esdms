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
    [Dou.Misc.Attr.MenuDef(Id = "Project", Name = "專案", MenuPath = "專案資料", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class ProjectController : APaginationModelController<Project>
    {
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<Project> dbEntity, IEnumerable<Project> objs)
        {
            var f = objs.First();

            f.BDate = DateTime.Now;
            f.BFno = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);
            ProjectSelectItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<Project> dbEntity, IEnumerable<Project> objs)
        {
            var f = objs.First();

            f.UDate = DateTime.Now;
            f.UFno = Dou.Context.CurrentUserBase.Id;
            f.UName = Dou.Context.CurrentUserBase.Name;

            base.UpdateDBObject(dbEntity, objs);
            ProjectSelectItems.Reset();
        }

        protected override void DeleteDBObject(IModelEntity<Project> dbEntity, IEnumerable<Project> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            ProjectSelectItems.Reset();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            opts.GetFiled("Year").align = "left";
            opts.GetFiled("Name").visible = false;
            opts.GetFiled("strName").visible = true;

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<Project> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Project>(new EsdmsModelContextExt());
        }
    }
}