using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JPB.Shell.Contracts.Attributes;
using JPB.Shell.Contracts.Interfaces;
using JPB.Shell.Example.Console.Contracts;

namespace JPB.SHell.Example.Console.BasicMathFunctions
{
    [ServiceExport("SimpleCalculator", typeof(IConsoleAction))]
    public class SimpleCalculator : IConsoleAction
    {
        private Dictionary<char, Func<decimal, decimal, decimal>> handlers;
        public void OnStart(IApplicationContext application)
        {
            handlers = new Dictionary<char, Func<decimal, decimal, decimal>>();
            handlers.Add('*', (e, f) => e * f);
            handlers.Add('/', (e, f) => e / f);
            handlers.Add('+', (e, f) => e + f);
            handlers.Add('-', (e, f) => e - f);
        }

        public string ID
        {
            get { return "Math"; }
        }

        private string ResolvePart(string content)
        {
            foreach (var handler in handlers)
            {
                int indexOfNext = -1;
                do
                {
                    indexOfNext = content.IndexOf(handler.Key);
                    if (indexOfNext == -1)
                        break;
                    //| = indexOfNext
                    // 300 |* 200
                    var leadingIndex = indexOfNext;
                    var endingIndex = indexOfNext;
                    char currChar = content[leadingIndex];
                    do
                    {
                        var maybeNext = leadingIndex - 1;
                        currChar = content[maybeNext];

                        if (!char.IsNumber(currChar) && currChar != '.')
                            break;

                        leadingIndex--;
                        if (leadingIndex <= 0)
                            break;
                    } while (true);

                    do
                    {
                        var maybeNext = endingIndex + 1;
                        currChar = content[maybeNext];
                        if (!char.IsNumber(currChar) && currChar != '.')
                            break;

                        endingIndex++;
                        if (endingIndex >= (content.Length - 1))
                            break;

                    } while (true);

                    // found start and end so calculate now
                    //@ = leadingIndex
                    //$ = endingIndex
                    //  0 12
                    // @1|+1$
                    // 1+1

                    var left = content.Substring(leadingIndex, indexOfNext - leadingIndex);
                    var subRight = indexOfNext + 1;
                    var right = content.Substring(subRight /*skip the operator*/, endingIndex - indexOfNext);

                    var leftDecimal = Decimal.Parse(left);
                    var rightDecimal = Decimal.Parse(right);
                    var result = handler.Value(leftDecimal, rightDecimal);
                    System.Console.WriteLine("---------------------------");
                    System.Console.WriteLine("Step Pre : {0}", content);
                    System.Console.WriteLine("Step: {0}{1}{2} = {3}", leftDecimal, handler.Key, rightDecimal, result);
                    var buff = (endingIndex - leadingIndex) + 1;
                    content = content.Remove(leadingIndex, buff);
                    content = content.Insert(leadingIndex, result.ToString());
                    System.Console.WriteLine("Step Post: {0}", content);
                } while (indexOfNext != -1);
            }

            return content;
        }

        public bool HandleInput(string args)
        {
            var content = args;
            content = content.Replace(" ", "");
            System.Console.WriteLine("Calc: {0}", content);

            try
            {
                content = ResolveBreackets(content);
            }
            catch (Exception)
            {
                System.Console.WriteLine("ERROR -> Not able to read the expression see output!");
                return false;
            }

            System.Console.WriteLine("Result of '{0}' = '{1}'", args, content);

            return true;
        }

        private string ResolveBreackets(string content)
        {
            int realFirst = -1;
            var realLast = content.Length;
            do
            {
                var first = content.IndexOf('(', realFirst + 1);
                if (first != -1)
                {
                    realFirst = first;
                }
                else
                {
                    break;
                }
            } while (realFirst != -1);

            if (realFirst != -1)
            {
                realLast = content.IndexOf(')', realFirst);
                var reange = realLast - realFirst - 1;
                var newContent = content.Substring(realFirst + 1, reange);
                System.Console.WriteLine("<---------------------------");
                System.Console.WriteLine("Sub Step Pre: {0}", newContent);
                var resolved = ResolveBreackets(newContent);
                System.Console.WriteLine("----------------------------");
                System.Console.WriteLine("Sub Step Post: {0}", resolved);
                System.Console.WriteLine("--------------------------->");
                content = content.Remove(realFirst, realLast - realFirst + 1);
                content = content.Insert(realFirst, resolved);
                content = ResolveBreackets(content);
            }
            return ResolvePart(content);
        }

        public string HelpText
        {
            get { return "Simple Calculator"; }
        }
    }
}
