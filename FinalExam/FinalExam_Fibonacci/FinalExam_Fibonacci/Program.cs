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
            int intFib;

            Console.Write("Please enter a number: ");
            var input = Console.ReadLine();
            intFib = int.Parse(input);

            Fibonacci(intFib);

            Console.ReadKey();
        }

        static int Fibonacci(int intInput)
        {
            int num = 0, firstNum;
            Console.Write(num + " ");

            if (intInput > 0 )
            {
                num = Fibonacci(intInput-1);
            }

            return 0;
        }
    }
}
