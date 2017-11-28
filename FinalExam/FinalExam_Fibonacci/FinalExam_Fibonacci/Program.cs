using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Fibonacci
{

    class Program
    {
        public static string Fibonacci(int n)
        {
            string series = "";
            int num1, num2, final = 0;
            num1 = num2 = 1;
            if (final < 2)
            {
                return n.ToString();
            }
            else
            {
                for (int x = 0; x < n; x++)
                {
                    final = num1 + num2;
                    num2 = num1;
                    num1 = final;
                    series += final.ToString() + ",";
                }
            }

            return series;
        }
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter: ");
            //var fibo = Console.ReadLine();
            //int number = Int32.Parse(fibo);
            string result = "";
            result = Fibonacci(20);
            Console.WriteLine(result);
            Console.ReadKey();
        }

    }

    
}
