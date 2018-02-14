using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorJRF.POJOS.Pagos
{
    public class ComponentePago
    {
        public long idPago
        {
            get;
            set;
        }

        public long numeroFactura
        {
            get;
            set;
        }

        public ComponentePago()
        {
        }

        public ComponentePago(long idPago, long numeroFactura)
        {
            this.idPago = idPago;
            this.numeroFactura = numeroFactura;
        }
    }
}
