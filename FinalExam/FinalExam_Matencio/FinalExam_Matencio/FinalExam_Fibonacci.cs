using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Matencio
{
    class FinalExam_Fibonacci
    {
        public static string Fibonacci(int num)
        {
            int recur = 0;
            int cnum = 0;
            int result = 1;
            int ctr;
            string output = "";
            
                for (int i = 0; i < num; i++)
                {
                    ctr = cnum;
                    cnum = result;
                    result = result + ctr;
                    Fibonacci(num -1);
                    output = result + " ";
                    recur = i;
                }

                return output;

        }
    }
}
