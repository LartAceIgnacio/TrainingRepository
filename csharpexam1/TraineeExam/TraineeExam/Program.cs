using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraineeExam
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region BabyShark
            Console.WriteLine("BabyShark:");
            BabySharkCute babySharkCute = new BabySharkCute();
            MommyShark mommyShark = new MommyShark();
            DaddyShark daddyShark = new DaddyShark();
            GrandMaShark grandMaShark = new GrandMaShark();
            GrandFaShark grandPaShark = new GrandFaShark();
            babySharkCute.GetNameAndSound();
            mommyShark.GetNameAndSound();
            daddyShark.GetNameAndSound();
            grandMaShark.GetNameAndSound();
            grandPaShark.GetNameAndSound();
            #endregion

            #region Palindrome
            Console.WriteLine("\n\nPalindrome:");
            List<string> Palindrome = new List<string>();
            Palindrome.Add("civic");
            Palindrome.Add("deified");
            Palindrome.Add("deleveled");
            Palindrome.Add("devoved");
            Palindrome.Add("dewed");
            Palindrome.Add("Hannah");
            Palindrome.Add("kayak");
            Palindrome.Add("level");
            Palindrome.Add("madam");
            Palindrome.Add("racecar");
            Palindrome.Add("radar");
            Palindrome.Add("redder");
            Palindrome.Add("refer");
            Palindrome.Add("repaper");
            Palindrome.Add("reviver");
            Palindrome.Add("rotator");
            Palindrome.Add("rotor");
            Palindrome.Add("sagas");
            Palindrome.Add("solos");
            Palindrome.Add("sexes");
            Palindrome.Add("stats");
            Palindrome.Add("tenet");
            Palindrome.Add("Dot");
            Palindrome.Add("A");
            Palindrome.Add("Palindrome");
            Palindrome.Add("");

            foreach (var item in Palindrome) {
                //var reversed = new string(item.Reverse().ToArray());
                //Console.WriteLine(reversed == item ? "True: " + item : "False: " + item);

                //int checker = item.Length / 2, ctr = 0;
                //for (int i = 0, c = item.Length - 1; i < item.Length && i < c; i++, c--) {
                //    if (item[i] == item[c])
                //        ctr++;
                //    else {
                //        Console.WriteLine("False: " + item);
                //        break;
                //    }
                //}

                //if(ctr == checker)
                //    Console.WriteLine("True: " + item);

                List<char> charItem = item.Reverse().ToList();

                string reverseItem = "";
                for (int i = 0; i < charItem.Count; i++) {
                    reverseItem += charItem[i];
                }

                Console.WriteLine(reverseItem == item ? "True: " + item : "False: " + item);
            }

            Console.ReadKey();
            #endregion
        }
    }

    public class BabySharkFamily
    {
        public string SharkName { get; set; }
        public string SharkSound { get; set; }

        public virtual void MyNameAndSound()
        {
            Console.WriteLine("My Name is: " + SharkName + " and my sound is: " + SharkSound);
        }
    }

    public class BabySharkCute : BabySharkFamily
    {
        public void GetNameAndSound()
        {
            BabySharkCute babyShark = new BabySharkCute { SharkName = "Baby Shark", SharkSound = "bobobobobo" };
            babyShark.MyNameAndSound();
        }
    }

    public class MommyShark : BabySharkFamily
    {
        public void GetNameAndSound()
        {
            MommyShark babyShark = new MommyShark { SharkName = "Mommy Shark", SharkSound = "momomomomo" };
            babyShark.MyNameAndSound();
        }
    }

    public class DaddyShark : BabySharkFamily
    {
        public void GetNameAndSound()
        {
            DaddyShark babyShark = new DaddyShark { SharkName = "Daddy Shark", SharkSound = "dododododo" };
            babyShark.MyNameAndSound();
        }
    }

    public class GrandMaShark : BabySharkFamily
    {
        public void GetNameAndSound()
        {
            GrandMaShark babyShark = new GrandMaShark { SharkName = "Grand Ma Shark", SharkSound = "gmogmogmogmo" };
            babyShark.MyNameAndSound();
        }
    }

    public class GrandFaShark : BabySharkFamily
    {
        public void GetNameAndSound()
        {
            GrandFaShark babyShark = new GrandFaShark { SharkName = "Grand Fa Shark", SharkSound = "gfogfogfogfo" };
            babyShark.MyNameAndSound();
        }
    }
}
