using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Esdms
{
    public class WaterMarkFormat
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 套用浮水印(專家清冊列印)
        /// </summary>
        /// <param name="path">xlsx檔案來源</param>
        /// <param name="watermark">浮水印文字</param>
        /// <param name="waterColor">浮水印深淺色(空值，跑預設)</param>
        public static bool WaterMarkF1(string path, string watermark, string waterColor = "")
        {
            bool result = false;

            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(path);

                Font font = new System.Drawing.Font("標楷體", 25, FontStyle.Bold);

                foreach (Worksheet sheet in workbook.Worksheets)
                {
                    //sheet.PageSetup.PageHeight  841.8897637795277   double
                    //sheet.PageSetup.PageWidth   595.27559055118115  double
                    //Gainsboro(剛好), Beige(有點淺), WhiteSmoke (複印無色), 
                    System.Drawing.Image imgWtrmrk = ExcelSpecHelper.DrawText(watermark, font, waterColor,
                                                        System.Drawing.Color.White,
                                                        sheet.PageSetup.PageHeight + 550, sheet.PageSetup.PageWidth + 30);

                    sheet.PageSetup.LeftHeaderImage = imgWtrmrk;
                    sheet.PageSetup.LeftHeader = "&G";
                    ////水印在此模式顯示
                    //sheet.ViewMode = ViewMode.Layout;

                    sheet.PageSetup.LeftMargin = 1.1;
                    sheet.PageSetup.RightMargin = 1.1;

                    //spire.XLS：浮水印無法左右展開
                    //AlignWithMargins：1 改不動(免費版本有此問題，商業版正常)
                    //https://www.e-iceblue.com/forum/post27913.html
                    //sheet.PageSetup.LeftMargin = 0.5;
                    //sheet.PageSetup.RightMargin = 0.5;
                    //sheet.PageSetup.AlignWithMargins = 1;
                    //NPOI不支援頁首插圖，則(spire.XLS)與(NPOI)都無法解浮水印無法左右展開
                }

                workbook.Save();

                result = true;
            }
            catch (Exception ex)
            {
                // 發生意外時只記在 log 裡，不拋出 exception，以確保迴圈持續執行.
                logger.Error("套用浮水印錯誤(WaterMarkF1):" + ex.ToString());
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// 套用浮水印(產製專家請款單)
        /// </summary>
        /// <param name="path">xlsx檔案來源</param>
        /// <param name="watermark">浮水印文字</param>
        /// <param name="waterColor">浮水印深淺色(空值，跑預設)</param>
        public static bool WaterMarkF2(string path, string watermark, string waterColor = "")
        {
            bool result = false;

            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(path);

                Font font = new System.Drawing.Font("標楷體", 50, FontStyle.Bold);

                foreach (Worksheet sheet in workbook.Worksheets)
                {
                    //sheet.PageSetup.PageHeight  841.8897637795277   double
                    //sheet.PageSetup.PageWidth   595.27559055118115  double
                    //Gainsboro(剛好), Beige(有點淺), WhiteSmoke (複印無色), 
                    int multiple = 2;
                    System.Drawing.Image imgWtrmrk = ExcelSpecHelper.DrawText(watermark, font, waterColor,
                                                        System.Drawing.Color.White,
                                                        sheet.PageSetup.PageHeight + 2300, sheet.PageSetup.PageWidth + 1050, multiple);

                    sheet.PageSetup.LeftHeaderImage = imgWtrmrk;
                    sheet.PageSetup.LeftHeader = "&G";
                    ////水印在此模式顯示
                    //sheet.ViewMode = ViewMode.Layout;

                    sheet.PageSetup.LeftMargin = 1.1;
                    sheet.PageSetup.RightMargin = 1.1;

                    //spire.XLS：浮水印無法左右展開
                    //AlignWithMargins：1 改不動(免費版本有此問題，商業版正常)
                    //https://www.e-iceblue.com/forum/post27913.html
                    //sheet.PageSetup.LeftMargin = 0.5;
                    //sheet.PageSetup.RightMargin = 0.5;
                    //sheet.PageSetup.AlignWithMargins = 1;
                    //NPOI不支援頁首插圖，則(spire.XLS)與(NPOI)都無法解浮水印無法左右展開
                }

                workbook.Save();

                result = true;
            }
            catch (Exception ex)
            {
                // 發生意外時只記在 log 裡，不拋出 exception，以確保迴圈持續執行.
                logger.Error("套用浮水印錯誤(WaterMarkF1):" + ex.ToString());
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }
    }
}