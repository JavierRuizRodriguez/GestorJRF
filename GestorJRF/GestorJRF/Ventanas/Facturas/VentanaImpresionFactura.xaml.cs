using FastMember;
using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Facturas;
using GestorJRF.POJOS.Mapas;
using GestorJRF.Utilidades;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace GestorJRF.Ventanas.Facturas
{
    /// <summary>
    /// Lógica de interacción para VentanaImpresionFactura.xaml
    /// </summary>
    public partial class VentanaImpresionFactura : Window
    {
        private Empresa empresa;
        private BusquedaFactura busqueda;
        private List<Resumen> resumenes;
        private bool facturaImpimida;
        private const string DIRECCION_NAVE = @"Calle La Habana, 28806 Alcalá de Henares";

        public VentanaImpresionFactura(BusquedaFactura busqueda)
        {
            InitializeComponent();

            UtilidadesVentana.SituarVentana(2, this);
            facturaImpimida = false;
            this.busqueda = busqueda;

            if (busqueda.tipo.Equals("resumenes"))
                generarFacturaResumen();
            else
                generarFacturaIva();
        }

        private void generarFacturaResumen()
        {
            empresa = busqueda.empresa;
            List<Empresa> _empresa = new List<Empresa>();
            _empresa.Add(empresa);
            resumenes = ResumenesCRUD.cogerResumenesParaFactura(busqueda).Cast<Resumen>().ToList();
            resumenes.OrderByDescending(d => d.fechaPorte);

            DataTable tablaEmpresa = new DataTable();
            DataTable tablaResumen = new DataTable();
            DataTable tablaItinerario = new DataTable();
            List<CadenaResumen> itinerarios = new List<CadenaResumen>();

            int numeroRepartos = 0;
            foreach (Resumen r in resumenes)
            {
                string direccion = "";
                foreach (Itinerario i in r.listaItinerarios)
                {
                    if (!i.direccion.Equals(DIRECCION_NAVE))
                        direccion += i.direccion + " - ";
                    if (!i.esEtapa)
                        numeroRepartos++;
                }
                numeroRepartos -= 2;

                if (numeroRepartos > 0)
                    direccion += " Repartos: " + numeroRepartos;
                else
                    direccion = direccion.Substring(0, direccion.Length - 2);

                itinerarios.Add(new CadenaResumen(r.id, direccion));
                numeroRepartos = 0;
            }

            using (var reader = ObjectReader.Create<Empresa>(_empresa))
            {
                tablaEmpresa.Load(reader);
            }

            using (var reader = ObjectReader.Create<Resumen>(resumenes))
            {
                tablaResumen.Load(reader);
            }

            using (var reader = ObjectReader.Create<CadenaResumen>(itinerarios))
            {
                tablaItinerario.Load(reader);
            }

            ReportDataSource dsEmpresa = new ReportDataSource("DataSet1", tablaEmpresa);
            ReportDataSource dsResumenes = new ReportDataSource("DataSet2", tablaResumen);
            ReportDataSource dsItinerario = new ReportDataSource("DataSet3", tablaItinerario);

            _reportViewer.LocalReport.DataSources.Add(dsEmpresa);
            _reportViewer.LocalReport.DataSources.Add(dsResumenes);
            _reportViewer.LocalReport.DataSources.Add(dsItinerario);

            _reportViewer.LocalReport.ReportEmbeddedResource = "GestorJRF.Ventanas.Facturas.Report1.rdlc";

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("idFactura", busqueda.letraTipoFactura + "-" + ResumenesCRUD.cogerNumeroFactura());
            _reportViewer.ShowParameterPrompts = true;
            _reportViewer.LocalReport.SetParameters(parameters);

            _reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            _reportViewer.RefreshReport();
        }

        private void generarFacturaIva()
        {
            Fechas fechas = new Fechas(busqueda.fechaInicio, busqueda.fechaFinal);
            List<Gasto> gastos = GastosCRUD.cogerTodosGastosPorFecha(fechas).Cast<Gasto>().ToList();
            DataTable tablaGasto = new DataTable();

            using (var reader = ObjectReader.Create<Gasto>(gastos))
            {
                tablaGasto.Load(reader);
            }

            ReportDataSource dsGasto = new ReportDataSource("DataSet4", tablaGasto);

            _reportViewer.LocalReport.DataSources.Add(dsGasto);
            _reportViewer.LocalReport.ReportEmbeddedResource = "GestorJRF.Ventanas.Facturas.Report2.rdlc";

            string cuatrimestre;
            if (fechas.fechaInicio.Month == 1)
                cuatrimestre = "1, ";
            else if (fechas.fechaInicio.Month == 4)
                cuatrimestre = "2, ";
            else if (fechas.fechaInicio.Month == 7)
                cuatrimestre = "3, ";
            else
                cuatrimestre = "4, ";
            cuatrimestre += Convert.ToString(fechas.fechaInicio.Year);

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("cuatrimestre", cuatrimestre);
            _reportViewer.ShowParameterPrompts = true;
            _reportViewer.LocalReport.SetParameters(parameters);

            _reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            _reportViewer.RefreshReport();
        }

        private void _reportViewer_PrintingBegin(object sender, ReportPrintEventArgs e)
        {
            facturaImpimida = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!facturaImpimida && busqueda.tipo.Equals("resumenes"))
                ResumenesCRUD.CancelarNumeroFactura();

            if (busqueda.tipo.Equals("resumenes"))
                new VentanaFacturaResumenes().Show();
            else
                new VentanaFacturaIva().Show();
        }
    }
}
