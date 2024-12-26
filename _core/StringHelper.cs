using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Esdms
{
    public class StringHelper
    {
        /// <summary>
        /// 判断字符串中是否包含中文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasChinese(string input)
        {
            string pattern = "[\u4e00-\u9fbb]";
            return Regex.IsMatch(input, pattern);
        }
    }
}