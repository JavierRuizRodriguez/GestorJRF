using System.Globalization;

namespace GestorJRF.POJOS
{
    public class Camion
    {
        public string marca
        {
            get;
            set;
        }

        public string modelo
        {
            get;
            set;
        }

        public string matricula
        {
            get;
            set;
        }

        public string nBastidor
        {
            get;
            set;
        }

        public string nBastidorAntiguo
        {
            get;
            set;
        }

        public double largoCaja
        {
            get;
            set;
        }

        public double largoVehiculo
        {
            get;
            set;
        }

        public long kilometraje
        {
            get;
            set;
        }

        public double galibo
        {
            get;
            set;
        }

        public string tipoCombustible
        {
            get;
            set;
        }

        public Camion()
        {
        }

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

        public Camion(string marca, string modelo, string matricula, string nBastidor, string nBastidorAntiguo, double largoCaja, double largoVehiculo, long kilometraje, double galibo, string tipoCombustible)
        {
            this.marca = marca;
            this.modelo = modelo;
            this.matricula = matricula;
            this.nBastidor = nBastidor;
            this.nBastidorAntiguo = nBastidorAntiguo;
            this.largoCaja = largoCaja;
            this.largoVehiculo = largoVehiculo;
            this.kilometraje = kilometraje;
            this.galibo = galibo;
            this.tipoCombustible = tipoCombustible;
        }

        public override string ToString()
        {
            return "MARCA - " + this.marca + "\nMODELO - " + this.modelo + "\nMATRICULA - " + this.matricula + "\nNº BASTIDOR - " + this.nBastidor + "\nKILOMETRAJE - " + this.kilometraje.ToString("#,#", CultureInfo.InvariantCulture) + "\nCOMBUSTIBLE - " + this.tipoCombustible;
        }
    }

}
