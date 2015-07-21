using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace CommonFunctions.IIS.Util
{
    public class ApplicationPool : WmiObjectBase
    {
        internal ApplicationPool(ManagementScope scope)
            : base(scope)
        {
        }

        internal ApplicationPool(ManagementScope scope, string name)
            : base(scope)
        {
            this.Name = name;
        }

        public string Name { get; internal set; }
    }
}