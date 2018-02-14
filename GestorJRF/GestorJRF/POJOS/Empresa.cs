using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GestorJRF.POJOS
{
    public class Empresa
    {
        public List<PersonaContacto> personasContacto;

        public string nombre
        {
            get;
            set;
        }

        public string cif
        {
            get;
            set;
        }

        public string cifAntiguo
        {
            get;
            set;
        }

        public string domicilio
        {
            get;
            set;
        }

        public string localidad
        {
            get;
            set;
        }

        public string provincia
        {
            get;
            set;
        }

        public string cp
        {
            get;
            set;
        }

        public int telefono
        {
            get;
            set;
        }

        public Empresa()
        {
        }

        public Empresa(string nombre, string cif, string domicilio, string localidad, string provincia, string cp, int telefono, ObservableCollection<PersonaContacto> personasContacto)
        {
            this.nombre = nombre;
            this.cif = cif;
            this.domicilio = domicilio;
            this.localidad = localidad;
            this.provincia = provincia;
            this.cp = cp;
            this.telefono = telefono;
            this.personasContacto = new List<PersonaContacto>(personasContacto);
        }

        public Empresa(string nombre, string cif, string cifAntiguo, string domicilio, string localidad, string provincia, string cp, int telefono, ObservableCollection<PersonaContacto> personasContacto)
        {
            this.nombre = nombre;
            this.cif = cif;
            this.cifAntiguo = cifAntiguo;
            this.domicilio = domicilio;
            this.localidad = localidad;
            this.provincia = provincia;
            this.cp = cp;
            this.telefono = telefono;
            this.personasContacto = new List<PersonaContacto>(personasContacto);
        }

        internal string generarDireccion()
        {
            return (domicilio + " ," + localidad + " ," + provincia + " ," + cp).ToUpper();
        }
    }
}
