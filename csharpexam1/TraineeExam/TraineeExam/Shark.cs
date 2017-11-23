using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraineeExam
{
    // Base Class
    public class Shark
    {
        public virtual void MakeSound()
        {
            Console.WriteLine("doo doo doo");
        }

    }
    // Derived Class
    public class BabyShark : Shark
    {
        public override void MakeSound()
        {
            Console.WriteLine("Baby Shark doo doo doo lalalalala");
        }
    }
    public class MommyShark : Shark
    {
        public override void MakeSound()
        {
            Console.WriteLine("Mommy Shark doo doo doo abuchike");
        }
    }
    public class DaddyShark : Shark
    {
        public override void MakeSound()
        {
            Console.WriteLine("Daddy Shark doo doo doo huray");
        }
    }
    public class GrandPa : Shark
    {
        public override void MakeSound()
        {
            Console.WriteLine("GrandPa Shark doo doo bidapdap doobi dobidapdap");
        }
    }
    public class Grandma : Shark
    {
        public override void MakeSound()
        {
            Console.WriteLine("GrandMa Shark doo doo doo bi doobi doo bi doo bi doo bi doo ahh");
        }
    }
}
