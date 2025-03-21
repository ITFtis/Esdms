﻿using Dou.Misc.Attr;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esdms
{
    public class Code
    {
        public enum TempUploadFile
        {
            none = 0,
            匯入專家資料 = 1,  //瀏覽檔案送出(前)
            匯出專家清單 = 2,
            匯出專家請款單 = 3,
        }

        public enum UploadFile
        {
            none = 0,

        }

        /// <summary>
        /// 管理者腳色
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAdminRoles() {

            List<string> specRoles = new List<string>() { "admin", "ftisadmin" };
            return specRoles;

        }

        /// <summary>
        /// 新增來源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetAddFrom()
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            result = result.Append(new KeyValuePair<string, object>("1", "專家資料庫"));
            result = result.Append(new KeyValuePair<string, object>("2", "線上系統"));

            return result;
        }

        /// <summary>
        /// 部門
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetDepartment()
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            result = result.Append(new KeyValuePair<string, object>("02", "講習訓練組"));
            result = result.Append(new KeyValuePair<string, object>("58", "永續創新研發中心"));
            result = result.Append(new KeyValuePair<string, object>("52", "綠色技術發展中心"));
            result = result.Append(new KeyValuePair<string, object>("31", "環境與資源服務中心"));
            result = result.Append(new KeyValuePair<string, object>("19", "低碳策略與技術服務組"));
            result = result.Append(new KeyValuePair<string, object>("16", "資訊室"));
            result = result.Append(new KeyValuePair<string, object>("22", "法務室"));
            result = result.Append(new KeyValuePair<string, object>("17", "人力資源室"));


            return result;
        }

        /// <summary>
        /// 取得年度
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetYaer()
        {
            int endYear = DateTime.Now.Year - 1911;

            //客製化(近年底114/12/31)，方便管理師登錄資料
            if (endYear < 114)
                endYear = 114;

            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();
            for (int i = endYear; i >= 110; i--)
            {
                result = result.Append(new KeyValuePair<string, object>(i.ToString(), i.ToString() + "年度"));
            }

            return result;
        }

        /// <summary>
        /// 會議類別
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetActivityCategoryType()
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            result = result.Append(new KeyValuePair<string, object>("1", "會內"));
            result = result.Append(new KeyValuePair<string, object>("2", "會外"));

            return result;
        }

        /// <summary>
        /// 性別
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetSex()
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            result = result.Append(new KeyValuePair<string, object>("1", "男"));
            result = result.Append(new KeyValuePair<string, object>("2", "女"));

            return result;
        }

        /// <summary>
        /// 在職
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetOnJob()
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            result = result.Append(new KeyValuePair<string, object>("OJ1", "在職"));
            result = result.Append(new KeyValuePair<string, object>("OJ2", "退休"));

            return result;
        }

        /// <summary>
        /// 是否(Y/N)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetYN()
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            result = result.Append(new KeyValuePair<string, object>("Y", "是"));
            result = result.Append(new KeyValuePair<string, object>("N", "否"));

            return result;
        }

        /// <summary>
        /// 有無(Y/N)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetHas()
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            result = result.Append(new KeyValuePair<string, object>("Y", "有"));
            result = result.Append(new KeyValuePair<string, object>("N", "無"));

            return result;
        }
    }

    #region  下拉

    /// <summary>
    /// 性別
    /// </summary>
    public class GetSexSelectItems : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.GetSexSelectItems, Esdms";

        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return Code.GetSex().Select(a => new KeyValuePair<string, object>(a.Key, a.Value));
        }
    }

    /// <summary>
    /// 在職
    /// </summary>
    public class GetOnJobSelectItems : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.GetOnJobSelectItems, Esdms";

        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return Code.GetOnJob().Select(a => new KeyValuePair<string, object>(a.Key, a.Value));
        }
    }

    /// <summary>
    /// 是否(Y/N)
    /// </summary>
    public class GetYNSelectItems : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.GetYNSelectItems, Esdms";

        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return Code.GetYN().Select(a => new KeyValuePair<string, object>(a.Key, a.Value));
        }
    }

    /// <summary>
    /// 有無(Y/N)
    /// </summary>
    public class GetHasSelectItems : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.GetHasSelectItems, Esdms";

        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return Code.GetHas().Select(a => new KeyValuePair<string, object>(a.Key, a.Value));
        }
    }

    #endregion
}