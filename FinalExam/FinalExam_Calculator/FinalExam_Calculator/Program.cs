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
            Console.WriteLine("Type your first number :");
            double num1 = double.Parse(Console.ReadLine());
            Console.WriteLine("Operator :");
            string mathOperation = Console.ReadLine();
            Console.WriteLine("Type your second number :");
            double num2 = double.Parse(Console.ReadLine());
            IMathComputation mc = new MathComputation();
            mc.ComputeOpration(num1, mathOperation, num2);
            Console.ReadKey();
        }
    }
}
