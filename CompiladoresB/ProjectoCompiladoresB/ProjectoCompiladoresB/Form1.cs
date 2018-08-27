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
        Dictionary<string, string> tablaSimbolos;// key => etiqueta, val=> tipo
        Dictionary<string, string> valorSimbolos;//key => etiqueta, val=> valor asignado
        Dictionary<string, string> codigoCadena;
        List<string> gramatica;
        List<string> gramaticaSinOr;
        List<char> terminales;
        List<string> productores;
        List<string> reducciones = new List<string>();
        string palabraRev;
        string codigo;
        List<string> palabraR;
        Queue colaId = new Queue();
        Queue colaNum = new Queue();
        Queue colaOp = new Queue();
        Queue colaOpSuma = new Queue();
        Queue colaSintx = new Queue();
        Stack pilaSint = new Stack();
        Stack pilaId = new Stack();
        Stack pilaNum = new Stack();
        Stack pilaOp = new Stack();
        Stack pilaArbol = new Stack();
        Stack pilaComa = new Stack();
        Stack pilaFor = new Stack();
        Queue colaFunciones = new Queue();
        Queue colaFuncionesR = new Queue();
        string op1, op2, op, res;
        string comiila = @"(\u0022.#\u0022$)";
        string reservadas;
        int temp = 0;
        int indice = 0;
        string temporalA = "#r";
        string tipoTemp = "";
        Stack pilaResultados = new Stack();
        List<Cuadruplo> lC = new List<Cuadruplo>();
        List<string> nodos = new List<string>();
        List<List<Cuadruplo>> funcionesCuad = new List<List<Cuadruplo>>();
        int contDepurador = 0;
        public Principal()
        {
            InitializeComponent();
        }
        private void Principal_Load(object sender, EventArgs e)
        {
            //palabras reservadas 
            palabraRev = "==|Loop|Mbox|label|CreaLabel|*|Main|ImprimeTextBox|LeeTextBox|CreaEvento|CreaBoton|CreaTextbox|CierraVentana|CreaVentana|def|vent|textBox|boton|" +
                "^|-|+|/|Concat|$|,|:=|:|;|=|!=|<=|>=|{|}|<|>|(|)|if|else|endif|switch|endswitch|int|char|string|float|Var|for" +
              "|step|endfor|while|endwhile|step|initWindow|closeWindow|case|default|break|endswitch|repeat|until";
            palabraR = new List<string>(palabraRev.Split('|'));
            palabraR.RemoveAll(x => x == "");
            tablaSimbolos = new Dictionary<string, string>();
            cargarTabla();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            TablaANS t = new TablaANS(gramatica, terminales, productores, TablaAnalisisSintactico);
            t.Show();
        }
        # region cargarTabla
        /***************************************************************
         * Carga un archivo de texto, y despues lee linea por linea
         * siguiendo un formato para determinar la gramatica original
         * ,la gramatica convertida, los terminales, productores 
         * y finalmente los estados y sus acciones
         * ***********************************************************/
        private void cargarTabla()
        {
            //  bool nuevo = false;
            // openFileDialog1.Title = " ";
            // openFileDialog1.FileName = "";
            // openFileDialog1.InitialDirectory = Application.StartupPath;
            // Application.StartupPath + "\\PrimerosCadenaLr1TablaAnalisisSintacticoPílade Llamadas\\Primeros\\bin";

            string path = "../../../../PrimerosCadenaLr1TablaAnalisisSintacticoPílade Llamadas/Primeros/bin/tabla.txt";
            TextReader archivo = null;
            archivo = new StreamReader(path);

            if (archivo != null)
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

                //LlenarTablaAS(gramatica, terminales, productores, TablaAnalisisSintactico);
                List<string> metacaracteres = new List<string>();
                metacaracteres.Add("" + (char)607 + "");//.
                metacaracteres.Add("" + (char)610 + "");//<
                metacaracteres.Add("" + (char)611 + "");//>
                metacaracteres.Add("" + (char)612 + "");//|
                metacaracteres.Add("" + (char)613 + "");//~
                metacaracteres.Add("" + (char)614 + "");//\
                                                        //metacaracteres.Add("\\e");//(char)615espacio
                foreach (string s in metacaracteres)
                {
                    int index = 0;
                    if (gramaticaSinOr.Exists(x => x.Contains(s)))
                    {
                        string auxS = gramaticaSinOr.Find(x => x.Contains(s));
                        index = gramaticaSinOr[gramaticaSinOr.FindIndex(x => x.Contains(s))].IndexOf(s[0]);
                        string terminal = "";
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
                    archivo.Close();

                }
            }
            else
            {
                MessageBox.Show("No se cargo el archivo");
            }
        }
        #endregion    
        #region evaluarCodigo
        /*************************************************************************
         * Funcion que evalua un codigo de entrada y apila los tokens introducidos
         * segun sea su tipo 
         * ************************************************************************/
        public bool evaluarExp(Dictionary<int, Dictionary<string, string>> tablaAnalisis)
        {
            string w = codigo;//cadena que cambiara y actualizara el codigo al evaluar un token 
            codigo += ('$'); //se agrega el terminador a la cadena de entrada 
            bool error = false;//bandera para terminar el ciclo por si ocurre un error 
            bool r = false;

            Dictionary<string, string> aux = tablaAnalisis[0];
            Stack pila = new Stack();//pila de estados 
            pila.Push(0);
            int s, t;
            int tamañoRemove = 0;
            string accion = "";
            StringBuilder stack = new StringBuilder("$0");
            Token token = new Token();
            bool funciones = false;

            while (codigo != null && !error && accion != "acc")
            {
                if (!r)
                {
                    while (codigo[0] == ' ')
                        codigo = codigo.Remove(0, 1);
                    token = getNextToken();//regresa el siguiente token a evaluar y modifica el la cadena de entrada 
                    w = codigo;
                    try
                    {

                        switch (token.Tipo)
                        {
                            case 0://numero
                                colaNum.Enqueue(token);
                                break;
                            case 1://id
                                if (funciones)
                                {
                                    colaFunciones.Enqueue(token);
                                    funciones = false;
                                }
                                else
                                {
                                    colaId.Enqueue(token);
                                }
                                break;
                            case 2:

                                if (token.Val == "def")
                                {
                                    funciones = true;
                                }
                                if (token.Val == "=" || token.Val == ":")
                                {
                                    pilaFor.Push(token);
                                }
                                else if (token.Val == "Main")
                                {
                                    colaFunciones.Enqueue(token);
                                }
                                else if (token.Val == "else")
                                {
                                    pilaId.Push(token);
                                }
                                else if (token.Val == ";")
                                {
                                    pilaOp.Push(token);
                                }
                                else if (token.Val == ",")
                                {
                                    pilaComa.Push(token);
                                }
                                else if (token.Val == "+" || token.Val == "-" || token.Val == "^" || token.Val == "*" || token.Val == "/")
                                {
                                    colaOpSuma.Enqueue(token);
                                }
                                else if (token.Val == "if" ||
                                     token.Val == "repeat" || token.Val == "switch" ||
                                     token.Val == "while" || token.Val == "for" || token.Val == "case")
                                {
                                    pilaSint.Push(token);
                                }
                                else if (token.Val == "CreaVentana" || token.Val == "CreaTextbox" ||
                                    token.Val == "CreaBoton" || token.Val == "LeeTextBox" ||
                                    token.Val == "ImprimeTextBox" || token.Val == "Concat" ||
                                    token.Val == "CreaEvento" || token.Val == "Mbox" || token.Val == "CierraVentana"
                                    || token.Val == "Loop")
                                {
                                    colaFuncionesR.Enqueue(token);
                                }
                                else if (token.Val != "break" && token.Val != ":" && token.Val != "=" && token.Val != "{" && token.Val != "}" && token.Val != "(" && token.Val != ")" && token.Val != "$" && token.Val != "until")
                                {
                                    colaOp.Enqueue(token);
                                }
                                break;
                        }
                    }
                    catch (Exception e) { MessageBox.Show("No existe el token"); }
                }
                if (token != null)
                {
                    for (int id = 0; id < token.Nombre.Length; id++)
                    {
                        char a = token.Nombre[id];
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
                                            r = false;
                                            break;
                                        case 'r':
                                            r = true;
                                            int num = 0;
                                            string llave = "";
                                            string st = "";
                                            Tuple<int, string, string> num_llave_prod = getNum_Llave_prod(accion);
                                            num = num_llave_prod.Item1;
                                            llave = num_llave_prod.Item2;
                                            st = num_llave_prod.Item3;

                                            for (int i = 0; i < num; i++)
                                                pila.Pop();

                                            stack.Remove(stack.Length - tamañoRemove, tamañoRemove);
                                            tamañoRemove = llave.Length;
                                            ListViewItem item1 = new ListViewItem(stack.ToString());
                                            item1.SubItems.Add(a.ToString());
                                            item1.SubItems.Add(w.ToString());
                                            item1.SubItems.Add(accion + "= " + st);
                                            tablaAcciones.Items.Add(item1);
                                            reducciones.Add(st);
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
                                                id--;
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

        private Tuple<int, string, string> getNum_Llave_prod(string accion)
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
        #region tokenizado
        /****************************************************************
        * Funcion que regresa un token, con sus atributos completos
        * y analizados, retornara un null, si el token no existe o 
        * tiene una sintaxis erronea
        * **************************************************************/
        private Token getNextToken()
        {
            //u0022(comilla doble)  circunflejo u005e        
            Token token = new Token();
            bool espacio = false;
            int index;
            //Expresion regular que separa por palabras las coincidencias y mantiene en su posicion las que no
            //lista de posibles tokens aun no tokenizados
            reservadas = @"(Loop$|CierraVentana|Mbox|Main$|label$|CreaLabel$|ImprimeTextBox$|LeeTextBox$|" +
          @"CreaEvento$|CreaBoton$|CreaTextbox$|CreaVentana$|def$|vent$|boton|textBox|\u005e|" +
          @"if$|else|\$|\(|\)|=|!=|:=|;|,|Concat$|endif$|endswitch|endwhile|endfor|\{|\}|" +
          @"until|repeat|Var$|int$|float$|string$|char$|switch|for|while|" +
          @"step$|initWindow|closeWindow|==|>|>=|<=|<|\+|-|\*|/|case|:|default|break|id$|num$|\#cad)";
            List<string> tokenPrevios = new List<string>(Regex.Split(codigo, reservadas));
            tokenPrevios.RemoveAll(x => x == "" || x == " ");
            //List<string> tokenPrevios2 = new List<string>(Regex.Split(codigo, comiila));
            //     tokenPrevios2.RemoveAll(x => x == "" || x == " ");

            string posibleVar = tokenPrevios.Find(x => x.IndexOf(x) == 0);//variable candidata como valor lexico del token
                                                                          //quita espacios que puedan interferir en el analisis 
                                                                          //de variables normales excepto cadenas 

            if (posibleVar.Contains(' ') && !posibleVar.Contains("\""))
            {
                char[] c = new char[1];
                c[0] = ' ';
                string[] aux = posibleVar.Split(c, StringSplitOptions.RemoveEmptyEntries);
                posibleVar = aux[0];
                tokenPrevios.RemoveAt(0);
                tokenPrevios.Insert(0, posibleVar);
                if (aux.Length > 1)
                    tokenPrevios.Insert(1, aux[1]);
                espacio = true;
            }
            if (posibleVar == "#cad")
            {
                if (tokenPrevios[0] == "#cad")
                {
                    string clave = tokenPrevios[0] + tokenPrevios[1];
                    tokenPrevios.RemoveRange(0, 2);
                    token = new Token(codigoCadena[clave], 1);
                    codigo = codigo.Remove(0, clave.Length);
                    codigo = codigo.Insert(0, token.Nombre);
                    return token;
                }
                else
                    return null;
            }
            else
            {
                Match match = Regex.Match(tokenPrevios.Find(x => x.IndexOf(x) == 0), reservadas);

                if (match.Value != "")
                {
                    if (palabraR.Exists(x => x == match.Value))//tokens de palabras reservadas tipo 2
                    {
                        token = new Token(match.Value, 2);
                        index = match.Index;
                        //remueve el preToken del codigo e inserta el token ya analizado al codigo 
                        if (espacio)
                            codigo = codigo.Remove(index, match.Value.Length + 1);
                        else
                            codigo = codigo.Remove(index, match.Value.Length);
                        codigo = codigo.Insert(index, token.Val);
                        return token;
                    }
                    else
                        return null;
                }
                else if (tokenPrevios.Count != 0)//tokens que pueden ser id o numeros
                {

                    if (posibleVar.All(c => char.IsLetter(c)) || posibleVar.Contains('"') ||
                        (posibleVar.Any(c => char.IsLetter(c)) &&
                         posibleVar.Any(c => char.IsNumber(c))))
                    {
                        token = new Token(posibleVar, 1);//token id tipo 1
                    }
                    else if (posibleVar.All(c => char.IsNumber(c)))
                    {
                        token = new Token(posibleVar, 0);//token n tipo 0
                    }

                    index = 0;
                    //remueve el preToken del codigo e inserta el token ya analizado al codigo 
                    if (espacio)
                        codigo = codigo.Remove(index, token.Val.Length + 1);
                    else
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

        #endregion
        /********************************************************
         * boton que compilara el codigo introducido en el cuadro
         * texto, separara los espacios y los saltos de linea
         * *****************************************************/
        private void CompilarBoton_Click(object sender, EventArgs e)
        {
            string reservadas = @"(Loop$|Main$|label$|CreaLabel$|ImprimeTextBox$|LeeTextBox$|" +
            @"CreaEvento$|CreaBoton$|CreaTextbox$|CreaVentana$|def$|vent$|boton|textBox|\u005e|" +
            @"if$|\$|\(|\)|=$|:=|;|,|Concat$|endif$|endswitch|endwhile|endfor|\{|\}|else$|" +
            @"until|repeat|Var$|int$|float|string|char|switch|for|while|" +
            @"step$|initWindow|closeWindow|==|case|:$|default|break|id$|num$)";
            tablaAcciones.Items.Clear();
            tablaSimbolos.Clear();
            colaId.Clear();
            colaNum.Clear();
            colaOp.Clear();
            pilaArbol.Clear();
            pilaId.Clear();
            pilaNum.Clear();
            pilaOp.Clear();
            pilaFor.Clear();
            pilaSint.Clear();
            colaFunciones.Clear();
            pilaComa.Clear();
            colaFuncionesR.Clear();
            colaSintx.Clear();
            tablaSimGrid.Rows.Clear();
            depuradorCuad.Rows.Clear();
            lC.Clear();
            indice = 0;
            temp = 0;
         //   valorSimbolos.Clear();
            //expresion regular que separa espacios y saltos de linea y acomoda una cadena dejando los espacios en ella
            string patterTexto = @"\t|\r|\n";
            // string cadenas = @"(\u0022.#\u0022)";

            List<string> codL = new List<string>(Regex.Split(cadenaCodigo.Text, patterTexto));
            codL.RemoveAll(x => x == "");
            codigo = "";

            codigoCadena = new Dictionary<string, string>();
            int clave = 0;
            foreach (string s in codL)
            {
                if (s.Contains("\""))
                {
                    List<string> aux = new List<string>(Regex.Split(s, reservadas));
                    aux.RemoveAll(x => x == "");
                    foreach (string sa in aux)
                    {
                        if (sa.Contains("\""))
                        {
                            codigoCadena.Add("#cad" + clave + "", sa);
                            codigo += "#cad" + clave + "";
                            clave++;
                        }
                        else
                        {
                            codigo += sa;
                        }
                    }
                }
                else
                {
                    codigo += s;
                }
            }
            reducciones.Clear();
            if (evaluarExp(TablaAnalisisSintactico))
            {
                if (esquema_Traduccion() == false)//no hay error
                {
                    MessageBox.Show("Codigo Correcto");
                    busquedaPronfundidad();
                    dibujarCuadruplos();
                    foreach (var s in tablaSimbolos)
                    {
                        dibujarSimbolos(s.Key,tablaSimbolos[s.Key]," ");
                    }
                    Ejecutar.Enabled = true;
                    button2.Enabled = true;
                }
                else
                    MessageBox.Show("No se pudo compilar");
            }
            else
                MessageBox.Show("Codigo Incorrecto");
        }
        private void dibujarCuadruplos()
        {           
            foreach (Cuadruplo c in lC)
            {
                depuradorCuad.Rows.Add(c.operador, c.operador1, c.operador2, c.res);               
            }
        }
        private void actualizaSimbolos()
        {            
            tablaSimGrid.Rows.Clear();
            foreach(var s in tablaSimbolos)
            {
                if (valorSimbolos.ContainsKey(s.Key))
                {
                    dibujarSimbolos(s.Key, s.Value, valorSimbolos[s.Key]);
                }
                else
                {
                    dibujarSimbolos(s.Key, s.Value," ");
                }
            }
        }
        private void dibujarSimbolos(string variable,string tipo,string valor)
        {
            tablaSimGrid.Rows.Add(variable, tipo, valor);            
        }
        private void Ejecutar_Click(object sender, EventArgs e)
        {
            ejecutarInterprete();
        }
        #region Esquema de Traduccion
        private bool esquema_Traduccion()
        {
            tablaSimbolos = new Dictionary<string, string>();
            Token t;
            Nodo n;
            Nodo b;
            Nodo a;
            Nodo c;
            bool error = false;
            bool declaracion = false;
            bool funcion = false;
            bool bif = false;
            int title = 0;
            foreach (string prod in reducciones)
            {
                if (!error)
                {

                    switch (prod)
                    {
                        case "sent-for->for(<variable>=<const>:<const>,num){<secuencia-sent>}":
                            Nodo sent = (Nodo)pilaArbol.Pop();//sentencia
                            b = new Nodo((Token)colaNum.Dequeue());
                            a = (Nodo)pilaArbol.Pop();
                            c = new Nodo((Token)pilaComa.Pop());
                            error = creaBinario(a, c, b);
                            if (!error)
                            {
                                Nodo aux1 = (Nodo)pilaArbol.Pop();
                                b = (Nodo)pilaArbol.Pop();
                                a = (Nodo)pilaArbol.Pop();
                                Nodo puntos = new Nodo((Token)pilaFor.Pop());
                                c = new Nodo((Token)pilaFor.Pop());
                                error = creaBinario(a, c, b);
                                if (!error)
                                {
                                    a = (Nodo)pilaArbol.Pop();
                                    error = creaBinario(a, puntos, aux1);
                                    if (!error)
                                    {
                                        a = (Nodo)pilaArbol.Pop();
                                        c = new Nodo((Token)pilaSint.Pop());
                                        error = creaBinario(a, c, sent);
                                    }
                                }
                            }
                            //Nodo 
                            break;
                        case "sent-while->while(<exp>){<secuencia-sent>}":
                            c = new Nodo((Token)pilaSint.Pop());
                            b = (Nodo)pilaArbol.Pop();
                            a = (Nodo)pilaArbol.Pop();
                            error = creaBinario(a, c, b);
                            break;
                        case "def-main->Main{<secuencia-sent>}":
                            c = new Nodo((Token)colaFunciones.Dequeue());
                            if (c.token.Val == "Main")
                            {
                                b = (Nodo)pilaArbol.Pop();
                                //a = (Nodo)pilaArbol.Pop();
                                error = creaHijoder(c, b);
                            }
                            else { error = true; }
                            break;
                        case "def-func->defid{<secuencia-sent>}":
                            b = (Nodo)pilaArbol.Pop();//der
                            c = new Nodo((Token)colaFunciones.Dequeue());//centro
                            t = (Token)colaOp.Dequeue();//def
                            if (!tablaSimbolos.ContainsKey(c.token.Val))
                            {
                                tablaSimbolos.Add(c.token.Val, t.Nombre);
                                error = creaHijoder(c, b);
                                funcion = true;
                            }
                            else
                            {
                                error = true;
                            }

                            break;
                        case "sent-func->LeeTextBox(id,id);":
                        case "sent-func->ImprimeTextBox(id,id);":
                        case "sent-func->Concat(id,id);":
                        case "def-evnt->CreaEvento(id,id);":
                            n = new Nodo((Token)colaFuncionesR.Dequeue());//centro
                            a = new Nodo((Token)colaId.Dequeue());//izq
                            b = new Nodo((Token)colaId.Dequeue());//der

                            error = creaBinario(a, n, b);
                            break;
                        case "sent-func->Loop(id);":
                        case "sent-func->CierraVentana(id);":
                        case "sent-func->Mbox(id);":
                            n = new Nodo((Token)colaFuncionesR.Dequeue());
                            a = new Nodo((Token)colaId.Dequeue());
                            error = creaHijoder(n, a);
                            break;
                        case "def-vent->CreaVentana(id,id,num,num,num,num);":
                            n = new Nodo((Token)colaFuncionesR.Dequeue());//centro
                            b = new Nodo((Token)colaId.Dequeue());//izq
                            c = new Nodo((Token)colaId.Dequeue());

                            if (!tablaSimbolos.ContainsKey(c.token.Val))
                            {
                                string auxS = c.token.Val;
                                c.token.Val = "#title" + title;
                                c.token.valAsignado = auxS;
                                tablaSimbolos.Add(c.token.Val, "string");
                                title++;
                            }

                            t = (Token)pilaComa.Pop();
                            creaHijoder(new Nodo(t), c);

                            for (int i = 0; i < 4; i++)
                            {
                                a = (Nodo)pilaArbol.Pop();
                                c = new Nodo((Token)colaNum.Dequeue());
                                t = (Token)pilaComa.Pop();
                                creaBinario(a, new Nodo(t), c);
                            }

                            a = (Nodo)pilaArbol.Pop(); //der
                            error = creaBinario(b, n, a);
                            break;
                        case "crea-control->CreaTextbox(id,id,num,num,num,num);":
                            n = new Nodo((Token)colaFuncionesR.Dequeue());//centro
                            b = new Nodo((Token)colaId.Dequeue());//izq

                            c = new Nodo((Token)colaId.Dequeue());
                            t = (Token)pilaComa.Pop();
                            creaHijoder(new Nodo(t), c);

                            for (int i = 0; i < 4; i++)
                            {
                                a = (Nodo)pilaArbol.Pop();
                                c = new Nodo((Token)colaNum.Dequeue());
                                t = (Token)pilaComa.Pop();
                                creaBinario(a, new Nodo(t), c);
                            }

                            a = (Nodo)pilaArbol.Pop(); //der
                            error = creaBinario(b, n, a);

                            break;
                        case "crea-control->CreaBoton(id,id,id,num,num,num,num);":
                            n = new Nodo((Token)colaFuncionesR.Dequeue());//centro

                            b = new Nodo((Token)colaId.Dequeue());//izq

                            c = new Nodo((Token)colaId.Dequeue());

                            t = (Token)pilaComa.Pop();
                            creaHijoder(new Nodo(t), c);

                            for (int i = 0; i < 5; i++)
                            {
                                if (i == 0)
                                {
                                    a = (Nodo)pilaArbol.Pop();
                                    c = new Nodo((Token)colaId.Dequeue());
                                    if (!tablaSimbolos.ContainsKey(c.token.Val))
                                    {
                                        string auxS = c.token.Val;
                                        c.token.Val = "#title" + title;
                                        c.token.valAsignado = auxS;
                                        tablaSimbolos.Add(c.token.Val, "string");
                                        title++;
                                    }

                                    t = (Token)pilaComa.Pop();
                                    creaBinario(a, new Nodo(t), c);
                                }
                                else
                                {
                                    a = (Nodo)pilaArbol.Pop();
                                    c = new Nodo((Token)colaNum.Dequeue());
                                    t = (Token)pilaComa.Pop();
                                    creaBinario(a, new Nodo(t), c);
                                }
                            }

                            a = (Nodo)pilaArbol.Pop(); //der
                            error = creaBinario(b, n, a);
                            break;
                        case "const->id":
                        case "factor->id":
                        case "sent-func->id();":
                        case "variable->id":
                            t = (Token)colaId.Dequeue();
                            n = new Nodo(t);
                            pilaArbol.Push(n);
                            break;
                        case "identificadores->id":
                            t = (Token)colaId.Dequeue();
                            pilaId.Push(t);
                            break;
                        case "op-comparacion->\\>":
                        case "op-comparacion->==":
                        case "op-comparacion->\\<":
                        case "op-comparacion->\\<=":
                        case "op-comparacion->\\>=":
                        case "op-comparacion->!=":
                            t = (Token)colaOp.Dequeue();
                            pilaOp.Push(t);
                            break;
                        case "factor->num":
                        case "const->num":
                            t = (Token)colaNum.Dequeue();
                            n = new Nodo(t);
                            pilaArbol.Push(n);
                            break;
                        case "exp-><exp-simple><op-comparacion><exp-simple>":

                            b = (Nodo)pilaArbol.Pop();
                            a = (Nodo)pilaArbol.Pop();
                            c = new Nodo((Token)pilaOp.Pop());
                            if (Regex.IsMatch(c.token.Val, @"<|>|==|>=|<=|!="))
                            {
                                error = creaBinario(a, c, b);
                            }
                            else
                            { error = true; }
                            break;
                        case "sent-assign-><variable>:=<exp>;":
                            t = (Token)colaOp.Dequeue();
                            c = new Nodo(t);
                            if (Regex.IsMatch(t.Val, ":="))
                            {
                                b = (Nodo)pilaArbol.Pop();
                                a = (Nodo)pilaArbol.Pop();
                                error = creaBinario(a, c, b);
                            }
                            else
                            { error = true; }
                            break;
                        case "sent-if->if(<exp>){<secuencia-sent>}":
                            c = new Nodo((Token)pilaSint.Pop());
                            b = (Nodo)pilaArbol.Pop();
                            a = (Nodo)pilaArbol.Pop();
                            if (Regex.IsMatch(c.token.Val, @"if"))
                            {
                                error = creaBinario(a, c, b);
                            }
                            else
                            {
                                error = true;
                            }

                            break;
                        case "sent-if->if(<exp>){<secuencia-sent>}else{<secuencia-sent>}":
                            c = new Nodo((Token)pilaSint.Pop());//nodo if
                            c.setDer(new Nodo((Token)pilaId.Pop()));//rama if->else
                            if (Regex.IsMatch(c.token.Val, "if") && Regex.IsMatch(c.der.token.Val, "else"))
                            {
                                c.der.setDer((Nodo)pilaArbol.Pop());//rama if->else->setencia
                                b = (Nodo)pilaArbol.Pop();
                                a = (Nodo)pilaArbol.Pop();
                                error = funcion_if_else(a, c, b);
                            }
                            else { error = true; }
                            break;

                        case "sent-declara-><tipo><identificadores>;":
                            List<Token> ids = new List<Token>();
                            Token aux = (Token)pilaId.Peek();
                            while (!Regex.IsMatch(aux.Nombre, @"int|string|float|char|boton|vent|textBox|label") && pilaId.Count != 0)
                            {
                                ids.Add((Token)pilaId.Pop());
                                aux = (Token)pilaId.Peek();
                            }
                            error = funcion_Declara((Token)pilaId.Pop(), ids);
                            break;
                        case "identificadores-><identificadores>,id":
                            t = (Token)colaId.Dequeue();
                            pilaId.Push(t);

                            break;
                        case "tipo->int":
                        case "tipo->string":
                        case "tipo->char":
                        case "tipo->float":
                        case "tipo->vent":
                        case "tipo->textBox":
                        case "tipo->label":
                        case "tipo->boton":
                            declaracion = true;
                            t = (Token)colaOp.Dequeue();
                            pilaId.Push(t);
                            break;
                        case "secuencia-sent-><secuencia-sent><sentencia>":
                            t = (Token)pilaOp.Pop();

                            if (Regex.IsMatch(t.Val, ";"))
                            {
                                c = new Nodo(t);
                                b = (Nodo)pilaArbol.Pop();
                                a = (Nodo)pilaArbol.Pop();
                                error = creaBinario(a, c, b);
                            }
                            else
                            {
                                MessageBox.Show("Error de sintaxis");
                                error = true;
                            }
                            break;
                        case "exp-simple-><exp-simple><opsuma><term>":
                            try
                            {
                                b = (Nodo)pilaArbol.Pop();
                                c = (Nodo)pilaOp.Pop();
                                a = (Nodo)pilaArbol.Pop();

                                if (c.token.Val == "+" || c.token.Val == "-" || c.token.Val == "/")
                                {
                                    error = creaBinario(a, c, b);
                                }
                            }
                            catch (Exception e) { error = true; }
                            break;
                        case "opsuma->+":
                        case "opsuma->-":
                        case "opmult->*":
                        case "opmult->/":
                            t = (Token)colaOpSuma.Dequeue();
                            pilaOp.Push(new Nodo(t));
                            break;
                        case "term-><term><opmult><potencia>":
                            try
                            {
                                b = (Nodo)pilaArbol.Pop();
                                c = (Nodo)pilaOp.Pop();
                                a = (Nodo)pilaArbol.Pop();
                                if (c.token.Val == "*" || c.token.Val == "/")
                                    error = creaBinario(a, c, b);
                            }
                            catch (Exception e) { error = true; }
                            break;
                        case "potencia-><potencia>^<factor>":
                            b = (Nodo)pilaArbol.Pop();
                            a = (Nodo)pilaArbol.Pop();
                            c = new Nodo((Token)colaOpSuma.Dequeue());
                            if (c.token.Val == "^")
                                error = creaBinario(a, c, b);
                            else
                                error = true;
                            break;
                        case "sentencia-case->case<const>{<secuencia-sent>}break;":
                            b = (Nodo)pilaArbol.Pop();
                            a = (Nodo)pilaArbol.Pop();
                            c = new Nodo((Token)pilaSint.Pop());
                            error = creaBinario_case(a, c, b);
                            break;
                        case "secuencia-case-><secuencia-case><sentencia-case>":
                            c = new Nodo((Token)pilaOp.Pop());//break
                            b = (Nodo)pilaArbol.Pop();
                            a = (Nodo)pilaArbol.Pop();
                            error = creaBinario_case(a, c, b);
                            break;
                        case "sent-switch->switch(<variable>){<secuencia-case>}":
                            b = (Nodo)pilaArbol.Pop();
                            a = (Nodo)pilaArbol.Pop();
                            c = new Nodo((Token)pilaSint.Pop());
                            error = creaBinario(a, c, b);
                            break;
                    }
                }
            }
            return error;
        }
        private bool creaHijoder(Nodo c, Nodo b)
        {

            if (verificarDeclaracion(b))
            {
                c.setDer(b);
                pilaArbol.Push(c);
                return false;
            }
            else
                return true;
        }
        private bool creaHijoIzq(Nodo c, Nodo b)
        {

            if (verificarDeclaracion(b))
            {
                c.setIzq(b);
                pilaArbol.Push(c);
                return false;
            }
            else
                return true;
        }
        private bool creaBinario_case(Nodo a, Nodo c, Nodo b)
        {
            c.setIzq(a);
            c.setDer(b);
            pilaArbol.Push(c);
            return false;
        }
        private bool creaBinario(Nodo a, Nodo c, Nodo b)
        {
            if (verificarDeclaracion(a) && verificarDeclaracion(b))
            {
                c.setIzq(a);
                c.setDer(b);
                pilaArbol.Push(c);
                return false;
            }
            else
                return true;
        }
        private bool funcion_Declara(Token tipo, List<Token> tokens)
        {
            foreach (Token t in tokens)
            {
                if (!tablaSimbolos.ContainsKey(t.Val))
                {                      //variable,tipo
                    tablaSimbolos.Add(t.Val, tipo.Val);
                }
                else
                {
                    MessageBox.Show("Se intento declarar una variable ya existente");
                    return true;
                }
            }
            if (tokens.Count != 0)
            {
                return false;
            }
            else { return true; }
        }
        private bool verificarDeclaracion(Nodo nodo)
        {
            if (nodo.token.Nombre == "id")
            {
                if (tablaSimbolos.Keys.Contains(nodo.token.Val))
                    return true;
                else
                {
                    MessageBox.Show("la variable " + nodo.token.Val + " no existe");
                    return false;
                }
            }
            else if (palabraR.Exists(x => x != nodo.token.Val))
                return true;
            else
            {
                MessageBox.Show("Una o mas variables no declaradas");
                return false;
            }
        }
        private bool funcion_if_else(Nodo a, Nodo nodoifelse, Nodo b)
        {
            try
            {
                nodoifelse.setIzq(a);
                nodoifelse.der.setIzq(b);
                pilaArbol.Push(nodoifelse);
                return false;
            }
            catch (Exception e) { return true; }

        }
        private void funcion_if_endif(Nodo a, Nodo nodoif, Nodo b)
        {
            nodoif.setIzq(a);
            nodoif.setDer(b);
            pilaArbol.Push(nodoif);
        }


        #endregion
        #region codigo intermedio
        private void busquedaPronfundidad()
        {
            pilaId.Clear();
            pilaNum.Clear();
            Nodo main = (Nodo)pilaArbol.Pop();

            while (pilaArbol.Count != 0)
            {
                Nodo n = (Nodo)pilaArbol.Pop();
                Cuadruplo cuadruplo = new Cuadruplo(n.token.Val, "", "", "");
                cuadruplo.indx = indice;
                indice++;
                lC.Add(cuadruplo);
                if (n.izq != null)
                {
                    Buscar(n.izq, false, true);

                }
                if (n.der != null)
                {
                    Buscar(n.der, true, false);

                }
                funcionesCuad.Add(new List<Cuadruplo>());
                foreach (Cuadruplo c in lC)
                {
                    funcionesCuad[funcionesCuad.Count - 1].Add(c);
                }

                indice = 0;
                tipoTemp = "";
                lC.Clear();
                pilaId.Clear();
                pilaNum.Clear();
                pilaResultados.Clear();
            }
            if (main.izq != null)
            {
                Buscar(main.izq, false, true);
            }
            if (main.der != null)
            {
                Buscar(main.der, true, false);
            }
        }
        private Tuple<bool, string, string> Buscar(Nodo n, bool der, bool izq)
        {
            string top1 = "", top2 = "";
            string temporal = temporalA;
            Tuple<bool, string, string> operadores = new Tuple<bool, string, string>(false, top1, top2);
            Cuadruplo c;

            if (n.izq != null && n.der != null)
            {
                switch (n.token.Val)
                {
                    case "CreaVentana":
                    case "CreaTextbox":
                    case "CreaBoton":
                        operadores = Buscar(n.izq, false, true);
                        top1 = operadores.Item2;
                        c = new Cuadruplo(n.token.Val, "", "", tablaSimbolos[top1]);
                        c.indx = indice;
                        indice++;
                        lC.Add(c);
                        operadores = Buscar(n.der, true, false);
                        operadores = new Tuple<bool, string, string>(false, "", "");
                        break;
                    case "CreaEvento":
                        operadores = Buscar(n.izq, false, true);
                        top1 = operadores.Item2;
                        operadores = Buscar(n.der, true, false);
                        top2 = operadores.Item3;
                        pilaSint.Push(new Evento(top1, top2));
                        break;
                    case "Concat":
                    case "LeeTextBox":
                    case "ImprimeTextBox":
                        operadores = Buscar(n.izq, false, true);
                        top1 = operadores.Item2;
                        operadores = Buscar(n.der, true, false);
                        top2 = operadores.Item3;
                        c = new Cuadruplo(n.token.Val, top1, top2, "");
                        c.indx = indice;
                        indice++;
                        lC.Add(c);
                        operadores = new Tuple<bool, string, string>(false, "", "");
                        break;
                    case "Mbox":
                    case "Loop":
                    case "CierraVentana":
                        if (n.izq != null)
                        {
                            operadores = Buscar(n.izq, false, true);
                            top1 = operadores.Item2;
                            c = new Cuadruplo(n.token.Val, top1, "", "");
                            c.indx = indice;
                            indice++;
                            lC.Add(c);
                            // operadores = Buscar(n.der, true, false);
                            operadores = new Tuple<bool, string, string>(false, "", "");

                        }
                        else if (n.der != null)
                        {
                            operadores = Buscar(n.der, true, false);
                            top1 = operadores.Item3;
                            c = new Cuadruplo(n.token.Val, top1, "", "");
                            c.indx = indice;
                            indice++;
                            lC.Add(c);
                            // operadores = Buscar(n.der, true, false);
                            operadores = new Tuple<bool, string, string>(false, "", "");
                        }
                        break;
                    case "switch":
                        pilaId.Clear();
                        pilaNum.Clear();
                        operadores = Buscar(n.izq, false, true);
                        op1 = operadores.Item2;

                        operadores = Buscar(n.der, true, false);


                        c = new Cuadruplo("endSwitch", "", "", "");
                        c.indx = indice;
                        indice++;
                        lC.Add(c);

                        while (pilaId.Count != 0)
                        {
                            //goto
                            int y = Convert.ToInt32((string)pilaId.Pop());
                            lC[y].operador1 = c.indx.ToString();

                            //gotoFalse
                            int x = Convert.ToInt32((string)pilaId.Pop());
                            lC[x].operador2 = (y + 1).ToString();
                        }
                        operadores = new Tuple<bool, string, string>(false, "", "");

                        break;
                    case "case":
                        operadores = Buscar(n.izq, false, true);
                        if (operadores.Item1) { top1 = (string)pilaResultados.Pop(); }
                        else { top1 = operadores.Item2; }
                        temp++;
                        temporal = temporal + "" + temp;
                        c = new Cuadruplo("==", top1, op1, temporal);
                        c.indx = indice;
                        indice++;
                        lC.Add(c);
                        tablaSimbolos.Add(temporal, "bool");

                        c = new Cuadruplo("gotoFalse", temporal, "", "");
                        c.indx = indice;
                        indice++;
                        lC.Add(c);
                        pilaId.Push((c.indx).ToString());

                        operadores = Buscar(n.der, true, false);
                        if (operadores.Item1) { top2 = (string)pilaResultados.Pop(); }
                        else { top2 = operadores.Item3; }

                        c = new Cuadruplo("goto", "", "", "");
                        c.indx = indice;
                        indice++;
                        lC.Add(c);
                        pilaId.Push((c.indx).ToString());

                        operadores = new Tuple<bool, string, string>(false, "", "");
                        break;
                    case ";":
                        Buscar(n.izq, false, true);
                        Buscar(n.der, true, false);
                        break;
                    case ":=":
                        operadores = Buscar(n.izq, false, true);
                        if (operadores.Item1) { top1 = (string)pilaResultados.Pop(); }
                        else
                        {
                            top1 = operadores.Item2;
                            tipoTemp = tablaSimbolos[top1];
                        }
                        operadores = Buscar(n.der, true, false);
                        if (operadores.Item1) { top2 = (string)pilaResultados.Pop(); }
                        else { top2 = operadores.Item3; }

                        c = new Cuadruplo(n.token.Val, top2, "", top1);
                        c.indx = indice;
                        indice++;
                        lC.Add(c);
                        break;
                    case "=":
                        operadores = Buscar(n.izq, false, true);
                        if (operadores.Item1) { top1 = (string)pilaResultados.Pop(); }
                        else
                        {
                            top1 = operadores.Item2;
                            tipoTemp = tablaSimbolos[top1];
                        }
                        operadores = Buscar(n.der, true, false);
                        if (operadores.Item1) { top2 = (string)pilaResultados.Pop(); }
                        else { top2 = operadores.Item3; }
                        temp++;
                        temporal = temporal + "" + temp;
                        tipoTemp = tablaSimbolos[top1];
                        if (!tablaSimbolos.ContainsKey(temporal)) { tablaSimbolos.Add(temporal, tipoTemp); }

                        c = new Cuadruplo(n.token.Val, top1, top2, temporal);
                        c.indx = indice;
                        indice++;
                        lC.Add(c);
                        pilaResultados.Push(temporal);
                        op1 = top1;
                        operadores = new Tuple<bool, string, string>(true, "", "");
                        break;
                    case "+":
                    case "-":
                    case "*":
                    case "/":
                        operadores = Buscar(n.izq, false, true);
                        if (operadores.Item1) { top1 = (string)pilaResultados.Pop(); }
                        else { top1 = operadores.Item2; }

                        operadores = Buscar(n.der, true, false);
                        if (operadores.Item1) { top2 = (string)pilaResultados.Pop(); }
                        else { top2 = operadores.Item3; }

                        temp++;
                        temporal = temporal + "" + temp;
                        if (!tablaSimbolos.ContainsKey(temporal)) { tablaSimbolos.Add(temporal, tipoTemp); }

                        c = new Cuadruplo(n.token.Val, top1, top2, temporal);
                        c.indx = indice;
                        indice++;

                        lC.Add(c);
                        pilaResultados.Push(temporal);
                        operadores = new Tuple<bool, string, string>(true, "", "");
                        break;
                    case "while":
                        operadores = Buscar(n.izq, false, true);
                        if (operadores.Item1) { top1 = (string)pilaResultados.Pop(); }

                        c = new Cuadruplo("gotoFalse", top1, "", "");
                        c.indx = indice;
                        indice++;

                        int i = c.indx;
                        lC.Add(c);

                        operadores = Buscar(n.der, true, false);
                        //top2 = operadores.Item3;

                        c = new Cuadruplo("goto", (i - 1).ToString(), "", "");
                        c.indx = indice;
                        indice++;

                        lC.Add(c);

                        c = new Cuadruplo("endWhile", "", "", "");
                        c.indx = indice;
                        indice++;
                        lC.Add(c);

                        lC[i].operador2 = (lC.Count - 1).ToString();
                        operadores = new Tuple<bool, string, string>(false, "", "");

                        break;
                    case "!=":
                    case ">":
                    case "<":
                    case ">=":
                    case "<=":
                    case "==":
                        operadores = Buscar(n.izq, false, true);
                        if (operadores.Item1) { top1 = (string)pilaResultados.Pop(); }
                        else { top1 = operadores.Item2; }

                        operadores = Buscar(n.der, true, false);
                        if (operadores.Item1) { top2 = (string)pilaResultados.Pop(); }
                        else { top2 = operadores.Item3; }

                        temp++;
                        temporal = temporal + "" + temp;
                        if (!tablaSimbolos.ContainsKey(temporal)) { tablaSimbolos.Add(temporal, "bool"); }

                        c = new Cuadruplo(n.token.Val, top1, top2, temporal);
                        c.indx = indice;
                        indice++;

                        lC.Add(c);
                        pilaResultados.Push(temporal);
                        operadores = new Tuple<bool, string, string>(true, "", "");
                        break;
                    case "if":
                        operadores = Buscar(n.izq, false, true);
                        if (operadores.Item1) { top1 = (string)pilaResultados.Pop(); }
                        c = new Cuadruplo("gotoFalse", top1, "", "");
                        c.indx = indice;
                        indice++;

                        int j = c.indx;
                        lC.Add(c);

                        operadores = Buscar(n.der, true, false);
                        if (operadores.Item1)
                        {
                            c = new Cuadruplo("endIf", "", "", "");
                            c.indx = indice;
                            indice++;

                            lC.Add(c);
                            top2 = operadores.Item2;
                            lC[Convert.ToInt32(top2)].operador1 = (lC.Count - 1).ToString();
                            lC[j].operador2 = (Convert.ToInt32(top2) + 1).ToString();
                        }
                        else
                        {

                            c = new Cuadruplo("endIf", "", "", "");
                            c.indx = indice;
                            indice++;

                            lC.Add(c);
                            lC[j].operador2 = (lC.Count - 1).ToString();
                        }

                        break;
                    case "else":
                        operadores = Buscar(n.izq, false, true);
                        //if (operadores.Item1) { top1 = (string)pilaResultados.Pop(); }
                        c = new Cuadruplo("goto", "", "", "");
                        c.indx = indice;
                        indice++;
                        int z = c.indx;
                        lC.Add(c);
                        operadores = Buscar(n.der, true, false);
                        // if (operadores.Item1) { top2 = (string)pilaResultados.Pop(); }

                        operadores = new Tuple<bool, string, string>(true, z.ToString(), "");
                        break;
                    case "for":
                        string aux1 = "";
                        operadores = Buscar(n.izq, false, true);
                        if (operadores.Item1)
                        {
                            top1 = (string)pilaResultados.Pop();
                            aux1 = operadores.Item2;
                            top2 = operadores.Item3;
                        }
                        else { top1 = operadores.Item2; }
                        c = new Cuadruplo("gotoFalse", top1, "", "");
                        c.indx = indice;
                        indice++;
                        int a = c.indx;
                        lC.Add(c);

                        c = new Cuadruplo("+", aux1, top2, aux1);
                        c.indx = indice;
                        indice++;
                        lC.Add(c);
                        c = new Cuadruplo(":=", aux1, "", op1);
                        c.indx = indice;
                        indice++;
                        lC.Add(c);

                        operadores = Buscar(n.der, true, false);

                        top1 = (a - 1).ToString();
                        c = new Cuadruplo("goto", top1, "", "");
                        c.indx = indice;
                        indice++;
                        lC.Add(c);

                        c = new Cuadruplo("endFor", "", "", "");
                        c.indx = indice;
                        indice++;
                        lC.Add(c);
                        lC[a].operador2 = c.indx.ToString();
                        break;
                    case ":":
                        operadores = Buscar(n.izq, false, true);
                        if (operadores.Item1) { top1 = (string)pilaResultados.Pop(); }
                        else { top1 = operadores.Item2; }

                        operadores = Buscar(n.der, true, false);
                        if (operadores.Item1) { top2 = (string)pilaResultados.Pop(); }
                        else { top2 = operadores.Item3; }
                        string aux = top2;
                        top2 = (string)pilaResultados.Pop();
                        temp++;
                        temporal = temporal + "" + temp;
                        if (!tablaSimbolos.ContainsKey(temporal)) { tablaSimbolos.Add(temporal, "bool"); }
                        c = new Cuadruplo(":", top1, top2, temporal);
                        c.indx = indice;
                        indice++;
                        lC.Add(c);
                        pilaResultados.Push(temporal);

                        operadores = new Tuple<bool, string, string>(true, top1, aux);

                        break;
                    case ",":
                        if (n.izq != null)
                        {
                            operadores = Buscar(n.izq, false, true);
                            if (operadores.Item2 != "")
                            {
                                top1 = operadores.Item2;
                            }
                            else { top1 = operadores.Item3; }
                            if (top1 != "")
                            {
                                temp++;
                                temporal = temporal + "" + temp;
                                c = new Cuadruplo(",", top1, "", temporal);
                                c.indx = indice;
                                indice++;
                                string tipo = "";
                                if (top1.Contains("\"")){
                                    tipo = "string";
                                }
                                else if (top1.Contains("."))
                                {
                                    tipo = "float";
                                }
                                else
                                {
                                    tipo = "int";
                                }
                                tablaSimbolos.Add(temporal, tipo);
                                pilaResultados.Push(temporal);
                                lC.Add(c);
                            }
                        }
                        if (n.der != null)
                        {
                            temporal = temporalA;
                            operadores = Buscar(n.der, true, false);
                            if (operadores.Item3 != "")
                            {
                                top2 = operadores.Item3;
                            }
                            else { top2 = operadores.Item2; }
                            if (top2 != "")
                            {
                                temp++;
                                temporal = temporal + "" + temp;
                                c = new Cuadruplo(",", top2, "", temporal);
                                c.indx = indice;
                                indice++;
                                string tipo = "";
                                if (top1.Contains("\"")){
                                    tipo = "string";
                                }
                                else if (top1.Contains("."))
                                {
                                    tipo = "float";
                                }
                                else
                                {
                                    tipo = "int";
                                }
                                tablaSimbolos.Add(temporal, tipo);
                                pilaResultados.Push(temporal);
                                lC.Add(c);
                            }
                        }
                        operadores = new Tuple<bool, string, string>(true, "", "");
                        break;
                }
                return operadores;
            }
            else if (n.izq != null && n.der == null)
            {
                operadores = Buscar(n.izq, false, true);
            }
            else if (n.izq == null && n.der != null)
            {
                operadores = Buscar(n.der, true, false);

            }
            else if (n.izq == null && n.der == null)
            {
                if (izq)
                {
                    if (tablaSimbolos.ContainsKey(n.token.Val) && tablaSimbolos[n.token.Val] == "def")
                    {
                        int idx = funcionesCuad.FindIndex(x => x[0].operador == n.token.Val);
                        c = new Cuadruplo(n.token.Val, idx.ToString(), "", "");
                        c.indx = indice;
                        indice++;
                        lC.Add(c);
                    }
                    else if (tablaSimbolos.ContainsKey(n.token.Val) && n.token.Val.Contains("#title"))
                    {
                        operadores = new Tuple<bool, string, string>(false, n.token.valAsignado, "");
                    }
                    else
                    {
                        operadores = new Tuple<bool, string, string>(false, n.token.Val, "");
                    }
                }
                else if (der)
                {
                    if (tablaSimbolos.ContainsKey(n.token.Val) && tablaSimbolos[n.token.Val] == "def")
                    {
                        int idx = funcionesCuad.FindIndex(x => x[0].operador == n.token.Val);
                        c = new Cuadruplo(n.token.Val, idx.ToString(), "", "");
                        c.indx = indice;
                        indice++;
                        lC.Add(c);
                    }
                    else if (tablaSimbolos.ContainsKey(n.token.Val) && n.token.Val.Contains("#title"))
                    {
                        operadores = new Tuple<bool, string, string>(false, "", n.token.valAsignado);
                    }
                    else
                    {
                        operadores = new Tuple<bool, string, string>(false, "", n.token.Val);
                    }
                }
            }


            return operadores;
        }
        #endregion
        #region interprete
        private void ejecutarInterprete()
        {
            int cont = 0;
            valorSimbolos = new Dictionary<string, string>();
            do
            {
                cont = interprete(lC[cont],cont);
                actualizaSimbolos();
            } while (cont < lC.Count);
        }
        private int interprete(Cuadruplo c,int pc)
        {
            
            switch (c.operador)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                    if (tablaSimbolos.ContainsKey(c.operador1) && (tablaSimbolos.ContainsKey(c.operador2)))
                    {
                        if (tablaSimbolos[c.operador1] == tablaSimbolos[c.operador2])
                        {
                            SumMulti(tablaSimbolos[c.operador1], c.operador,valorSimbolos[c.operador1],valorSimbolos[c.operador2], c.res);
                        }
                        pc++;
                    }
                    else if (tablaSimbolos.ContainsKey(c.operador1) || tablaSimbolos.ContainsKey(c.operador2))
                    {
                        string tipo = "";
                        if (tablaSimbolos.ContainsKey(c.operador1))
                        {
                            tipo = tablaSimbolos[c.operador1];
                            SumMulti(tipo, c.operador, valorSimbolos[c.operador1], c.operador2, c.res);
                        }
                        else if(tablaSimbolos.ContainsKey(c.operador2) )
                        {
                            tipo = tablaSimbolos[c.operador2];
                            SumMulti(tipo, c.operador, c.operador1,valorSimbolos[c.operador2], c.res);
                        }
                        else
                        {
                                tipo = tablaSimbolos[c.operador2];
                                SumMulti(tipo, c.operador, c.operador1, c.operador2, c.res);                           
                        }
                        pc++;
                    }
                    else
                    {
                        if (c.operador1.Contains(".") || c.operador2.Contains("."))
                        {
                            SumMulti("float", c.operador, c.operador1, c.operador2, c.res);
                            pc++;
                        }
                        else
                        {
                            SumMulti("int", c.operador, c.operador1, c.operador2, c.res);
                            pc++;
                        }
                    }
                    break;
                case ":=":
                    if (tablaSimbolos.ContainsKey(c.operador1))
                    {
                        valorSimbolos[c.res]=valorSimbolos[c.operador1];
                    }
                    else
                    {
                        valorSimbolos.Add(c.res, c.operador1);
                    }
                    pc++;
                    break;
                case "gotoFalse":
                    if (tablaSimbolos.ContainsKey(c.operador1))
                    {
                        if (valorSimbolos[c.operador1] == "false")
                        {
                            pc = Convert.ToInt32(c.operador2);
                        }
                        else
                        {
                            pc++;
                        }
                    }
                    break;
                case "goto":
                    if (tablaSimbolos.ContainsKey(c.operador1))
                    {
                        pc = Convert.ToInt32(valorSimbolos[c.operador1]);
                    }
                    else
                    {
                           pc = Convert.ToInt32(c.operador1);                        
                    }
                    break;
                case "<":
                case ">":
                case ">=":
                case "<=":
                case "==":
                case "!=":
                case ":":
                    if (tablaSimbolos.ContainsKey(c.operador1) && tablaSimbolos.ContainsKey(c.operador2))
                    {
                        if (tablaSimbolos[c.operador1] == tablaSimbolos[c.operador2])
                        {
                            condicionalesTipo(tablaSimbolos[c.operador1], c.operador, valorSimbolos[c.operador1],valorSimbolos[c.operador2], c.res);
                        }
                    }
                    else if (tablaSimbolos.ContainsKey(c.operador1) && !tablaSimbolos.ContainsKey(c.operador2))
                    {
                        if (c.operador2.Contains("\"") && tablaSimbolos[c.operador1]=="string") { condicionalesTipo("string", c.operador,valorSimbolos[c.operador1], c.operador2, c.res); }
                        else if (c.operador2.Contains(".") && tablaSimbolos[c.operador1]=="float") { condicionalesTipo("float", c.operador,valorSimbolos[c.operador1], c.operador2, c.res); }
                        else if(tablaSimbolos[c.operador1] == "int") { condicionalesTipo("int", c.operador,valorSimbolos[c.operador1], c.operador2, c.res); }
                    }
                    else if (tablaSimbolos.ContainsKey(c.operador2) && !tablaSimbolos.ContainsKey(c.operador1))
                    {
                        if (c.operador1.Contains("\"") && tablaSimbolos[c.operador2] == "string") { condicionalesTipo("string", c.operador, c.operador1,valorSimbolos[c.operador2], c.res); }
                        else if (c.operador1.Contains(".") && tablaSimbolos[c.operador2] == "float") { condicionalesTipo("float", c.operador, c.operador1,valorSimbolos[c.operador2], c.res); }
                        else if (tablaSimbolos[c.operador2] == "int") { condicionalesTipo("int", c.operador, c.operador1,valorSimbolos[c.operador2], c.res); }
                    }
                    pc++;
                    break;
                case "endWhile":
                case "endIf":
                case "endFor":
                case "endSwitch":
                    pc++;
                    break;
                case "=":
                    if (tablaSimbolos.ContainsKey(c.operador1) && tablaSimbolos.ContainsKey(c.operador2))
                    {
                        valorSimbolos.Add(c.res, valorSimbolos[c.operador2]);
                    }
                    else
                    {
                        valorSimbolos.Add(c.res, c.operador2);
                    }
                    pc++;
                    break;
                case ",":
                    if (tablaSimbolos.ContainsKey(c.operador1))
                    {
                        valorSimbolos.Add(c.res, valorSimbolos[c.operador1]);
                    }
                    else
                    {
                        valorSimbolos.Add(c.res, c.operador1);
                    }
                    pc++;
                    break;
            }
            return pc;
        }
        private void condicionalesTipo(string tipo,string op,string op1,string op2,string res)
        {
            int a, b;
            float d, e;
            switch (tipo)
            {
                case "int":
                    a = Convert.ToInt32(op1);
                    b = Convert.ToInt32(op2);
                    condicionInt(op, a, b, res);
                    break;
                case "float":
                    e = Convert.ToSingle(op1);
                    d = Convert.ToSingle(op2);
                    condicionFloat(op, e, d, res);
                    break;
                case "string":
                    if (op1 == op2) { valorSimbolos.Add(res, "true"); }
                    else { valorSimbolos.Add(res, "false"); }
                    break;
                case "char":
                    if (op1[0] == op2[0]) { valorSimbolos.Add(res, "true"); }
                    else { valorSimbolos.Add(res, "false"); }
                    break;
            }
        }
        private void condicionFloat(string condicion, float a, float b, string res)
        {
            switch (condicion)
            {
                case "<":
                case ":":
                    if (a < b)
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "true";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "true");
                        }
                    }
                    else
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "false";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "false");
                        }
                    }
                    break;
                case ">":
                    if (a > b)
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "true";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "true");
                        }

                    }
                    else
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "false";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "false");
                        }
                    }
                    break;
                case "==":
                    if (a == b)
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "true";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "true");
                        }
                    }
                    else
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "false";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "false");
                        }
                    }
                    break;
                case "!=":
                    if (a != b)
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "true";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "true");
                        }
                    }
                    else
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "false";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "false");
                        }
                    }
                    break;
                case ">=":
                    if (a >= b)
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "true";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "true");
                        }
                    }
                    else
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "false";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "false");
                        }
                    }
                    break;
                case "<=":
                    if (a <= b)
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "true";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "true");
                        }
                    }
                    else
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "false";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "false");
                        }
                    }
                    break;
            }           
        }
        private void condicionInt(string condicion,int a,int b,string res)
        {
            switch (condicion)
            {
                case "<":
                case ":":
                    if (a < b)
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] ="true";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "true");
                        }
                    }
                    else
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "false";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "false");
                        }
                    }
                    break;
                case ">":
                    if (a > b)
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "true";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "true");
                        }
                        
                    }
                    else
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "false";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "false");
                        }
                    }
                    break;
                case "==":
                    if (a == b)
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "true";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "true");
                        }
                    }
                    else
                    {
                       if(valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "false";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "false");
                        }
                    }
                    break;
                case "!=":
                    if (a != b)
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "true";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "true");
                        }
                    }
                    else
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "false";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "false");
                        }
                    }
                    break;
                case ">=":
                    if (a >= b)
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "true";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "true");
                        }
                    }
                    else
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "false";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "false");
                        }
                    }
                    break;
                case "<=":
                    if (a <= b)
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "true";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "true");
                        }
                    }
                    else
                    {
                        if (valorSimbolos.ContainsKey(res))
                        {
                            valorSimbolos[res] = "false";
                        }
                        else
                        {
                            valorSimbolos.Add(res, "false");
                        }
                    }
                    break;
            }
        }

        

        private void SumMulti(string tipo,string op, string op1, string op2, string res)
        {
            if (tipo == "int")
            {
                int t = 0;
                switch (op)
                {
                    case "+":
                        t = Convert.ToInt32(op1) + Convert.ToInt32(op2);                       
                        break;
                    case "-":
                        t = Convert.ToInt32(op1) - Convert.ToInt32(op2);                       
                        break;
                    case "*":
                        t = Convert.ToInt32(op1) * Convert.ToInt32(op2);                       
                        break;
                    case "/":
                        t = Convert.ToInt32(op1) / Convert.ToInt32(op2);                       
                        break;
                }
                if (valorSimbolos.ContainsKey(res))
                {
                    valorSimbolos[res] = t.ToString();
                }
                else
                {
                    valorSimbolos.Add(res, t.ToString());
                }
            }
            else if (tipo == "float")
            {
                float t = 0;
                switch (op)
                {
                    case "+":
                        t = Convert.ToInt32(op1) + Convert.ToInt32(op2);                        
                        break;
                    case "-":
                        t = Convert.ToInt32(op1) - Convert.ToInt32(op2);
                        break;
                    case "*":
                        t = Convert.ToInt32(op1) * Convert.ToInt32(op2);
                        break;
                    case "/":
                        t = Convert.ToInt32(op1) / Convert.ToInt32(op2);
                        break;
                }
                valorSimbolos.Add(res, t.ToString());
            }            
        }
        #endregion
        #region depurador
        private void button2_Click(object sender, EventArgs e)
        {
            int cont = 0;
            valorSimbolos = new Dictionary<string, string>();
            tablaSimGrid.Rows.Clear();
            tablaSimGrid.Rows.Clear();
            temp = 0;
            foreach (var s in tablaSimbolos)
            {
                dibujarSimbolos(s.Key, tablaSimbolos[s.Key], " ");
            }
            while (cont < depuradorCuad.CurrentRow.Index)
            {
                cont = interprete(lC[cont], cont);
                actualizaSimbolos();
            }
            if (cont == depuradorCuad.CurrentRow.Index)
            {
                contDepurador = depuradorCuad.CurrentRow.Index;
                button3.Enabled = true;
                button2.Enabled = false;
                MessageBox.Show("Inicio la depuracion");
            }
            else if(cont > depuradorCuad.CurrentRow.Index)
            {
                contDepurador = depuradorCuad.CurrentRow.Index;
                MessageBox.Show("Termino la ejecucion");
                button3.Enabled = false;
                button2.Enabled = true;
            }
        }

        private void depura()
        {
            indice = contDepurador;
            contDepurador = interprete(lC[contDepurador], contDepurador);
            actualizaSimbolos();            
        }
        private void button3_Click(object sender, EventArgs e)
        {            
            depura();
            depuradorCuad.Rows[indice].Selected = false;
            depuradorCuad.Rows[contDepurador].Selected = true;
            if (contDepurador == lC.Count-1)
            {
                MessageBox.Show("Termino la ejecucion");
                button3.Enabled = false;
                button2.Enabled = true;
            }
        }

        #endregion
    }
}