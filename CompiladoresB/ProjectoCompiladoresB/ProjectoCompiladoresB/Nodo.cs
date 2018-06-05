using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectoCompiladoresB
{
    class Nodo
    {
        public Nodo izq;
        public Nodo der;
        public Token token;
        public bool visitado;
        public Nodo()
        {

        }
        public Nodo(Token t)
        {
            token = t;
            izq = null;
            der = null;
            visitado = false;
        }
        public void setIzq(Nodo n)
        {
            izq = n;
        }
        public void setDer(Nodo n)
        {
            der = n;
        }
    }
}
