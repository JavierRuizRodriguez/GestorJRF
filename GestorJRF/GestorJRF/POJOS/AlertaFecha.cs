using System;

namespace GestorJRF.POJOS
{
    public class AlertaFecha : Alerta
    {
        public int diasAntelacion
        {
            get;
            set;
        }

        public DateTime fechaLimite
        {
            get;
            set;
        }

        public AlertaFecha()
        {
        }

        public AlertaFecha(string matricula, string descripcion, int diasAntelacion, DateTime fechaLimite)
        {
            base.matricula = matricula;
            base.descripcion = descripcion;
            base.tipoAviso = "fecha";
            this.diasAntelacion = diasAntelacion;
            this.fechaLimite = fechaLimite.Date;
        }

        public AlertaFecha(long idAntiguo, string matricula, string descripcion, int diasAntelacion, DateTime fechaLimite)
        {
            base.idAntiguo = idAntiguo;
            base.matricula = matricula;
            base.descripcion = descripcion;
            base.tipoAviso = "fecha";
            this.diasAntelacion = diasAntelacion;
            this.fechaLimite = fechaLimite.Date;
        }
    }
}
