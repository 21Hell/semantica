/*
    Melendez Chavez Ivan
*/
using System;
using System.IO;

namespace semantica
{
    public class Error : Exception
    {
        public Error(string mensaje, StreamWriter log) : base(mensaje)
        {
            log.WriteLine(mensaje);
        }
    }
}