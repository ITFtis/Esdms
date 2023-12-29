using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using Esdms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using static Esdms.Controllers.Es.Fun_BasicUserPIdController;

namespace Esdms.Controllers.Es
{
    [Dou.Misc.Attr.MenuDef(Id = "Fun_BasicUserPId", Name = "客製化-專家身分證", MenuPath = "專家資料", Action = "Index", Index = 100, Func = Dou.Misc.Attr.FuncEnum.None, AllowAnonymous = false)]
    public class Fun_BasicUserPIdController : AGenericModelController<BasicUser>
    {
        // GET: Fun_BasicUserPId
        public ActionResult Index()
        {
            return View();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var _opts = base.GetDataManagerOptions();

            foreach (var field in _opts.fields)
            {
                field.visible = false;
                field.visibleView = false;
                field.visibleEdit = false;
                field.filter = false;
            }

            _opts.GetFiled("PId").visible = true;
            _opts.GetFiled("PId").visibleView = true;
            _opts.GetFiled("Name").visible = true;
            _opts.GetFiled("Name").visibleView = true;
            _opts.GetFiled("BDate").visible = true;
            _opts.GetFiled("BDate").visibleView = true;
            _opts.GetFiled("BName").visible = true;
            _opts.GetFiled("BName").visibleView = true;

            return _opts;
        }

        //更新身分證
        public ActionResult UpdatePId(string PId, string newPId)
        {
            //驗證新身分證:空值
            if (string.IsNullOrEmpty(newPId))
            {
                return Json(new { result = false, errorMessage = string.Format("不可為Null或空值：newPId({0})", newPId) }, JsonRequestBehavior.AllowGet);
            }

            //驗證新身分證:已存在
            int n = GetModelEntity().GetAll().Where(a => a.PId == newPId).Count();
            if (n > 0)
            {
                return Json(new { result = false, errorMessage = string.Format("新身分證已存在，不可更新：newPId({0})", newPId) }, JsonRequestBehavior.AllowGet);
            }

            using (var db = new EsdmsModelContextExt())
            {
                string sql = @"
                                Update BasicUser Set PId = @newPId  Where PId = @PId
                                Update BasicUser_Private Set PId = @newPId  Where PId = @PId
                                Update Expertise Set PId = @newPId  Where PId = @PId
                                Update UserHistoryOpinion Set PId = @newPId  Where PId = @PId
                                Update Resume Set PId = @newPId  Where PId = @PId				
                                Update FTISUserHistory Set PId = @newPId  Where PId = @PId
                                Update BasicUser_License Set PId = @newPId  Where PId = @PId
                            ";

                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("PId", PId),
                    new SqlParameter("newPId", newPId),
                };

                int s = db.Database.ExecuteSqlCommand(sql, sqlParameters);
                if (s == 0)
                {
                    return Json(new { result = false, errorMessage = "更新身分證，執行錯誤" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //reset
                    //BasicUser 專家基本資料

                    //個人資料 BasicUser_Private
                    BasicUser_Private.ResetGetAllDatas();

                    //專長 Expertise
                    Expertise.ResetGetAllDatas();

                    //意見 UserHistoryOpinion
                    UserHistoryOpinion.ResetGetAllDatas();

                    //經歷 Resume
                    Resume.ResetGetAllDatas();

                    //Ftis活動計畫參與 FTISUserHistory
                    FTISUserHistory.ResetGetAllDatas();

                    //證照 BasicUser_License
                    BasicUser_License.ResetGetAllDatas();
                }
            }

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        protected override Dou.Models.DB.IModelEntity<BasicUser> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<BasicUser>(new EsdmsModelContextExt());
        }
    }
}