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
            Calculator cal = new Calculator();
            Console.WriteLine("Type your First Number");
            cal.first_number = int.Parse(Console.ReadLine());
            Console.WriteLine("Type your Operator:");
            cal.@operator = char.Parse(Console.ReadLine());
            Console.WriteLine("Type your Second Number");
            cal.second_number = int.Parse(Console.ReadLine());
            cal.operate();
            cal.calculate();
            Console.ReadKey();
        }
    }

    interface Icalculator
    {
            void calculate();
            int operate();
        
    }

    class Calculator : Icalculator
    {
        public int first_number { get; set; }
        public int second_number { get; set; }
        public char @operator { get; set; }

        public int result;

        public int operate()
        {
            int result = 0;
            if(@operator == '+')
            {
                result = this.first_number + this.second_number;
            }
            else if(@operator == '-')
            {
                result = this.first_number - this.second_number;
            }
            else if(@operator == '*')
            {
                result = this.first_number * this.second_number;

            }else if(@operator == '/')
            {
                result = this.first_number / second_number;
            }
            this.result = result;
            return result;
        }

        public void calculate()
        {
            Console.WriteLine("The result of");
            Console.WriteLine(first_number);
            Console.WriteLine(@operator);
            Console.WriteLine(second_number);
            Console.WriteLine("is "+ result);

        }
    }
    
}


