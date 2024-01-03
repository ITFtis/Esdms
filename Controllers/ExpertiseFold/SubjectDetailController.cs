using Dou.Controllers;
using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.ExpertiseFold
{
    [Dou.Misc.Attr.MenuDef(Id = "SubjectDetail", Name = "專長領域代碼", MenuPath = "專業項目", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class SubjectDetailController : APaginationModelController<SubjectDetail>
    {
        // GET: SubjectDetail
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<SubjectDetail> dbEntity, IEnumerable<SubjectDetail> objs)
        {
            base.AddDBObject(dbEntity, objs);
            SubjectDetailSelectItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<SubjectDetail> dbEntity, IEnumerable<SubjectDetail> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            SubjectDetailSelectItems.Reset();
        }

        protected override void DeleteDBObject(IModelEntity<SubjectDetail> dbEntity, IEnumerable<SubjectDetail> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            SubjectDetailSelectItems.Reset();
        }

        protected override Dou.Models.DB.IModelEntity<SubjectDetail> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<SubjectDetail>(new EsdmsModelContextExt());
        }
    }
}