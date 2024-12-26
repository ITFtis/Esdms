using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Esdms
{
    public class ColorCode
    {
        /// <summary>
        /// 取得浮水印色碼
        /// </summary>
        /// <param name="alpha">透明度(100%=>255)</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetWaterColor(int alpha = 255)
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            //System.Drawing.Color.Beige            
            result = result.Append(new KeyValuePair<string, object>("Gainsboro", Color.FromArgb(alpha, 220, 220, 220)));           
            result = result.Append(new KeyValuePair<string, object>("BurlyWood", Color.FromArgb(alpha, 222, 184, 135)));            
            result = result.Append(new KeyValuePair<string, object>("DarkCyan", Color.FromArgb(alpha, 0, 139, 139)));            
            result = result.Append(new KeyValuePair<string, object>("CornflowerBlue", Color.FromArgb(alpha, 100, 149, 237)));            
            result = result.Append(new KeyValuePair<string, object>("Gray", Color.FromArgb(alpha, 128, 128, 128)));            
            result = result.Append(new KeyValuePair<string, object>("LightSlateGray", Color.FromArgb(alpha, 119, 136, 153)));            
            result = result.Append(new KeyValuePair<string, object>("Navy", Color.FromArgb(alpha, 0, 0, 128)));            
            result = result.Append(new KeyValuePair<string, object>("Maroon", Color.FromArgb(alpha, 128, 0, 0)));
            result = result.Append(new KeyValuePair<string, object>("LightSkyBlue", Color.FromArgb(alpha, 135, 206, 250)));            
            result = result.Append(new KeyValuePair<string, object>("Black", Color.FromArgb(alpha, 0, 0, 0)));

            return result;
        }
    }
}