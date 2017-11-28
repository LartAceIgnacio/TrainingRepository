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
            double fnum;
            double snum;
            string opr;

            Console.WriteLine("Type your first number: ");
            fnum = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Type your operator: ");
            opr = Console.ReadLine();
            Console.WriteLine("Type your second number: ");
            snum = Convert.ToDouble(Console.ReadLine());


            var calc = new Calculator(fnum, snum, opr);
            

            switch (opr)
            {
                case "+":
                    calc.AddCalculate();
                    break;
                case "-":
                    calc.SubtractCalculate();
                    break;
                case "*":
                    calc.MultiplyCalculate();
                    break;
                case "/":
                    calc.DivideCalculate();
                    break;
                default:
                    break;
            }

            Console.ReadKey();
            
        }
    }


    class Calculator : ICalculator
    {
        public double fnum { get; set; }
        public double snum { get; set; }

        public string operand { get; set; }

        public Calculator(double num1, double num2, string opr)
        {
            fnum = num1;
            snum = num2;
            operand = opr;
        }

        public void AddCalculate()
        {
            Console.WriteLine("The result of {0} and {1} is {2}", fnum, snum, (fnum + snum));
        }

        public void DivideCalculate()
        {
            if (snum  == 0 )
            {
                Console.WriteLine("Cannot divided by zero.");
            }
            else
            {
                Console.WriteLine("The result of {0} and {1} is {2}", fnum, snum, (fnum / snum));
            }
        }

        public void MultiplyCalculate()
        {
            Console.WriteLine("The result of {0} and {1} is {2}", fnum, snum, (fnum * snum));
        }

        public void SubtractCalculate()
        {
            Console.WriteLine("The result of {0} and {1} is {2}", fnum, snum, (fnum - snum));
        }
    }


    interface ICalculator
    {
        void AddCalculate();
        void SubtractCalculate();
        void DivideCalculate();
        void MultiplyCalculate();

    }
}
