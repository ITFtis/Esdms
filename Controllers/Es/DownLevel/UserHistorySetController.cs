using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using FtisHelperV2.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.Es.DownLevel
{
    public class UserHistorySetController : Dou.Controllers.AGenericModelController<UserHistorySet>
    {
        // GET: UserHistorySet
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<UserHistorySet> dbEntity, IEnumerable<UserHistorySet> objs)
        {
            base.AddDBObject(dbEntity, objs);
            UserHistorySet.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<UserHistorySet> dbEntity, IEnumerable<UserHistorySet> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            UserHistorySet.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<UserHistorySet> dbEntity, IEnumerable<UserHistorySet> objs)
        {
            var obj = objs.FirstOrDefault();

            //DB沒關聯
            var dbContext = new EsdmsModelContextExt();

            if (obj.UserHistorySetBids != null)
            {
                Dou.Models.DB.IModelEntity<UserHistorySetBid> userHistorySetBid = new Dou.Models.DB.ModelEntity<UserHistorySetBid>(dbContext);
                userHistorySetBid.Delete(obj.UserHistorySetBids);
            }

            base.DeleteDBObject(dbEntity, objs);
            UserHistorySet.ResetGetAllDatas();            
        }

        public virtual ActionResult getDataDetail(int FtisUHId)
        {
            var datas = GetModelEntity().GetAll().Where(a => a.FtisUHId == FtisUHId);

            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            opts.datas = datas;            

            var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

        protected override Dou.Models.DB.IModelEntity<UserHistorySet> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<UserHistorySet>(new EsdmsModelContextExt());
        }
    }
}