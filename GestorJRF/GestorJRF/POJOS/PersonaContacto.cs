using System;

namespace GestorJRF.POJOS
{
    public class PersonaContacto
    {
        public string nombre { get; set; }
        public int telefono { get; set; }
        public string email { get; set; }
        public string cif { get; set; }

        public PersonaContacto() { }

        public PersonaContacto(string nombre, int telefono, string email)
        {
            this.nombre = nombre;
            this.telefono = telefono;
            this.email = email;
        }

        public PersonaContacto(string nombre, int telefono, string email, string cif)
        {
            this.nombre = nombre;
            this.telefono = telefono;
            this.email = email;
            this.cif = cif;
        }
    }
}
