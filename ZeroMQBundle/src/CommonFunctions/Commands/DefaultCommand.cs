using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonFunctions.Commands
{
    public class DefaultCommand : MarshalByRefObject, ICommand
    {
        public string Execute(string command)
        {
            string currentAppdomainName = AppDomain.CurrentDomain.FriendlyName;
            return "Executing App domain" + currentAppdomainName + " Received - " + command + "from updated 1 plugin";
        }
    }
}
