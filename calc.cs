using lab3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    internal class calc : numbers, Interface
    {

        public void add()
        {
            Console.WriteLine($"Your result: {num1} + {num2} = " + (num1 + num2));
        }
        public void subtract()
        {
            Console.WriteLine($"Your result: {num1} - {num2} = " + (num1 - num2));
        }
        public void divide()
        {
            Console.WriteLine($"Your result: {num1} / {num2} = " + (num1 / num2));
        }
        public void multiply()
        {
            Console.WriteLine($"Your result: {num1} * {num2} = " + (num1 * num2));
        }

        void CALC()
        {
            Console.WriteLine("Type a number, and then press Enter");
            num1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Type another number, and then press Enter");
            num2 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Choose an option from the following list:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            Console.Write("Your option? ");

            switch (Console.ReadLine())
            {
                case "a":
                    add();
                    break;
                case "s":
                    subtract();
                    break;
                case "m":
                    multiply();
                    break;
                case "d":
                    divide();
                    break;

            }

        }

        static void Main(string[] args)

        {
            calc a = new calc();
            a.CALC();
        }

    }


}
