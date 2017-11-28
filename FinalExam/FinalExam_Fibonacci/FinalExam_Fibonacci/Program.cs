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
            Console.WriteLine("Please Enter a number:");
            var data = Console.ReadLine();
            Program n = new Program();
            for(int i= 0; i < int.Parse(data); i++)
            {
                Console.WriteLine("{0}", n.fibonacci(0, 1));
            }
  
            Console.ReadKey();
        }
        public int fibonacci(int num, int num1)
            {
            int a = 0;
            int b = 1;
            int sum = a + b;
                return sum;
                
            }
    }
}
