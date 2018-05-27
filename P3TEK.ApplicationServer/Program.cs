using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.ExpressApp.Security.ClientServer;
using System.Configuration;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp;
using System.Collections;
using System.ServiceModel;
using DevExpress.ExpressApp.Security.ClientServer.Wcf;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.MiddleTier;

namespace P3TEK.ApplicationServer {
    class Program {
        static Program() {
            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
        }
        private static void serverApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }
        static void Main(string[] args) {
            try {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                ValueManager.ValueManagerType = typeof(MultiThreadValueManager<>).GetGenericTypeDefinition();

                SecurityAdapterHelper.Enable();
                ServerApplication serverApplication = new ServerApplication();
                serverApplication.ApplicationName = "P3TEK";
                serverApplication.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
#if DEBUG
                if(System.Diagnostics.Debugger.IsAttached && serverApplication.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                    serverApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
                }
#endif

                serverApplication.Modules.BeginInit();
                serverApplication.Modules.Add(new DevExpress.ExpressApp.Security.SecurityModule());
                serverApplication.Modules.Add(new P3TEK.Module.P3TEKModule());
                serverApplication.Modules.Add(new P3TEK.Module.Win.P3TEKWindowsFormsModule());
                serverApplication.Modules.Add(new P3TEK.Module.Web.P3TEKAspNetModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.AuditTrail.AuditTrailModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.Chart.ChartModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.Chart.Win.ChartWindowsFormsModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.Chart.Web.ChartAspNetModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.CloneObject.CloneObjectModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.Dashboards.DashboardsModule() { DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.DashboardData) });
                serverApplication.Modules.Add(new DevExpress.ExpressApp.Dashboards.Win.DashboardsWindowsFormsModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.Dashboards.Web.DashboardsAspNetModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.FileAttachments.Web.FileAttachmentsAspNetModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.HtmlPropertyEditor.Win.HtmlPropertyEditorWindowsFormsModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.Kpi.KpiModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.PivotChart.PivotChartModuleBase());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.PivotChart.Win.PivotChartWindowsFormsModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.PivotChart.Web.PivotChartAspNetModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.PivotGrid.PivotGridModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.PivotGrid.Win.PivotGridWindowsFormsModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.PivotGrid.Web.PivotGridAspNetModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.ReportsV2.ReportsModuleV2() { ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportDataV2) });
                serverApplication.Modules.Add(new DevExpress.ExpressApp.ReportsV2.Win.ReportsWindowsFormsModuleV2());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.ReportsV2.Web.ReportsAspNetModuleV2());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.TreeListEditors.Win.TreeListEditorsWindowsFormsModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.TreeListEditors.Web.TreeListEditorsAspNetModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.Validation.ValidationModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.Validation.Web.ValidationAspNetModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule());
                serverApplication.Modules.EndInit();

                serverApplication.DatabaseVersionMismatch += new EventHandler<DatabaseVersionMismatchEventArgs>(serverApplication_DatabaseVersionMismatch);
                serverApplication.CreateCustomObjectSpaceProvider += (s, e) => {
                    e.ObjectSpaceProviders.Add(new XPObjectSpaceProvider(e.ConnectionString, e.Connection));
                    e.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(serverApplication.TypesInfo, null));
                };

                serverApplication.ConnectionString = connectionString;

                Console.WriteLine("Setup...");
                serverApplication.Setup();
                Console.WriteLine("CheckCompatibility...");
                serverApplication.CheckCompatibility();
                serverApplication.Dispose();

                Console.WriteLine("Starting server...");
                Func<IDataServerSecurity> dataServerSecurityProvider = () =>
                {
                    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), new AuthenticationStandard());
                    security.SupportNavigationPermissionsForTypes = false;
                    return security;
                };

                WcfXafServiceHost serviceHost = new WcfXafServiceHost(connectionString, dataServerSecurityProvider);
                serviceHost.AddServiceEndpoint(typeof(IWcfXafDataServer), WcfDataServerHelper.CreateNetTcpBinding(), "net.tcp://127.0.0.1:1451/DataServer");
                serviceHost.Open();
                Console.WriteLine("Server is started. Press Enter to stop.");
                Console.ReadLine();
                Console.WriteLine("Stopping...");
                serviceHost.Close();
                Console.WriteLine("Server is stopped.");
            }
            catch(Exception e) {
                Console.WriteLine("Exception occurs: " + e.Message);
                Console.WriteLine("Press Enter to close.");
                Console.ReadLine();
            }
        }
    }
}
