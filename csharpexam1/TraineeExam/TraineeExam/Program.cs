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
            #region Baby Shark 1
            //Shark[] myShark = new Shark[5];
            //myShark[0] = new BabyShark();
            //myShark[1] = new MommmyShark();
            //myShark[2] = new DaddyShark();
            //myShark[3] = new GrandMaShark();
            //myShark[4] = new GrandPaShark();

            //foreach (var sound in myShark)
            //{
            //    sound.Sound();
            //}
            //Console.ReadKey();
            #endregion


            #region Palindrome
            string[] palindrome = new string[26] {
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
            "Palindrome",
            ""};




            foreach (var item in palindrome)
            {
                if(item.Length != 0)
                {
                    
                    //Console.WriteLine(item.ToLower());
                    string newWord = item.ToLower();
                    //Console.WriteLine(item.Length);
                    var newItem = new char[item.Length];
                    int noItem = 0;
                    for (int i = item.Length - 1; i >= 0; i--)
                    {
                        //Console.WriteLine(item[noItem]);
                        newItem[noItem] = newWord[i];
                        noItem++;                     
                    }
                    string newName = "";
                    foreach (var name in newItem)
                    {
                        newName += name;
                    }
                    //Console.WriteLine(newName);
                    if(newWord == newName)
                    {
                        //Console.WriteLine(item +" True");
                        Console.WriteLine("{0} is same as {1} then TRUE",item,newName);
                    }
                    else
                    {
                        Console.WriteLine("{0} is NOT same as {1} then FALSE", item, newName);
                        //Console.WriteLine(item +" false");
                    }
                }
                else
                {
                    Console.WriteLine("not a word: false");
                }
                
                
            }
            Console.ReadKey();
            #endregion

        }
    }
    #region Baby Shark Class
    abstract class Shark
    {
        public abstract void Sound();

    }
    class BabyShark : Shark
    {
        public override void Sound()
        {
            Console.WriteLine("Baby Shark doo doo doo");
        }
    }
    class MommmyShark : Shark
    {
        public override void Sound()
        {
            Console.WriteLine("Mommy Shark doo doo doo");
        }
    }
    class DaddyShark : Shark
    {
        public override void Sound()
        {
            Console.WriteLine("Daddy Shark doo doo doo");
        }
    }
    class GrandMaShark : Shark
    {
        public override void Sound()
        {
            Console.WriteLine("Grand Ma Shark doo doo doo");
        }
    }
    class GrandPaShark : Shark
    {
        public override void Sound()
        {
            Console.WriteLine("Grand Pa Shark doo doo doo");
        }
    }
    #endregion 
}
