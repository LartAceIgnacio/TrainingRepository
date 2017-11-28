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
            List<int> resultList = new List<int>();
            resultList.Add(0);
            resultList.Add(1);
            Console.WriteLine("Please Enter a number : ");
            var input = int.Parse(Console.ReadLine());

            for (int i = 0; i < input -2 ; i++)
            {
                resultList.Add(resultList[i] + resultList.Last());
            }

            foreach (var item in resultList)
            {
                Console.Write(item + " ");
            }
            
            Console.ReadKey();
        }
    }
}
