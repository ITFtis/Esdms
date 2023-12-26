using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.Es
{    
    public class BasicUser_PrivateController : Dou.Controllers.AGenericModelController<BasicUser_Private>
    {
        // GET: BasicUser_Private
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<BasicUser_Private> dbEntity, IEnumerable<BasicUser_Private> objs)
        {
            var f = objs.First();

            f.BDate = DateTime.Now;
            f.BFno = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);
            BasicUser_Private.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<BasicUser_Private> dbEntity, IEnumerable<BasicUser_Private> objs)
        {
            var f = objs.First();

            if (dbEntity.GetAll().Where(a => a.PId == f.PId).Count() == 0)
            {
                AddDBObject(dbEntity, objs);                
            }
            else
            {
                f.UDate = DateTime.Now;
                f.UFno = Dou.Context.CurrentUserBase.Id;
                f.UName = Dou.Context.CurrentUserBase.Name;

                base.UpdateDBObject(dbEntity, objs);
                BasicUser_Private.ResetGetAllDatas();
            }
        }

        protected override void DeleteDBObject(IModelEntity<BasicUser_Private> dbEntity, IEnumerable<BasicUser_Private> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            BasicUser_Private.ResetGetAllDatas();
        }

        protected override Dou.Models.DB.IModelEntity<BasicUser_Private> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<BasicUser_Private>(new EsdmsModelContextExt());
        }
    }
}