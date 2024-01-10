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

            //13碼(S20240110001) S20240110 + 001
            var zz = u.Where(a => a.PId.Length == 12).ToList();
            var v = u.Where(a => a.PId.Length == 12).
                Where(a => a.PId.Substring(0, 9) == title);

            int max = 1;
            if (v.Count() > 0)
            {
                max = v.Select(a => int.Parse(a.PId.Substring(9, 3))).Max() + 1;
            }
            
            FId = title + max.ToString().PadLeft(3, '0');

            return FId;
        }
    }    
}