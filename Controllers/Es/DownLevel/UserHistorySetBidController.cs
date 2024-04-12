using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.Es.DownLevel
{
    public class UserHistorySetBidController : Dou.Controllers.AGenericModelController<UserHistorySetBid>
    {
        // GET: UserHistorySetBid
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<UserHistorySetBid> dbEntity, IEnumerable<UserHistorySetBid> objs)
        {
            base.AddDBObject(dbEntity, objs);
            UserHistorySetBid.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<UserHistorySetBid> dbEntity, IEnumerable<UserHistorySetBid> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            UserHistorySetBid.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<UserHistorySetBid> dbEntity, IEnumerable<UserHistorySetBid> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            UserHistorySetBid.ResetGetAllDatas();
        }

        protected override Dou.Models.DB.IModelEntity<UserHistorySetBid> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<UserHistorySetBid>(new EsdmsModelContextExt());
        }
    }
}