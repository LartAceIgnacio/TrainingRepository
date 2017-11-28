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
            Console.WriteLine();
            
            Console.WriteLine(Fib(5));

            Console.ReadKey();
        }


        static int Fib( int n)
        {
            if (n == 0)
            {
                return 1;
            }
            else if (n == 1)
            {
                return 1;
            }
            else
            {
                return Fib(n - 2) + Fib(n - 1);
            }
        }


    }
}
