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
            ValidateSave(f, "Add");

            f.BDate = DateTime.Now;
            f.BFno = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);
            ProjectInvoiceBasic.ResetGetAllDatas();            
        }

        protected override void UpdateDBObject(IModelEntity<ProjectInvoiceBasic> dbEntity, IEnumerable<ProjectInvoiceBasic> objs)
        {
            var f = objs.First();
            ValidateSave(f, "Update");

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

        private bool ValidateSave(ProjectInvoiceBasic f, string type)
        {
            bool result = false;
            List<string> errors = new List<string>();

            var fs = GetModelEntity().GetAll();

            if (type == "Update")
            {
                fs = fs.Where(a => a.Id != f.Id);
            }

            //key驗證
            if (fs.Any(a => a.MId == f.MId && a.BasicName == f.BasicName))
            {                
                throw new Exception("該請款單已存在此專家學者，不可重複：" + f.BasicName);
            }

            //驗證專家學者
            if (!BasicUserNameSelectItems.BasicUsers.Any(a => a.Name == f.BasicName))
            {
                errors.Add("此專家學者不存在：" + f.BasicName);
            }

            if (errors.Count > 0)
            {
                string str = string.Join("\n", errors);
                throw new Exception(str);
            }

            result = true;

            return result;
        }
    }
}