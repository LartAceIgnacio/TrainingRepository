using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Fibonacci
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter a number");
            int number = int.Parse(Console.ReadLine());
            if(number > 0)
                FibonacciMethod(0, 1, number - 1);
            else
                Console.WriteLine(number);
            Console.ReadKey();
        }

        public static void FibonacciMethod(int num1, int num2, int ctr)
        {
            int result = num1;
            Console.Write(result + " ");
            result = num1 + num2;
            num1 = num2;
            num2 = result;
            if (ctr > 0) {
                ctr--;
                FibonacciMethod(num1, num2, ctr);
            }
        }
    }
}
