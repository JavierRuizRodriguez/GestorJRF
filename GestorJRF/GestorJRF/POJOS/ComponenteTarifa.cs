namespace GestorJRF.POJOS
{
    public class ComponenteTarifa
    {
        public string nombreTarifa
        {
            get;
            set;
        }

        public string etiqueta
        {
            get;
            set;
        }

        public double precio
        {
            get;
            set;
        }

        public string tipoCamion
        {
            get;
            set;
        }

        public ComponenteTarifa()
        {
        }

        public ComponenteTarifa(string etiqueta, double precio, string tipoCamion)
        {
            this.etiqueta = etiqueta;
            this.precio = precio;
            this.tipoCamion = tipoCamion;
        }

        public ComponenteTarifa(string nombreTarifa, string etiqueta, double precio, string tipoCamion)
        {
            this.nombreTarifa = nombreTarifa;
            this.etiqueta = etiqueta;
            this.precio = precio;
            this.tipoCamion = tipoCamion;
        }
    }
}
