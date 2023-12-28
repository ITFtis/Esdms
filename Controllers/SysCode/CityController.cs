using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.SysCode
{
    [Dou.Misc.Attr.MenuDef(Id = "City", Name = "縣市代碼", MenuPath = "代碼維護", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class CityController : APaginationModelController<City>
    {
        // GET: City
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<City> dbEntity, IEnumerable<City> objs)
        {
            base.AddDBObject(dbEntity, objs);
            CitySelectItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<City> dbEntity, IEnumerable<City> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            CitySelectItems.Reset();
        }

        protected override void DeleteDBObject(IModelEntity<City> dbEntity, IEnumerable<City> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            CitySelectItems.Reset();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<City> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<City>(new EsdmsModelContextExt());
        }        
    }
}