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
            MathProxy proxy = new MathProxy();

            Console.Write("Type your first number: ");
            var firstInput = Console.ReadLine();
            int x = int.Parse(firstInput);

            Console.Write("Type your operator: ");
            var operatorInput = Console.ReadLine();

            Console.Write("Type your operator: ");
            var secondInput = Console.ReadLine();
            int y = int.Parse(secondInput);

          
            switch (operatorInput)
            {
                case "+":
                    Console.Write("+");
                    Console.WriteLine("The result of " + x + " " + operatorInput + " " + y + " = " + proxy.Add(x, y));
                    break;
                case "-":
                    Console.Write("-");
                    Console.WriteLine("The result of " + x + " " + operatorInput + " " + y + " = " + proxy.Sub(x, y));
                    break;
                case "*":
                    Console.Write("*");
                    Console.WriteLine("The result of " + x + " " + operatorInput + " " + y + " = " + proxy.Mul(x, y));
                    break;
                case "/":
                    Console.Write("/");
                    Console.WriteLine("The result of " + x + " " + operatorInput + " " + y + " = " + proxy.Div(x, y));
                    break;
                default:
                    Console.Write("Error");
                    Console.ReadLine();
                    break;
            }

            Console.ReadKey();
        }
    public interface IMath
    {
        double Add(double x, double y);
        double Sub(double x, double y);
        double Mul(double x, double y);
        double Div(double x, double y);
    }

    class Math : IMath
    {
        public double Add(double x, double y) { return x + y; }
        public double Sub(double x, double y) { return x - y; }
        public double Mul(double x, double y) { return x * y; }
        public double Div(double x, double y) { return x / y; }
    }

    class MathProxy : IMath
    {
        private Math _math = new Math();

        public double Add(double x, double y)
        {
            return _math.Add(x, y);
        }
        public double Sub(double x, double y)
        {
            return _math.Sub(x, y);
        }
        public double Mul(double x, double y)
        {
            return _math.Mul(x, y);
        }
        public double Div(double x, double y)
        {
            return _math.Div(x, y);
        }
    }
}
}
