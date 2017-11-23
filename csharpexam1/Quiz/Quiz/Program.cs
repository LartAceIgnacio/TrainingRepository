using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    class Program
    {
        abstract class Shark
        {
            public abstract void Sound();
        }

        class MommyShark : Shark
        {
            public override void Sound()
            {
                Console.WriteLine("I'm Mommy shark dadada");
            }
        }

        class BabyShark : Shark
        {
            public override void Sound()
            {
                Console.WriteLine("I'm Baby shark dededede");
            }
        }

        class DaddyShark : Shark
        {
            public override void Sound()
            {
                Console.WriteLine("I'm Daddy shark didididi");
            }
        }

        class GrandmaShark : Shark
        {
            public override void Sound()
            {
                Console.WriteLine("I'm grandma shark dududududu");
            }
        }

        class GrandpaShark : Shark
        {
            public override void Sound()
            {
                Console.WriteLine("I'm Grandpa shark dododododo");
            }
        }


        static void Main(string[] args)
        {
            #region shark
            Shark[] shark = new Shark[5];
            shark[0] = new BabyShark();
            shark[1] = new MommyShark();
            shark[2] = new DaddyShark();
            shark[3] = new GrandmaShark();
            shark[4] = new GrandpaShark();

            foreach (var item in shark)
            {
                item.Sound();
            }
            #endregion

            #region palindrome
            List<string> palindrome = new List<string>();
            palindrome.Add("renz");
            palindrome.Add("murdrum");

            palindrome.Add("deified");
            palindrome.Add("deleveled");
            palindrome.Add("devoved");
            palindrome.Add("dewed");
            palindrome.Add("Hannah");
            palindrome.Add("kayak");
            palindrome.Add("level");
            palindrome.Add("madam");
            palindrome.Add("racecar");
            palindrome.Add("radar");
            palindrome.Add("redder");
            palindrome.Add("refer");
            palindrome.Add("repaper");
            palindrome.Add("reviver");
            palindrome.Add("rotator");
            palindrome.Add("rotor");
            palindrome.Add("sagas");
            palindrome.Add("solos");
            palindrome.Add("sexes");
            palindrome.Add("stats");
            palindrome.Add("tenet");
            palindrome.Add("Dot");
            palindrome.Add("A");
            palindrome.Add("Palindrome");
            palindrome.Add("");
            List<string> reverse = new List<string>();
            foreach (var item in palindrome)
            {
                string rev = "";
                int i = 1;
                while (i <= item.Length)
                {
                    rev = rev.Insert(i - 1, item.Substring(item.Length - i, 1));
                    if (rev.Length == item.Length)
                    {
                        // Console.WriteLine(rev);
                        reverse.Add(rev);
                    }
                    i++;
                }



            }

            for (int i = 1; i <= reverse.Count; i++)
            {
                if ((palindrome[i - 1].ToLower() == reverse[i - 1].ToLower()))
                {

                    Console.WriteLine("{0} / {1} = true", palindrome[i - 1], reverse[i - 1]);
                }
                else
                {
                    Console.WriteLine("{0} / {1} = false", palindrome[i - 1], reverse[i - 1]);
                }
            }     

            #endregion
            Console.ReadKey();
        } 
    }
}
