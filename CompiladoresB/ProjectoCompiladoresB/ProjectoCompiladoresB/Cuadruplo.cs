using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectoCompiladoresB
{
    class Cuadruplo
    {
        public string operador;
        public string operador1;
        public string operador2;
        public string res;
        public int indx;
        public Cuadruplo(string op,string op1,string op2,string r)
        {
            operador = op;
            operador1 = op1;
            operador2 = op2;
            res = r;
        }
        public Cuadruplo() { }

    }
}
