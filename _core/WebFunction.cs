using Dou.Misc;
using Esdms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms
{
    public class WebFunction
    {
        /// <summary>
        /// 取得專案(like 專案編號,名稱,財編)
        /// </summary>
        /// <param name="searchKeyword"></param>
        /// <returns>json字串</returns>
        public static string GetAutocompleteProjectF1(string searchKeyword)
        {
            var projects = ProjectSelectItems.Projects.Where(a => !string.IsNullOrEmpty(a.PrjId));

            var result = projects.Where(a => a.PrjId.Contains(searchKeyword)
                                        || (!string.IsNullOrEmpty(a.PjNoM) && a.PjNoM.Contains(searchKeyword))
                                        || (!string.IsNullOrEmpty(a.Name) && a.Name.Contains(searchKeyword))
                                        );

            var jstr = JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return jstr;
        }

        /// <summary>
        /// 取得專案
        /// </summary>
        /// <param name="prjId"></param>
        /// <returns></returns>
        public static string GetProject(string prjId)
        {
            var result = ProjectSelectItems.Projects.Where(a => a.PrjId == prjId).FirstOrDefault();

            var jstr = JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");

            return jstr;
        }
    }
}