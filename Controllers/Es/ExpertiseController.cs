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

namespace Esdms.Controllers.Es
{    
    public class ExpertiseController : Dou.Controllers.AGenericModelController<Expertise>
    {
        // GET: Expertise
        public ActionResult Index()
        {
            return View();
        }

        protected override IEnumerable<Expertise> GetDataDBObject(IModelEntity<Expertise> dbEntity, params KeyValueParams[] paras)
        {
            return base.GetDataDBObject(dbEntity, paras);
        }

        protected override void AddDBObject(IModelEntity<Expertise> dbEntity, IEnumerable<Expertise> objs)
        {
            if (!ValidateSave(objs.First(), "Add"))
                return;

            base.AddDBObject(dbEntity, objs);
            Expertise.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<Expertise> dbEntity, IEnumerable<Expertise> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            Expertise.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<Expertise> dbEntity, IEnumerable<Expertise> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            Expertise.ResetGetAllDatas();
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
            opts.GetFiled("SubjectId").colsize = 6;
            opts.GetFiled("TypeName").colsize = 6;
            opts.GetFiled("SName").colsize = 6;

            opts.GetFiled("Note").colsize = 12;
            opts.GetFiled("Note").datatype = "textarea";
            opts.GetFiled("Note").textareaheight = 9;

            return opts;
        }

        private bool ValidateSave(Expertise f, string type)
        {
            bool result = false;

            var v = GetModelEntity().GetAll().Where(a => a.PId == f.PId && a.SubjectId == f.SubjectId);
            if (type == "Add")
            {                
                if (v.Count() > 0)
                {
                    string errorMessage = string.Format("身分證 + 科目類別，不可重複");
                    throw new Exception(errorMessage);
                }
            }

            result = true;

            return result;
        }

        protected override Dou.Models.DB.IModelEntity<Expertise> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Expertise>(new EsdmsModelContextExt());
        }
    }
}