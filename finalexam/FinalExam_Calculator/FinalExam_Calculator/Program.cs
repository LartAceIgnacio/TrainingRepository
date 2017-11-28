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
            Console.WriteLine("Enter first number: ");
            int firstNum = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the operator: ");
            string oprtr = Console.ReadLine().ToString();

            Console.WriteLine("Enter the second number: ");
            int secondNum = int.Parse(Console.ReadLine());
            
            Calculate cal = new Calculate();
            cal.firstNum = firstNum;
            cal.secondNum = secondNum;
            cal.oprtr = oprtr;
            cal.Compute();
            Console.ReadKey();
        }
    }
}
