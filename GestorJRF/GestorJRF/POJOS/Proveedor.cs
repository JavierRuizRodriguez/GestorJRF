using System;

namespace GestorJRF.POJOS
{
    public class Proveedor
    {
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

        public string domicilio { get; set; }
        public string localidad { get; set; }
        public string provincia { get; set; }
        public int cp { get; set; }

        public Proveedor()
        {
        }

        public Proveedor(string nombre, string cif, string domicilio, string localidad, string provincia, int cp)
        {
            this.nombre = nombre;
            this.cif = cif;
            this.domicilio = domicilio;
            this.localidad = localidad;
            this.provincia = provincia;
            this.cp = cp;
        }

        public Proveedor(string nombre, string cif, string cifAntiguo, string domicilio, string localidad, string provincia, int cp)
        {
            this.nombre = nombre;
            this.cif = cif;
            this.cifAntiguo = cifAntiguo;
            this.domicilio = domicilio;
            this.localidad = localidad;
            this.provincia = provincia;
            this.cp = cp;
        }

        internal string generarDireccion()
        {
            return (domicilio + " ," + localidad + " ," + provincia + " ," + cp).ToUpper();
        }
    }
}
