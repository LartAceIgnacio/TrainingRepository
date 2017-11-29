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
            #region List
            //List<int> resultList = new List<int>
            //{
            //    0,
            //    1
            //};
            #endregion
            Console.WriteLine("Please Enter a number : ");
            var input = int.Parse(Console.ReadLine()) -3; // user input
            #region Fibo
            //for (int i = 0; i < input -2 ; i++)
            //{
            //    resultList.Add(resultList[i] + resultList.Last());
            //}

            #endregion

            Fibo fb = new Fibo(input); // instantiate FIbo
            var resultList = fb.GetFibo(); // get Fibo List

            foreach (var item in resultList) // Print Fibo List
            {
                Console.Write(item + " ");
            }
            
            Console.ReadKey();
        }
    }
}
