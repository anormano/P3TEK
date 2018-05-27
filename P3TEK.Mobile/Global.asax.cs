using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace P3TEK.Mobile
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            CorsSupport.HandlePreflightRequest(HttpContext.Current);
        }
        protected void Application_Start(Object sender, EventArgs e) {
            DevExpress.XtraReports.Web.WebDocumentViewer.DefaultWebDocumentViewerContainer.Register<DevExpress.XtraReports.Web.WebDocumentViewer.IWebDocumentViewerReportResolver, DevExpress.ExpressApp.ReportsV2.Mobile.XafReportsResolver<P3TEKMobileApplication>>();
        }
    }
}