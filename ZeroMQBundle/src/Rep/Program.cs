//----------------------------------------------------------------------------------
// Rep Socket Sample
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

namespace Rep
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using CommandLine;
    using ZeroMQ;
    using System.ServiceProcess;
    using System.Reflection;

    class Program
    {

        private static Assembly commandAssembly = null;

        static void Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);
            
            commandAssembly = Assembly.LoadFile(Environment.CurrentDirectory+ "\\CommonFunctions.dll");

            using (var context = ZmqContext.Create())
            {
                using (var socket = context.CreateSocket(SocketType.REP))
                {
                    foreach (var bindEndPoint in options.bindEndPoints)
                        socket.Bind(bindEndPoint);

                    while (true)
                    {
                        Thread.Sleep(options.delay);
                        var rcvdMsg = socket.Receive(Encoding.UTF8);
                        Console.WriteLine("Received: " + rcvdMsg);

                        if (rcvdMsg.Equals("Update"))
                        {
                            commandAssembly = ReLoadCommandAssembly(rcvdMsg);
                        }
                        else
                        {
                          object commadnObj =  GetCommandObject(rcvdMsg);

                          MethodInfo methodInfo = commadnObj.GetType().GetMethod("Execute");
                             object[] parametersArray = new object[] { rcvdMsg };
                             string result = (string)methodInfo.Invoke(commadnObj, parametersArray);
                             Console.WriteLine("Sending : " + result + Environment.NewLine);
                             socket.Send(result, Encoding.UTF8);
                        }
                        //ICommand command = CommandFactory.GetCommand(rcvdMsg);
                        //string result = command.Execute(rcvdMsg);
                        
                    }
                }
            }
        }

        private static Assembly ReLoadCommandAssembly(string commadnMsg)
        {
            Assembly assembly = Assembly.LoadFile("CommonFunctions");
            return assembly;
        }

        private static Object GetCommandObject(string commadnMsg)
        {
            Type type = commandAssembly.GetType("CommonFunctions.CommandFactory");
            if (type != null)
            {
                MethodInfo methodInfo = type.GetMethod("GetCommand");
                if (methodInfo != null)
                {
                    object result = null;
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    //object classInstance = Activator.CreateInstance(type, null);
                    if (parameters.Length == 0)
                    {
                        return methodInfo.Invoke(null, null);
                    }
                    else
                    {
                        object[] parametersArray = new object[] { commadnMsg };
                        //The invoke does NOT work it throws "Object does not match target type"             
                        return methodInfo.Invoke(null, parametersArray);
                    }
                }
            }
            return null;
        }
    }
}
