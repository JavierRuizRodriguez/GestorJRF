using System;

namespace GestorJRF.POJOS.Estadisticas
{
    public class BusquedaEstadisticas
    {

        public DateTime fechaInicio { get; set; }
        public DateTime fechaFinal { get; set; }
        public string tipo { get; set; }
        public string cif { get; set; }
        public string dni { get; set; }

        public BusquedaEstadisticas(DateTime fechaInicio, DateTime fechaFinal, string tipo)
        {
            this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.tipo = tipo;
        }

        public BusquedaEstadisticas(DateTime fechaInicio, DateTime fechaFinal, string tipo, string cif)
        {
            this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.tipo = tipo;
            this.cif = cif;
        }

        public BusquedaEstadisticas(string dni, DateTime fechaInicio, DateTime fechaFinal, string tipo)
        {
            this.dni = dni;
            this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.tipo = tipo;
        }
    }
}
