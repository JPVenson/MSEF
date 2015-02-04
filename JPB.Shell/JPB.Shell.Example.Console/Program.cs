using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JPB.Shell.Example.Console.Contracts;
using JPB.Shell.MEF.Factorys;
using JPB.Shell.MEF.Services;
using JPB.Shell.MEF.Services.Extentions;

namespace JPB.Shell.Example.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }

        private void Run()
        {
            //Start the service lookup
            ServicePoolFactory.CreatePool("JPB", Directory.GetCurrentDirectory());

            //All dll's with JPB in the name are searched
            var consoleServices = ServicePool.Instance.GetServices<IConsoleAction>().ToList();

            //Register refresh

            ServicePool.Instance.RegisterExportsChanged((e, f) =>
            {
                var old = System.Console.BackgroundColor;
                System.Console.BackgroundColor = ConsoleColor.Red;
                System.Console.WriteLine("STOP UPDATE IN PROGRESS");
                consoleServices = ServicePool.Instance.GetServices<IConsoleAction>().ToList();
                System.Console.BackgroundColor = old;
                System.Console.WriteLine("Done ...");
            });

            string input = "";
            while (input != "exit")
            {
                input = System.Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                    continue;

                foreach (var consoleService in consoleServices)
                {
                    if (!input.StartsWith(consoleService.ID))
                        continue;
                    var handeld = consoleService.HandleInput(input);
                    if(handeld)
                        break;
                }
            }

        }
    }
}
