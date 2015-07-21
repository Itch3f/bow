using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonFunctions;
using System.ServiceProcess;
using System.Threading;

namespace CommonUnitTests
{
    [TestFixture]
    public class ServieControllerTests
    {
        [Test]
        public void Should_list_all_services()
        {
            ServiceControllerManager scm = new ServiceControllerManager();
            IEnumerable<ServiceController> service = scm.GetAllServices();
            ServiceController sc = service.Where(ser => ser.ServiceName == "ActiveMQ").FirstOrDefault();
            if (sc != null)
            {
                scm.Start(sc);

                Assert.That(sc.Status, Is.EqualTo(ServiceControllerStatus.Running));
            }
            else
                Assert.Fail("Service does not exist");
        }

    }
}
