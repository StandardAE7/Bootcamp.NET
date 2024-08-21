using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Helpers
    {
        public static int GetChoice(List<string> options)
        {
            try
            {
                Console.WriteLine();
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine($"{i+1}. {options[i]}");
                }
                Console.WriteLine();
                Console.WriteLine("Pick an option:");
                var choice = int.Parse(Console.ReadLine() ?? "-1");

                if (choice < 1 || choice > options.Count)
                    return -1;

                return choice;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
    }
}
