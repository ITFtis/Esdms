using System;
using System.Collections.Generic;
using Esdms.Models;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Esdms
{
    public class BkTask
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BkTask()
        {
            // 從組態檔載入相關參數，例如 SmtpHost、SmtpPort、SenderEmail 等等.
        }
        private DateTime startdt = DateTime.Now;
        private int runCount = 0;
        private bool _stopping = false;


        public void Run()
        {
            logger.Info("啟動BkTask背景");
            System.IO.File.AppendAllText(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(("~/logs")), "startlog.txt"), $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}啟動BkTask背景" + Environment.NewLine);
            var aThread = new Thread(TaskLoop);
            aThread.IsBackground = true;
            aThread.Priority = ThreadPriority.BelowNormal;  // 避免此背景工作拖慢 ASP.NET 處理 HTTP 請求.
            aThread.Start();
        }

        public void Stop()
        {
            _stopping = true;
        }



        private void TaskLoop()
        {
            // 設定每一輪工作執行完畢之後要間隔幾分鐘再執行下一輪工作.
            //const int LoopIntervalInMinutes = 1000 * 60 * 15;//15分鐘1次
            const int LoopIntervalInMinutes = 1000 * 60 * 60 * 8;//8小時1次(系統資料剛建立，避免"薪資資料"尚未完成，執行更新)

            logger.Info("背景TaskLoop on thread ID: " + Thread.CurrentThread.ManagedThreadId.ToString());
            while (!_stopping)
            {
                try
                {
                    Todo();
                    logger.Info("==================================================================================");
                }
                catch (Exception ex)
                {
                    // 發生意外時只記在 log 裡，不拋出 exception，以確保迴圈持續執行.
                    logger.Error("BkTask.TaskLoop錯誤:" + ex.ToString());
                }
                finally
                {
                    // 每一輪工作完成後的延遲.
                    System.Threading.Thread.Sleep(LoopIntervalInMinutes);
                }
            }
        }

        private void Todo()
        {
            logger.Info($"To do ......啟動時間:{startdt.ToString("yyyy/MM/dd HH:mm:ss")};次數:{(++runCount)}");

            DateTime start_time = DateTime.Now;

            //工作1
            logger.Info("job1");
            start_time = DateTime.Now;
            logger.Info("Start:" + "iHub匯入專案");
            To_ImportiHubToProject();
            logger.Info(@"Execution time(sec)=" + DateTime.Now.Subtract(start_time).TotalSeconds);
        }

        /// <summary>
        /// iHub匯入專案
        /// </summary>
        private void To_ImportiHubToProject()
        {
            try
            {
                //重要：太多年，跑很久影響網站效能
                int upYear = 2; //upYear年內專案處理
                int nowYear = DateTime.Now.Year - 1911;

                //所有專案
                var datas = FtisHelperV2.DB.Helper.GetAllProject()
                            .Where(a => (nowYear - a.PrjYear) < upYear)  //配合系統上線，取113年度後計畫
                                                                         //.Where(a => a.PrjID == "114040700")    //測試專案
                            .Where(a => a.PrjYear >= 114)
                            .ToList();

                Dou.Models.DB.IModelEntity<Project> model = new Dou.Models.DB.ModelEntity<Project>(new EsdmsModelContextExt());
                var projects = model.GetAll().ToList();

                //不存在比對錯誤
                var adds = datas.Where(a => !projects.Any(b => b.PrjId == a.PrjID));     //不存在

                //新增(adds)
                var addProjects = adds.Select(a => new Project
                {
                    PrjId = a.PrjID,
                    PjNo = a.PjNo,
                    //PrjYear = a.PrjYear != 0 ? DateFormat.ToYear1(a.PrjYear) : int.Parse(Code.GetProjectYear().Min(p => p.Value).ToString()),
                    Year = a.PrjYear,
                    CommissionedUnit = a.OwnerA,
                    OwnerA = a.OwnerA,
                    OwnerB = a.OwnerB,
                    Name = a.PrjName,
                    BriefName = a.BriefName,
                    PrjStartDate = a.PrjStartDate,
                    PrjEndDate = a.PrjEndDate,
                    PjNoM = a.PjNoM,
                    PjNameM = a.PjNameM,
                    BDate = DateTime.Now,
                    BFno = "system",
                    BName = "To_ImportiHubToProject"
                }).ToList();

                logger.Info("執行中，專案(新增)數量：" + addProjects.Count());
                foreach(var av in addProjects)
                {
                    model.Add(av);
                }                
                logger.Info("成功新增");

                //修改(updates)  少有異動修改的判斷
                //沒異動日欄位，只同步n年內資料
                //重要：太多年，跑很久影響網站效能。限定3年內專案才更新
                int n = 3;
                var ppp = datas.Where(a => (nowYear - a.PrjYear) < n)
                            .ToList();

                var updates = projects.Where(a => !addProjects.Any(b => b.PrjId == a.PrjId))  //不存在(addProjects)
                                    .Where(a => ppp.Any(b => b.PrjID == a.PrjId))      //存在
                                    .ToList();
                foreach (var update in updates)
                {
                    var f = datas.Where(a => a.PrjID == update.PrjId).First();
                    update.PjNo = f.PjNo;
                    //update.PrjYear = f.PrjYear != 0 ? DateFormat.ToYear1(f.PrjYear) : int.Parse(Code.GetProjectYear().Min(p => p.Value).ToString());
                    update.Year = f.PrjYear;
                    update.CommissionedUnit = f.OwnerA;
                    update.OwnerA = f.OwnerA;
                    update.OwnerB = f.OwnerB;
                    update.Name = f.PrjName;
                    update.BriefName = f.BriefName;
                    update.PrjStartDate = f.PrjStartDate;
                    update.PrjEndDate = f.PrjEndDate;
                    update.PjNoM = f.PjNoM;
                    update.PjNameM = f.PjNameM;
                    update.UDate = DateTime.Now;
                    update.UFno = "system";
                    update.UName = "To_ImportiHubToProject";
                }

                logger.Info("執行中，專案(修改)數量：" + updates.Count());
                model.Update(updates);
                logger.Info("修改成功");

                //Project.ResetGetAllDatas();
                ProjectSelectItems.Reset();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}