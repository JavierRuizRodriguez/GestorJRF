using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorJRF.POJOS.Facturas
{
    public class Factura
    {
        public long numeroFactura
        {
            get;
            set;
        }

        public string nombreCliente
        {
            get;
            set;
        }

        public DateTime fechaAltaFactura
        {
            get;
            set;
        }

        public double baseImponible
        {
            get;
            set;
        }

        public double importeIva
        {
            get;
            set;
        }

        public double importeTotal
        {
            get;
            set;
        }

        public List<ComponenteFactura> resumenes
        {
            get;
            set;
        }

        public char tipoFactura
        {
            get;
            set;
        }

        public Factura()
        {
        }

        public Factura(long numeroFactura, string nombreCliente, DateTime fechaAltaFactura, double baseImponible, double importeIva, double importeTotal, List<ComponenteFactura> resumenes, char tipoFactura)
        {
            this.numeroFactura = numeroFactura;
            this.nombreCliente = nombreCliente;
            this.fechaAltaFactura = fechaAltaFactura.Date;
            this.baseImponible = baseImponible;
            this.importeIva = importeIva;
            this.importeTotal = importeTotal;
            this.resumenes = new List<ComponenteFactura>(resumenes);
            this.tipoFactura = tipoFactura;
        }
    }
}
