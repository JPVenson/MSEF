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
        public void OnStart(IApplicationContext application)
        {

        }

        public string ID
        {
            get { return "Math"; }
        }

        public bool HandleInput(string[] args)
        {
            var handlers = new Dictionary<char, Func<decimal, decimal, decimal>>();
            handlers.Add('*', (e, f) => e * f);
            handlers.Add('/', (e, f) => e / f);
            handlers.Add('+', (e, f) => e + f);
            handlers.Add('-', (e, f) => e - f);

            var content = args.FirstOrDefault();

            if (content == null)
                return false;

            content = content.Replace(" ", "");

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
                    char currChar = content[leadingIndex--];
                    do
                    {
                        currChar = content[leadingIndex--];
                    } while (char.IsNumber(currChar) && currChar != '.');

                    do
                    {
                        currChar = content[endingIndex++];
                    } while (char.IsNumber(currChar) && currChar != '.');

                    // found start and end so calculate now
                    //@ = leadingIndex
                    //$ = endingIndex
                    // @300|*200$

                    var left = content.Substring(leadingIndex, indexOfNext);
                    var right = content.Substring(indexOfNext + 1 /*skip the operator*/, endingIndex);

                    var leftDecimal = Decimal.Parse(left);
                    var rightDecimal = Decimal.Parse(right);
                    var result = handler.Value(leftDecimal, rightDecimal);

                    content = content.Remove(leadingIndex, endingIndex).Insert(leadingIndex, result.ToString());
                } while (indexOfNext != -1);
            }

            System.Console.WriteLine(content);

            return true;

        }
    }
}
