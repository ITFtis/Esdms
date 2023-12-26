using FtisHelperV2.DB;
using Microsoft.Owin;
using Owin;
using System;
using System.Configuration;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Esdms.Startup))]

namespace Esdms
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //20230421, ADD設定連線方式(true=測試, false=正式) by markhong
            FtisModelContext.LocalTest = bool.Parse(ConfigurationSettings.AppSettings["LocalTest"].ToString());

            Dou.Context.Init(new Dou.DouConfig
            {
                DefaultPassword = "3922",
                PasswordEncode = (p) =>
                {
                    return (int.Parse(p) * 4 + 13579) + "";
                    //return System.Web.Helpers.Crypto.HashPassword(p);
                },
                VerifyPassword = (ep, vp) =>
                {
                    int pint = 0;
                    bool tp = int.TryParse(vp, out pint);
                    if (!tp)
                        return false;
                    else
                    {
                        return ep == (pint * 4 + 13579) + "";
                    }
                    //return System.Web.Helpers.Crypto.VerifyHashedPassword(ep, vp);
                },
                SqlDebugLog = true,
                LoginPage = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext).Action("DouLogin", "User")
            });
        }

    }
}
