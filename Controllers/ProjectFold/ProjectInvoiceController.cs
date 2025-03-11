using Dou.Controllers;
using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.ProjectFold
{
    [Dou.Misc.Attr.MenuDef(Id = "ProjectInvoice", Name = "專家請款明細", MenuPath = "計畫請款(專家)", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class ProjectInvoiceController : APaginationModelController<ProjectInvoice>
    {
        // GET: ProjectInvoice
        public ActionResult Index()
        {
            return View();
        }

        protected override Dou.Models.DB.IModelEntity<ProjectInvoice> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<ProjectInvoice>(new EsdmsModelContextExt());
        }

        protected override void AddDBObject(IModelEntity<ProjectInvoice> dbEntity, IEnumerable<ProjectInvoice> objs)
        {
            var f = objs.First();
            ////ValidateSave("Add", f);

            f.BDate = DateTime.Now;
            f.BFno = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);
            ProjectInvoice.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<ProjectInvoice> dbEntity, IEnumerable<ProjectInvoice> objs)
        {
            var f = objs.First();
            ////ValidateSave("Update", f);

            f.UDate = DateTime.Now;
            f.UFno = Dou.Context.CurrentUserBase.Id;
            f.UName = Dou.Context.CurrentUserBase.Name;

            base.UpdateDBObject(dbEntity, objs);
            ProjectInvoice.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<ProjectInvoice> dbEntity, IEnumerable<ProjectInvoice> objs)
        {
            var obj = objs.FirstOrDefault();

            //DB沒關聯
            var dbContext = new EsdmsModelContextExt();

            if (obj.ProjectInvoiceBasics != null)
            {
                Dou.Models.DB.IModelEntity<ProjectInvoiceBasic> projectInvoiceBasic = new Dou.Models.DB.ModelEntity<ProjectInvoiceBasic>(dbContext);
                projectInvoiceBasic.Delete(obj.ProjectInvoiceBasics);
            }

            ProjectInvoiceBasic.ResetGetAllDatas();            

            base.DeleteDBObject(dbEntity, objs);
            ProjectInvoice.ResetGetAllDatas();            
        }
    }
}