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
    public class FTISUserHistoryController : Dou.Controllers.AGenericModelController<FTISUserHistory>
    {
        // GET: FTISUserHistory
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<FTISUserHistory> dbEntity, IEnumerable<FTISUserHistory> objs)
        {
            base.AddDBObject(dbEntity, objs);
            FTISUserHistory.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<FTISUserHistory> dbEntity, IEnumerable<FTISUserHistory> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            FTISUserHistory.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<FTISUserHistory> dbEntity, IEnumerable<FTISUserHistory> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            FTISUserHistory.ResetGetAllDatas();
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
            opts.GetFiled("DCode").colsize = 6;
            opts.GetFiled("Year").colsize = 6;
            opts.GetFiled("ActivityCategoryType").colsize = 6;
            opts.GetFiled("ActivityCategoryJoinNum").colsize = 6;
            opts.GetFiled("OutYear").colsize = 6;
            opts.GetFiled("ActivityCategoryId").colsize = 12;
            opts.GetFiled("ProjectId").colsize = 12;

            opts.GetFiled("ActivityCategoryType").align = "left";
            opts.GetFiled("ActivityCategoryId").align = "left";
            opts.GetFiled("Year").align = "left";
            opts.GetFiled("ProjectId").align = "left";

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<FTISUserHistory> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<FTISUserHistory>(new EsdmsModelContextExt());
        }
    }
}