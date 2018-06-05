
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
using System.Text.RegularExpressions;
using System.Collections;
using System.Runtime.InteropServices;
namespace Primeros
{
    public partial class Form1 : Form
    {
        List<string> metacaracteres = new List<string>();
        List<string> simGram = new List<string>();
        StringBuilder grama = new StringBuilder();
        List<string> gramOriginal;
        // >   <    |          ~    \ 
        string pattern = @"(\\>|\\<|\\\u007C|\\\u007E|\\\\|";
        string pattern2 = @"(\\>|\\<|\\\u007C|\\\u007E|\\\\)";
        bool epsilon = false;
        int nodo;
        int estado = 0;//estado actual
        int clave = 0;

        List<string> sinOr = new List<string>();
        List<string> simbProductor = new List<string>();
        List<char> simbTerminales = new List<char>();

        Dictionary<int, string> clavesE = new Dictionary<int, string>();
        Dictionary<string, List<char>> primeros = new Dictionary<string, List<char>>();
        Dictionary<char, List<char>> siguientes = new Dictionary<char, List<char>>();
        Dictionary<string, List<string>> prodcucciones = new Dictionary<string, List<string>>();
        Dictionary<int, Dictionary<string, string>> tableAnalisis;

        Automata automata;
        public Form1()
        {
            InitializeComponent();
        }
        /*******************************************************
	        Area del boton para cargar un archivo de .txt, se
	        selecciona y se guarda su ruta en "path", se leera 
	        el archivo por medio del TextReader y se mostrara en 
	        el textArea de la ventana principal
	    ********************************************************/
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = " ";
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = Application.StartupPath+ "\\Pruebas";
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                TextReader text = new StreamReader(path);
                string[] g = text.ReadToEnd().Split('\r','\n');

                GramaticaTextBox.Clear();

                foreach (var s in g)
                {
                    if(s!="")
                        GramaticaTextBox.AppendText(s+'\n');
                    
                }
                text.Close();
                button1.Text = "Archivo Cargado";
                button2.Enabled = true;
              
