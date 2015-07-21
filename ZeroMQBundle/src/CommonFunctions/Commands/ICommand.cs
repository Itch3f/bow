using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonFunctions.Commands
{
   public interface ICommand
    {
        string Execute(string command);
    }
}
