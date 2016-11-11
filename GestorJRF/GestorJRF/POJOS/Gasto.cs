using System;

namespace GestorJRF.POJOS
{
    public class Gasto
    {
        public long id { get; set; }
        public string dni { get; set; }
        public string bastidor { get; set; }
        public string concepto { get; set; }
        public DateTime fecha { get; set; }
        public string descripcion { get; set; }
        public double importeBase { get; set; }
        public double cuotaDeducible { get; set; }
        public int iva { get; set; }
        public int irpf { get; set; }
        public string referencia { get; set; }
        public string proveedor { get; set; }
        public string cifProveedor { get; set; }

        public Gasto() { }

        public Gasto(string dni, string bastidor, string concepto, DateTime fecha, string descripcion, double importeBase, double cuotaDeducible, int iva, int irpf, string referencia, string proveedor, string cifProveedor)
        {
            this.dni = dni;
            this.bastidor = bastidor;
            this.concepto = concepto;
            this.fecha = fecha.Date;
            this.descripcion = descripcion;
            this.importeBase = importeBase;
            this.cuotaDeducible = cuotaDeducible;
            this.iva = iva;
            this.irpf = irpf;
            this.referencia = referencia;
            this.proveedor = proveedor;
            this.cifProveedor = cifProveedor;
        }

        public Gasto(long id, string dni, string bastidor, string concepto, DateTime fecha, string descripcion, double importeBase, double cuotaDeducible, int iva, int irpf, string referencia, string proveedor, string cifProveedor)
        {
            this.id = id;
            this.dni = dni;
            this.bastidor = bastidor;
            this.concepto = concepto;
            this.fecha = fecha.Date;
            this.descripcion = descripcion;
            this.importeBase = importeBase;
            this.cuotaDeducible = cuotaDeducible;
            this.iva = iva;
            this.irpf = irpf;
            this.referencia = referencia;
            this.proveedor = proveedor;
            this.cifProveedor = cifProveedor;
        }
    }
}