             //   labe1.Text = "Comprobar Gramatica";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button1.Enabled = true;
            metacaracteres.Add("\\<");//(char)610
            metacaracteres.Add("\\>");//(char)611
            metacaracteres.Add("\\|");//(char)612
            metacaracteres.Add("\\~");//(char)613
            metacaracteres.Add("\\\\");//(char)614
            metacaracteres.Add("\\e");//(char)615espacio
        }
        /*******************************************************
	        Area del elemento TextArea de la ventana principal
	    ********************************************************/
        private void button2_Click(object sender, EventArgs e)
        {
            if (GramaticaTextBox.Text != "")
            {
                string[] texto = GramaticaTextBox.Text.Split('\n', '\r');
                gramOriginal = new List<string>(texto);
                gramOriginal.RemoveAll(x => x == "");

                foreach(string st in gramOriginal)
                {
                    if (st.Contains("->") && st[0]!='-' && st.Length>0)
                        continue;
                    else
                    {
                        MessageBox.Show("Gramatica no compatible");
                        break;
                    }
                }

               /* List<string> l = new List<string>();
                l.Add("fgf");
                l.Add("ereq-plpi");
                string p = "<fgf>ht<ereq-plpi>";
                if (l.Exists(x=>p.Contains(x)))
                {

                    Match productor = Regex.Match(p,"<"+l.Find(x=>p.Contains(x))+">");
                }*/
                

                simbProductor.Clear();
                simbTerminales.Clear();
                sinOr.Clear();
                prodcucciones = new Dictionary<string, List<string>>();
                if (texto != null)
                {
                    cargaText(gramOriginal);
                }


                //simGram es una lista que guarda todos los 
                //tokens d la gramatica que encuentre, removiendo el epsilon "~"
                foreach (string simbolo in simbProductor)
                    simGram.Add(simbolo);

                foreach (char term in simbTerminales)
                    simGram.Add(term.ToString());

                simGram.Remove("~");
            }
        }
        private void cargarPattern()
        {
            pattern = @"(\\>|\\<|\\\u007C|\\\u007E|\\\\|";
            foreach (string s in simbProductor)
            {
                if (simbProductor.IndexOf(s) == simbProductor.Count - 1)
                {
                    pattern += s + "->|<" + s + ">)";
                }
                else
                {
                    pattern += s + "->|<" + s + ">|";
                }
            }
        }
        private List<String> sustituirMetacaracteres(List<string> gramatica)
        {
            StringBuilder si = new StringBuilder();
            List<string> sO = new List<string>();
            foreach(string s in gramatica)
            {
              
                if (metacaracteres.Exists(x => s.Contains(x)))
                {
                    si = new StringBuilder();
                    si.Append(s);
                    do
                    {
                       
                        int index = si.ToString().IndexOf('\\');

                        switch (si[index + 1])
                        {
                            case '<':
                                si.Remove(index, 2);
                                si.Insert(index, (char)610);
                                break;
                            case '>':                                
                                si.Remove(index, 2);
                                si.Insert(index, (char)611);
                                break;
                            case '|':                                
                                si.Remove(index, 2);
                                si.Insert(index, (char)612);
                                break;
                            case '~':                                
                                si.Remove(index, 2);
                                si.Insert(index, (char)613);
                                break;
                            case '\\':                                
                                si.Remove(index, 2);
                                si.Insert(index, (char)614);
                                break;
                            case 'e':
                                si.Remove(index, 2);
                                si.Insert(index, (char)615);
                                break;
                        }
                        
                    } while (metacaracteres.Exists(x => si.ToString().Contains(x)));
                    sO.Add(si.ToString());
                }
                else
                {
                    sO.Add(s);
                }
                
            }
            return sO;
        }
        /********************************************************
	    Comprueba que la gramatica Introducida sea del tipo 
	    "Libre de Contexto"
	    ********************************************************/
        public void cargaText(List<string> text)
        {

            simbProductor = llenarProductores(text);
            sinOr = quitaOr(text);
            sinOr = sustituirMetacaracteres(sinOr);
            cargarPattern();
            simbTerminales = llenarterminales(sinOr);
            
            foreach(string p in simbProductor)
            {
                prodcucciones.Add(p, new List<string>());
            }

            
            foreach (string s in sinOr)
            {
                string aux = "";
                string p = "";
                int i = 0;
                for(;i<s.Length;i++)
                {
                    if(s[i]=='-' && s[i+1]=='>')
                    {
                        i += 2;
                        break;
                    }
                    p += s[i];
                }
                for(;i<s.Length;i++)
                {
                    aux += s[i];
                }
                prodcucciones[p].Add(aux);
            }
            
            NTerminalesBox.Items.Clear();
            TerminalesBox.Items.Clear();

            foreach (string p in simbProductor)
            {
                NTerminalesBox.Items.Add(p);
            }

            foreach (char t in simbTerminales)
            {
                if (t == (char)606)
                {
                    TerminalesBox.Items.Add("\\~");
                }
                else
                    TerminalesBox.Items.Add(t.ToString());
            }         
           	
        }
        /*******************************************************
		Encuentra todos los productores de la gramatica 
		y los separa en una lista diferente
	    ********************************************************/
        public List<string> llenarProductores(List<string> pr)
        {
            List<string> prod = new List<string>();

            foreach (string s in pr)
            {
                string[] cadAux = Regex.Split(s, pattern2);
                string producciones="";

                for (int i = 0; i < cadAux[0].Length; i++)
                {
                    if (cadAux[0][i] == '-' && cadAux[0][i + 1] == '>')
                    {
                        break;
                    }
                    else
                    {
                        producciones += cadAux[0][i];
                    }
                        
                }
                    if (!prod.Exists(x => x == producciones))
                        prod.Add(producciones);
                

            }
            if (prod.Contains(""))
                prod.Remove("");

            return prod;
        }
        /********************************************************
         Encuentra todos los terminales de la gramatica 
         y los separa en una lista diferente 
        ********************************************************/
        public List<char> llenarterminales(List<string> g)
        {            
            List<char> terminales = new List<char>();
            List<string> cd = new List<string>();
            int i;
            int j;

            foreach (string s in g)
            {
               
                string[] r = Regex.Split(s, pattern);
                for (i = 0; i < r.Length; i++)
                {
                    if (r[i] != "")
                    {
                        if (!simbProductor.Exists(x => r[i].Contains(x)))
                        {
                            for (j = 0; j < r[i].Length; j++)
                            {
                                if (r[i][j] == '\\')
                                {
                                    j++;
                                }
                                if (!terminales.Exists(x => x == r[i][j]))
                                {
                                    terminales.Add(r[i][j]);
                                }
                            }
                        }
                    }
                }
            

                cd.Clear();
            }

            return terminales;
        }
        /********************************************************
            Separa cada Or como una produccion Idependiente 
            si es que existen en gramatica original
        ********************************************************/
        private List<string> quitaOr(List<string> g)
        {
            List<string> sO = new List<string>();
            //	List<string> remov = new List<string>();
            bool b = false;
            string pr = "";
            

            foreach (string s in g)
            {                
                if (s.Contains("\\|"))
                    b = true;
                else
                    b = false;

                if (b)
                {
                    char ant = s[0];
                    string[] prod = s.Split('-');
                    pr = "";
                    foreach (char c in s)
                    {
                        if (ant != '\\' && c == '|')
                        {
                            sO.Add(pr);
                            pr = prod[0] + "->";
                        }
                        else
                        {
                            pr += c;
                            ant = c;
                        }
                    }
                    sO.Add(pr);
                }
                else
                {

                    if (s != "" && s.Contains('|'))
                    {


                        string[] aux = s.Split('|');
                        string produc = "";
                        for (int k = 0; k < aux[0].Length;k++)
                        {
                            if (aux[0][k] == '-' && aux[0][k + 1] == '>')
                                break;                           
                            else
                                produc += aux[0][k];
                        }
                        string concat = produc+ "->";
                        if(aux[0].Contains('.'))
                        {
                            string nueva = aux[0].Replace('.', (char)607);
                            aux[0] = nueva;
                        }
                        sO.Add(aux[0]);
                        for (int i = 1; i < aux.Length; i++)
                        {
                            for (int j = 0; j < aux[i].Length; j++)
                            {
                                if (aux[i][j] == '.')
                                    concat += (char)607;
                                else
                                    concat += aux[i][j];
                            }
                            sO.Add(concat);
                            concat = produc+ "->";
                        }
                    }
                    else
                    {
                        sO.Add(s);
                    }
                }
            }
            if (sO.Exists(x => x == ""))
                sO.Remove("");

            return sO;
        }
        /********************************************************
	     Encuentra los terminales primeros de toda la gramatica 
	    ********************************************************/

