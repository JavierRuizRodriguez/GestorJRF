using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorJRF.POJOS.Pagos
{
    public class Pago
    {
        public List<ComponentePago> facturas;

        public long id
        {
            get;
            set;
        }

        public DateTime fechaPagare
        {
            get;
            set;
        }

        public string banco
        {
            get;
            set;
        }

        public Pago()
        {
        }

        public Pago(long id, DateTime fechaPagare, string banco, List<ComponentePago> facturas)
        {
            this.id = id;
            this.fechaPagare = fechaPagare.Date;
            this.banco = banco;
            this.facturas = new List<ComponentePago>(facturas);
        }

        public Pago(DateTime fechaPagare, string banco, List<ComponentePago> facturas)
        {
            this.fechaPagare = fechaPagare.Date;
            this.banco = banco;
            this.facturas = new List<ComponentePago>(facturas);
        }

        public Pago(DateTime fechaPagare, List<ComponentePago> facturas)
        {
            this.fechaPagare = fechaPagare.Date;
            this.facturas = new List<ComponentePago>(facturas);
        }

        public Pago(string banco, List<ComponentePago> facturas)
        {
            this.banco = banco;
            this.facturas = new List<ComponentePago>(facturas);
        }
    }
}
