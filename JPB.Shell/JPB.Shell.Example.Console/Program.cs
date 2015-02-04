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
            System.Console.WriteLine("STOP START IN PROGRESS");
            var consoleServices = ServicePool.Instance.GetServices<IConsoleAction>().ToList();
            System.Console.WriteLine("Done ... {0} Actions", consoleServices.Count);

            //Register refresh
            ServicePool.Instance.RegisterExportsChanged((e, f) =>
            {
                var old = System.Console.BackgroundColor;
                System.Console.Clear();
                System.Console.BackgroundColor = ConsoleColor.Red;
                System.Console.WriteLine("STOP UPDATE IN PROGRESS");
                consoleServices = ServicePool.Instance.GetServices<IConsoleAction>().ToList();
                System.Console.BackgroundColor = old;
                System.Console.Clear();
                System.Console.WriteLine("Done ... {0} Actions", consoleServices.Count);

                System.Console.WriteLine("Key");
                foreach (var consoleService in consoleServices)
                {
                    System.Console.WriteLine("{0} : ", consoleService.ID);
                }
            });

            string input = "";

            while (input != "exit")
            {
                //input = System.Console.ReadLine();
                input = "math 1+1+(1+2+(3+3)*3";
                if (string.IsNullOrEmpty(input))
                    continue;

                foreach (var consoleService in consoleServices)
                {
                    if (input.StartsWith(consoleService.ID) || input.ToLower().StartsWith(consoleService.ID.ToLower()))
                    {
                        var handeld = consoleService.HandleInput(input.Remove(0, consoleService.ID.Length));
                        if (handeld)
                            break;
                    }
                }
                input = "exit";
            }

            System.Console.ReadKey();

        }
    }
}
