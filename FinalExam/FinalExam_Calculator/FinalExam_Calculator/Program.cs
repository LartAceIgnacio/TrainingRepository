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
            float firstNum, secNum;
            string operation;

            Console.WriteLine("Type your first number: ");
            var input = Console.ReadLine();
            firstNum = float.Parse(input);

            Console.WriteLine("Type your operator: ");
            operation = Console.ReadLine();

            Console.WriteLine("Type your second number: ");
            input = Console.ReadLine();
            secNum = float.Parse(input);

            if (operation == "-")
            {
                Subtract sub = new Subtract();
                sub.operation(firstNum, secNum);
            }
            else if (operation == "+")
            {
                Add add = new Add();
                add.operation(firstNum, secNum);
            }
            else if (operation == "/")
            {
                Divide div = new Divide();
                div.operation(firstNum, secNum);
            }
            else if (operation == "*")
            {
                Multiply mul = new Multiply();
                mul.operation(firstNum, secNum);
            }

            Console.ReadKey();
        }
    }

    interface calculator
    {
        void operation(float firstNum, float secNum);
    }

    class Add : calculator
    {
        public void operation(float firstNum, float secNum)
        {
            float ans = firstNum + secNum;
            Console.WriteLine(firstNum + " + " + secNum);
            Console.WriteLine("The result is: " + ans);
        }
    }

    class Subtract : calculator
    {
        public void operation(float firstNum, float secNum)
        {
            float ans = firstNum - secNum;
            Console.WriteLine(firstNum + " - " + secNum);
            Console.WriteLine("The result is: " + ans);
        }
    }

    class Multiply : calculator
    {
        public void operation(float firstNum, float secNum)
        {
            float ans = firstNum * secNum;
            Console.WriteLine(firstNum + " * " + secNum);
            Console.WriteLine("The result is: " + ans);
        }
    }

    class Divide : calculator
    {
        public void operation(float firstNum, float secNum)
        {
            float ans = firstNum / secNum;
            Console.WriteLine(firstNum + " / " + secNum);
            Console.WriteLine("The result is: " + ans);
        }
    }
}
