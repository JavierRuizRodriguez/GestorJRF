namespace GestorJRF.POJOS
{
    public class CadenaResumen
    {
        public long idResumen { get; set; }
        public string direccion { get; set; }

        public CadenaResumen() { }
        public CadenaResumen(long idResumen, string direccion)
        {
            this.idResumen = idResumen;
            this.direccion = direccion;
        }
    }
}
