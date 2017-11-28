using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Matencio
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a number: ");
            int num1 = int.Parse(Console.ReadLine());

            for (int numcount = 0; numcount < num1; numcount++)
            {
                Console.Write(FinalExam_Fibonacci.Fibonacci(numcount) + " ");
            }

            Console.ReadKey();

        }
    }
}
