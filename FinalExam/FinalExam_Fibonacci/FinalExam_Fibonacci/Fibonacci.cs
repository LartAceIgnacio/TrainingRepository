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

            Console.WriteLine("Please enter a number:");
            var userNum = Console.ReadLine();
            Fibonacci fibo = new Fibonacci();
            fibo.fib(int.Parse(userNum));
            Console.ReadKey();
        }
    }
    public class Fibonacci
    {
        public int fib(int num)
        {
            int a = 0;
            int b = 1;

            for(int i = 0; i < num; i++)
            {
                Console.Write(a);
                int x = a;
                a = b;
                b = x + b;
                
                Console.Write(" ");
            }
            return a;
        }
    }
}
