namespace GestorJRF.POJOS
{
    public class AlertaKM : Alerta
    {
        public long kmAntelacion
        {
            get;
            set;
        }

        public long kmLimite
        {
            get;
            set;
        }

        public AlertaKM()
        {
        }

        public AlertaKM(string matricula, string descripcion, long kmAntelacion, long kmLimite)
        {
            base.matricula = matricula;
            base.descripcion = descripcion;
            base.tipoAviso = "kilometro";
            this.kmAntelacion = kmAntelacion;
            this.kmLimite = kmLimite;
        }

        public AlertaKM(long idAntiguo, string matricula, string descripcion, long kmAntelacion, long kmLimite)
        {
            base.idAntiguo = idAntiguo;
            base.matricula = matricula;
            base.descripcion = descripcion;
            base.tipoAviso = "kilometro";
            this.kmAntelacion = kmAntelacion;
            this.kmLimite = kmLimite;
        }
    }
}
