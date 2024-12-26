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
            result = result.Append(new KeyValuePair<string, object>("BurlyWood", System.Drawing.Color.BurlyWood));
            result = result.Append(new KeyValuePair<string, object>("DarkCyan", System.Drawing.Color.DarkCyan));
            result = result.Append(new KeyValuePair<string, object>("CornflowerBlue", System.Drawing.Color.CornflowerBlue));
            result = result.Append(new KeyValuePair<string, object>("Gray", System.Drawing.Color.Gray));
            result = result.Append(new KeyValuePair<string, object>("LightSlateGray", System.Drawing.Color.LightSlateGray));
            result = result.Append(new KeyValuePair<string, object>("Navy", System.Drawing.Color.Navy));
            result = result.Append(new KeyValuePair<string, object>("Maroon", System.Drawing.Color.Maroon));
            result = result.Append(new KeyValuePair<string, object>("LightSkyBlue", System.Drawing.Color.LightSkyBlue));
            result = result.Append(new KeyValuePair<string, object>("Black", System.Drawing.Color.Black));

            return result;
        }
    }
}