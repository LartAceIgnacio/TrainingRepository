using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Inheritance
            Console.WriteLine("---------------Inheritance---------------");
            Volvo volvo = new Volvo() { Color = "Yellow" };
            volvo.CarType();
            volvo.SetColor();
            Console.WriteLine();

            Toyota toyota = new Toyota() { Color = "Gray" };
            toyota.CarType();
            toyota.SetColor();
            Console.WriteLine();

            Console.WriteLine();
            #endregion


            #region Polymorphism
            Console.WriteLine("---------------Polymorphism---------------");
            Car volvoCar = new Volvo() { Color = "Yello-Green" };
            volvoCar.CarType();
            volvo.SetColor();
            Console.WriteLine();

            Car toyotaCar = new Toyota() { Color = "Matte Gray" };
            toyotaCar.CarType();
            toyotaCar.SetColor();
            Console.WriteLine();

            Console.WriteLine();
            #endregion


            #region Abstraction
            Birdie birdie = new Birdie();
            birdie.Eat();
            #endregion

            Console.ReadKey();
        }

        void Palindrome() {

        }
    }

    class Volvo : Car
    {
        public override void CarType()
        {
            Console.WriteLine("I am a Volvo.");
        }

    }

    class Toyota : Car
    {
        public override void CarType()
        {
            Console.WriteLine("I am a Toyota.");
        }
    }

    class Car
    {
        public string Color { get; set; }

        public void SetColor()
        {
            Console.WriteLine("My color is {0}", Color);
        }

        public virtual void CarType()
        {
            Console.WriteLine("Im a car.");
        }
    }

    abstract class Animal
    {
        public abstract void Eat();

        public virtual void haha()
        {
            Console.WriteLine("HAHA");
        }
    }

    class Birdie : Animal
    {
        public override void Eat()
        {
            Console.WriteLine("I eat bird seeds.");
        }
    }




}
