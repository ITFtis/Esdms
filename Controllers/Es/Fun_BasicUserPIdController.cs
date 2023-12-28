using Dou.Controllers;
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
    public class Fun_BasicUserPIdController : AGenericModelController<vwe_Fun_BasicUserPId>
    {
        // GET: Fun_BasicUserPId
        public ActionResult Index()
        {
            return View();
        }

        protected override IEnumerable<vwe_Fun_BasicUserPId> GetDataDBObject(IModelEntity<vwe_Fun_BasicUserPId> dbEntity, params KeyValueParams[] paras)
        {
            var dbContext = new EsdmsModelContextExt();
            Dou.Models.DB.IModelEntity<BasicUser> basicUser = new Dou.Models.DB.ModelEntity<BasicUser>(dbContext);

            var result = basicUser.GetAll().Select(a => new vwe_Fun_BasicUserPId
            {
                PId = a.PId,
                Name = a.Name
            });

            return result;
            //return base.GetDataDBObject(dbEntity, paras);
        }

        protected override Dou.Models.DB.IModelEntity<vwe_Fun_BasicUserPId> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<vwe_Fun_BasicUserPId>(new EsdmsModelContextExt());
        }

        public class vwe_Fun_BasicUserPId
        {
            [Key]
            [Display(Name = "身分證字號")]
            public string PId { get; set; }

            [Display(Name = "姓名")]
            public string Name { get; set; }
        }
    }
}