using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorJRF.POJOS
{
    public class GastoNormal : Gasto
    {
        public double importeBase
        {
            get;
            set;
        }

        public double irpf
        {
            get;
            set;
        }

        public GastoNormal()
        {
        }

        public GastoNormal(string dni, string concepto, DateTime fecha, string descripcion, double cuotaDeducible, int iva, string referencia, string proveedor, string cifProveedor, double tasas, double importeBase, double irpf, int numeroTrimestre, DateTime fechaAltaSistema, int año)
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
            this.irpf = irpf;
            this.importeBase = importeBase;
            base.numeroTrimestre = numeroTrimestre;
            base.fechaAltaSistema = fechaAltaSistema.Date;
            base.año = año;
        }

        public GastoNormal(long id, string dni, string concepto, DateTime fecha, string descripcion, double cuotaDeducible, int iva, string referencia, string proveedor, string cifProveedor, double tasas, double importeBase, double irpf, int numeroTrimestre, DateTime fechaAltaSistema, int año)
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
            this.irpf = irpf;
            this.importeBase = importeBase;
            base.numeroTrimestre = numeroTrimestre;
            base.fechaAltaSistema = fechaAltaSistema.Date;
            base.año = año;
        }
    }
}
