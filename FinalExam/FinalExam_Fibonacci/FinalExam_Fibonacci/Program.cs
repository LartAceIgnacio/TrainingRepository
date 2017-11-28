using System;

namespace FinalExam_Fibonacci
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("INPUT: ");
            int num = int.Parse(Console.ReadLine());
            int x = 0, y = 1;

            if(num > 2)
            {
                Console.Write("\n" + x + " ");
                Console.Write(y + " ");
                fibonacci(num - 2, x, y);
            }
            else Console.WriteLine("Invalid input");

            Console.ReadKey();
        }

        static void fibonacci(int num, int x, int y)
        {
            int res = x + y;
            if (num > 0)
            {
                Console.Write(res + " ");
                fibonacci(num-1, y, res);
            }
        }
    }
}