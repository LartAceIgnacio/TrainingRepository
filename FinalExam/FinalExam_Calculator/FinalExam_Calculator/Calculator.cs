using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Calculator
{
    public class Calculator : ICalculator
    {
        public int FirstNumber { get; private set; }
        public string Operator { get; private set; }
        public int SecondNumber { get; private set; }

        public Calculator(int fn, string op, int sn)
        {
            FirstNumber = fn;
            Operator = op;
            SecondNumber = sn;
        }
        public void Calculate() {
            Console.WriteLine(this.FirstNumber);
            Console.WriteLine(this.Operator);
            Console.WriteLine(this.SecondNumber);
            if (Operator == "+")
            {
                Add();
            }
            else if (Operator == "-")
            {
                Subtract();
            }
            else if (Operator == "/")
            {
                Divide();
            }
            else if (Operator == "*")
            {
                Multiply();
            }
            else {
                Console.WriteLine("Not Allowed");
            }
        }

        public void Add()
        {
            Console.WriteLine("Is " + (this.FirstNumber + this.SecondNumber));
        }

        public void Subtract()
        {
            Console.WriteLine("Is " + (this.FirstNumber - this.SecondNumber));

        }

        public void Multiply()
        {
            Console.WriteLine("Is " + (this.FirstNumber * this.SecondNumber));

        }

        public void Divide()
        {
            Console.WriteLine("Is " + (this.FirstNumber / this.SecondNumber));

        }
    }
}
