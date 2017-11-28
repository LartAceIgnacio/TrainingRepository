using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Ravina
{
    class Program
    {
        static void Main(string[] args)
        {
            //Recursive Method
         

            Console.Write("Please Enter a number: ");
            var userInput = Console.ReadLine();

            
            for (int i = 1; i <= int.Parse(userInput); i++)
            {
                Fibonacci_Sequence n = new Fibonacci_Sequence();
                n.fibonacci(i);
                Console.Write("{0} ", n.fibonacci(6));
               
            }


            Console.ReadKey();
        }
       
    }
}
