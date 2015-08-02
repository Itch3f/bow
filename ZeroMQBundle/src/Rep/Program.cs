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
    using AppDomainToolkit;
    using System.IO;

    class Program
    {
        //        private static Assembly commandAssembly = null;
        static void Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);

            //commandAssembly = Assembly.LoadFile(Environment.CurrentDirectory+ "\\CommonFunctions.dll");

            RunInZeroMqMode(options);

            //RunInConsoleMode();
        }

        private static void RunInConsoleMode()
        {
            string input;
            do
            {
                input = Console.ReadLine();
                string returnValue = CommandProcessor(input);
                Console.WriteLine(returnValue);
            }
            while (input != "exit");
        }

        private static void RunInZeroMqMode(Options options)
        {
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
                        
                        string returnValue = CommandProcessor(rcvdMsg);

                       
                        socket.Send(returnValue, Encoding.UTF8);
                    }
                }
            }
        }

        private static string CommandProcessor(string rcvdMsg)
        {
            if (rcvdMsg.Equals("update"))
            {
                //commandAssembly = ReLoadCommandAssembly(rcvdMsg);
                //take backup and fetch new dll and dependencies
                string SourcePath = @"D:\Dev\ZeroMq\ZeroMQBundle_Source\ZeroMQBundle\src\CommonFunctions\bin\Debug";
                string destinationPath = Environment.CurrentDirectory + "\\command";

                Console.WriteLine("Destination - " + destinationPath);

                foreach (string path in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                {
                    try
                    {
                        Console.WriteLine("Copyign file - " + path);
                        File.Copy(path, path.Replace(SourcePath, destinationPath), true);
                        Console.WriteLine("Copied file - " + path);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }

                return "dlls copied";
            }
            else
            {
                Console.WriteLine("Parent app domain - " + AppDomain.CurrentDomain.FriendlyName);
                AppDomainSetup appdomainSetup = new AppDomainSetup()
                {
                    ApplicationName = "RemoteAppDomain",
                    ShadowCopyFiles = "true"
                };

                var appdomainContext = AppDomainContext.Create(appdomainSetup);
                try
                {
                    IAssemblyTarget targetAssembly = appdomainContext.LoadAssembly(LoadMethod.LoadFrom, Environment.CurrentDirectory + "\\command\\CommonFunctions.dll");

                    var returnValue = RemoteFunc.Invoke(appdomainContext.Domain,
                        rcvdMsg,
                          (commadn) =>
                          {
                              var assembly = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name == "CommonFunctions").FirstOrDefault();
                              if (assembly != null)
                              {
                                  Type type = assembly.GetType("CommonFunctions.CommandFactory");
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
                                              object commadnObj = methodInfo.Invoke(null, null);
                                          }
                                          else
                                          {
                                              object[] parametersArray = new object[] { commadn };
                                              //The invoke does NOT work it throws "Object does not match target type"             
                                              object commadnObj = methodInfo.Invoke(null, parametersArray);

                                              MethodInfo methodInfo1 = commadnObj.GetType().GetMethod("Execute");

                                              object[] parametersArray1 = new object[] { commadn };
                                              string result1 = (string)methodInfo1.Invoke(commadnObj, parametersArray1);

                                              Console.WriteLine("Sending : " + result1 + Environment.NewLine);
                                              return result1;
                                          }
                                      }
                                      else
                                      {
                                          return "Method GetCommand Not Found.";
                                      }
                                  }
                                  else
                                  {
                                      return "Type Not Found.";
                                  }
                              }
                              else
                              {
                                  return "assembly not found";
                              }
                              return "assembly not found";
                          }
                          );

                    return returnValue;
                }
                finally
                {
                    appdomainContext.Dispose();

                    while (!appdomainContext.IsDisposed)
                    {
                        Console.WriteLine("Child app domain not disposed.");
                    }
                }
            }
        }

        private static Assembly ReLoadCommandAssembly(string commadnMsg)
        {
            Assembly assembly = Assembly.LoadFile("CommonFunctions");
            return assembly;
        }

        //private static Object GetCommandObject(string commadnMsg)
        //{
        //    Type type = commandAssembly.GetType("CommonFunctions.CommandFactory");
        //    if (type != null)
        //    {
        //        MethodInfo methodInfo = type.GetMethod("GetCommand");
        //        if (methodInfo != null)
        //        {
        //            object result = null;
        //            ParameterInfo[] parameters = methodInfo.GetParameters();
        //            //object classInstance = Activator.CreateInstance(type, null);
        //            if (parameters.Length == 0)
        //            {
        //                return methodInfo.Invoke(null, null);
        //            }
        //            else
        //            {
        //                object[] parametersArray = new object[] { commadnMsg };
        //                //The invoke does NOT work it throws "Object does not match target type"             
        //                return methodInfo.Invoke(null, parametersArray);
        //            }
        //        }
        //    }
        //    return null;
        //}
    }
}
