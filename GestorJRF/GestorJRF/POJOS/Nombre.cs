using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorJRF.POJOS
{
    class Nombre
    {
        public string nombre { get; set; }
        public string apellidos { get; set; }

        public Nombre() { }

        public Nombre(string nombre, string apellidos)
        {
            this.nombre = nombre;
            this.apellidos = apellidos;
        }
    }
}
