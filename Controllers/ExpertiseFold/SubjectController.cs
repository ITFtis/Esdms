using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.ExpertiseFold
{
    [Dou.Misc.Attr.MenuDef(Id = "Subject", Name = "專業科目代碼", MenuPath = "專業項目", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class SubjectController : APaginationModelController<Subject>
    {        
        // GET: Subject
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<Subject> dbEntity, IEnumerable<Subject> objs)
        {
            base.AddDBObject(dbEntity, objs);
            SubjectSelectItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<Subject> dbEntity, IEnumerable<Subject> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            SubjectSelectItems.Reset();
        }

        protected override void DeleteDBObject(IModelEntity<Subject> dbEntity, IEnumerable<Subject> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            SubjectSelectItems.Reset();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<Subject> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Subject>(new EsdmsModelContextExt());
        }
    }
}