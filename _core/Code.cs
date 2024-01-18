using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esdms
{
    public class Code
    {
        /// <summary>
        /// 部門
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetDepartment()
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            result = result.Append(new KeyValuePair<string, object>("1", "講習訓練組"));
            result = result.Append(new KeyValuePair<string, object>("2", "永續創新研發中心"));
            result = result.Append(new KeyValuePair<string, object>("3", "綠色技術發展中心"));
            result = result.Append(new KeyValuePair<string, object>("4", "環境與資源服務中心"));
            result = result.Append(new KeyValuePair<string, object>("5", "低碳策略與技術服務組"));
            result = result.Append(new KeyValuePair<string, object>("6", "資訊室"));
            result = result.Append(new KeyValuePair<string, object>("7", "法務室"));
            result = result.Append(new KeyValuePair<string, object>("8", "人資室"));


            return result;
        }

        /// <summary>
        /// 取得年度
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetYaer()
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();
            for (int i = DateTime.Now.Year - 1911; i >= 110; i--)
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
    }
}