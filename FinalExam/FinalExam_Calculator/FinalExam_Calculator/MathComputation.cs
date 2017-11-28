using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Calculator
{
    public class MathComputation : IMathComputation
    {
        public void ComputeOpration(double num1, string mathOperation, double num2)
        {
            Console.Write("The result of\n" + num1 + "\n" + mathOperation + "\n" + num2 + "\n");
            if (mathOperation == "+")
                Console.Write("is {0}", num1 + num2);
            else if (mathOperation == "-")
                Console.Write("is {0}", num1 - num2);
            else if (mathOperation == "*")
                Console.Write("is {0}", num1 * num2);
            else if (mathOperation == "/")
                Console.Write("is {0}", num1 / num2);
        }
    }
}
