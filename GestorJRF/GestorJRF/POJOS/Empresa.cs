using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GestorJRF.POJOS
{
    public class Empresa
    {
        public string nombre { get; set; }
        public string cif { get; set; }
        public string cifAntiguo { get; set; }
        public string domicilio { get; set; }
        public string localidad { get; set; }
        public string provincia { get; set; }
        public int cp { get; set; }
        public int telefono { get; set; }
        public string email { get; set; }
        public List<PersonaContacto> personasContacto;
        public Empresa() { }

        public Empresa(string nombre, string cif, string domicilio, string localidad, string provincia, int cp, int telefono, string email, ObservableCollection<PersonaContacto> personasContacto)
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

        public Empresa(string nombre, string cif, string cifAntiguo, string domicilio, string localidad, string provincia, int cp, int telefono, string email, ObservableCollection<PersonaContacto> personasContacto)
        {
            this.nombre = nombre;
            this.cif = cif;
            this.cifAntiguo = cifAntiguo;
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
