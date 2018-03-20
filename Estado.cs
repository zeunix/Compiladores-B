using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primeros
{
    public  class Estado
    {
        private int estado;
        public int Edo { get { return estado; } set { estado = value; } }
        public List<char> relaciones;
        public List<Elemento> listElementos;
        public string clave = "";
        public Estado()
        {
            listElementos = new List<Elemento>();
        }
        public Estado(int edo)
        {
            estado = edo;
            listElementos = new List<Elemento>();
        }
        public void crearClave(Dictionary<int, string> claves)
        {
            foreach (Elemento el in listElementos)
            {
                foreach(var key in claves)
                {
                    if (claves[key.Key] == el.ProduccionP)
                    {
                        clave += claves[key.Key]+el.Caracter;
                    }
                }
                
            }
        }
        public void setElementos(List<Elemento> l)
        {
            foreach (Elemento i in l)
            {
                listElementos.Add(i);
            }
        }
    }
}
