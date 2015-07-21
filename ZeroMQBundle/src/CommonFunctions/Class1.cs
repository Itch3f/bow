//using CommonFunctions.IIS.Util;
//static class Program
//{
//    internal static string EnvironmentName { get; set; }
//    static int Main(string[] args)
//    {
//        EnvironmentName = args[0];
//        InternetInformationServer iis = new InternetInformationServer(Program.EnvironmentName);
//        WebSite webSite = iis.GetWebSite(ServerComment);
//        ShutdownSite(webSite);

//        // DO SOME STUFF

//        // create the website
//        if (webSite == null)
//        {
//            ApplicationPool pool = iis.GetApplicationPool(poolName);
//            if (pool == null)
//                iis.CreateAppliationPool(poolName);

//            webSite = iis.CreateWebSite(ServerComment, webRootPath);

//            // apply any virtual root properties here
//            WebVirtualDirectory virtualDirectory = webSite.DirectorySettings;
//            virtualDirectory.SetFrameworkVersion("v2.0.50727");
//        }

//        // start up the website...
//        webSite.Start();
//    }

//    private static void ShutdownSite(WebSite webSite)
//    {
//        if (webSite != null)
//        {
//            webSite.Stop();
//        }
//    }
//}