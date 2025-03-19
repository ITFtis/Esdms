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
        public static bool IsAdminRole() {

            var roles = Dou.Context.CurrentUser<User>().RoleUsers;
            var result = roles.Any(a => Code.GetAdminRoles().Any(b => b == a.RoleId));

            return result;
        }

        /// <summary>
        /// 取得專家學者(like 姓名)
        /// </summary>
        /// <param name="searchKeyword"></param>
        /// <returns>json字串</returns>
        public static string GetAutocompleteBasic(string searchKeyword)
        {
            var jquery = BasicUserNameSelectItems.BasicUsers;//.Where(a => a.PId == PId);

            jquery = jquery.Where(a => a.Name.Contains(searchKeyword));

            var result = jquery.Select(a => new {
                a.PId,
                a.Name,
            });

            var jstr = JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");

            return jstr;
        }

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