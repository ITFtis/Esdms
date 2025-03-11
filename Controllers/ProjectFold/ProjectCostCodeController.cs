using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.ProjectFold
{
    [Dou.Misc.Attr.MenuDef(Id = "ProjectCostCode", Name = "專案費用科目代碼", MenuPath = "專案資料", Action = "Index", Index = 3, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class ProjectCostCodeController : Dou.Controllers.AGenericModelController<ProjectCostCode>
    {
        // GET: ProjectCostCode
        public ActionResult Index()
        {
            return View();
        }
        protected override Dou.Models.DB.IModelEntity<ProjectCostCode> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<ProjectCostCode>(new EsdmsModelContextExt());
        }

        protected override void AddDBObject(IModelEntity<ProjectCostCode> dbEntity, IEnumerable<ProjectCostCode> objs)
        {
            var f = objs.First();

            f.BDate = DateTime.Now;
            f.BFno = Dou.Context.CurrentUserBase.Id;

            base.AddDBObject(dbEntity, objs);
            ProjectCostCode.ResetGetAllDatas();
        }

        protected override void UpdateDBObject(IModelEntity<ProjectCostCode> dbEntity, IEnumerable<ProjectCostCode> objs)
        {
            var f = objs.First();

            f.UDate = DateTime.Now;
            f.UFno = Dou.Context.CurrentUserBase.Id;            

            base.UpdateDBObject(dbEntity, objs);
            ProjectCostCode.ResetGetAllDatas();
        }

        protected override void DeleteDBObject(IModelEntity<ProjectCostCode> dbEntity, IEnumerable<ProjectCostCode> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            ProjectCostCode.ResetGetAllDatas();
        }
    }
}