using System;
using System.Collections.Generic;

namespace GestorJRF.POJOS.Facturas
{
    public class BusquedaFactura
    {
        public List<Resumen> resumenes
        {
            get;
            set;
        }

        public char letraTipoFactura
        {
            get;
            set;
        }

        public Empresa empresa
        {
            get;
            set;
        }

        public Empleado empleado
        {
            get;
            set;
        }

        public DateTime fechaInicio
        {
            get;
            set;
        }

        public DateTime fechaFinal
        {
            get;
            set;
        }

        public string referencia
        {
            get;
            set;
        }

        public string tipo
        {
            get;
            set;
        }

        public string numeroFactura
        {
            get;
            set;
        }

        public DateTime fechaEmisionFactura
        {
            get;
            set;
        }

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

        public BusquedaFactura(Empleado empleado, DateTime fechaInicio, DateTime fechaFinal, string tipo)
        {
            this.empleado = empleado;
            this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.tipo = tipo;
        }

        public BusquedaFactura(string numeroFactura, char letraTipoFactura, Empresa empresa, List<Resumen> resumen, string tipo, DateTime fechaEmisionFactura)
        {
            this.numeroFactura = numeroFactura;
            this.letraTipoFactura = letraTipoFactura;
            this.empresa = empresa;
            this.resumenes = new List<Resumen>(resumen);
            this.tipo = tipo;
            this.fechaEmisionFactura = fechaEmisionFactura;
        }

        public BusquedaFactura(char letraTipoFactura, Empresa empresa, List<Resumen> resumen, string tipo, DateTime fechaEmisionFactura)
        {
            this.letraTipoFactura = letraTipoFactura;
            this.empresa = empresa;
            this.resumenes = new List<Resumen>(resumen);
            this.tipo = tipo;
            this.fechaEmisionFactura = fechaEmisionFactura;
        }
    }
}
