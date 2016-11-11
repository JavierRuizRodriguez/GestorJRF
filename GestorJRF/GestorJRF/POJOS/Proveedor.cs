namespace GestorJRF.POJOS
{
    public class Proveedor
    {
        public string nombre { get; set; }
        public string cif { get; set; }
        public string cifAntiguo { get; set; }

        public Proveedor() { }

        public Proveedor(string nombre, string cif)
        {
            this.nombre = nombre;
            this.cif = cif;
        }

        public Proveedor(string nombre, string cif, string cifAntiguo)
        {
            this.nombre = nombre;
            this.cif = cif;
            this.cifAntiguo = cifAntiguo;
        }
    }
}
