using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Http;
using Google.Apis.Services;


namespace Esdms.Controllers.Notify
{
    [Dou.Misc.Attr.MenuDef(Id = "GCal", Name = "加入Google行事曆", Action = "Index", Index = -1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = true)]
    public class GCalController : Controller
    {
        // GET: GCal
        public ActionResult Index()
        {
            //2.利用Google Calendar API，新增活動至Google行事曆
            string path = Server.MapPath("~/Data/t_key/ftis-prj-esdms-aaa74f95b1a6.json");
            CalendarService _service = this.GetCalendarService(path);
            CreateEvent(_service);

            return View();
        }

        /// <summary>
        /// 驗證 Google 登入授權
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidGoogleLogin()
        {
            string formCredential = Request.Form["credential"]; //回傳憑證
            string formToken = Request.Form["g_csrf_token"]; //回傳令牌
            string cookiesToken = Request.Cookies["g_csrf_token"].ToString(); //Cookie 令牌

            //1.驗證 Google Token，取得帳號資料
            GoogleJsonWebSignature.Payload payload = VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;

            //2.利用Google Calendar API，新增活動至Google行事曆
            string path = Server.MapPath("~/Data/ftis-prj-esdms-aaa74f95b1a6.json");
            CalendarService _service = this.GetCalendarService(path);
            CreateEvent(_service);

            if (payload == null)
            {
                // 驗證失敗
                ViewData["Msg"] = "驗證 Google 授權失敗";
            }
            else
            {
                //驗證成功，取使用者資訊內容
                ViewData["Msg"] = "驗證 Google 授權成功" + "<br>";
                ViewData["Msg"] += "Email:" + payload.Email + "<br>";
                ViewData["Msg"] += "Name:" + payload.Name + "<br>";
                ViewData["Msg"] += "Picture:" + payload.Picture;
            }

            return View();
        }

        /// <summary>
        /// 驗證 Google Token
        /// </summary>
        /// <param name="formCredential"></param>
        /// <param name="formToken"></param>
        /// <param name="cookiesToken"></param>
        /// <returns></returns>
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string formCredential, string formToken, string cookiesToken)
        {
            // 檢查空值
            if (formCredential == null || formToken == null && cookiesToken == null)
            {
                return null;
            }

            GoogleJsonWebSignature.Payload payload;
            try
            {
                ////// 驗證 token
                ////if (formToken != cookiesToken)
                ////{
                ////    return null;
                ////}

                // 驗證憑證                
                string GoogleApiClientId = AppConfig.GoogleApiClientId;
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { GoogleApiClientId }
                };
                //payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
                payload = await GoogleJsonWebSignature.ValidateAsync(formCredential).ConfigureAwait(false);
                if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals(AppConfig.GoogleApiAccountsUrl))
                {
                    return null;
                }
                if (payload.ExpirationTimeSeconds == null)
                {
                    return null;
                }
                else
                {
                    DateTime now = DateTime.Now.ToUniversalTime();
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
                    if (now > expiration)
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
            return payload;
        }

        private CalendarService GetCalendarService(string keyfilepath)
        {
            try
            {
                string[] Scopes = {
            CalendarService.Scope.Calendar,
            CalendarService.Scope.CalendarEvents,
            CalendarService.Scope.CalendarEventsReadonly,
        };

                GoogleCredential credential;
                using (var stream = new FileStream(keyfilepath, FileMode.Open, FileAccess.Read))
                {
                    // As we are using admin SDK, we need to still impersonate user who has admin access
                    // https://developers.google.com/admin-sdk/directory/v1/guides/delegation
                    credential = GoogleCredential.FromStream(stream)
                        .CreateScoped(Scopes).CreateWithUser("saftis-esdms@ftis-prj-esdms.iam.gserviceaccount.com");

                }

                // Create Calendar API service.
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Calendar Sample",
                });
                return service;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 利用Google Calendar API，新增活動至Google行事曆
        /// </summary>
        /// <param name="_service"></param>
        private void CreateEvent(CalendarService _service)
        {
            //Event body = new Event();
            //EventAttendee a = new EventAttendee();
            //a.Email = "brianlin12345@gmail.com";
            ////a.Id = JwtId;

            //List<EventAttendee> attendees = new List<EventAttendee>();
            //attendees.Add(a);

            //body.Attendees = attendees;
            //EventDateTime start = new EventDateTime();
            //start.DateTime = Convert.ToDateTime("2024-06-01T09:00:00+0530");
            //EventDateTime end = new EventDateTime();
            //end.DateTime = Convert.ToDateTime("2024-06-02T11:00:00+0530");
            //body.Start = start;
            //body.End = end;
            //body.Location = "Taiwan";
            //body.Summary = "測試新活動aaaa";

            ////var zzz = _service.Calendars.Get("primary").Execute();
            ////'Error:"unauthorized_client", Description:"Client is unauthorized to retrieve access tokens using this method, or client not authorized for any of the scopes requested

            //String calendarId = "primary";
            //EventsResource.InsertRequest request = new EventsResource.InsertRequest(_service, body, calendarId);
            //Event response = request.Execute();

            Event newEvent = new Event()
            {
                Summary = "Google I/O 2015",
                Location = "800 Howard St., San Francisco, CA 94103",
                Description = "A chance to hear more about Google's developer products.",
                Start = new EventDateTime()
                {
                    DateTime = DateTime.Parse("2024-06-28T09:00:00-07:00"),
                    TimeZone = "America/Los_Angeles",
                },
                End = new EventDateTime()
                {
                    DateTime = DateTime.Parse("2024-06-28T17:00:00-07:00"),
                    TimeZone = "America/Los_Angeles",
                },
                //Recurrence = new String[] { "RRULE:FREQ=DAILY;COUNT=2" },
                Attendees = new EventAttendee[] {
                new EventAttendee() { Email = "brianlin12345@gmail.com" },
            },
                //Reminders = new Event.RemindersData()
                //{
                //    UseDefault = false,
                //    Overrides = new EventReminder[] {
                //    new EventReminder() { Method = "email", Minutes = 24 * 60 },
                //    new EventReminder() { Method = "sms", Minutes = 10 },
                //}
                //}
            };

            String calendarId = "primary";
            EventsResource.InsertRequest request = _service.Events.Insert(newEvent, calendarId);
            Event createdEvent = request.Execute();
            Console.WriteLine("Event created: {0}", createdEvent.HtmlLink);
        }
    }
}