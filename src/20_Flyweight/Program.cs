﻿using System;

namespace Flyweight
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("please enter a number !");
                Console.ReadLine();
                System.Environment.Exit(0);

            }
            BigString bigString = new BigString(args[0]);

            bigString.print();
        }
    }
}
