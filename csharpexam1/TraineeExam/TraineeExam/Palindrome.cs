using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraineeExam
{
    public class Palindrome
    {
        public void CheckPalindrome(List <string> palindromeList) {

            foreach (var item in palindromeList)
            {
                var actualWOrd = item; // actual word to be displayed
                var newString = ""; // new string for reversed checking
                var x = item.Replace(" ", string.Empty); // removing of spaces

                var result = false; // for result

                var ctr = x.Length - 1; // just my counter for reversing

                for (int i = ctr; i >= 0; i--) // loop for generating new reversed string
                {
                    newString += x[i];
                }

                if (x == newString) // new word and actual word checking
                {
                    result = true;
                }

                //Console.WriteLine(x.Length);
                Console.WriteLine("Word : {0} / Result : {1}", item, result); // output result
            }
        }
    }
}
