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
    [Dou.Misc.Attr.MenuDef(Id = "Subject", Name = "專長類別", MenuPath = "專長項目", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class SubjectController : APaginationModelController<Subject>
    {        
        // GET: Subject
        public ActionResult Index()
        {
            return View();
        }

        protected override IQueryable<Subject> BeforeIQueryToPagedList(IQueryable<Subject> iquery, params KeyValueParams[] paras)
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
                iquery = iquery.OrderBy(a => a.Sort);
            }

            return base.BeforeIQueryToPagedList(iquery, paras);
        }

        protected override void AddDBObject(IModelEntity<Subject> dbEntity, IEnumerable<Subject> objs)
        {
            base.AddDBObject(dbEntity, objs);
            SubjectSelectItems.Reset();
            Subject.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<Subject> dbEntity, IEnumerable<Subject> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            SubjectSelectItems.Reset();
            Subject.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<Subject> dbEntity, IEnumerable<Subject> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            SubjectSelectItems.Reset();
            Subject.ResetGetAllDatas();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            opts.GetFiled("Sort").visible = true;
            opts.GetFiled("Sort").align = "left";

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<Subject> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Subject>(new EsdmsModelContextExt());
        }
    }
}