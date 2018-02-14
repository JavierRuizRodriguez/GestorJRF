using System;

namespace GestorJRF.POJOS
{
    public class Gasto
    {
        public long id
        {
            get;
            set;
        }

        public string dni
        {
            get;
            set;
        }

        public string concepto
        {
            get;
            set;
        }

        public DateTime fecha
        {
            get;
            set;
        }

        public string descripcion
        {
            get;
            set;
        }

        public double cuotaDeducible
        {
            get;
            set;
        }

        public int iva
        {
            get;
            set;
        }

        public string referencia
        {
            get;
            set;
        }

        public string proveedor
        {
            get;
            set;
        }

        public string cifProveedor
        {
            get;
            set;
        }

        public double tasas
        {
            get;
            set;
        }

        public int numeroTrimestre
        {
            get;
            set;
        }

        public DateTime fechaAltaSistema
        {
            get;
            set;
        }

        public int año { get; set; }
    }

}
