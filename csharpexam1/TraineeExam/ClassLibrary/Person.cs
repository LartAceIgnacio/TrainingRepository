using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Person
    {
        public string Name { get; set; }

        public void Palindrome(List<string> palindromList) {

            foreach (var item in palindromList)
            {
                var actualWOrd = item; // actual word to be displayed

                var newString = ""; // new string for reversed checking

                var x = item.Replace(" ", string.Empty); // removing of spaces

                var ctr = x.Length - 1; // just my counter for reversing

                for (int i = ctr; i >= 0; i--) // loop for generating new reversed string
                {
                    newString += x[i];
                }
                    
                var result = x == newString ? true : false; // newString and Actualword comaparison

                Console.WriteLine("Word : {0} / Result : {1}", item, result); // output result
            }

        }

    }
}
