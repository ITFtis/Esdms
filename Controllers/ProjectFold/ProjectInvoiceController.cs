using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Controllers.Es;
using Esdms.Models;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static NPOI.HSSF.Util.HSSFColor;

namespace Esdms.Controllers.ProjectFold
{
    [Dou.Misc.Attr.MenuDef(Id = "ProjectInvoice", Name = "專家請款單", MenuPath = "請款", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
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
            ValidateSave(f, "Add");

            f.BDate = DateTime.Now;
            f.BFno = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);
            ProjectInvoice.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<ProjectInvoice> dbEntity, IEnumerable<ProjectInvoice> objs)
        {
            var f = objs.First();
            ValidateSave(f, "Update");

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

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
            {
                field.sortable = true;
            }

            ////opts.addable = false;

            opts.ctrlFieldAlign = "left";
            opts.editformWindowStyle = "modal";
            opts.editformWindowClasses = "modal-lg";
            opts.editformSize.height = "fixed";
            opts.editformSize.width = "auto";

            //共用頁面
            opts.editformWindowStyle = "showEditformOnly";

            return opts;
        }

        //取得專案(like 專案編號,名稱,財編)
        public ActionResult GetAutocompleteProject(string searchKeyword)
        {
            return Content(WebFunction.GetAutocompleteProjectF1(searchKeyword), "application/json");
        }

        //取得專案
        public ActionResult GetProject(string prjId)
        {
            return Content(WebFunction.GetProject(prjId), "application/json");
        }

        private bool ValidateSave(ProjectInvoice f, string type)
        {
            bool result = false;
            List<string> errors = new List<string>();

            var fs = GetModelEntity().GetAll();

            if (type == "Update")
            {
                fs = fs.Where(a => a.Id != f.Id);
            }

            //key驗證
            if (fs.Any(a => a.PrjId == f.PrjId
                                && a.WorkItem == f.WorkItem && a.CostCode == f.CostCode))
            {
                var vCost = ProjectCostCode.GetAllDatas().Where(a => a.Code == f.CostCode).FirstOrDefault();
                throw new Exception(string.Format("該請款單已存在，不可重複：(專案編號{0}),工項({1}),科目({2})",
                                    f.PrjId,
                                    f.WorkItem,
                                    vCost != null ? vCost.Name : ""));
            }

            //驗證專案編號
            if (!ProjectSelectItems.Projects.Any(a => a.PrjId == f.PrjId))
            {
                errors.Add("此專案編號不存在：" + f.PrjId);
            }

            if (errors.Count > 0)
            {
                string str = string.Join("\n", errors);
                throw new Exception(str);
            }

            result = true;

            return result;
        }

        /// <summary>
        /// 產製請款單
        /// </summary>
        /// <param name="prjId"></param>
        /// <returns></returns>
        public ActionResult ExportInvoice(string prjId)
        {
            Rpt_ProjectInvoice rep = new Rpt_ProjectInvoice();
            string url = rep.Export(prjId);

            if (url == "")
            {
                return Json(new { result = false, errorMessage = rep.ErrorMessage }, JsonRequestBehavior.AllowGet);                
            }
            else
            {
                return Json(new { result = true, url = url }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}