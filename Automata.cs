using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primeros
{
    public class Automata
    {

        public List<Estado> estados = new List<Estado>();
        public Dictionary<int, Dictionary<string, int>> Relaciones;
        public Dictionary<int, Dictionary<string, string>> tablaAnalisis;
        Dictionary<string, int> r;
        public List<string> simbolosGramaticales;
        public List<string> gramatica;
        public string clave = "";

        public Automata()
        {
            Relaciones = new Dictionary<int, Dictionary<string, int>>();
            tablaAnalisis = new Dictionary<int, Dictionary<string, string>>();
            simbolosGramaticales = new List<string>();
            gramatica = new List<string>();
        }
        public void creaEstado(int i)
        {
            Estado e = new Estado(i);
            estados.Add(e);
            Relaciones.Add(i, new Dictionary<string, int>());
        }
     
        public void putGram(List<string> g)
        {
            foreach (string s in g)
                gramatica.Add(s);
        }
        public void putSimbolos(List<string> s)
        {
            foreach (string i in s)
                simbolosGramaticales.Add(i);

            simbolosGramaticales.Sort();
            // simbolosGramaticales.Reverse();
           
            simbolosGramaticales.Insert(0, "$");
        }
        public void creaClaveEdo(int i,Dictionary<int,string> cl)
        {
            foreach (Estado edo in estados)
            {
                if (edo.Edo == i)
                {
                    edo.crearClave(cl);
                    break;
                }
            }
        }
        public void putElementos(List<Elemento> el, int i)
        {
            estados[i].setElementos(el);

        }
        public void putRelaciones(string arista, int nodo)
        {
            r.Add(arista, nodo);
        }
        public void enlazarRelaciones(int origen)
        {
            Relaciones[origen] = r;
        }
        public void nuevaRelacion()
        {
            r = new Dictionary<string, int>();
        }
        public void verificarEpsilon()
        {
            foreach(Estado e in estados)
            {
                foreach(Elemento elem in e.listElementos)
                {
                    if(elem.ProduccionP.Contains(".~"))
                    {
                        string[] aux = elem.ProduccionP.Split('.');
                        elem.ProduccionP = aux[0] += ".";
                    }
                }
            }
        }
    }
}
