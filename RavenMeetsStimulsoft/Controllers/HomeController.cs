using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Raven.Client;
using RavenMeetsStimulsoft.Models;
using Stimulsoft.Report;
using Stimulsoft.Report.MvcMobile;

namespace RavenMeetsStimulsoft.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetReportSnapshot()
        {
            var report = new StiReport();
            report.Load(Server.MapPath("~/App_Data/Report.mrt"));
            report.RegData("Orders", GetOrdersReportData());;
            
            return StiMvcMobileViewer.GetReportSnapshotResult(HttpContext, report);
        }

        public ActionResult ViewerEvent()
        {
            return StiMvcMobileViewer.ViewerEventResult(HttpContext);
        }

        public ActionResult PrintReport()
        {
            return StiMvcMobileViewer.PrintReportResult(HttpContext);
        }

        public FileResult ExportReport()
        {
            return StiMvcMobileViewer.ExportReportResult(HttpContext);
        }

        private static IEnumerable<OrderInfo> GetOrdersReportData()
        {
            using (var session = MvcApplication.DocumentStore.OpenSession())
            {
                var query = session.Query<Order, OrdersReportingIndex>()
                    .ProjectFromIndexFieldsInto<OrderInfo>()
                    .OrderBy(x => x.OrderId);

                // This uses the Unbounded Results API to stream the entire query response, which is essential for reporting.
                // http://ravendb.net/docs/2.5/client-api/advanced/unbounded-results

                using (var enumerator = session.Advanced.Stream(query))
                {
                    while (enumerator.MoveNext())
                    {
                        yield return enumerator.Current.Document;
                    }
                }
            }
        }

    }
}
