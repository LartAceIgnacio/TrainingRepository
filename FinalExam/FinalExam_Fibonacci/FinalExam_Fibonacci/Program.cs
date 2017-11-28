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
            Fibonacci myfib = new Fibonacci();

            
            Console.WriteLine("Pleas enter a number");
            var input = Console.ReadLine();
            myfib.Fib(int.Parse(input));
            Console.ReadKey();
        }
    }
    

}
