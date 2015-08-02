using CommonFunctions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonFunctions
{
    public class CommandFactory : MarshalByRefObject
    {
        private CommandFactory() { }

        public static object GetCommand(string rcvdMsg)
        {
            string currentAppdomainName = AppDomain.CurrentDomain.FriendlyName;

            Console.WriteLine("Executing app domain - " + currentAppdomainName);

            if (rcvdMsg.Equals("getservices") || rcvdMsg.StartsWith("startservice") || rcvdMsg.StartsWith("stopservice"))
            {
                return AppDomain.CurrentDomain.CreateInstanceAndUnwrap("CommonFunctions", "CommonFunctions.Commands.GetServices");
                //return new GetServices();  
            }
            else
            {
                return AppDomain.CurrentDomain.CreateInstanceAndUnwrap("CommonFunctions", "CommonFunctions.Commands.DefaultCommand");
                //return new DefaultCommand();
            }
        }
    }
}
