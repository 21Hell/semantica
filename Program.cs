/*
    Melendez Chavez Ivan
*/
using System;
using System.IO;
using System.Collections.Generic;

namespace semantica
{
    public class Program
    {
        static void Main(string[] args)
        {

            try
            {
                crearInstancia();
                GC.Collect();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void crearInstancia()
        {
            Lenguaje a = new Lenguaje();
            a.Programa();
        }
    }
}