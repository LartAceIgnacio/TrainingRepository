using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraineeExam
{
    class Shark
    {
        public string shark { get; set; }

        public void SetSound()
        {
            Console.WriteLine("Do do do do " + shark);
        }
        public virtual void SharkType()
        {
            Console.WriteLine("I am baby shark do do do do");
        }
    }
    class PapaShark : Shark
    {
        public override void SharkType()
        {
            Console.WriteLine("Papa Shark du du du du");
        }
    }
    class MamaShark : Shark
    {
        public override void SharkType()
        {
            Console.WriteLine("Mama Shark da da da da");
        }
    }
    class GrandmaShark : Shark
    {
        public override void SharkType()
        {
            Console.WriteLine("Grandma Shark de de de de");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            #region Baby Shark
            /*
            Shark[] shark = new Shark[4];
            shark[0] = new Shark();
            shark[1] = new PapaShark();
            shark[2] = new MamaShark();
            shark[3] = new GrandmaShark();

            foreach (Shark sharks in shark)
            {
                sharks.SharkType();
            }
            */
            #endregion

            #region Palindrome
            string input = "", reverse = "";
            Console.WriteLine("Input: ");
            input = Console.ReadLine();
            for (int x = input.Length - 1; x >= 0; x--)
            {
                reverse += input[x].ToString();
            }
            if (reverse == input)
            {
                Console.WriteLine(input+ " is Palindrome! " +reverse);
            }
            else
            {
                Console.WriteLine(input + " not Palindrome! " + reverse);
            }
            #endregion
        }
    }
}
