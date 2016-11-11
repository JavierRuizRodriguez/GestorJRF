using System;

namespace GestorJRF.POJOS.Facturas
{
    public class BusquedaFactura
    {
        public char letraTipoFactura { get; set; }
        public Empresa empresa { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFinal { get; set; }
        public string referencia { get; set; }
        public string tipo { get; set; }

        public BusquedaFactura(char letraTipoFactura, Empresa empresa, DateTime fechaInicio, DateTime fechaFinal, string referencia, string tipo)
        {
            this.letraTipoFactura = letraTipoFactura;
            this.empresa = empresa;
            this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.referencia = referencia;
            this.tipo = tipo;
        }

        public BusquedaFactura(DateTime fechaInicio, DateTime fechaFinal, string tipo)
        {
            this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.tipo = tipo;
        }
    }
}
