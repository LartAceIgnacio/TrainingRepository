using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1
{
    class Program
    {
        static void Main(string[] args)
        {
            var shark1 = new BabyShark1();
            var shark2 = new BabyShark2();

            Console.Write("Shark1: ");
            shark1.Sound();
            Console.Write("Shark1: ");
            shark2.Sound();

            Console.ReadKey();
        }
    }

    abstract class Shark
    {
        public abstract void Sound();
    }

    class BabyShark1 : Shark
    {
        public override void Sound()
        {
            Console.WriteLine("Lalalalal.");
        }
    }

    class BabyShark2 : Shark
    {
        public override void Sound()
        {
            Console.WriteLine("awhoooooo.");
        }
    }

}
