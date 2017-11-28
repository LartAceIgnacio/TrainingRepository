using System;

namespace FinalExam_Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("First number: ");
            float num1 = float.Parse(Console.ReadLine());
            Console.Write("Operator: ");
            string operand = Console.ReadLine();
            Console.Write("Second number: ");
            float num2 = float.Parse(Console.ReadLine());
            float result = 0;

            switch (operand)
            {
                case "+":
                    Calculator sum = new Addition(num1,num2);
                    result = sum.solve();
                    break;
                case "-":
                    Calculator diff = new Subtraction(num1, num2);
                    result = diff.solve();
                    break;
                case "*":
                    Calculator prod = new Multiplication(num1, num2);
                    result = prod.solve();
                    break;
                case "/":
                    Calculator div = new Division(num1, num2);
                    result = div.solve();
                    break;
                default:
                    break;
            }
            Console.WriteLine("\nThe result of \n {0} \n {1} \n {2} \n is {3}",num1,operand,num2,result);
            Console.ReadKey();
        }
    }

    interface Calculator
    {
        float solve();
    }

    class Addition : Calculator
    {
        float num1, num2;
        public Addition(float num1, float num2)
        {
            this.num1 = num1;
            this.num2 = num2;
        }
        float Calculator.solve()
        {
            return num1 + num2;
        }
    }

    class Subtraction : Calculator
    {
        float num1, num2;
        public Subtraction(float num1, float num2)
        {
            this.num1 = num1;
            this.num2 = num2;
        }
        float Calculator.solve()
        {
            return num1 - num2;
        }
    }

    class Multiplication : Calculator
    {
        float num1, num2;
        public Multiplication(float num1, float num2)
        {
            this.num1 = num1;
            this.num2 = num2;
        }
        float Calculator.solve()
        {
            return num1 * num2;
        }
    }

    class Division : Calculator
    {
        float num1, num2;
        public Division(float num1, float num2)
        {
            this.num1 = num1;
            this.num2 = num2;
        }
        float Calculator.solve()
        {
            return num1 / num2;
        }
    }
}