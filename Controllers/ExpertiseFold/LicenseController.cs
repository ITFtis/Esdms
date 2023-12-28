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
    [Dou.Misc.Attr.MenuDef(Id = "License", Name = "證照代碼", MenuPath = "專業項目", Action = "Index", Index = 3, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class LicenseController : APaginationModelController<License>
    {
        // GET: License
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<License> dbEntity, IEnumerable<License> objs)
        {
            base.AddDBObject(dbEntity, objs);
            LicenseSelectItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<License> dbEntity, IEnumerable<License> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            LicenseSelectItems.Reset();
        }

        protected override void DeleteDBObject(IModelEntity<License> dbEntity, IEnumerable<License> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            LicenseSelectItems.Reset();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<License> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<License>(new EsdmsModelContextExt());
        }
    }
}