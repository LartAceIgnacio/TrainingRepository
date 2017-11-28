using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Calculator
{
    class Calc : Operator
    {
        public int first;
        public int second;
        public string MyOperator;

        public void Method()
        {
            Console.WriteLine("the result of");
            Console.WriteLine(first);
            Console.WriteLine(MyOperator);
            Console.WriteLine(second);

            if (MyOperator == "+")
            {
                Console.WriteLine("is {0}", first + second);
            }
            else if (MyOperator == "-")
            {
                Console.WriteLine("is {0}", first - second);
            }
            else if (MyOperator == "*")
            {
                Console.WriteLine("is {0}", first * second);
            }
            else if (MyOperator == "/")
            {
                Console.WriteLine("is {0}", first / second);
            }
        }
    }
}
