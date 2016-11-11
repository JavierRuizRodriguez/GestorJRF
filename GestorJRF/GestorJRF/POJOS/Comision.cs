namespace GestorJRF.POJOS
{
    public class Comision
    {
        public long id { get; set; }
        public long idResumenFinal { get; set; }
        public string dni { get; set; }
        public double porcentaje { get; set; }

        public Comision() { }

        public Comision(string dni, double porcentaje)
        {
            this.dni = dni;
            this.porcentaje = porcentaje;
        }
    }
}
