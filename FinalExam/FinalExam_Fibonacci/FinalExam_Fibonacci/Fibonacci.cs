using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Fibonacci
{
    public class Fibonacci
    {
        public void Fib(int num)
        {
            int x = 0;
            int y = 1;
            int add = 0;
            for (int i = 0; i < num; i++)
            {

                Console.Write(x + " ");
                add = x + y;
                x = y;
                y = add;

            }

        }

    }


}
