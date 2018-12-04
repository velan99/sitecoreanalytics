using Newtonsoft.Json;
using Sitecore.Analytics;
using Sitecore.Analytics.Data;
using Sitecore.Diagnostics;
using System;
using System.Web;
using System.Web.SessionState;

namespace Sitecore.Scientist.Analytics
{
    /// <summary>
    /// Summary description for ClientEventTracker
    /// </summary>
    public class ClientEventTracker : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/javascript";
            TriggerEvent(context);
        }


        private static void TriggerEvent(HttpContext context)
        {
            try
            {
                string jsonData = context.Request["jsonData"];

                var clientEventData = JsonConvert.DeserializeObject<ClientEventData>(jsonData);
                if (clientEventData == null)
                    return;
                if (string.IsNullOrEmpty(clientEventData.EventName))
                {
                    return;
                }
                if (!Tracker.Enabled)
                {
                    return;
                }

                if (!Tracker.IsActive || Tracker.Current == null)
                {
                    Tracker.StartTracking(); // init tracker without tracking the endpoint as a page
                }

                if (Tracker.Current == null || Tracker.Current.CurrentPage == null)
                {
                    return;
                }
             
                if (!string.IsNullOrEmpty(clientEventData.PageEventId))
                {
                    var eventData = new PageEventData(clientEventData.EventName, Guid.Parse(clientEventData.PageEventId))
                    {
                        Text = !string.IsNullOrEmpty(clientEventData.Text) ? clientEventData.Text : string.Empty,
                        DataKey = !string.IsNullOrEmpty(clientEventData.Text) ? clientEventData.Text : string.Empty,
                        Data = !string.IsNullOrEmpty(clientEventData.Data) ? clientEventData.Data : string.Empty,
                    };
                    Tracker.Current.CurrentPage.Register(eventData);
                    return;
                }
                else if (string.IsNullOrEmpty(clientEventData.Text))
                {
                    Tracker.Current.CurrentPage.Register(clientEventData.EventName, string.Empty);
                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(clientEventData.Datakey) || string.IsNullOrEmpty(clientEventData.Data))
                    {
                        Tracker.Current.CurrentPage.Register(clientEventData.EventName, clientEventData.Text);
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(string.Concat("Sitecore.Scientist.Analytics.ClientEventTracker.TriggerEvent: error in event triggering. requestUrl: ", context.Request.Url.AbsolutePath), exception, typeof(ClientEventTracker));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }


    public class ClientEventData
    {
        public string PageEventId { get; set; }
        public string EventName { get; set; }
        public string Data { get; set; }
        public string Datakey { get; set; }
        public string Text { get; set; }
    }
}
