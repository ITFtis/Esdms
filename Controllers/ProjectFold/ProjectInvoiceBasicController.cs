using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.ProjectFold
{
    public class ProjectInvoiceBasicController : AGenericModelController<ProjectInvoiceBasic>
    {
        // GET: ProjectInvoiceBasic
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<ProjectInvoiceBasic> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<ProjectInvoiceBasic>(new EsdmsModelContextExt());
        }

        protected override void AddDBObject(IModelEntity<ProjectInvoiceBasic> dbEntity, IEnumerable<ProjectInvoiceBasic> objs)
        {
            var f = objs.First();
            ////ValidateSave("Add", f);

            f.BDate = DateTime.Now;
            f.BFno = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);
            ProjectInvoiceBasic.ResetGetAllDatas();            
        }

        protected override void UpdateDBObject(IModelEntity<ProjectInvoiceBasic> dbEntity, IEnumerable<ProjectInvoiceBasic> objs)
        {
            var f = objs.First();
            ////ValidateSave("Update", f);

            base.UpdateDBObject(dbEntity, objs);
            ProjectInvoiceBasic.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<ProjectInvoiceBasic> dbEntity, IEnumerable<ProjectInvoiceBasic> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            ProjectInvoiceBasic.ResetGetAllDatas();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            opts.editformWindowClasses = "modal-lg";

            return opts;
        }
    }
}