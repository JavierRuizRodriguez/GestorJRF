namespace GestorJRF.POJOS
{
    public abstract class Alerta
    {
        public long id { get; set; }
        public long idAntiguo { get; set; }
        public string matricula { get; set; }
        public string descripcion { get; set; }
        public string tipoAviso { get; set; }
    }
}
