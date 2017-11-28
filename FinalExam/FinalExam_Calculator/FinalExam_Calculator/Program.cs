using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Calc myCalc = new Calc();

            Console.WriteLine("Type your first number:");
            var first = Console.ReadLine();
            
            myCalc.first = int.Parse(first);

            Console.WriteLine("Type your Operator:");
            var mySign = Console.ReadLine();
            myCalc.MyOperator = mySign;

            Console.WriteLine("Type your second number:");
            var second = Console.ReadLine();
            myCalc.second = int.Parse(second);

            myCalc.Method();
            

            Console.ReadKey();
        }
    }
    
    
    
}
