using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Fibonacci
{
    class Recursion
    { 
        public int Fibbonaci(int num)
        {
            int i = 0;
            int total = 0;
            
            do
            { 
                Console.WriteLine(i);
                i++;

            } while (i<=num);

            return total;
        }
    }
}
