using Esdms.Controllers.Es;
using Esdms.Models;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Esdms
{
    public class Rpt_ProjectInvoice : ReportClass
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Export(string prjId)
        {
            string url = "";

            try
            {
                url = "abc";
            }
            catch (Exception ex)
            {
                _errorMessage = "產製請款單失敗：" + ex.Message;
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                return "";
            }

            return url;
        }

    }
}