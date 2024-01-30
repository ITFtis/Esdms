using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Esdms
{
    public class HtmlHelper
    {
        /// <summary>
        /// 移除字串Html Tag
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtmlTag(string str)
        {
            return Regex.Replace(str, @"<[^>]*>", String.Empty);
        }
    }
}