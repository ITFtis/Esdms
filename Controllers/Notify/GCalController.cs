using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Controllers.Notify
{
    [Dou.Misc.Attr.MenuDef(Id = "GCal", Name = "加入Google行事曆", Action = "Index", Index = -1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = true)]
    public class GCalController : Controller
    {
        // GET: GCal
        public ActionResult Index()
        {
            return View();
        }
    }
}