        public void buscarPrimero(List<string> gramatica)
        {
            bool salir = false;
            List<bool> salida = new List<bool>();
            foreach (var pr in sinOr)
            {
                salida.Add(false);
            }
            foreach (string key in simbProductor)
            {
                primeros.Add(key, new List<char>());
            }
            
            do
            {
                int num = 0;
                foreach(var p in simbProductor)
                {
                    foreach(string pro in prodcucciones[p])
                    {
                        char[] charSeparator = { '<', '>' };
                        string[] productores = pro.Split(charSeparator,StringSplitOptions.RemoveEmptyEntries);
                        int k = 0;
                        if(simbProductor.Exists(x=>x==productores[k]))
                        {
                            if(primeros[productores[k]].Count==0)
                            {
                                salida[num] = false;
                                
                            }
                            else
                            {
                                foreach(char t in primeros[productores[k]])
                                {
                                    if (t != '~')
                                    {
                                        if (t == '\\')
                                        {
                                            int index=primeros[productores[k]].IndexOf(t);

                                            if (!primeros[p].Exists(x => x == productores[k][index + 1]))
                                            {
                                                primeros[p].Add(productores[k][index + 1]);
                                                salida[num] = true;
                                            }
                                            else
                                                salida[num] = false;
                                        }
                                        else if (!primeros[p].Exists(x => x == t))
                                        {
                                            primeros[p].Add(t);
                                            salida[num] = true;
                                        }
                                        else
                                        {
                                            salida[num] = false;
                                        }
                                    }
                                    else
                                        epsilon = true;
                                }
                                if (epsilon)
                                {
                                    k++;
                                    if (k < productores.Length)
                                    {
                                        string nuevo;

                                        if (simbProductor.Exists(x => x == productores[k]))
                                        {
                                            nuevo = productores[k];
                                            salida[num] = recorridoP(nuevo, p, productores);
                                        }
                                        else
                                        {
                                            int j = k;
                                            char token;
                                            token = productores[j][0];
                                            if (token == '\\')
                                            {
                                                if (!primeros[p].Exists(x => x == productores[j][1]))
                                                {
                                                    primeros[p].Add(productores[j][1]);
                                                    salida[num] = true;
                                                }
                                                else
                                                    salida[num] = false;
                                            }
                                            else if (!primeros[p].Exists(x => x == token))
                                            {
                                                primeros[p].Add(token);
                                                salida[num] = true;
                                            }
                                            else
                                                salida[num] = false;

                                        }
                                    }
                                    else
                                    {
                                        if (!primeros[p].Exists(x => x == '~'))
                                        {
                                            primeros[p].Add('~');
                                            salida[num] = true;
                                        }
                                        else
                                            salida[num] = false;
                                        
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (productores[k][0] == '\\')
                            {
                                try
                                {
                                    if (productores[k][1] == '~')
                                    {
                                        if (!primeros[p].Exists(x => x == (char)606))
                                        {
                                            primeros[p].Add((char)606);
                                            salida[num] = true;
                                        }
                                        else
                                            salida[num] = false;
                                    }
                                    else if (!primeros[p].Exists(x => x == productores[k][1]))
                                    {
                                        primeros[p].Add(productores[k][1]);
                                        salida[num] = true;
                                    }
                                    else
                                        salida[num] = false;
                                }
                                catch
                                {
                                    switch(pro[1])
                                    {
                                        case '<':
                                            if (!primeros[p].Exists(x => x == '<'))
                                            {
                                                primeros[p].Add('<');
                                                salida[num] = true;
                                            }
                                            else
                                                salida[num] = false;
                                                break;
                                            
                                        case '>':
                                            if (!primeros[p].Exists(x => x == '>'))
                                            {
                                                primeros[p].Add('>');
                                                salida[num] = true;
                                            }
                                            else
                                                salida[num] = false;
                                            break;
                                    }

                                }
                            }
                            else if (!primeros[p].Exists(x => x == productores[k][0]))
                            {
                                primeros[p].Add(productores[k][0]);
                                salida[num] = true;
                            }
                            else
                                salida[num] = false;

                        }
                        

                        num++;
                    }
                }
                
                foreach (bool s in salida)
                {
                    if (!s)
                        salir = true;
                    else
                    {
                        salir = false;
                        break;
                    }
                }
            } while (salir == false);
           // verificaEpsilon();
        }

        /*******************************************************
            Devuelve un True o False si la Produccion, produce 
            epsilon
        *******************************************************/
        public void verificaEpsilon()
        {
            List<bool> salida = new List<bool>();
            bool b = false;
            foreach (var pr in sinOr)
            {
                salida.Add(false);
            }
            do {
                foreach (string pr in sinOr)
                {
                    string[] produccion = pr.Split('-', '<', '>');
                    string productor = produccion[0];
                    foreach (string comp in produccion)
                    {
                        if (productor != comp && simbProductor.Exists(x => x == comp))
                        {
                            if (primeros[comp].Exists(x => x == '~'))
                            {
                                epsilon = true;
                                salida[sinOr.IndexOf(pr)] = true;
                            }
                            else
                            {
                                epsilon = false;
                                salida[sinOr.IndexOf(pr)] = false;
                                break;
                            }
                        }
                        else if (comp != "" && simbTerminales.Exists(x => x == comp[0]) && comp != productor)
                        {
                            epsilon = false;
                            salida[sinOr.IndexOf(pr)] = false;

                        }
                    }
                    if (epsilon && primeros[productor].Exists(x => x != '~'))
                    {
                        primeros[productor].Add('~');
                        salida[sinOr.IndexOf(pr)] = true;
                    }
                    else
                        salida[sinOr.IndexOf(pr)] = false;

                }
                foreach (var i in salida)
                {
                    if (i)
                    {
                        b = false;
                        break;
                    }
                    else
                    {
                        b = true;
                    }
                }
            } while (!b) ;
            int y = 0;
        }

        /******************************************************
            Recorre las producciones de los No terminales siguientes
            de una produccion en la que el no terminal contenga como primero
            un epsilon.
            Devuelve un true o false si es que modifico algun productor o no 
        *******************************************************/
        public bool recorridoP(string productor_nuevo, string prod, string[] produccion)
        {
            bool s = false;
            epsilon = false;
            if (primeros[productor_nuevo].Count != 0)
            {
                foreach (char prim in primeros[productor_nuevo])
                {
                    if (prim != '~')
                    {
                        if (!primeros[prod].Exists(x => x == prim))
                        {
                            primeros[prod].Add(prim);
                            s = true;
                        }
                        else
                            s = false;
                    }
                    else
                    {
                        epsilon = true;
                    }
                }

                if(epsilon)
                {
                    int i = 0;
                    for(i=0;i<produccion.Length;i++)
                    {
                        if(produccion[i]==productor_nuevo)
                        {
                            i++;
                            break;
                        }
                    }
                    if (i < produccion.Length)
                    {
                        if (simbProductor.Exists(x => x == produccion[i]))
                        {
                            recorridoP(produccion[i], prod, produccion);
                        }
                        else
                        {
                            int j = i;
                            char token;
                            token = produccion[j][0];
                            if (token == '\\')
                            {
                                if (!primeros[prod].Exists(x => x == produccion[j][1]))
                                {
                                    primeros[prod].Add(token);
                                    s = true;
                                }
                                else
                                    s = false;
                            }
                            else if (!primeros[prod].Exists(x => x == token))
                            {
                                primeros[prod].Add(token);
                                s = true;
                            }
                        }
                    }
                    else
                    {
                        if (!primeros[prod].Exists(x => x == '~'))
                        {
                            primeros[prod].Add('~');
                            s = true;
                        }
                        else
                            s = false;
                    }
                    
                }
            }
            return s;
        }
        /********************************************************
         * Llama a la funcion busca primeros para que se ejecute,
         * despues, almacena la informacion en una listView para 
         * que se muestre en la ventana principal 
         * ******************************************************/
        private void primeros_button_Click(object sender, EventArgs e)
        {
            primeros.Clear();
            PrimerosView.Items.Clear();
            buscarPrimero(sinOr);

            foreach(char c in simbTerminales)
            {
                if(c!='~')
                {
                    primeros.Add(c.ToString(), new List<char>());
                    primeros[c.ToString()].Add(c);
                }
            }
            //cadena.Enabled = true;
        //    textBox1.Enabled = true;
            foreach (string pri in primeros.Keys)
            {
                ListViewItem item = new ListViewItem(pri);
                string pr = "";
                foreach (char st in primeros[pri])
                {
                    if (st == (char)606)
                    {
                        pr += "\\~";
                    }
                    else if (st == (char)607)
                    {
                        pr += ".";
                    }
                    else
                    {
                        pr += st;
                    }
                }
                item.SubItems.Add(pr);
                
                PrimerosView.Items.Add(item);
            }

            //button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            epsilon = true;
            StringBuilder cad = new StringBuilder();
            int index = 0;
            /*if (cadena.Text != "")
            {
                foreach (char c in cadena.Text)
                {
                    if (epsilon)
                    {
                        if (simbProductor.Exists(x => x == c.ToString()))
                        {
                            if (primeros[c.ToString()].Contains('~'))
                            {
                                epsilon = true;
                            }
                            foreach (char s in primeros[c.ToString()])
                            {
                                if (s != '~')
                                {
                                    cad.Append(s);
                                }
                            }
                        }
                        else if (simbTerminales.Exists(x => x == c))
                        {
                            cad.Append(c);
                            epsilon = false;

                        }
                        else
                            break;
                    }
                    index++;
                }
                if (index == cadena.Text.Length - 1)
                {
                    textBox1.Text = "Cadena no Valida";
                }
                else
                    textBox1.Text = cad.ToString();
            }*/
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (primeros.Count != 0)
            {
                automata = new Automata();
                treeView1.Nodes.Clear();
                listView2.Items.Clear();
                listView1.Items.Clear();
                
                tableAnalisis = new Dictionary<int, Dictionary<string, string>>();
                automata = creaAutomata();
                automata.tablaAnalisis = crearTabla(automata);
                if (automata.tablaAnalisis != null)
                {
                    tableAnalisis = automata.tablaAnalisis;
                    LlenarTablaAS(automata);
                    crearArbolAFD(automata);
                }
               
            }
            else
                MessageBox.Show("No se han calculado los primeros de la gramtica");
        }
        #region Automata
        /***************************************************************
         *Crea Automata, funcion que genera el automata AFD de la 
         * gramatica introducida, enlaza y crea un diccionario para
         * que el automata este relacionado entre todos los estados 
         * generados  
         ***************************************************************/
        public Automata creaAutomata()
        {
            nodo = 0;
            //creacion del automata AFD 
            automata = new Automata();
            automata.putSimbolos(simGram);
            automata.putGram(sinOr);
            //se agrega la produccion aumentada a la gramatica 
            string cadenaAumentada = simbProductor[0] + "Aumentada->.";
            string cadenaA = simbProductor[0] + "Aumentada->";
            cadenaAumentada += '<' + simbProductor[0] + '>';
            cadenaA += '<' + simbProductor[0] + '>';
            clavesE.Add(clave, cadenaAumentada);
            //Creacion del estado cero 
            Elemento elem = new Elemento(cadenaA, cadenaAumentada, '$');
            List<Elemento> listaElementos = new List<Elemento>();
            listaElementos.Add(elem);
            List<Elemento> lElem = cerradura(listaElementos);
            automata.creaEstado(0);
            automata.putElementos(lElem, 0);
            automata.creaClaveEdo(0,clavesE);
            lElem.Clear();

            int i = 0;//estado actual
            int j = 0;//nuevo estado 
           // generarAutomata(i);

           

        
      /*  private void generarAutomata(int j) {
            List<Elemento> lElem = new List<Elemento>();
            
            foreach (string nt in simGram)
            {

                lElem = ir_a(automata.estados[j], nt);
                if (lElem.Count != 0)
                {
                    if (verificaEdo(lElem, automata.estados))
                    {
                        //aut.nuevaRelacion();
                       
                        automata.putRelaciones(nt, nodo);
                        automata.enlazarRelaciones(automata.estados[j].Edo);
                    }
                    else
                    {
                       
                        estado++;
                        automata.creaEstado(estado);
                        automata.putElementos(lElem, estado);
                       
                        automata.nuevaRelacion();
                        
                        automata.putRelaciones(nt, estado);
                        automata.enlazarRelaciones(estado);
                        generarAutomata(estado);
                    }


                }
            }
            automata.nuevaRelacion();
        }*/

            do
            {
                foreach (string nt in simGram)
                {
                    lElem = ir_a(automata.estados[i], nt);
                    if (lElem.Count != 0)
                    {
                        if (verificaEdo(lElem, automata.estados))
                        {
                            //aut.nuevaRelacion();
                            automata.putRelaciones(nt, nodo);
                            automata.enlazarRelaciones(automata.estados[i].Edo);
                        }
                        else
                        {
                            j++;
                            automata.creaEstado(j);
                            automata.putElementos(lElem, j);
                            if (nt == simGram[0])
                            {
                                automata.nuevaRelacion();
                            }
                            automata.creaClaveEdo(j, clavesE);
                            automata.putRelaciones(nt, j);
                            automata.enlazarRelaciones(i);
                        }
                    }
                }
                automata.nuevaRelacion();
                i++;
            } while (i <= automata.estados.Count - 1);
            
            return automata;
        }
        /************************************************************
         * Funcion que encuentra los elementos de un estado, haciendo
         * uno por cada uno de los caracteres que tenga los primeros 
         * de cada Beta de la produccion evaluada 
         * **********************************************************/
        public List<Elemento> cerradura(List<Elemento> elementos)
        {
            if (elementos.Count > 0)
            {
                int i = 0;
                bool epsilon = false;
                List<string> beta = new List<string>();
                string B;//productor "B" con el que se encontrara las nuevas producciones 
                List<string> listaSB = new List<string>();//lista de producciones sin punto que pertenecen al elemento 
                List<string> listaSBP = new List<string>();//lista de producciones con punto que pertenecen al elemento 
                List<char> primerosBeta = new List<char>();//primeros de beta 
                do
                {
                    primerosBeta = new List<char>();
                    if (elementos[i].ProduccionP.IndexOf('.') != elementos[i].ProduccionP.Length - 1)
                    {
                        string[] auxS = elementos[i].ProduccionP.Split('.');

                        if (auxS[1][0] == '<')
                        {
                            B = "";
                            listaSB = new List<string>();
                            listaSBP = new List<string>();

                            string[] a = Regex.Split(auxS[1], pattern);
                            string[] p = a[1].Split('<', '>');
                            B = p[1];
                            //Match match = Regex.Match(a[1], simbProductor.Find(y => y==p[1]));
                          //  B = match.Value.ToString();
                            
                            foreach (string s in prodcucciones[B])
                            {
                                listaSB.Add(B + "->" + s);
                            }

                            beta = new List<string>();
                            int k = 0;
                            
                            for (; k < a.Length; k++)
                            {
                                if (a[k] != "")
                                {
                                    if (k > 1)
                                    {
                                        if (a[k][0] == '<')
                                        {
                                            string[] auxNuevo = a[k].Split('<', '>');
                                            //  Match matchE = Regex.Match(a[k], simbProductor.Find(y =>y==auxNuevo[1]));
                                            // string item = matchE.Value.ToString();
                                            string item = auxNuevo[1];
                                            beta.Add(item);
                                        }
                                        else
                                        {
                                            beta.Add(a[k]);
                                        }

                                    }
                                }
                            }
                      
                            if (beta.Count > 0)
                            {
                                foreach (string pB in beta)
                                {

                                    if (simbProductor.Exists(x => x == pB) && simbTerminales.Exists(x => x.ToString() != pB))
                                    {
                                        foreach (char prim in primeros[pB])
                                        {
                                            if (!primerosBeta.Exists(x => x == prim))
                                            {
                                                //     primerosBeta.Remove('~');
                                                primerosBeta.Add(prim);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!primerosBeta.Exists(x => x == pB[0]))
                                        {
                                            //  primerosBeta.Remove('~');
                                            primerosBeta.Add(pB[0]);
                                        }


                                    }
                                    if (primerosBeta.Contains('~') && beta.IndexOf(pB) < beta.Count - 1)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                if (primerosBeta.Contains('~'))
                                {
                                    epsilon = true;
                                    primerosBeta.Remove('~');
                                }
                            }
                            if (epsilon || beta.Count == 0)
                            {

                                if (primeros.ContainsKey(elementos[i].Caracter.ToString()))
                                {
                                    foreach (char pi in primeros[elementos[i].Caracter.ToString()])
                                    {
                                        if (!primerosBeta.Exists(x => x == pi))
                                        {
                                            primerosBeta.Add(pi);
                                        }
                                    }
                                }
                                else if (elementos[i].Caracter == '$')
                                {
                                    if (!primerosBeta.Exists(x => x == elementos[i].Caracter))
                                    {
                                        primerosBeta.Add(elementos[i].Caracter);
                                    }
                                }
                                epsilon = false;
                            }

                            foreach (string s in listaSB)
                            {
                                int l = 0;
                                l = s.IndexOf('>') + 1;

                                string sr;
                                if (s[l] == '~')
                                {
                                    sr = s.Insert(l + 1, ".");
                                    l = sr.IndexOf('~');
                                    sr = sr.Remove(l, 1);
                                }

                                else
                                {
                                    sr = s.Insert(l, ".");
                                }

                                listaSBP.Add(sr);
                            }

                            foreach(string m in listaSBP)
                            {
                                if(!clavesE.ContainsValue(m))
                                {
                                    clave++;
                                    clavesE.Add(clave, m);
                                }
                            }
                            foreach (char pr in primerosBeta)
                            {
                                foreach (string el in listaSBP)
                                {
                                    //Elemento(produccion normal,produccion con punto,primero de beta)
                                    Elemento e = new Elemento(listaSB[listaSBP.IndexOf(el)], el, pr);
                                    if (!elementos.Exists(x => x.ProduccionP == e.ProduccionP && x.Caracter == e.Caracter))
                                    {
                                        elementos.Add(e);
                                    }
                                }
                            }
                            i++;
                        }
                        else
                            i++;
                    }
                    else
                        i++;

                } while (i < elementos.Count);
            }
            return elementos;
        }
        /********************************************************************
         * Esta funcion aumenta el punto de las producciones y genera los
         * nuevos elementos de esta, generando asi un nuevo estado o uno 
         * ya existente, x(simbolo gramatical)
         * *****************************************************************/
        public List<Elemento> ir_a(Estado edo, string x)
        {
            List<Elemento> elem = new List<Elemento>();
            List<Elemento> elR = new List<Elemento>();
            
            foreach (Elemento c in edo.listElementos)
            {
                int indice = c.ProduccionP.IndexOf('.');
                string sx="";
                Elemento e = new Elemento();
                string productor = "";
              
                if (indice != c.ProduccionP.Length - 1)
                {
                    string[] auxS = c.ProduccionP.Split('.');
                    if (auxS[1][0] == '<')
                    {                       
                        indice=0;                      
                        string[] a = Regex.Split(auxS[1], pattern);
                        string[] auxD = a[1].Split('<', '>');
                        //Match match = Regex.Match(a[1], simbProductor.Find(y =>y==auxD[1]));
                        //productor = match.Value.ToString();                       
                        productor = auxD[1];
                        indice=a[1].Length;
                        indice += auxS[0].Length;
                        sx = c.Produccion.Insert(indice, ".");
                    }
                    else
                    {                      
                        sx = c.Produccion.Insert(indice + 1, ".");
                        productor = auxS[1][0].ToString();
                    }
                    if (productor == x)
                    {
                        
                        if (!clavesE.ContainsValue(sx))
                        {
                            clave++;
                            clavesE.Add(clave, sx);
                        }                        
                        e = new Elemento(c.Produccion, sx, c.Caracter);
                        elem.Add(e);                        
                    }
                }
            }
            elR = cerradura(elem);
            return elR;
        }
        /**********************************************************************
         * Regresa un true si un estado nuevo dado, ya existe o no, si esque 
         * existe se actualizara la variable "nodo" con el numero del estado que 
         * encontro, si no existe regresara un false
         * ********************************************************************/
        public bool verificaEdo(List<Elemento> elemento, List<Estado> estados)
        {
            bool band = false;
            string claveProvicional = "";
            foreach (Elemento el in elemento)
            {
                foreach (var key in clavesE)
                {
                    if (clavesE[key.Key] == el.ProduccionP)
                    {
                        claveProvicional += clavesE[key.Key] + el.Caracter;
                    }
                }
            }

            if (estados.Exists(x => x.clave == claveProvicional))
            {
                band = true;
                nodo = estados.FindIndex(x => x.clave == claveProvicional);
            }
            else
            {
                band = false;
            }        

            return band;
        }
        public void crearArbolAFD(Automata automata)
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Add("AFD");
            foreach (Estado edo in automata.estados)
            {
                treeView1.Nodes[0].Nodes.Add(edo.Edo.ToString());
                foreach (Elemento el in edo.listElementos)
                {
                    treeView1.Nodes[0].Nodes[edo.Edo].Nodes.Add(el.ProduccionP + " ," + el.Caracter);
                }
                foreach (var d in automata.Relaciones[edo.Edo])
                {
                    if (d.Key[0] == (char)606 && d.Key.Length == 1)
                        treeView1.Nodes[0].Nodes[edo.Edo].Nodes.Add("\\~" + "," + d.Value);
                    else
                        treeView1.Nodes[0].Nodes[edo.Edo].Nodes.Add(d.Key + "," + d.Value);
                }

            }
            treeView1.EndUpdate();
        }
        #endregion
        #region TablaAnalisisSintactico
        /******************************************************************************************
         * Funcion que devuelve un diccionario que contiene la tabla de analisis sintactico 
         * para el automata AFD generado previamente
         * d(desplazar)
         * r(reducir)
         * acc(aceptar)
         * El diccionario contiene la llave que es el terminal o el productor mas el caracter de 
         * aceptacion '$' y sus relaciones directas.
         *  
         * ***************************************************************************************/
        public Dictionary<int, Dictionary<string, string>> crearTabla(Automata automata)
        {
            Dictionary<int, Dictionary<string, string>> tabla = new Dictionary<int, Dictionary<string, string>>();
            Dictionary<string, int> diccAux;
            Dictionary<string, string> diccAux2 = new Dictionary<string, string>();
            List<Elemento> listaB;

            automata.verificarEpsilon();
            int indice = 0;
            bool ci = false;
            string produccionError;
            foreach (Estado e in automata.estados)
            {
                listaB = new List<Elemento>();

                foreach (Elemento elem in e.listElementos)
                {
                    //inciso C
                    if (elem.ProduccionP.Contains("Aumentada")  && elem.ProduccionP[elem.ProduccionP.Length - 1] == '.')
                    {
                        ci = true;
                    }
                    else
                    {
                        //inciso B
                        indice = elem.ProduccionP.IndexOf('.');
                        if (indice == elem.ProduccionP.Length - 1)
                        {
                            listaB.Add(elem);
                        }
                    }
                }
                diccAux2 = new Dictionary<string, string>();
                foreach (Elemento c in listaB)
                {
                    int num = automata.gramatica.IndexOf(c.Produccion) + 1;
                 
                    try
                    {
                        diccAux2.Add(c.Caracter.ToString(), "r" + num);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Se intento insertar dos reducciones en\n el simobolo gramatical \" " + c.Caracter + " \", no es gramatica LR1");
                        return null;

                    }
                }

                diccAux = new Dictionary<string, int>();
                diccAux = automata.Relaciones[e.Edo];
                foreach (string p in automata.simbolosGramaticales)
                {
                    if ((diccAux.ContainsKey(p) && !simbProductor.Exists(x=>x==p) && (simbTerminales.Exists(x=>x.ToString()==p) || p != "$")))
                    {
                        string d = "d" + diccAux[p];
                        try
                        {
                            diccAux2.Add(p, d);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Se intento insertar dos acciones en\n el simobolo gramatical \" "+p+" \", no es gramatica LR1");
                            return null;

                        }
                    }
                    else if (diccAux.ContainsKey(p) && simbProductor.Exists(x=>x==p))
                        diccAux2.Add(p, diccAux[p].ToString());
                }

                if (ci)
                {
                    diccAux2.Add("$", "acc");
                    ci = false;
                }
                //151
                tabla.Add(e.Edo, diccAux2);


            }
            return tabla;
        }
        
        public void LlenarTablaAS(Automata automata)
        {
            string ruta = Application.StartupPath;
            ruta = ruta.Substring(0, ruta.Length - 5)+"tabla.txt";
            StreamWriter tablaText = new StreamWriter(ruta);

            int index = 1;
            char terminal=' ';
            bool espacio = false;
            string espacioT = " ";
            /*
             *  "." => (char)607
                metacaracteres.Add("\\<");//(char)610
                metacaracteres.Add("\\>");//(char)611
                metacaracteres.Add("\\|");//(char)612
                metacaracteres.Add("\\~");//(char)613
                metacaracteres.Add("\\\\");//(char)614
                metacaracteres.Add("\\e");//(char)615espacio
            */
            //gramatica original
            tablaText.WriteLine(gramOriginal.Count.ToString());
            
            foreach(string s in gramOriginal)
            {

                tablaText.WriteLine(s);
            }
            //gramatica sinOr
            tablaText.WriteLine(automata.gramatica.Count);

            foreach (string s in automata.gramatica)
            {
                tablaText.WriteLine(s);
            }

            foreach (char t in simbTerminales)
            {
                
                if (t != '~')
                {
                    switch (t)
                    {
                        case (char)607:// .
                            terminal = '.';
                            break;
                        case (char)610:// \<
                            terminal = '<';
                            break;
                        case (char)611: // \>
                            terminal = '>';
                            break;
                        case (char)612: // ||
                            terminal = '|';
                            break;
                        case (char)613:// \~
                            terminal = '~';
                            break;
                        case (char)614: // \\
                            terminal = '\\';
                            break;
                        case (char)615:// \e
                            espacio = true;
                            espacioT = "espacio";
                            break;
                        default:
                            terminal = t;
                            break;
                    }
                    if (espacio)
                    {
                        listView1.Columns.Insert(index, espacioT, 100);
                        index++;
                        tablaText.Write(espacioT + " ");
                        Console.Write(t.ToString());
                        espacio = false;
                    }
                    else
                    {
                        listView1.Columns.Insert(index, terminal.ToString(), 100);
                        index++;
                        tablaText.Write(terminal + " ");
                        Console.Write(t.ToString());
                    }
                }
            }
            
            tablaText.WriteLine();
            listView1.Columns.Insert(index, "$", 100);
            index++;
            tablaText.WriteLine('$');
            
            Console.Write("$");

            foreach (string st in simbProductor)
            {
                listView1.Columns.Insert(index, st, 100);
                index++;
                tablaText.Write(st +" ");
                Console.Write(st);
            }
            Console.WriteLine();
            tablaText.WriteLine();
            index = 1;
            string punto = "";
            punto += (char)607;
            string tilde = "";
            tilde += (char)606;
            //no pinta los metacaracters en la tabla 
            string menorQue = "";
            menorQue += (char)610;
            string mayorQue = "";
            mayorQue += (char)611;
            string or = "";
            or += (char)612;
            string tide = "";
            tide += (char)613;
            string diagonalInvert = "";
            diagonalInvert += (char)614;
            foreach (int s in automata.tablaAnalisis.Keys)
            {
                ListViewItem item = new ListViewItem(s.ToString());
                for (; index < listView1.Columns.Count; index++)
                {
                    if (listView1.Columns[index].Text == "." && automata.tablaAnalisis[s].ContainsKey(punto))
                    {
                        item.SubItems.Add(automata.tablaAnalisis[s][punto]);
                        tablaText.Write(automata.tablaAnalisis[s][punto] + " ");
                    }
                    else if (listView1.Columns[index].Text == "<" && automata.tablaAnalisis[s].ContainsKey(menorQue))
                    {
                        item.SubItems.Add(automata.tablaAnalisis[s][menorQue]);
                        tablaText.Write(automata.tablaAnalisis[s][menorQue]+ " ");
                    }
                    else if (listView1.Columns[index].Text == ">" && automata.tablaAnalisis[s].ContainsKey(mayorQue))
                    {
                        item.SubItems.Add(automata.tablaAnalisis[s][mayorQue]);
                        tablaText.Write(automata.tablaAnalisis[s][mayorQue]+ " ");
                    }
                    else if (listView1.Columns[index].Text == "|" && automata.tablaAnalisis[s].ContainsKey(or))
                    {
                        item.SubItems.Add(automata.tablaAnalisis[s][or]);
                        tablaText.Write(automata.tablaAnalisis[s][or].ToString() + " ");
                    }
                    else if (listView1.Columns[index].Text == "\\" && automata.tablaAnalisis[s].ContainsKey(diagonalInvert))
                    {
                        item.SubItems.Add(automata.tablaAnalisis[s][diagonalInvert]);
                        tablaText.Write(automata.tablaAnalisis[s][diagonalInvert].ToString() + " ");
                    }
                    else if (automata.tablaAnalisis[s].ContainsKey(listView1.Columns[index].Text))
                    {
                        item.SubItems.Add(automata.tablaAnalisis[s][listView1.Columns[index].Text]);
                        tablaText.Write(automata.tablaAnalisis[s][listView1.Columns[index].Text].ToString()+" ");
                    }
                    else if (automata.tablaAnalisis[s].ContainsKey(tilde) && listView1.Columns[index].Text == "~(tilde)")
                    {
                        item.SubItems.Add(automata.tablaAnalisis[s][tilde]);
                        tablaText.Write(automata.tablaAnalisis[s][tilde].ToString()+" ");
                    }
                   
                    else
                    {
                        item.SubItems.Add(" ", Color.Black, Color.White, DefaultFont);
                        if(index !=listView1.Columns.Count-1)
                            tablaText.Write((char)700 + " ");
                    }
                }
                listView1.Items.Add(item);
                tablaText.WriteLine();
                index = 1;
            }
            tablaText.Close();
        }

        #endregion
        #region Evaluar
        public bool evaluarExp(StringBuilder w,Automata automata)
        {
            bool error = false;          
            
            w.Append('$');
            string cadena = w.ToString();
            Dictionary<string, string> aux = automata.tablaAnalisis[0];
            Stack pila = new Stack();
            pila.Push((int)0);
            int s, t;
            int tamañoRemove = 0;
            string accion = "";
            StringBuilder stack = new StringBuilder("$0");
            foreach (char a in cadena)
            {
                s = (int)pila.Peek();
                Dictionary<string, string> auxD = automata.tablaAnalisis[s];
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
                                    w.Remove(0, 1);

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
                                    listView2.Items.Add(item);
                                    break;
                                case 'r':
                                    
                                    int num = 0;
                                    foreach (string st in automata.gramatica)
                                    {
                                        string[] n = accion.Split('r');
                                        int nm = Convert.ToInt32(n[1]);
                                        if (automata.gramatica.IndexOf(st) + 1 == nm)
                                        {
                                            string llave = "";
                                            int m = 0;
                                            for (;m<st.Length;m++)
                                            {
                                                if (!(st[m]== '-' && st[m + 1] == '>'))
                                                {
                                                    llave += st[m];
                                                }
                                                else
                                                    break;
                                            }
                                            m+=2;

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
                                                else if (st[m] == '>')
                                                {
                                                    y = false;
                                                }
                                                else if (st[m] == '\\')
                                                {
                                                    m++;
                                                    num++;
                                                }
                                                else if (simbTerminales.Exists(x => x == st[m]) && !y) 
                                                    num++;
                                            }
                                           
                                            for (int i = 0; i < num; i++)
                                            {
                                                pila.Pop();                                                                                                
                                            }
                                            stack.Remove(stack.Length - tamañoRemove, tamañoRemove);
                                            tamañoRemove = llave.Length;
                                            ListViewItem item1 = new ListViewItem(stack.ToString());
                                            item1.SubItems.Add(a.ToString());
                                            item1.SubItems.Add(w.ToString());
                                            item1.SubItems.Add(accion+"= "+st);
                                            listView2.Items.Add(item1);

                                            if (pila.Count == 0)
                                                t = 0;
                                            else
                                                t = (int)pila.Peek();
                                            Dictionary<string, string> ir_a = automata.tablaAnalisis[t];
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
                                                listView2.Items.Add(item2);
                                                
                                                s = (int)pila.Peek();
                                                tamañoRemove+=ir_a[llave].Length;
                                                Dictionary<string, string> auxD1 = automata.tablaAnalisis[s];
                                                if (auxD1.ContainsKey(a.ToString()))
                                                {
                                                    accion = auxD1[a.ToString()];
                                                    if (accion[0] == 'd')
                                                    {
                                                        w.Remove(0, 1);
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
                                                        listView2.Items.Add(item3);
                                                        
                                                    }
                                                }
                                                else { error = true; break; }
                                            }
                                            else { error = true; break; }
                                            break;
                                        }
                                    }
                                    break;
                            }
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
                    listView2.Items.Add(item2);
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

        #endregion

        private void button5_Click(object sender, EventArgs e)
        {
           // listView2.Clear();
            if (CadenaW.Text != "" && automata.estados.Count != 0)
            {
                if (evaluarExp(new StringBuilder(CadenaW.Text), automata))
                    MessageBox.Show("Cadena Correcta");
                else
                    MessageBox.Show("Cadena Incorrecta");
            }
            else
                MessageBox.Show("No se han calculado el AFD LR1");
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}
