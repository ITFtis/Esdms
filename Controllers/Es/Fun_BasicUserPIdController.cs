using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using static Esdms.Controllers.Es.Fun_BasicUserPIdController;

namespace Esdms.Controllers.Es
{
    [Dou.Misc.Attr.MenuDef(Id = "Fun_BasicUserPId", Name = "客製化-專家身分證", MenuPath = "專家資料", Action = "Index", Index = 100, Func = Dou.Misc.Attr.FuncEnum.None, AllowAnonymous = false)]
    public class Fun_BasicUserPIdController : AGenericModelController<BasicUser>
    {
        // GET: Fun_BasicUserPId
        public ActionResult Index()
        {
            return View();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var _opts = base.GetDataManagerOptions();

            foreach (var field in _opts.fields)
            {
                field.visible = false;
                field.visibleView = false;
                field.visibleEdit = false;
                field.filter = false;
            }

            _opts.GetFiled("PId").visible = true;
            _opts.GetFiled("PId").visibleView = true;
            _opts.GetFiled("Name").visible = true;
            _opts.GetFiled("Name").visibleView = true;

            return _opts;
        }

        protected override Dou.Models.DB.IModelEntity<BasicUser> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<BasicUser>(new EsdmsModelContextExt());
        }
    }
}