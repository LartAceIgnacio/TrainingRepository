using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Calculator
{
    class Calculator
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type your first number:");
            var FirstNum = Console.ReadLine();
            Console.WriteLine("Type your operator: ");
            var myOperator = Console.ReadLine();
            Console.WriteLine("Type your second number: ");
            var SecondNum = Console.ReadLine();
            Console.ReadKey();
        }
    }
    public interface ICalculator
    {
        string Operator();
    }
    class Calculate : ICalculator
    {
        public int First;
        public int Second;
        public int ICalculator.Operator;
        {

        }
    }
}
