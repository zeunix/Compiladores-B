using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primeros
{
    public class Elemento
    {
        private string producionPoint;
        private string produccion;
        private char caracter;
        public string ProduccionP { get { return producionPoint; } set { producionPoint = value; } }
        public string Produccion { get { return produccion; } set { produccion = value; } }
        public char Caracter { get { return caracter; } set { caracter = value; } }

        public Elemento(string p, string pp, char car)
        {
            produccion = p;
            producionPoint = pp;
            caracter = car;
        }
        public Elemento() { }
    }
}
