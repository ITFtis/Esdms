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
    public class BasicUser_LicenseController : Dou.Controllers.AGenericModelController<BasicUser_License>
    {
        // GET: BasicUser_License
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<BasicUser_License> dbEntity, IEnumerable<BasicUser_License> objs)
        {
            var f = objs.First();

            f.BDate = DateTime.Now;
            f.BFno = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);
            BasicUser_License.ResetGetAllDatas();
        }        

        protected override void UpdateDBObject(IModelEntity<BasicUser_License> dbEntity, IEnumerable<BasicUser_License> objs)
        {
            var f = objs.First();

            f.UDate = DateTime.Now;
            f.UFno = Dou.Context.CurrentUserBase.Id;
            f.UName = Dou.Context.CurrentUserBase.Name;

            base.UpdateDBObject(dbEntity, objs);
            BasicUser_License.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<BasicUser_License> dbEntity, IEnumerable<BasicUser_License> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            BasicUser_License.ResetGetAllDatas();
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
            opts.GetFiled("LicenseId").colsize = 6;

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<BasicUser_License> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<BasicUser_License>(new EsdmsModelContextExt());
        }
    }
}