using CommonFunctions.IIS.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUnitTests
{
    [TestFixture]
    public class IISManagerTests
    {

        [Test]
        public void GetAllWebSites()
        {
            InternetInformationServer iis = new InternetInformationServer();
            var webSites = iis.Sites;
            foreach (WebSite site in webSites)
            {
                Console.WriteLine(site.ServerComment);
            }

        }

        [Test]
        public void CreateNewWebSite()
        {
            string poolName = "testAppPool";
            string ServerComment = "testWEbSite";
            string webRootPath = @"d:\websites\testwebsite";

            InternetInformationServer iis = new InternetInformationServer();
            ApplicationPool pool = iis.GetApplicationPool(poolName);
            
            if (pool == null)
                iis.CreateAppliationPool(poolName);

            var webSite = iis.CreateWebSite(ServerComment, webRootPath);

            // apply any virtual root properties here
            //WebVirtualDirectory virtualDirectory = webSite.DirectorySettings;
            //virtualDirectory.SetFrameworkVersion("v2.0.50727");
            
        }
    }
}
