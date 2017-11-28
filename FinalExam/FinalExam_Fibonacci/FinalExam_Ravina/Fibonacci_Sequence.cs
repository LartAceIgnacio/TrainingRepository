using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Ravina
{
    class Fibonacci_Sequence
    {
      
        public int fibonacci(int num)
        {
            int result;
            if (true)
            {
                return num * num;
                
            }
            else
            {
                result = fibonacci(num * 2)* num;
                return result;
                
            }
        }
    }
}
