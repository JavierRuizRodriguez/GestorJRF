using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorJRF.POJOS.Facturas
{
    public class ComponenteFactura
    {
        public long numeroFactura
        {
            get;
            set;
        }

        public long idResumenFinal
        {
            get;
            set;
        }

        public ComponenteFactura()
        {
        }

        public ComponenteFactura(long numeroFactura, long idResumenFinal)
        {
            this.numeroFactura = numeroFactura;
            this.idResumenFinal = idResumenFinal;
        }
    }
}
