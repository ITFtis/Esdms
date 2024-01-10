using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.ExpertiseFold
{
    [Dou.Misc.Attr.MenuDef(Id = "SubjectDetail", Name = "專長領域", MenuPath = "專長項目", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class SubjectDetailController : APaginationModelController<SubjectDetail>
    {
        // GET: SubjectDetail
        public ActionResult Index()
        {
            return View();
        }

        protected override IQueryable<SubjectDetail> BeforeIQueryToPagedList(IQueryable<SubjectDetail> iquery, params KeyValueParams[] paras)
        {
            KeyValueParams ksort = paras.FirstOrDefault((KeyValueParams s) => s.key == "sort");
            KeyValueParams korder = paras.FirstOrDefault((KeyValueParams s) => s.key == "order");
            //分頁排序
            if (ksort.value != null && korder.value != null)
            {
            }
            else
            {
                //預設排序                
                iquery = iquery.OrderBy(a => a.SubjectId).ThenBy(a => a.Sort);
            }

            return base.BeforeIQueryToPagedList(iquery, paras);
        }

        protected override void AddDBObject(IModelEntity<SubjectDetail> dbEntity, IEnumerable<SubjectDetail> objs)
        {
            base.AddDBObject(dbEntity, objs);
            SubjectDetailSelectItems.Reset();
            SubjectDetail.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<SubjectDetail> dbEntity, IEnumerable<SubjectDetail> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            SubjectDetailSelectItems.Reset();
            SubjectDetail.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<SubjectDetail> dbEntity, IEnumerable<SubjectDetail> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            SubjectDetailSelectItems.Reset();
            SubjectDetail.ResetGetAllDatas();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            opts.GetFiled("SubjectId").align = "left";
            opts.GetFiled("Sort").visible = true;
            opts.GetFiled("Sort").align = "left";

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<SubjectDetail> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<SubjectDetail>(new EsdmsModelContextExt());
        }
    }
}