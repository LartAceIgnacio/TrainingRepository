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
            #region shark
            //Shark[] shark = new Shark[5];
            //shark[0] = new Baby();
            //shark[1] = new Mommy();
            //shark[2] = new Daddy();
            //shark[3] = new Grandma();
            //shark[4] = new Grandpa();

            //foreach (Shark sharktype in shark)
            //{
            //    sharktype.Sound();
            //}
            #endregion

            #region Palindrome
            //List<string> list = new List<string>();
            //list.Add("christmas");
            //list.Add("murdrum");
            //list.Add("relever");
            //list.Add("race car");
            //list.Add("asp.net");
            //list.Add("alula");
            //list.Add("anna");
            //list.Add("avid diva");
            //list.Add("deleveled");


            //foreach (var palindromes in list)
            //    {
            //        Console.WriteLine(palindromes);
            //    }
            //string ls = list();
            //while (list)
            //{
            //    Console.WriteLine();

            //}

            #endregion

           
            List<string> list = new List<string>();
                list.Add("civic");
                list.Add("deified");
                list.Add("relever");
                list.Add("deleveled");
                list.Add("devoved");
                list.Add("Hannah");
                list.Add("kayak");
                list.Add("level");
                list.Add("madam");       
                list.Add("racecar");
                list.Add("radar");
                list.Add("redder");
                list.Add("refer");
                list.Add("repaper");
                list.Add("reviver");
                list.Add("rotator");
                list.Add("rotor");
                list.Add("sagas");
                list.Add("rotator");
                list.Add("solos");
                list.Add("sexes");
                list.Add("stats");
                list.Add("tenet");
                list.Add("Dot");
                list.Add("A");
                list.Add("Palindrome");
          
            foreach (var palindromes in list)
            {
                string s;
                string rev = "";

                s = palindromes.ToString();

                for (int i = s.Length - 1; i >= 0; i--) 
                {
                    rev += s[i].ToString();
                }

                if (rev == s) 
                {
                    Console.WriteLine("{0} is true", s);
                }
                else
                {
                    Console.WriteLine("{0} is false", s);
                }
            }
            
            Console.ReadKey();
            
        }
    }

    #region shark class
    //abstract class Shark
    //{
    //    public abstract void Sound();
       
    //}
    //class Baby : Shark
    //{
    //    public override void Sound()
    //    {
    //        Console.WriteLine(" Baby Shark chuchuchuchu");
    //    }
    //}
    //class Mommy : Shark
    //{
    //    public override void Sound()
    //    {
    //        Console.WriteLine(" Mommy mumumumumu");
    //    }
    //}
    //class Daddy : Shark
    //{
    //    public override void Sound()
    //    {
    //        Console.WriteLine(" Daddy dudududududu");
    //    }
    //}
    //class Grandma : Shark
    //{
    //    public override void Sound()
    //    {
    //        Console.WriteLine(" Grandma chichichichichi");
    //    }
    //}
    //class Grandpa : Shark
    //{
    //    public override void Sound()
    //    {
    //        Console.WriteLine(" Grandpa uhuhuhuhuhuhuhuhh");
    //    }
    //}
    #endregion

    
}
