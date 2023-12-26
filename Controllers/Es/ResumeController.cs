using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.Es
{    
    public class ResumeController : Dou.Controllers.AGenericModelController<Resume>
    {
        // GET: Resume
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<Resume> dbEntity, IEnumerable<Resume> objs)
        {
            base.AddDBObject(dbEntity, objs);
            Resume.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<Resume> dbEntity, IEnumerable<Resume> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            Resume.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<Resume> dbEntity, IEnumerable<Resume> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            Resume.ResetGetAllDatas();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
            {
                field.filter = false;
                field.sortable = true;
            }

            opts.GetFiled("PId").editable = false;

            opts.GetFiled("PId").colsize = 6;
            opts.GetFiled("UnitName").colsize = 6;
            opts.GetFiled("Position").colsize = 6;
            opts.GetFiled("Description").colsize = 6;
            opts.GetFiled("StartDate").colsize = 6;
            opts.GetFiled("EndDate").colsize = 6;

            opts.GetFiled("Note").colsize = 12;
            opts.GetFiled("Note").datatype = "textarea";
            opts.GetFiled("Note").textareaheight = 9;

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<Resume> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Resume>(new EsdmsModelContextExt());
        }
    }
}