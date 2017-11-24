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
            #region Baby Shark

            List<string> sharkName = new List<string> { "Baby", "Mommy", "Daddy", "Grandma", "Grandpa" };
            Shark sharks = new Shark();
            foreach(var data in sharkName)
            {
                sharks.getName(data);
                sharks.sound();
            }

            #endregion

            #region palindrome 
            //List<string> palindromeCollection = new List<string> {
            //"civic",
            //"deified",
            //"deleveled",
            //"devoved",
            //"dewed",
            //"Hannah",
            //"kayak",
            //"level",
            //"madam",
            //"racecar",
            //"radar",
            //"redder",
            //"refer",
            //"repaper",
            //"reviver",
            //"rotator",
            //"rotor",
            //"sagas",
            //"solos",
            //"sexes",
            //"stats",
            //"tenet",
            //"Dot",
            //"A",
            //"Palindrome",
            //"" };
            //Palindrome pal = new Palindrome();
            //foreach (var data in palindromeCollection)
            //{
            //    Console.WriteLine(data + " is palindrome? " + pal.checkPalindrome(data));
            //}
            Console.ReadKey();
            #endregion
        }
    }

    class Palindrome
    {
        public Boolean checkPalindrome(string input)
        {
            Boolean result = false;
            int length = input.Length;
            if (input.Length == 2) {
            }
            else
            {
              // do (charAt){

               
            }
            return result;
        }
    }
    
    class Sharks
    {
        public string name { get; set; }
        public virtual void SetName()
        {
            Console.WriteLine("My name is: " + name);
        }
        public void sound()
        {
            Console.WriteLine("My sound is Doo doo doo");
        }
    }

    class Animal
    { 
        public void sound()
        {
            Console.WriteLine("Doo Doo Dooo");
        }
    }

    class Shark : Animal
    {
        public void getName(string sharkName)
        {
            Console.WriteLine(sharkName);
        }
    }
}
