using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Exercise
{
    class Program
    {
        static void Main(string[] args)
        {

            Palindrome();

            Console.ReadKey();
        }


        static void Palindrome()
        {
            List<string> list = new List<string>
            {
                "deified",
                "deleveled",
                "devoved",
                "dewed",
                "Hannah",
                "kayak",
                "level",
                "madam",
                "racecar",
                "radar",
                "redder",
                "refer",
                "repaper",
                "reviver",
                "rotator",
                "rotor",
                "sagas",
                "solos",
                "sexes",
                "stats",
                "tenet",
                "Dot",
                "A",
                "Palindrome",
                ""
            };

            foreach (var item in list)
            {
                
                var reversed = "";
                for (int i = item.Length - 1; i >= 0; i--)
                {
                    reversed += item[i];
                }
                Console.WriteLine("{0} {1}", item, item.Replace(" ", "").ToLower().Equals(reversed.Replace(" ", "").ToLower()));
            }
        }
        
            //static string RemoveWhiteSpace(this string input)
            //{
            //    return Regex.Replace(input, @"\s+", "").ToString();
            //}

    }
}
