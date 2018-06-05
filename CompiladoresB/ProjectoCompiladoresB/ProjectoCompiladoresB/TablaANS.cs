using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectoCompiladoresB
{
    public partial class TablaANS : Form
    {
        List<string> gramatica;
        List<char> term;
        List<string> prod;
        Dictionary<int, Dictionary<string, string>> tabla;
        public TablaANS(List<string> gramOriginal, List<char> simbTerminales, List<string> simbProductor, Dictionary<int, Dictionary<string, string>> tablaAnS)
        {
            gramatica = gramOriginal;
            term = simbTerminales;
            prod = simbProductor;
            tabla = tablaAnS;
            InitializeComponent();
        }

        private void TablaANS_Load(object sender, EventArgs e)
        {
            LlenarTablaAS(gramatica, term, prod, tabla);
        }
        #region dibujarTabla
        /*******************************************************************
         * Pinta el listView donde se imprime la 
         * tabla de analisis sintactico
         * ****************************************************************/
        public void LlenarTablaAS(List<string> gramOriginal, List<char> simbTerminales, List<string> simbProductor, Dictionary<int, Dictionary<string, string>> tablaAnS)
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

                for (; index < tablaAS.Columns.Count; index++)
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
    }
}
