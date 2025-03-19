using Dou.Misc.Attr;
using Dou.Models;
using DouHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Esdms.Models
{
    [Table("User")]
    public partial class User : Dou.Models.UserBase
    {
        [Display(Name = "部門")]
        [ColumnDef(
            VisibleEdit = false,
            EditType = EditType.Select,
            SelectItemsClassNamespace = Esdms.Models.UserDCodeSelectItems.AssemblyQualifiedName,            
            ColSize = 3)]
        public string DCode { get; set; }

        //取得User所有角色
        public List<RoleUser> GetUserRoles()
        {
            var dbContext = new EsdmsModelContextExt();
            Dou.Models.DB.IModelEntity<RoleUser> roleUser = new Dou.Models.DB.ModelEntity<RoleUser>(dbContext);
            var roles = roleUser.GetAll().Where(a => a.UserId == Dou.Context.CurrentUserBase.Id).ToList();

            return roles;
        }

        /// <summary>
        /// 特定權限(做帳管理師)
        /// </summary>
        /// <returns></returns>
        public bool IsFinances()
        {
            bool result = false;
            
            List<string> Finances = new List<string>() { "DataFinance" };
            bool isFinances = this.RoleUsers.Any(a => Finances.Contains(a.RoleId));

            result = isFinances;

            return result;
        }

        static object lockGetAllDatas = new object();
        public static IEnumerable<User> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "Esdms.Models.User";
            var allData = DouHelper.Misc.GetCache<IEnumerable<User>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<User> modle = new Dou.Models.DB.ModelEntity<User>(new EsdmsModelContextExt());
                    allData = modle.GetAll().ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "Esdms.Models.User";
            Misc.ClearCache(key);
        }
    }

    /// <summary>
    /// 員工下拉：部門
    /// </summary>
    public class UserDCodeSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string AssemblyQualifiedName = "Esdms.Models.UserDCodeSelectItems, Esdms";

        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            //有時部門出現連線錯誤問題
            //return FtisHelperV2.DB.Helpe.Department.GetAllDepartment().Select(s => new KeyValuePair<string, object>(s.DCode, s.DName));

            IEnumerable<KeyValuePair<string, object>> deps = new List<KeyValuePair<string, object>>();
            try
            {
                deps = FtisHelperV2.DB.Helper.GetAllDepartment().Select(s => new KeyValuePair<string, object>(s.DCode, s.DName));
                var aaa = deps.ToList();
            }
            catch (Exception ex)
            {
                logger.Error("執行錯誤 - Esdms.Models.UserDCodeSelectItems, Esdms");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return deps;
        }
    }

}