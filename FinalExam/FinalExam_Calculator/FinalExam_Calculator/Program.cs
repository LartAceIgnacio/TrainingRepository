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
            string num1, num2, operation;
            int oper1, oper2, result;
            Console.WriteLine("Enter first number: ");
            num1 = Console.ReadLine();
            oper1 = Int32.Parse(num1);
            Console.WriteLine("Enter operator: ");
            operation = Console.ReadLine();
            Console.WriteLine("Enter second number: ");
            num2 = Console.ReadLine();
            oper2 = Int32.Parse(num2);

            
            if(operation == "+")
            {
                result = oper1 + oper2;
                Console.WriteLine("The result of " + num1 + operation + num2 + " is " + result);
                Console.ReadKey();
            }
            else if(operation == "-")
            {
                result = oper1 - oper2;
                Console.WriteLine("The result of " + num1 + operation + num2 + " is " + result);
                Console.ReadKey();
            }
            else if (operation == "/")
            {
                result = oper1 / oper2;
                Console.WriteLine("The result of " + num1 + operation + num2 + " is " + result);
                Console.ReadKey();
            }
            else
            {
                result = oper1 * oper2;
                Console.WriteLine("The result of " + num1 + operation + num2 + " is " + result);
                Console.ReadKey();
            }
        }
    }
}
