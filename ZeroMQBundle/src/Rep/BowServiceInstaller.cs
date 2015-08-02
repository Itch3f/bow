using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topshelf;

namespace Rep
{
    class BowServiceInstaller
    {
        static void Main(string[] arg)
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
            //if (!parser.ParseArguments(args, options))
            //    Environment.Exit(1);

            //commandAssembly = Assembly.LoadFile(Environment.CurrentDirectory+ "\\CommonFunctions.dll");

            string commandArgs = null;

            HostFactory.Run(x =>                                 //1
            {
                x.AddCommandLineDefinition("arguments", f => { commandArgs = f; });
                x.ApplyCommandLine();

                Console.WriteLine(commandArgs);

                string[] args = commandArgs.Split(' ');

                parser.ParseArguments(args, options);
                x.Service<BowService>(s =>                        //2
                {
                    s.ConstructUsing(name => new BowService());     //3
                    s.WhenStarted(tc => tc.Start(options));              //4
                    s.WhenStopped(tc => tc.Stop());               //5
                });
                x.RunAsLocalSystem();                            //6

                x.SetDescription("Sample Bow Service Host");        //7
                x.SetDisplayName("BowService");                       //8
                x.SetServiceName("BowService");                       //9
            });                                                  //10
        }
    }
}
