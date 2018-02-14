using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorJRF.POJOS
{
    public class GastoBienInversion : Gasto
    {
        public double amortizacion
        {
            get;
            set;
        }

        public double intereses
        {
            get;
            set;
        }

        public GastoBienInversion()
        {
        }

        public GastoBienInversion(string dni, string concepto, DateTime fecha, string descripcion, double cuotaDeducible, int iva, string referencia, string proveedor, string cifProveedor, double tasas, double amortizacion, double intereses, int numeroTrimestre, DateTime fechaAltaSistema, int año)
        {
            base.dni = dni;
            base.concepto = concepto;
            base.fecha = fecha.Date;
            base.descripcion = descripcion;
            base.cuotaDeducible = cuotaDeducible;
            base.iva = iva;
            base.referencia = referencia;
            base.proveedor = proveedor;
            base.cifProveedor = cifProveedor;
            base.tasas = tasas;
            this.amortizacion = amortizacion;
            this.intereses = intereses;
            base.numeroTrimestre = numeroTrimestre;
            base.fechaAltaSistema = fechaAltaSistema.Date;
            base.año = año;
        }

        public GastoBienInversion(long id, string dni, string concepto, DateTime fecha, string descripcion, double cuotaDeducible, int iva, string referencia, string proveedor, string cifProveedor, double tasas, double amortizacion, double intereses, int numeroTrimestre, DateTime fechaAltaSistema, int año)
        {
            base.id = id;
            base.dni = dni;
            base.concepto = concepto;
            base.fecha = fecha.Date;
            base.descripcion = descripcion;
            base.cuotaDeducible = cuotaDeducible;
            base.iva = iva;
            base.referencia = referencia;
            base.proveedor = proveedor;
            base.cifProveedor = cifProveedor;
            base.tasas = tasas;
            this.amortizacion = amortizacion;
            this.intereses = intereses;
            base.numeroTrimestre = numeroTrimestre;
            base.fechaAltaSistema = fechaAltaSistema.Date;
            base.año = año;
        }
    }
}
