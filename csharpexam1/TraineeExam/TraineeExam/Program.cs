using System;
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

            #region BabyShark
            //BabyShark babyShark = new BabyShark();
            //babyShark.Sound();

            //MummyShark mummyShark = new MummyShark();
            //mummyShark.Sound();

            //DaddyShark daddyShark = new DaddyShark();
            //daddyShark.Sound();

            //GrandmaShark grandmaShark = new GrandmaShark();
            //grandmaShark.Sound();

            //GrandpaShark grandpaShark = new GrandpaShark();
            //grandpaShark.Sound();
            #endregion

            #region Palindrome
             Palindrome.palindrome();
            #endregion

            Console.ReadKey();
        }
    }

    #region Baby Shark
    class Shark
    {
        public virtual void Sound()
        {
            Console.WriteLine("Shark: ");
        }
    }

    class BabyShark: Shark
    {
        public override void Sound()
        {
            Console.WriteLine("Baby Shark Sound: dadada");
        }
    }

    class MummyShark : Shark
    {
        public override void Sound()
        {
            Console.WriteLine("Mummy Shark Sound: dedede");
        }
    }

    class DaddyShark : Shark
    {
        public override void Sound()
        {
            Console.WriteLine("Daddy Shark Sound:dididi ");
        }
    }

    class GrandmaShark : Shark
    {
        public override void Sound()
        {
            Console.WriteLine("Grandma Shark Sound: dododo ");
        }
    }
    class GrandpaShark : Shark
    {
        public override void Sound()
        {
            Console.WriteLine("Grandpa Shark Sound: dududu ");
        }
    }
    #endregion

    class Palindrome
    {
        
        public static void palindrome()
        {
            List<string> palist = new List<string>()
            {
                "civic",
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
            "Palindrome"
            };

            string current, reversed, letter;
            int index;

            for (int i = 0; i < palist.Count; i++)
            {
                reversed = "";
                current = palist[i];
                Console.Write(current + ": ");

                for (int counter = 0; counter < current.Length; counter++)
                {                    
                    index = current.Length-1;
                    letter = char.ToString(current[index - counter]);
                    letter = letter.ToLower();
                    reversed = reversed + letter;
                }

               
                reversed = reversed.ToLower();
                current = current.ToLower();

                if (reversed == current) {
                    Console.Write("true\n");
                }
                else
                {
                    Console.Write("false\n");
                }

                //Console.WriteLine(reversed);

            }
        }
        
    }
}
