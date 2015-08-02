using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rep
{
   public class BowService 
    {
       readonly RequestProcessor _requestProcessor;

       public void Start(Options options) { _requestProcessor.RunInZeroMqMode(options); }
       public void Stop() { _requestProcessor.StopMqListener(); }
    }
}
