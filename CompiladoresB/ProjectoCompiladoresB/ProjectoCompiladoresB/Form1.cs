using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace ProjectoCompiladoresB
{
    public partial class Principal : Form
    {
        Dictionary<int, Dictionary<string, string>> TablaAnalisisSintactico;
        List<string> gramatica;
        List<string> gramaticaSinOr;
        List<char> terminales;
        List<string> productores;
        string palabraRev;
        string codigo;
        List<string> palabraR;
        Stack pilaId = new Stack();
        Stack pilaNum = new Stack();
        Stack pilaArbol = new Stack();

        public Principal()
        {
            InitializeComponent();
        }
        private void Principal_Load(object sender, EventArgs e)
        {
            //palabras reservadas 
            palabraRev = "concatenar|$|,|:=|:|;|=|{|}|<|>|(|)|if|endif|switch|endswitch|int|char|string|float|Var|for" +
              "|endfor|while|endwhile|step|initWindow|closeWindow|case|default|break|endswitch";
            palabraR = new List<string>(palabraRev.Split('|'));
            palabraR.RemoveAll(x => x == "");
        }
        # region cargarTabla
        /***************************************************************
         * Carga un archivo de texto, y despues lee linea por linea
         * siguiendo un formato para determinar la gramatica original
         * ,la gramatica convertida, los terminales, productores 
         * y finalmente los estados y sus acciones
         * ***********************************************************/
        private void cargarTabla_Click(object sender, EventArgs e)
        {
            bool nuevo = false;
            openFileDialog1.Title = " ";
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = Application.StartupPath + "\\PrimerosCadenaLr1TablaAnalisisSintacticoPílade Llamadas\\Primeros\\bin";
            TextReader archivo = null;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                archivo = new StreamReader(path);
                nuevo = true;
            }
            if (nuevo)
            {
                int numProducciones = Convert.ToInt32((archivo.ReadLine()));
                gramatica = new List<string>();

                for (int i = 0; i < numProducciones; i++)
                {
                    gramatica.Add(archivo.ReadLine());
                }
                gramaticaSinOr = new List<string>();
                numProducciones = Convert.ToInt32((archivo.ReadLine()));
                for (int i = 0; i < numProducciones; i++)
                {
                    gramaticaSinOr.Add(archivo.ReadLine());
                }
                List<string> sterminales = new List<string>(archivo.ReadLine().Split(' '));
                sterminales.Add(archivo.ReadLine());
                sterminales.RemoveAll(x => x == "");
                productores = new List<string>(archivo.ReadLine().Split(' '));
                productores.Remove("");

                List<string> coord = new List<string>();
                terminales = new List<char>();
                foreach (string s in sterminales)
                    terminales.Add(s[0]);

                coord.InsertRange(0, sterminales);
                coord.InsertRange(coord.Count, productores);

                string aux = "";
                int numEdo = 0;
                TablaAnalisisSintactico = new Dictionary<int, Dictionary<string, string>>();
                do
                {
                    aux = archivo.ReadLine();
                    if (aux != null)
                    {
                        List<string> estado = new List<string>(aux.Split(' '));
                        estado.RemoveAll(x => x == "");

                        TablaAnalisisSintactico.Add(numEdo, new Dictionary<string, string>());

                        for (int i = 0; i < coord.Count; i++)
                        {
                            if (estado[i] != "ʼ")
                            {
                                TablaAnalisisSintactico[numEdo].Add(coord[i], estado[i]);
                            }
                        }
                        numEdo++;
                    }
                } while (aux != null);

                LlenarTablaAS(gramatica, terminales, productores, TablaAnalisisSintactico);
            }
            archivo.Close();
            List<string> metacaracteres = new List<string>();
            metacaracteres.Add(""+(char)607+"");//.
            metacaracteres.Add(""+(char)610+"");//<
            metacaracteres.Add("" + (char)611 + "");//>
            metacaracteres.Add(""+(char)612+"");//|
            metacaracteres.Add(""+(char)613+"");//~
            metacaracteres.Add(""+(char)614+"");//\
            //metacaracteres.Add("\\e");//(char)615espacio
            foreach (string s in metacaracteres)
            {
                int index = 0;
                if (gramaticaSinOr.Exists(x => x.Contains(s)))
                {
                    string auxS = gramaticaSinOr.Find(x => x.Contains(s));
                    index = gramaticaSinOr[gramaticaSinOr.FindIndex(x=>x.Contains(s))].IndexOf(s[0]);
                    string terminal="";
                    switch (s[0])
                    {
                        case (char)607:// .
                            terminal = ".";
                            break;
                        case (char)610:// \<
                            terminal = "\\<";
                            break;
                        case (char)611: // \>
                            terminal = "\\>";
                            break;
                        case (char)612: // ||
                            terminal = "|";
                            break;
                        case (char)613:// \~
                            terminal = "~";
                            break;
                        case (char)614: // \\
                            terminal = "\\";
                            break;                            
                    }
                    auxS = auxS.Remove(index);
                    auxS = auxS.Insert(index, terminal);
                    gramaticaSinOr[gramaticaSinOr.FindIndex(x => x.Contains(s))] = auxS;
                }
            }
        }
        #endregion
        #region dibujarTabla
        /*******************************************************************
         * Pinta el listView donde se imprime la 
         * tabla de analisis sintactico
         * ****************************************************************/
        public void LlenarTablaAS(List<string> gramOriginal,List<char> simbTerminales,List<string> simbProductor,Dictionary<int,Dictionary<string,string>> tablaAnS)
        {
            int index = 1;
           
            foreach (char t in simbTerminales)
            {
                tablaAS.Columns.Insert(index, t.ToString(), 100);
                index++;                     
            }
         
            foreach (string st in simbProductor)
            {
                tablaAS.Columns.Insert(index, st, 100);
                index++;
            }

            index = 1;
            
            foreach (int s in tablaAnS.Keys)
            {
                ListViewItem item = new ListViewItem(s.ToString());
        
                for (; index<tablaAS.Columns.Count; index++)
                {
                    if (tablaAnS[s].ContainsKey(tablaAS.Columns[index].Text))
                    {
                        item.SubItems.Add(tablaAnS[s][tablaAS.Columns[index].Text]);                        
                    }
                    else
                    {
                        item.SubItems.Add(" ", Color.Black, Color.White, DefaultFont);                        
                    }
                }

                tablaAS.Items.Add(item);            
                index = 1;
            }
            
        }
        #endregion
        #region evaluarCodigo
        /*************************************************************************
         * Funcion que evalua un codigo de entrada y apila los tokens introducidos
         * segun sea su tipo 
         * ************************************************************************/
        public bool evaluarExp(Dictionary<int,Dictionary<string,string>> tablaAnalisis)
        {
            string w = codigo;//cadena que cambiara y actualizara el codigo al evaluar un token 
            codigo += ('$'); //se agrega el terminador a la cadena de entrada 
            bool error = false;//bandera para terminar el ciclo por si ocurre un error 
            
            Dictionary<string, string> aux = tablaAnalisis[0];
            Stack pila = new Stack();//pila de estados 
            pila.Push(0);
            int s, t;
            int tamañoRemove = 0;
            string accion = "";
            StringBuilder stack = new StringBuilder("$0");

            while (codigo != null && !error && accion!="acc")
            {
                Token token = getNextToken();//regresa el siguiente token a evaluar y modifica el la cadena de entrada 
                w = codigo;
                if (token != null)
                {
                    foreach (char a in token.Nombre)
                    {
                        s = (int)pila.Peek();
                        Dictionary<string, string> auxD = tablaAnalisis[s];
                        if (auxD.ContainsKey(a.ToString()))
                        {
                            accion = auxD[a.ToString()];

                            do
                            {
                                if (accion != "acc")
                                {
                                    switch (accion[0])
                                    {
                                        case 'd':
                                            w = w.Remove(0, 1);

                                            stack.Append(a);
                                            tamañoRemove = 1;

                                            string[] nu = accion.Split('d');
                                            t = Convert.ToInt32(nu[1]);
                                            tamañoRemove += nu[1].Length;
                                            stack.Append(t);
                                            pila.Push((int)t);
                                            ListViewItem item = new ListViewItem(stack.ToString());
                                            item.SubItems.Add(a.ToString());
                                            item.SubItems.Add(w.ToString());
                                            item.SubItems.Add(accion);
                                            tablaAcciones.Items.Add(item);
                                            break;
                                        case 'r':
                                            
                                            int num = 0;
                                            string llave = "";
                                            string st = "";
                                            Tuple<int, string,string> num_llave_prod = getNum_Llave_prod(accion);
                                            num = num_llave_prod.Item1;
                                            llave = num_llave_prod.Item2;
                                            st = num_llave_prod.Item3;
                                                
                                            for (int i = 0; i < num; i++)
                                            {
                                                pila.Pop();
                                            }
                                            stack.Remove(stack.Length - tamañoRemove, tamañoRemove);
                                            tamañoRemove = llave.Length;
                                            ListViewItem item1 = new ListViewItem(stack.ToString());
                                            item1.SubItems.Add(a.ToString());
                                            item1.SubItems.Add(w.ToString());
                                            item1.SubItems.Add(accion + "= " + st);
                                            tablaAcciones.Items.Add(item1);

                                            if (pila.Count == 0)
                                                t = 0;
                                            else
                                                t = (int)pila.Peek();
                                            Dictionary<string, string> ir_a = tablaAnalisis[t];
                                            if (ir_a.ContainsKey(llave))
                                            {
                                                int edo = Convert.ToInt32(ir_a[llave]);
                                                pila.Push(edo);
                                                stack.Append(llave);
                                                stack.Append(edo);
                                                ListViewItem item2 = new ListViewItem(stack.ToString());
                                                item2.SubItems.Add(a.ToString());
                                                item2.SubItems.Add(w.ToString());
                                                item2.SubItems.Add(edo.ToString());
                                                tablaAcciones.Items.Add(item2);

                                                s = (int)pila.Peek();
                                                tamañoRemove += ir_a[llave].Length;
                                                Dictionary<string, string> auxD1 = tablaAnalisis[s];
                                                if (auxD1.ContainsKey(a.ToString()))
                                                {
                                                    accion = auxD1[a.ToString()];
                                                    if (accion[0] == 'd')
                                                    {
                                                        w=w.Remove(0, 1);
                                                        stack.Append(a);

                                                        tamañoRemove = 1;
                                                        string[] nv = accion.Split('d');
                                                        t = Convert.ToInt32(nv[1]);
                                                        tamañoRemove += nv[1].Length;
                                                        pila.Push(t);
                                                        stack.Append(t);
                                                        ListViewItem item3 = new ListViewItem(stack.ToString());
                                                        item3.SubItems.Add(a.ToString());
                                                        item3.SubItems.Add(w.ToString());
                                                        item3.SubItems.Add(accion);
                                                        tablaAcciones.Items.Add(item3);
                                                        codigo = w;

                                                    }
                                                }
                                                else
                                                {
                                                    error = true; break;
                                                }
                                            }
                                            else
                                            {
                                                error = true; break;
                                            }
                                            break;
                                    }
                                    break;
                                }
                                else
                                {
                                    error = false;
                                }
                            } while (accion[0] == 'r' && !error);
                        }

                        else { error = true; break; }
                        if (error)
                            break;

                        if (accion == "acc")
                        {
                            ListViewItem item2 = new ListViewItem(stack.ToString());
                            item2.SubItems.Add(a.ToString());
                            item2.SubItems.Add(w.ToString());
                            item2.SubItems.Add(accion);
                            tablaAcciones.Items.Add(item2);
                            break;
                        }
                    }
                    codigo = w;
                }
                else
                {
                    error = true;
                    break;
                }
            }
            if (!error && accion == "acc")
            {
                return true;
            }
            else
                return false;
        }

        private Tuple<int, string,string> getNum_Llave_prod(string accion)
        {
            int num = 0;
            string llave = "";
            string s = "";
            foreach (string st in gramaticaSinOr)
            {
                string[] n = accion.Split('r');
                int nm = Convert.ToInt32(n[1]);
                if (gramaticaSinOr.IndexOf(st) + 1 == nm)
                {                    
                    int m = 0;
                    for (; m < st.Length; m++)
                    {
                        if (!(st[m] == '-' && st[m + 1] == '>'))
                        {
                            llave += st[m];
                        }
                        else
                            break;
                    }
                    m += 2;

                    num = 0;
                    bool y = false;
                    //numero de elementos a reducir (num)
                    //cuando encuentra un metacaracter no lo toma en cuenta y aumenta el contador 
                    for (; m < st.Length; m++)
                    {
                        if (st[m] == '<' && !y)
                        {
                            num++;
                            y = true;
                        }
                        else if (st[m] == '\\')
                        {
                            m++;
                            num++;
                        }
                        else if (st[m] == '>')
                        {
                            y = false;
                        }

                        else if (terminales.Exists(x => x == st[m]) && !y)
                            num++;
                    }
                    s = st;
                    break;
                }
            }
            Tuple<int, string, string> t = new Tuple<int, string, string>(num, llave, s);
            return t;
        }
        
        #endregion
        /********************************************************
         * boton que compilara el codigo introducido en el cuadro
         * texto, separara los espacios y los saltos de linea
         * *****************************************************/
        private void CompilarBoton_Click(object sender, EventArgs e)
        {
            tablaAcciones.Items.Clear();
            //expresion regular que separa espacios y saltos de linea y acomoda una cadena dejando los espacios en ella
            string patterTexto = @"\r|\n|' '|\u0022.#\u0022$";
          
            List<string> codL = new List<string>(Regex.Split(cadenaCodigo.Text,patterTexto));
            codL.RemoveAll(x => x == "");
            codigo = "";
           
            foreach (string s in codL)
                codigo += s;

            if (evaluarExp(TablaAnalisisSintactico))
                MessageBox.Show("Codigo Correcto");
            else
                MessageBox.Show("Codigo Incorrecto");
        }
        /****************************************************************
         * Funcion que regresa un token, con sus atributos completos
         * y analizados, retornara un null, si el token no existe o 
         * tiene una sintaxis erronea
         * **************************************************************/
        private Token getNextToken()
        {
            //u0022(comilla doble)           
            Token token=new Token();
            int index;
            //Expresion regular que separa por palabras las coincidencias y mantiene en su posicion las que no
            string reservadas = @"(if$|\$|\(|\)|=|:=|;|,|\u0022.#\u0022$|concatenar$|endif$|endswitch|endwhile|endfor|\{|\}|else|" +
                @"until|repeat|Var|int$|float|string|char|switch|for|while|" +
                @"step|initWindow|closeWindow|==|>|<|\+|-|^|\*|/|case|:|default|break|id|n$)";
           
            //lista de tokens aun no tokenizados
            List<string> tokenPrevios = new List<string>(Regex.Split(codigo,reservadas));
            tokenPrevios.RemoveAll(x => x == "");
            string posibleVar = tokenPrevios.Find(x => x.IndexOf(x) == 0);//variable candidata

            Match match = Regex.Match(tokenPrevios.Find(x => x.IndexOf(x) == 0), reservadas);          
           
            if (match.Value !="")
            {
                if (palabraR.Exists(x => x == match.Value))//tokens de palabras reservadas 
                {
                    token = new Token(match.Value, 2);
                    index = match.Index;
                    codigo = codigo.Remove(index, match.Value.Length);
                    codigo = codigo.Insert(index, token.Val);
                    return token;
                }
                else
                    return null;                
            }
            else if(tokenPrevios.Count!=0)//tokens que pueden ser id o nuemros
            {
                if (posibleVar.All(c => char.IsLetter(c)) || posibleVar.Contains('"') ||
                    (posibleVar.Any(c => char.IsLetter(c)) && 
                     posibleVar.Any(c => char.IsNumber(c))))
                {
                    token = new Token(posibleVar, 1);
                }
                else if (posibleVar.All(c => char.IsNumber(c)))
                {
                    token = new Token(posibleVar, 0);
                }

                index = match.Index;
                codigo = codigo.Remove(index, posibleVar.Length);
                codigo = codigo.Insert(index, token.Nombre);

                return token;
            }
            else
            {
                return null;
            }
        }     
    }
}
