namespace GestorJRF.POJOS
{
    public class AlertaKM : Alerta
    {
        public long kmAntelacion { get; set; }
        public long kmLimite { get; set; }

        public AlertaKM() : base() {}

        public AlertaKM(string matricula, string descripcion, long kmAntelacion, long kmLimite)
        {
            this.matricula = matricula;
            this.descripcion = descripcion;
            tipoAviso = "kilometro";
            this.kmAntelacion = kmAntelacion;
            this.kmLimite = kmLimite;
        }
        
        public AlertaKM(long idAntiguo, string matricula, string descripcion, long kmAntelacion, long kmLimite)
        {
            this.idAntiguo = idAntiguo;
            this.matricula = matricula;
            this.descripcion = descripcion;
            tipoAviso = "kilometro";
            this.kmAntelacion = kmAntelacion;
            this.kmLimite = kmLimite;
        }
    }
}
