using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorJRF.POJOS
{
    public class PersonaContacto
    {
        private string nombre { get; set; }
        private int telefono { get; set; }
        private string email { get; set; }

        public PersonaContacto(string nombre, int telefono, string email)
        {
            this.nombre = nombre;
            this.telefono = telefono;
            this.email = email;
        }
    }
}
