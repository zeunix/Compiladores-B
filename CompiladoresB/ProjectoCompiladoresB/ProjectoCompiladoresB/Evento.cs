using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectoCompiladoresB
{
    class Evento
    {
        public string disparador;
        public string funcion;
        public int pc;
        public Evento(string d,string f)
        {
            disparador = d;
            funcion = f;
            pc = 0;
        }
    }
}
