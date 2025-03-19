using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Controllers.Es;
using Esdms.Models;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
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

        protected override IQueryable<ProjectInvoice> BeforeIQueryToPagedList(IModelEntity<ProjectInvoice> dbEntity, IQueryable<ProjectInvoice> iquery, params KeyValueParams[] paras)
        {
            var Names = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "Names");

            if (!string.IsNullOrEmpty(Names))
            {
                List<string> strs = Names.Split(',').ToList();
                var MIds = ProjectInvoiceBasic.GetAllDatas()
                                        .Where(a => strs.Contains(a.BasicName))
                                        .Select(a => a.MId).ToList();
                iquery = iquery.Where(a => MIds.Any(b => b == a.Id));
            }

            return base.BeforeIQueryToPagedList(dbEntity, iquery, paras);
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
        /// 產製專家請款單
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ExportInvoice(int id)
        {
            var basicNames = ProjectInvoiceBasic.GetAllDatas().Where(a => a.MId == id).Select(a => a.BasicName).ToList();            
            var basics = BasicUserNameSelectItems.BasicUsers.Where(a => basicNames.Any(b => b == a.Name)).ToList();

            if (basics.Count == 0)
            {
                return Json(new { result = false, errorMessage = "尚未設定請款專家學者明細" }, JsonRequestBehavior.AllowGet);
            }

            List<Export> outputs = new List<Export>();

            //1.請款明細
            Export output1 = new Export();
            Rpt_ProjectInvoice rep = new Rpt_ProjectInvoice();
            string a_url = rep.Export(id);

            if (a_url == "")
            {
                output1.result = false;
                output1.errorMessage = "請款專家學者明細：" + rep.ErrorMessage;                
            }
            else
            {
                output1.result = true;
                output1.url = a_url;
                output1.fileName = "(1)" + Path.GetFileName(a_url);
            }
            outputs.Add(output1);

            //2.專家學者基本資料
            vwe_ChkExport chks = new vwe_ChkExport() {
                ChkCategoryId = true,
                ChkPosition = true,
                ChkUnitName = true,
            };

            int autoSizeColumn = 3;
            string waterColor = "";

            Export output2 = new Export();
            Rpt_BasicUserList rep2 = new Rpt_BasicUserList();
            string a2_url = rep2.Export(chks, basics, autoSizeColumn, waterColor);

            if (a2_url == "")
            {
                output2.result = false;
                output2.errorMessage = "專家學者基本資料：" + rep2.ErrorMessage;
            }
            else
            {
                output2.result = true;
                output2.url = a2_url;
                output2.fileName = "(2)" + Path.GetFileName(a2_url);
            }
            outputs.Add(output2);

            return Json(outputs, JsonRequestBehavior.AllowGet);
        }
    }

    public class Export
    {
        public bool result { get; set; }
        public string url { get; set; }
        public string fileName { get; set; }
        public string errorMessage { get; set; }        
    }
}