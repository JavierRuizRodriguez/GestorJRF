namespace GestorJRF.POJOS
{
    public class Camion
    {
        public string marca { get; set; }
        public string modelo { get; set; }
        public string matricula { get; set; }
        public string nBastidor { get; set; }
        public double largoCaja { get; set; }
        public double largoVehiculo { get; set; }
        public long kilometraje { get; set; }
        public double galibo { get; set; }
        public string tipoCombustible { get; set; }

        public Camion() { }

        public Camion(string marca, string modelo, string matricula, string nBastidor, double largoCaja, double largoVehiculo, long kilometraje, double galibo, string tipoCombustible)
        {
            this.marca = marca;
            this.modelo = modelo;
            this.matricula = matricula;
            this.nBastidor = nBastidor;
            this.largoCaja = largoCaja;
            this.largoVehiculo = largoVehiculo;
            this.kilometraje = kilometraje;
            this.galibo = galibo;
            this.tipoCombustible = tipoCombustible;
        }
    }
}
