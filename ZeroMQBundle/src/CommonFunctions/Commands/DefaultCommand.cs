using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonFunctions.Commands
{
  public  class DefaultCommand :ICommand
    {
        public string Execute(string command)
        {
            return command;
        }
    }
}
