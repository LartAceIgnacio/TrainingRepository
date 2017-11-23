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

            Console.WriteLine("==========================================");
            Console.WriteLine("Activity 1");

            Shark[] sharks = new Shark[5]; // instantiate Shark class 
            sharks[0] = new BabyShark();
            sharks[1] = new MommyShark();
            sharks[2] = new DaddyShark();
            sharks[3] = new Grandma();
            sharks[4] = new GrandPa();

            foreach (var shark in sharks) // loop on sharks array to call each shark.Makesound
            {
                shark.MakeSound();
            }


            Console.ReadKey();


            Console.WriteLine("==========================================");
            Console.WriteLine("Activity 2");

            List<string> ListOfPalindrome = new List<string>(); // Palindrome list
            ListOfPalindrome.Add("civic");
            ListOfPalindrome.Add("deified");
            ListOfPalindrome.Add("deleveled");
            ListOfPalindrome.Add("devoved");
            ListOfPalindrome.Add("dewed");
            ListOfPalindrome.Add("Hannah");
            ListOfPalindrome.Add("kayak");
            ListOfPalindrome.Add("level");
            ListOfPalindrome.Add("madam");
            ListOfPalindrome.Add("racecar");
            ListOfPalindrome.Add("radar");
            ListOfPalindrome.Add("redder");
            ListOfPalindrome.Add("refer");
            ListOfPalindrome.Add("repaper");
            ListOfPalindrome.Add("reviver");
            ListOfPalindrome.Add("rotator");
            ListOfPalindrome.Add("rotor");
            ListOfPalindrome.Add("sagas");
            ListOfPalindrome.Add("solos");
            ListOfPalindrome.Add("sexes");
            ListOfPalindrome.Add("stats");
            ListOfPalindrome.Add("tenet");
            ListOfPalindrome.Add("Dot");
            ListOfPalindrome.Add("A");
            ListOfPalindrome.Add("Palindrome");
            ListOfPalindrome.Add("Race caR");
            ListOfPalindrome.Add("RaCe CaR");
            ListOfPalindrome.Add("rAce caR");

            Palindrome pl = new Palindrome(); // instantiate palindrome class
            pl.CheckPalindrome(ListOfPalindrome); // call checkPalindrome Method

            Console.ReadKey();

        }
    }
}
