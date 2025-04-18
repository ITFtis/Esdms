﻿using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Esdms.Controllers.ProjectFold
{
    [Dou.Misc.Attr.MenuDef(Id = "Project", Name = "專案", MenuPath = "專案資料", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.None, AllowAnonymous = false)]
    public class ProjectController : APaginationModelController<Project>
    {
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        protected override IQueryable<Project> BeforeIQueryToPagedList(IModelEntity<Project> dbEntity, IQueryable<Project> iquery, params KeyValueParams[] paras)
        {
            //預設排序
            iquery = iquery.OrderByDescending(a => a.Year).ThenByDescending(a => a.Id);

            return base.BeforeIQueryToPagedList(dbEntity, iquery, paras);
        }

        protected override void AddDBObject(IModelEntity<Project> dbEntity, IEnumerable<Project> objs)
        {
            var f = objs.First();

            f.BDate = DateTime.Now;
            f.BFno = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);
            ProjectSelectItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<Project> dbEntity, IEnumerable<Project> objs)
        {
            var f = objs.First();

            f.UDate = DateTime.Now;
            f.UFno = Dou.Context.CurrentUserBase.Id;
            f.UName = Dou.Context.CurrentUserBase.Name;

            base.UpdateDBObject(dbEntity, objs);
            ProjectSelectItems.Reset();
        }

        protected override void DeleteDBObject(IModelEntity<Project> dbEntity, IEnumerable<Project> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            ProjectSelectItems.Reset();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            opts.viewable = true;

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            opts.ctrlFieldAlign = "left";
            opts.GetFiled("PrjId").title = "專案編號<span style='color:blue'>(可匯入用)</span>";
            opts.GetFiled("Year").align = "left";
            opts.GetFiled("PrjId").editable = false;

            return opts;
        }

        protected override Dou.Models.DB.IModelEntity<Project> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Project>(new EsdmsModelContextExt());
        }
    }
}