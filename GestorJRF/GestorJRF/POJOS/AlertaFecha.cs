using System;

namespace GestorJRF.POJOS
{
    public class AlertaFecha : Alerta
    {
        public int diasAntelacion { get; set; }
        public DateTime fechaLimite { get; set; }

        public AlertaFecha() : base() { }

        public AlertaFecha(string matricula, string descripcion, int diasAntelacion, DateTime fechaLimite)
        {
            this.matricula = matricula;
            this.descripcion = descripcion;
            tipoAviso = "fecha";
            this.diasAntelacion = diasAntelacion;
            this.fechaLimite = fechaLimite.Date;
        }

        public AlertaFecha(long idAntiguo, string matricula, string descripcion, int diasAntelacion, DateTime fechaLimite)
        {
            this.idAntiguo = idAntiguo;
            this.matricula = matricula;
            this.descripcion = descripcion;
            tipoAviso = "fecha";
            this.diasAntelacion = diasAntelacion;
            this.fechaLimite = fechaLimite.Date;
        }
    }
}
