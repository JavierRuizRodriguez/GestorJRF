using System;

namespace GestorJRF.POJOS
{
    public class Fechas
    {
        public DateTime fechaInicio
        {
            get;
            set;
        }

        public DateTime fechaFinal
        {
            get;
            set;
        }

        public Fechas()
        {
        }

        public Fechas(DateTime fechaInicio, DateTime fechaFinal)
        {
            this.fechaInicio = fechaInicio.Date;
            this.fechaFinal = fechaFinal.Date;
        }
    }
}
