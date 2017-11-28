using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Calculator
{
    public class Calculate : Result
    {
        public int firstNum { get; set; }
        public int secondNum { get; set; }
        public string oprtr { get; set; }
        public int total { get; set; }
        public void Compute()
        {
            if (oprtr == "+")
            {
                Addition();
            }
            else if (oprtr == "-")
            {
                Subtraction();
            }
            else if (oprtr == "*")
            {
                Multiplication();
            }
            else if (oprtr == "/")
            {
                Division();
            }
            else
            {
                Console.WriteLine("You have enter a wrong operator");
            }

        }

        public void Addition()
        {
            total = firstNum + secondNum;
            Console.WriteLine("The result of \n {0} \n {1}\n {2}\n is {3}", firstNum, oprtr, secondNum, total);
        }
        public void Subtraction()
        {
            total = firstNum - secondNum;
            Console.WriteLine("The result of \n {0} \n {1}\n {2}\n is {3}", firstNum, oprtr, secondNum, total);
        }

        public void Multiplication()
        {
            total = firstNum * secondNum;
            Console.WriteLine("The result of \n {0} \n {1}\n {2}\n is {3}", firstNum, oprtr, secondNum, total);
        }

        public void Division()
        {
            total = firstNum / secondNum;
            Console.WriteLine("The result of \n {0} \n {1}\n {2}\n is {3}", firstNum, oprtr, secondNum, total);
        }
    }
}
