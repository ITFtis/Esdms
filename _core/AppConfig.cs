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
        private static bool _isBkTask;
        private static string _googleApiClientId;
        private static string _googleApiAccountsUrl;

        #endregion

        #region 建構子

        static AppConfig()
        {
            _rootPath = ConfigurationManager.AppSettings["RootPath"].ToString();

            //實體路徑(解決開發者專案於不同目錄)
            _rootPath = _rootPath.Replace("~\\", HttpContext.Current.Server.MapPath("~\\"));
            
            _systemWebSite = ConfigurationManager.AppSettings["SystemWebSite"].ToString();
            _googleApiClientId = ConfigurationManager.AppSettings["GoogleApiClientId"].ToString();
            _googleApiAccountsUrl = ConfigurationManager.AppSettings["GoogleApiAccountsUrl"].ToString();
            bool.TryParse(ConfigurationManager.AppSettings["IsBkTask"].ToString(), out _isBkTask);
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

        /// <summary>
        /// GoogleApiClientId (管理者gmail:ftisuser@gmail.com/xxxxxxxxx)
        /// </summary>
        public static string GoogleApiClientId
        {
            get { return _googleApiClientId; }
        }

        /// <summary>
        /// Accounts Google Url
        /// </summary>
        public static string GoogleApiAccountsUrl
        {
            get { return _googleApiAccountsUrl; }
        }

        /// <summary>
        /// 是否執行背景程式
        /// </summary>
        public static bool IsBkTask
        {
            get { return _isBkTask; }
        }

        #endregion
    }
}