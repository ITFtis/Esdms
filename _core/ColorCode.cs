using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esdms
{
    public class ColorCode
    {
        /// <summary>
        /// 取得浮水印色碼
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetWaterColor()
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            //System.Drawing.Color.Beige
            result = result.Append(new KeyValuePair<string, object>("Gainsboro", System.Drawing.Color.Gainsboro));
            result = result.Append(new KeyValuePair<string, object>("Beige", System.Drawing.Color.Beige));

            return result;
        }
    }
}