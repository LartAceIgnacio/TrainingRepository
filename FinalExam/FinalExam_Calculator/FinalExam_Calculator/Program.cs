using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type your first number");
            var fn = int.Parse(Console.ReadLine());
            Console.WriteLine("Type your operator");
            var op = Console.ReadLine();
            Console.WriteLine("Type your second number");
            var sn = int.Parse(Console.ReadLine());

            ICalculator calc = new Calculator(fn, op,sn);
            calc.Calculate();

            Console.ReadKey();
        }
    }
}
