using FastMember;
using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Facturas;
using GestorJRF.POJOS.Mapas;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas.GestionInformes;
using Microsoft.Reporting.WinForms;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionInformes
{
    /// <summary>
    /// Lógica de interacción para VentanaVisualizacionInformes.xaml
    /// </summary>
    public partial class VentanaVisualizacionInformes : Window
    {
        private BusquedaFactura opcionesBusqueda;

        private List<Resumen> resumenes;

        private List<Factura> facturas;

        public VentanaVisualizacionInformes(BusquedaFactura busqueda)
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(2, this);
            this.opcionesBusqueda = busqueda;
            if (busqueda.tipo.Equals("informeFacturasEmitidas"))
            {
                this.generarInformeFacturasEmitidas();
            }
            else
            {
                this.generarInforme();
            }
        }

        private void generarInformeFacturasEmitidas()
        {
            base.Show();
            this.facturas = FacturasCRUD.cogerTodasFacturasPorFechas(this.opcionesBusqueda).Cast<Factura>().ToList();
            this.facturas = new List<Factura>(from f in this.facturas
                                              orderby f.numeroFactura
                                              select f);
            if (this.facturas.Count > 0)
            {
                double total = 0.0;
                double ivaTotal = 0.0;
                double importeTotal = 0.0;
                foreach (Factura factura in this.facturas)
                {
                    total += factura.importeTotal;
                    ivaTotal += factura.importeIva;
                    importeTotal += factura.baseImponible;
                }
                DataTable tablaFacturas = new DataTable();
                using (ObjectReader reader = ObjectReader.Create(this.facturas))
                {
                    tablaFacturas.Load(reader);
                }
                ReportDataSource dsResumenes = new ReportDataSource("DataSet5", tablaFacturas);
                this._reportViewer.LocalReport.DataSources.Add(dsResumenes);
                string exeDir = AppDomain.CurrentDomain.BaseDirectory;
                string reportDir = Path.Combine(exeDir, "Reportes\\Report4.rdlc");
                this._reportViewer.LocalReport.ReportPath = reportDir;
                ReportParameter[] parameters = new ReportParameter[4]
                {
                new ReportParameter("titulo", "FACTURAS EMITIDAS EN " + this.opcionesBusqueda.fechaInicio.ToString("yyyy")),
                new ReportParameter("total", total.ToString()),
                new ReportParameter("ivaTotal", ivaTotal.ToString()),
                new ReportParameter("importeTotal", importeTotal.ToString())
                };
                this._reportViewer.ShowParameterPrompts = true;
                this._reportViewer.LocalReport.SetParameters(parameters);
                this._reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                this._reportViewer.RefreshReport();
            }
            else
            {
                MessageBox.Show("No se encontraron facturas para las fechas dadas.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                base.Close();
            }
        }

        private void generarInforme()
        {
            base.Show();
            this.resumenes = ResumenesCRUD.cogerResumenesParaInforme(this.opcionesBusqueda).Cast<Resumen>().ToList();
            this.resumenes = new List<Resumen>(from d in this.resumenes
                                               orderby d.fechaPorte descending
                                               select d);
            DataTable tablaResumen = new DataTable();
            DataTable tablaItinerario = new DataTable();
            List<Resumen> resumenesSalida2 = new List<Resumen>();
            List<CadenaResumen> itinerarios = new List<CadenaResumen>();
            int contadorIdFalso = 100000;
            double importeTotal = 0.0;
            foreach (Resumen resumene in this.resumenes)
            {
                Resumen resumenSalida = new Resumen(resumene.id, resumene.referencia, resumene.cif, resumene.nombreCliente, 0.0, 0.0, "", "", "", resumene.fechaPorte, new ObservableCollection<Itinerario>(), resumene.precioFinal, new List<Comision>());
                string direccion2 = "";
                foreach (Itinerario listaItinerario in resumene.listaItinerarios)
                {
                    direccion2 = direccion2 + listaItinerario.clienteDeCliente + " (" + listaItinerario.poblacion + ") ";
                    direccion2 += "- ";
                    if (listaItinerario.palets > 0)
                    {
                        Resumen resumenAux = new Resumen(resumenSalida.id, resumenSalida.referencia, resumenSalida.cif, resumenSalida.nombreCliente, 0.0, 0.0, "", "", "", resumenSalida.fechaPorte, new ObservableCollection<Itinerario>(), resumenSalida.precioFinal, new List<Comision>());
                        resumenAux.id = contadorIdFalso;
                        resumenAux.precioFinal = (double)(1 * listaItinerario.palets);
                        resumenesSalida2.Add(resumenAux);
                        importeTotal += resumenAux.precioFinal;
                        itinerarios.Add(new CadenaResumen(resumenAux.id, listaItinerario.palets + " PALETS"));
                        contadorIdFalso--;
                        resumenSalida.precioFinal -= (double)listaItinerario.palets;
                    }
                }
                direccion2 = ((resumene.referencia == null || resumene.referencia.Equals("")) ? direccion2.Substring(0, direccion2.Length - 2) : (direccion2 + " Ref. " + resumene.referencia));
                resumenSalida.id = contadorIdFalso;
                resumenesSalida2.Add(resumenSalida);
                importeTotal += resumenSalida.precioFinal;
                itinerarios.Add(new CadenaResumen(contadorIdFalso, direccion2.ToUpper()));
                contadorIdFalso--;
            }
            resumenesSalida2 = new List<Resumen>(from r in resumenesSalida2
                                                 orderby r.id
                                                 select r);
            if (resumenesSalida2.Count > 0)
            {
                using (ObjectReader reader2 = ObjectReader.Create(resumenesSalida2))
                {
                    tablaResumen.Load(reader2);
                }
                using (ObjectReader reader = ObjectReader.Create(itinerarios))
                {
                    tablaItinerario.Load(reader);
                }
                ReportDataSource dsResumenes = new ReportDataSource("DataSet2", tablaResumen);
                ReportDataSource dsItinerario = new ReportDataSource("DataSet3", tablaItinerario);
                this._reportViewer.LocalReport.DataSources.Add(dsResumenes);
                this._reportViewer.LocalReport.DataSources.Add(dsItinerario);
                string exeDir = AppDomain.CurrentDomain.BaseDirectory;
                string reportDir = Path.Combine(exeDir, "Reportes\\Report3.rdlc");
                this._reportViewer.LocalReport.ReportPath = reportDir;
                ReportParameter[] parameters = new ReportParameter[4];
                DateTime fechaInicio;
                if (this.opcionesBusqueda.tipo.Equals("informeGeneral"))
                {
                    parameters[0] = new ReportParameter("nombreEmpleado", "GENERAL");
                    parameters[1] = new ReportParameter("dni", " - ");
                    ReportParameter[] array = parameters;
                    fechaInicio = this.opcionesBusqueda.fechaInicio;
                    string str = fechaInicio.ToString("MMMM").ToUpper();
                    fechaInicio = this.opcionesBusqueda.fechaInicio;
                    array[2] = new ReportParameter("titulo", "INFORME GENERAL DEL MES DE " + str + " DE " + fechaInicio.ToString("yyyy"));
                    parameters[3] = new ReportParameter("total", importeTotal.ToString());
                }
                else
                {
                    parameters[0] = new ReportParameter("nombreEmpleado", this.opcionesBusqueda.empleado.getNombreApellidos());
                    parameters[1] = new ReportParameter("dni", this.opcionesBusqueda.empleado.dni);
                    ReportParameter[] array2 = parameters;
                    string[] obj = new string[6]
                    {
                    "INFORME DEL MES DE ",
                    null,
                    null,
                    null,
                    null,
                    null
                    };
                    fechaInicio = this.opcionesBusqueda.fechaInicio;
                    obj[1] = fechaInicio.ToString("MMMM").ToUpper();
                    obj[2] = " DE ";
                    fechaInicio = this.opcionesBusqueda.fechaInicio;
                    obj[3] = fechaInicio.ToString("yyyy");
                    obj[4] = " DEL EMPLEADO ";
                    obj[5] = this.opcionesBusqueda.empleado.getNombreApellidos();
                    array2[2] = new ReportParameter("titulo", string.Concat(obj));
                    parameters[3] = new ReportParameter("total", importeTotal.ToString());
                }
                this._reportViewer.ShowParameterPrompts = true;
                this._reportViewer.LocalReport.SetParameters(parameters);
                this._reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                this._reportViewer.RefreshReport();
            }
            else
            {
                MessageBox.Show("No se encontraron resumenes para las fechas dadas.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                base.Close();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            new VentanaSeleccionInformes().Show();
        }

    }
}
