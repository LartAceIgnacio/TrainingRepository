using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Fibonacci
{
    public class Fibo
    {
        public int Counter { get; private set;}
        public Fibo(int _counter) {
            Counter = _counter;
        }

        private List<int> resultList = new List<int> { 0, 1 }; // Fibo Return List with default 2 values

        public List<int> GetFibo() {

            resultList.Add(resultList[resultList.Count - 2] + resultList.Last()); // algo to get the right Fibo

            if (this.Counter > 0) // checker to apply fibo
            {
                this.Counter--;
                GetFibo(); // recursion
            }

            return resultList; // return value
        }

    }
}
