using System;

namespace GestorJRF.POJOS.Estadisticas
{
    public class GastoEstadistica
    {
        public DateTime fecha { get; set; }
        public double sumaImporteBase { get; set; }

        public GastoEstadistica() { }

        public GastoEstadistica(DateTime fecha, double sumaImporteBase)
        {
            this.fecha = fecha.Date;
            this.sumaImporteBase = sumaImporteBase;
        }
    }
}
