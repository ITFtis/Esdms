using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;

namespace Esdms
{
    public class GenerateHelper
    {
        /// <summary>
        /// (BasicUser)產出身分代碼
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static string FId(List<BasicUser> u)
        {
            string FId = "";
            string title = "S" + DateFormat.ToDate8(DateTime.Now);

            //14碼(S202401100001) S20240110 + 0001
            var zz = u.Where(a => a.PId.Length == 13).ToList();
            var v = u.Where(a => a.PId.Length == 13).
                Where(a => a.PId.Substring(0, 9) == title);

            int max = 1;
            if (v.Count() > 0)
            {
                max = v.Select(a => int.Parse(a.PId.Substring(9, 4))).Max() + 1;
            }
            
            FId = title + max.ToString().PadLeft(4, '0');

            return FId;
        }
    }    
}