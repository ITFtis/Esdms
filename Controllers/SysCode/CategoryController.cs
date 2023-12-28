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
    [Dou.Misc.Attr.MenuDef(Id = "Category", Name = "職稱代碼", MenuPath = "代碼維護", Action = "Index", Index = 3, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class CategoryController : APaginationModelController<Category>
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<Category> dbEntity, IEnumerable<Category> objs)
        {
            base.AddDBObject(dbEntity, objs);
            CategorySelectItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<Category> dbEntity, IEnumerable<Category> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            CategorySelectItems.Reset();
        }

        protected override void DeleteDBObject(IModelEntity<Category> dbEntity, IEnumerable<Category> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            CategorySelectItems.Reset();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<Category> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Category>(new EsdmsModelContextExt());
        }
    }
}