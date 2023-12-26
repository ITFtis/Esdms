using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.SysCode
{
    [Dou.Misc.Attr.MenuDef(Id = "Town", Name = "鄉鎮代碼", MenuPath = "代碼維護", Action = "Index", Index = 3, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class TownController : APaginationModelController<Town>
    {
        // GET: Town
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<Town> dbEntity, IEnumerable<Town> objs)
        {
            base.AddDBObject(dbEntity, objs);
            TownSelectItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<Town> dbEntity, IEnumerable<Town> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            TownSelectItems.Reset();
        }

        protected override void DeleteDBObject(IModelEntity<Town> dbEntity, IEnumerable<Town> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            TownSelectItems.Reset();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            return opts;
        }
        protected override Dou.Models.DB.IModelEntity<Town> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Town>(new EsdmsModelContextExt());
        }
    }
}