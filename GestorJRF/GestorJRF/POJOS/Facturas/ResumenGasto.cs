using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorJRF.POJOS.Facturas
{
    public class ResumenGasto
    {
        public double importeBaseTotal { get; set; }
        public int iva { get; set; }
        public double ivaTotal { get; set; }
        public double irpfTotal { get; set; }
        public double tasasTotales { get; set; }
        public double cuotaDeducibleTotal { get; set; }

        public ResumenGasto() { }

        public ResumenGasto(double importeBaseTotal, int iva, double ivaTotal, double irpfTotal, double tasasTotal, double cuotaDeducibleTotal)
        {
            this.cuotaDeducibleTotal = cuotaDeducibleTotal;
            this.iva = iva;
            this.irpfTotal = irpfTotal;
            this.ivaTotal = ivaTotal;
            this.tasasTotales = tasasTotal;
            this.importeBaseTotal = importeBaseTotal;
        }

    }
}
