using System;
using System.IO;

namespace semantica
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Lenguaje a = new Lenguaje();

                a.Programa();
                /* while(!a.FinArchivo())
                 {
                     a.NextToken();
                 }*/
                a.cerrar();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}