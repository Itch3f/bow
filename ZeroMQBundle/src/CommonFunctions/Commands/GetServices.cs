using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace CommonFunctions.Commands
{
    public class GetServices : MarshalByRefObject, ICommand
    {
        public string Execute(string command)
        {
            if (command.StartsWith("getservice"))
            {
                ServiceControllerManager svccm = new ServiceControllerManager();
                IEnumerable<string> services = svccm.GetAllServices().ForEach<ServiceController, string>(x => x.DisplayName);
                StringBuilder sb = new StringBuilder();

                int counter = 1;
                sb.Append("<Services>");
                foreach (var service in services)
                {
                    sb.Append(string.Format("<Service id= {0}>", counter++));
                    sb.Append(service);
                    sb.Append("</Service>");

                }
                sb.Append("</Services>");
                return sb.ToString();
            }
            else
            {
                if (command.StartsWith("startservice"))
                {
                    ServiceControllerManager svccm = new ServiceControllerManager();
                    var sc = svccm.GetAllServices().Where(x => x.DisplayName == "ActiveMQ").FirstOrDefault();
                    svccm.Start(sc);
                    return "ActiveMQ service started.";
                }
                else
                {
                    ServiceControllerManager svccm = new ServiceControllerManager();
                    var sc = svccm.GetAllServices().Where(x => x.DisplayName == "ActiveMQ").FirstOrDefault();
                    svccm.Stop(sc);
                    return "ActiveMQ service stopped.";
                }
            }
        }
    }
}
