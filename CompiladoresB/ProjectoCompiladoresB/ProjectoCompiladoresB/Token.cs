using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectoCompiladoresB
{
    class Token
    {
        private string nombre;
        private string val;
        private int tipo;
        public string valAsignado;
        public string Nombre { get { return nombre; } set { nombre = value; } }
        public string Val { get => val; set => val = value; }
        public int Tipo { get => tipo; set => tipo = value; }

        public Token ()
        {

        }
        public Token(string n)
        {
            nombre = n;
        }
        public Token(string n,int t)
        {
            switch(t)
            {
                case 0:
                    tipo = 0;//int
                    nombre = "num";
                    break;                
                case 1:
                    tipo = 1;//string
                    nombre = "id";
                    break;
                case 2:
                    tipo = 2;
                    nombre = n;
                break;
            }
            Val = n;
        }       
    }
}
