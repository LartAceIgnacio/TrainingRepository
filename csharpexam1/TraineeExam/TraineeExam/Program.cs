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
            #region Babyshark
            //BabyShark newBabyShark = new BabyShark();
            //MommyShark newMommyShark = new MommyShark();
            //DaddyShark newDaddyShark = new DaddyShark();
            //GrandmaShark newGrandmaShark = new GrandmaShark();
            //GrandpaShark newGrandpaShark = new GrandpaShark();
            //newBabyShark.sound();
            //newMommyShark.sound();
            //newDaddyShark.sound();
            //newGrandmaShark.sound();
            //newGrandpaShark.sound();

            //Console.ReadKey();
            #endregion

            #region Palindrome
            String[] Palindrome = new String[26]
            {
           "civic", "deified",
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

            foreach (var Item in Palindrome)
            {
                if (Item.Length != 0)
                {


                }

            }

            Console.ReadKey();
#endregion
        }
    }
    #region Babyshark class
    abstract class Shark
    {
        public abstract void sound();   
    }
    class BabyShark : Shark
    {
        public override void sound()
        {
            Console.WriteLine("Baby shark: da da da da");
        }
    }
    class MommyShark : Shark
    {
        public override void sound()
        {
            Console.WriteLine("Mommy shark: de de de de");
        }
    }
    class DaddyShark : Shark
    {
        public override void sound()
        {
            Console.WriteLine("Daddy shark: di di di di");
        }
    }
    class GrandmaShark : Shark
    {
        public override void sound()
        {
            Console.WriteLine("Grandma shark: do do do do");
        }
    }
    class GrandpaShark : Shark
    {
        public override void sound()
        {
            Console.WriteLine("Grandpa shark: du du du du");
        }
    }
    #endregion
}