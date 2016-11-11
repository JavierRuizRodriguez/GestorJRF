using GestorJRF.POJOS.Mapas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GestorJRF.POJOS
{
    public class Resumen
    {
        public long id { get; set; }
        public long idAntiguo { get; set; }
        public string referencia { get; set; }
        public string cif { get; set; }
        public string nombreCliente { get; set; }
        public double kilometrosIda { get; set; }
        public double kilometrosVuelta { get; set; }
        public string nombreTarifa { get; set; }
        public string etiqueta { get; set; }
        public string tipoCamion { get; set; }
        public DateTime fechaPorte { get; set; }
        public List<Itinerario> listaItinerarios { get; set; }
        public double precioFinal { get; set; }
        public List<Comision> listaComisiones { get; set; }

        public Resumen() { }

        public Resumen(string cif, string nombreCliente, double kilometrosIda, double kilometrosVuelta, string nombreTarifa, string etiqueta, string tipoCamion, DateTime fechaPorte, ObservableCollection<Itinerario> listaItinerarios, double precioFinal)
        {
            this.nombreCliente = nombreCliente;
            this.kilometrosIda = kilometrosIda;
            this.kilometrosVuelta = kilometrosVuelta;
            this.nombreTarifa = nombreTarifa;
            this.etiqueta = etiqueta;
            this.tipoCamion = tipoCamion;
            this.cif = cif;
            this.fechaPorte = fechaPorte.Date;
            this.listaItinerarios = new List<Itinerario>(listaItinerarios);
            this.precioFinal = precioFinal;
        }

        public Resumen(string referencia, string cif, string nombreCliente, double kilometrosIda, double kilometrosVuelta, string nombreTarifa, string etiqueta, string tipoCamion, DateTime fechaPorte, ObservableCollection<Itinerario> listaItinerarios, double precioFinal, List<Comision> comisiones)
        {
            this.nombreCliente = nombreCliente;
            this.kilometrosIda = kilometrosIda;
            this.kilometrosVuelta = kilometrosVuelta;
            this.referencia = referencia;
            this.cif = cif;
            this.fechaPorte = fechaPorte.Date;
            this.listaItinerarios = new List<Itinerario>(listaItinerarios);
            this.precioFinal = precioFinal;
            this.listaComisiones = new List<Comision>(comisiones);
        }

        public Resumen(long idAntiguo, string referencia, string cif, string nombreCliente, double kilometrosIda, double kilometrosVuelta, string nombreTarifa, string etiqueta, string tipoCamion, DateTime fechaPorte, ObservableCollection<Itinerario> listaItinerarios, double precioFinal, List<Comision> comisiones)
        {
            this.nombreCliente = nombreCliente;
            this.referencia = referencia;
            this.idAntiguo = idAntiguo;
            this.kilometrosIda = kilometrosIda;
            this.kilometrosVuelta = kilometrosVuelta;
            this.idAntiguo = idAntiguo;
            this.referencia = referencia;
            this.cif = cif;
            this.fechaPorte = fechaPorte.Date;
            this.listaItinerarios = new List<Itinerario>(listaItinerarios);
            this.precioFinal = precioFinal;
            this.listaComisiones = new List<Comision>(comisiones);
        }
    }
}
