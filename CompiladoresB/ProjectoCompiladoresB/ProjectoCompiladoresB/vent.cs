using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ProjectoCompiladoresB
{
    class vent
    {
        public string id;
        public string nombre;
        public string posX;
        public string posY;
        public string alto;
        public string ancho;
        public vent(string i,string nom,string x,string y,string h,string w)
        {
            id = i;
            nombre = nom;
            posX = x;
            posY = y;
            alto = h;
            ancho = w;
        }

        public void creaVentana()
        {
            Form f = new Form();
            f.Height = Convert.ToInt32(alto);
            f.Width = Convert.ToInt32(ancho);
            f.Name = nombre;
            f.Show();
            
        }
    }

}
