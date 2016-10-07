using System.Collections.Generic;

namespace GestorJRF.POJOS
{
    public class Empresa
    {
        private string nombre { get; set; }
        private string cif { get; set; }
        private string domicilio { get; set; }
        private string localidad { get; set; }
        private string provincia { get; set; }
        private int cp { get; set; }
        private string telefono { get; set; }
        private string email { get; set; }
        private List<PersonaContacto> personasContacto { get; set; }

        public Empresa() { }

        public Empresa(string nombre, string cif, string domicilio, string localidad, string provincia, int cp, string telefono, string email, List<PersonaContacto> personasContacto)
        {
            this.nombre = nombre;
            this.cif = cif;
            this.domicilio = domicilio;
            this.localidad = localidad;
            this.provincia = provincia;
            this.cp = cp;
            this.telefono = telefono;
            this.email = email;
            this.personasContacto = new List<PersonaContacto>(personasContacto);
        }
    }
}
