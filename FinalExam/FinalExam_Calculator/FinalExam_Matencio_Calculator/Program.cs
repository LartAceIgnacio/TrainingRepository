using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Matencio_Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
          
            //num1
            Console.Write("Type your first number: ");
            double num1 = double.Parse(Console.ReadLine());
            //operator
            Console.Write("Type your operator: ");
            string oper = Console.ReadLine();

            //num2
            Console.Write("Enter your second number: ");
            double num2 = double.Parse(Console.ReadLine());

            InterfaceImplementer inter = new InterfaceImplementer();


            switch (oper)
            {
                case "+" :
                    Console.WriteLine("The result of {0} {1} {2} is {3}", num1, oper, num2, inter.Add(num1, num2));
                    break;
                case "-":
                    Console.WriteLine("The result of {0} {1} {2} is {3}", num1, oper, num2, inter.Subtract(num1, num2));
                    break;
                case "*":
                    Console.WriteLine("The result of {0} {1} {2} is {3}", num1, oper, num2, inter.Multiply(num1, num2));
                    break;
                case "/":
                    Console.WriteLine("The result of {0} {1} {2} is {3}", num1, oper, num2, inter.Divide(num1, num2));
                    break;
                default:
                    Console.WriteLine("Invalid operator");
                    break;
            }

            Console.ReadKey();
        }
    }
    interface IMyInterface
    {
        double Add(double n1, double n2);
        double Subtract(double n1, double n2);
        double Multiply(double n1, double n2);
        double Divide(double n1, double n2);

    }
    class InterfaceImplementer : IMyInterface
    {
        public double Add(double n1, double n2)
        {
            double ans = n1 + n2;
            return ans;
            
        }
        public double Subtract(double n1, double n2)
        {
            double ans = n1 - n2;
            return ans;
        }
        public double Multiply(double n1, double n2)
        {
            double ans = n1 * n2;
            return ans;
        }
        public double Divide(double n1, double n2)
        {
            double ans = n1 / n2;
            return ans;
        }

    }
}
