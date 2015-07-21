using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace CommonFunctions
{
    public class ServiceControllerManager
    {
        public IEnumerable<ServiceController> GetAllServices()
        {
            return ServiceController.GetServices();
        }

        public void Start(ServiceController sc)
        {
            if (sc.Status != ServiceControllerStatus.Running)
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
            }
        }

        public void Stop(ServiceController sc)
        {
            if (sc.Status != ServiceControllerStatus.Stopped)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
            }
        }
    }
}
