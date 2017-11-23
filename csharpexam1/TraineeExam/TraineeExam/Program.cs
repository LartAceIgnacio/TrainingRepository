using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraineeExam
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Baby Shark
            Console.WriteLine("1. BABY SHARK");
            Dictionary<string, string> shark = new Dictionary<string, string>();
            shark.Add("nyaw nyaw", "baby shark");
            shark.Add("myaw myaw", "mommy shark");
            shark.Add("braw braw", "daddy shark");
            shark.Add("shaw shaw", "grandma shark");
            shark.Add("klaw klaw", "grandpa shark");

            List<string> sound = new List<string>();
            sound.AddRange(shark.Keys);

            List<string> name = new List<string>();
            name.AddRange(shark.Values);

            Console.WriteLine("--Sound--");
            foreach (string s in sound)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("\n--Shark--");
            foreach (string s in name)
            {
                Console.WriteLine(s);
            }
            #endregion

            #region Palindrome
            Console.WriteLine("\n-----------------------");
            Console.WriteLine("\n2. PALINDROME");
            string[] words = new string[9];
            words[0] = "christmas";
            words[1] = "murdrum";
            words[2] = "releveler";
            words[3] = "race car";
            words[4] = "asp .net";
            words[5] = "alula";
            words[6] = "anna";
            words[7] = "avid diva";
            words[8] = "deleveled";

            for (int ctr = 0; ctr < words.Length; ctr++)
            {
                string curWord = words[ctr];
                int length = curWord.Length - 1;

                char[] newWord = new char[length + 1];
                int j = 0;
                Boolean pal = true;

                for (int i = length; i >= 0; i--)
                {
                    newWord[j] = curWord[i];
                    j++;
                }

                for (int i = 0; i <= length; i++)
                {
                    if (newWord[i] == curWord[i]) //wrong condition, should be !=
                    {
                        pal = false;
                        //forgot to add break;
                    }
                }

                Console.Write(curWord);
                if (pal)
                {
                    Console.WriteLine(" - TRUE");
                }
                else Console.WriteLine(" - FALSE");

            }
            #endregion

            Console.ReadKey();
        }
    }
}
