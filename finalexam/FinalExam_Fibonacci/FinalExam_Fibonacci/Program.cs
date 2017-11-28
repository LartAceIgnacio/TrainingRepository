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

            Console.Write("Please enter a number: ");
            int num = int.Parse(Console.ReadLine());

            Recursion r = new Recursion();
            r.Fibbonaci(num);

            Console.ReadKey();
        }
    }
}
