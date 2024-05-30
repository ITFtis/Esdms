using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Esdms
{
    public class AppConfig
    {
        #region 私有變數

        private static string _rootPath;
        private static string _systemWebSite;

        #endregion

        #region 建構子

        static AppConfig()
        {
            _rootPath = ConfigurationManager.AppSettings["RootPath"].ToString();

            //實體路徑(解決開發者專案於不同目錄)
            _rootPath = _rootPath.Replace("~\\", HttpContext.Current.Server.MapPath("~\\"));
            
            _systemWebSite = ConfigurationManager.AppSettings["SystemWebSite"].ToString();
        }

        #endregion

        #region 公用屬性      

        /// <summary>
        /// 檔案存放跟目錄
        /// </summary>
        public static string RootPath
        {
            get { return _rootPath; }
        }

        /// <summary>
        /// 網站網址
        /// </summary>
        public static string SystemWebSite
        {
            get { return _systemWebSite; }
        }

        #endregion
    }
}