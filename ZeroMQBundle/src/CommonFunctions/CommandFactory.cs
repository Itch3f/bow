using CommonFunctions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonFunctions
{
    public class CommandFactory
    {
        private CommandFactory() { }
        public static Commands.ICommand GetCommand(string rcvdMsg)
        {
            if (rcvdMsg.Equals("getservices"))
            {
                return new GetServices();  
            }
            else
            {
                return new DefaultCommand();
            }
        }
    }
}
