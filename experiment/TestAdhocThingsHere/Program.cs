﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestAdhocThingsHere
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test");

            var test = string.Format(@"""something {0}""", "strange");

            Console.WriteLine(test);

            var input = Console.ReadLine();



        }
       
    }
}
