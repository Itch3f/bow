using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace CommonFunctions.Commands
{
    public class GetServices : ICommand
    {
        public string Execute(string command)
        {
            ServiceControllerManager svccm = new ServiceControllerManager();
            IEnumerable<string> services = svccm.GetAllServices().ForEach<ServiceController, string>(x => x.ServiceName);
            StringBuilder sb = new StringBuilder();
            sb.Append("<Services>");
            foreach (var service in services)
            {
                sb.Append("<Service>");
                sb.Append(service);
                sb.Append("</Service>");
            }
            sb.Append("</Services>");
            return sb.ToString();
        }
    }
}
