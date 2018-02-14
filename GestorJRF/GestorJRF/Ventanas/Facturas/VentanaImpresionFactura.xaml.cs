using FastMember;
using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Facturas;
using GestorJRF.POJOS.Mapas;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.Facturas;
using GestorJRF.Ventanas.Login;
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
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.Facturas
{
    /// <summary>
    /// Lógica de interacción para VentanaImpresionFactura.xaml
    /// </summary>
    public partial class VentanaImpresionFactura : Window
    {
        private DateTime t1;
        private DateTime t2;
        private DateTime t3;

        private BusquedaFactura busqueda;

        private List<Resumen> resumenes;

        private List<ComponenteFactura> componentesFactura;

        private List<Empresa> _empresa;

        private bool facturaImpimida;

        private int trimestre;

        private int año;

        private long idFactura;

        private double baseImponible;

        private double importeIva;

        private double importeTotal;

        private string tipo;

        public VentanaImpresionFactura(BusquedaFactura busqueda)
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(2, this);
            this.componentesFactura = new List<ComponenteFactura>();
            this.facturaImpimida = false;
            this.busqueda = busqueda;
            this.tipo = busqueda.tipo;
            this.baseImponible = 0.0;
            this.importeIva = 0.0;
            this.importeTotal = 0.0;
            if (busqueda.tipo.Equals("factura"))
            {
                this.generarFacturaResumen();
            }
            else
            {
                this.generarFacturacionAnual();
            }
        }

        public VentanaImpresionFactura(string tipo, int trimestre, int año)
        {
            this.InitializeComponent();
            this.año = año;
            this.trimestre = trimestre;
            this.tipo = tipo;
            UtilidadesVentana.SituarVentana(2, this);
            this.componentesFactura = new List<ComponenteFactura>();
            this.facturaImpimida = false;
            this.baseImponible = 0.0;
            this.importeIva = 0.0;
            this.importeTotal = 0.0;
            if (tipo.Equals("ivaNormal"))
            {
                this.generarFacturaIva();
            }
            else if(tipo.Equals("ivaBienes"))
            {
                this.generarFacturaIvaBienes();
            }
        }

        public VentanaImpresionFactura(string tipo, String tipo1, int año)
        {
            this.InitializeComponent();
            t1 = new DateTime(año, 3, 31);
            t2 = new DateTime(año, 6, 30);
            t3 = new DateTime(año, 9, 30);
            this.año = año;
            this.tipo = tipo;
            UtilidadesVentana.SituarVentana(2, this);
            this.componentesFactura = new List<ComponenteFactura>();
            this.facturaImpimida = false;
            this.baseImponible = 0.0;
            this.importeIva = 0.0;
            this.importeTotal = 0.0;
            this.generarFacturaOperacionesTerceros(tipo1);
        }

        private void generarFacturacionAnual()
        {
            base.Show();
            var facturas = FacturasCRUD.cogerTodasFacturasPorFechas(this.busqueda).Cast<Factura>().OrderBy(x => x.fechaAltaFactura).ToList();

            if (facturas.Count > 0)
            {
                double total = 0.0;
                double ivaTotal = 0.0;
                double importeTotal = 0.0;
                foreach (Factura factura in facturas)
                {
                    total += factura.importeTotal;
                    ivaTotal += factura.importeIva;
                    importeTotal += factura.baseImponible;
                }
                DataTable tablaFacturas = new DataTable();
                using (ObjectReader reader = ObjectReader.Create(facturas))
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
                new ReportParameter("titulo", "FACTURAS EMITIDAS EN " + this.busqueda.fechaInicio.ToString("yyyy")),
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

        private void generarFacturaOperacionesTerceros(string tipo)
        {
            TrimestreAño ta = new TrimestreAño(0, this.año);

            List<OperacionesTerceros> op;

            if (tipo.Equals("facturas"))
            {
                Dictionary<string, Tuple<string,string>> empresas = EmpresasCRUD.cogerTodasEmpresas().Cast<Empresa>().ToDictionary(x => x.nombre, x => new Tuple<string,string>(x.cif,x.generarDireccion()));

                op = FacturasCRUD.cogerTodasFacturasPorFechas(new BusquedaFactura(new DateTime(this.año, 1, 1), new DateTime(this.año, 12, 31), ""))
                    .Cast<Factura>()
                    .ToList()
                    .Select(x => new OperacionesTerceros(x.nombreCliente, empresas.ContainsKey(x.nombreCliente) ? empresas[x.nombreCliente].Item2 : "#############", empresas.ContainsKey(x.nombreCliente) ? empresas[x.nombreCliente].Item1 : "#############", cogerTrimestre(x.fechaAltaFactura.Month)  == 1 ? x.importeTotal : 0.0, cogerTrimestre(x.fechaAltaFactura.Month) == 2 ? x.importeTotal : 0.0, cogerTrimestre(x.fechaAltaFactura.Month) == 3 ? x.importeTotal : 0.0, cogerTrimestre(x.fechaAltaFactura.Month) == 4 ? x.importeTotal : 0.0, Math.Round(x.importeTotal, 2)))
                    .GroupBy(g => new { g.nombre, g.direccion, g.cif })
                    .Select(x => new OperacionesTerceros(x.Key.nombre, x.Key.direccion, x.Key.cif, x.Sum(a => a.t1), x.Sum(a => a.t2), x.Sum(a => a.t3), x.Sum(a => a.t4), x.Sum(a => a.tTotal)))
                    .OrderBy(x => x.nombre)
                    .ToList()
                    .FindAll(x => x.tTotal > 3005.06);
            }
            else
            {
                Dictionary<string, string> proveedores = ProveedoresCRUD.cogerTodosProveedores().Cast<Proveedor>().ToDictionary(x => x.cif, x => x.generarDireccion());

                var _op = GastosCRUD.cogerTodosGastosNormalesPorTrimestreAño(ta)
                    .Cast<GastoNormal>()
                    .ToList()
                    .Select(x => new OperacionesTerceros(x.proveedor, proveedores.ContainsKey(x.cifProveedor) ? proveedores[x.cifProveedor] : "#############", x.cifProveedor, x.numeroTrimestre == 1 ? x.cuotaDeducible : 0.0, x.numeroTrimestre == 2 ? x.cuotaDeducible : 0.0, x.numeroTrimestre == 3 ? x.cuotaDeducible : 0.0, x.numeroTrimestre == 4 ? x.cuotaDeducible : 0.0, Math.Round(x.cuotaDeducible,2)));

                var temp = GastosCRUD.cogerTodosGastosBienInversionPorTrimestreAño(ta)
                    .Cast<GastoBienInversion>()
                    .ToList()
                    .Select(x => new OperacionesTerceros(x.proveedor, proveedores.ContainsKey(x.cifProveedor) ? proveedores[x.cifProveedor] : "#############", x.cifProveedor, x.numeroTrimestre == 1 ? x.cuotaDeducible : 0.0, x.numeroTrimestre == 2 ? x.cuotaDeducible : 0.0, x.numeroTrimestre == 3 ? x.cuotaDeducible : 0.0, x.numeroTrimestre == 4 ? x.cuotaDeducible : 0.0, Math.Round(x.cuotaDeducible, 2)))
                    .Concat(_op);
                
                op = temp.GroupBy(g => new { g.nombre, g.direccion, g.cif })
                    .Select(x => new OperacionesTerceros(x.Key.nombre, x.Key.direccion, x.Key.cif, x.Sum(a => a.t1), x.Sum(a => a.t2), x.Sum(a => a.t3), x.Sum(a => a.t4), x.Sum(a => a.tTotal)))
                    .OrderBy(x => x.nombre)
                    .ToList()
                    .FindAll(x => x.tTotal > 3005.06);
            }

            if (op.Count > 0)
            {
                base.Show();

                DataTable tablaResumen = new DataTable();
                using (ObjectReader reader = ObjectReader.Create(op))
                {
                    tablaResumen.Load(reader);
                }
                ReportDataSource dsGasto = new ReportDataSource("OperacionesTerceros", tablaResumen);

                this._reportViewer.LocalReport.DataSources.Add(dsGasto);
                string exeDir = AppDomain.CurrentDomain.BaseDirectory;
                string reportDir = Path.Combine(exeDir, "Reportes\\template_OperacionesTerceros.rdlc");
                this._reportViewer.LocalReport.ReportPath = reportDir;

                string tituloCabecera = "Informe Operaciones a Terceros, " + this.año;
                ReportParameter[] parameters = new ReportParameter[2]
                {
                    new ReportParameter("tipo",this.tipo),
                    new ReportParameter("tituloCabecera", tituloCabecera)
                };

                this._reportViewer.ShowParameterPrompts = true;
                this._reportViewer.LocalReport.SetParameters(parameters);
                this._reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                this._reportViewer.RefreshReport();
            }
            else
            {
                MessageBox.Show("No existen gastos para las fechas dadas.", "Error factura", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                base.Close();
            }
        }

        private int cogerTrimestre(int mes)
        {
            if (mes > 0 && mes <=3)
                return 1;
            else if (mes > 3 && mes <= 6)
                return 2;
            else if (mes > 6 && mes <= 9)
                return 3;
            else
                return 4;
        }

        private void generarFacturaResumen()
        {
            try
            {
                this.idFactura = ResumenesCRUD.cogerNumeroFactura();
                this._empresa = new List<Empresa>();
                this._empresa.Add(this.busqueda.empresa);
                this.resumenes = new List<Resumen>(this.busqueda.resumenes);
                base.Show();
                this.resumenes = new List<Resumen>(from d in this.resumenes
                                                   orderby d.fechaPorte descending
                                                   select d);
                DataTable tablaEmpresa = new DataTable();
                DataTable tablaResumen = new DataTable();
                DataTable tablaItinerario = new DataTable();
                List<CadenaResumen> itinerarios = new List<CadenaResumen>();
                List<Resumen> resumenesSalida2 = new List<Resumen>();
                int contadorIdFalso = 100000;
                foreach (Resumen resumen in this.resumenes)
                {
                    resumen.etiqueta = ((resumen.etiqueta != null) ? (resumen.tipoCamion.Equals("CAMIÓN GRANDE") ? (resumen.etiqueta + " (G)") : (resumen.tipoCamion.Equals("CAMIÓN PEQUEÑO") ? (resumen.etiqueta + " (P)") : resumen.etiqueta)) : null);
                    Resumen resumenSalida = new Resumen(resumen.id, resumen.referencia, resumen.cif, resumen.nombreCliente, 0.0, 0.0, resumen.nombreTarifa, resumen.etiqueta, resumen.tipoCamion, resumen.fechaPorte, new ObservableCollection<Itinerario>(), resumen.precioFinal, new List<Comision>());
                    string direccion2 = "";
                    foreach (Itinerario listaItinerario in resumen.listaItinerarios)
                    {
                        direccion2 = direccion2 + listaItinerario.clienteDeCliente + " (" + listaItinerario.poblacion + ") ";
                        direccion2 += "- ";
                        if (listaItinerario.palets > 0)
                        {
                            Resumen resumenAux = new Resumen(resumenSalida.id, resumenSalida.referencia, resumenSalida.cif, resumenSalida.nombreCliente, 0.0, 0.0, resumen.nombreTarifa, resumen.etiqueta, resumen.tipoCamion, resumenSalida.fechaPorte, new ObservableCollection<Itinerario>(), resumenSalida.precioFinal, new List<Comision>());
                            resumenAux.id = contadorIdFalso;
                            resumenAux.precioFinal = (double)(1 * listaItinerario.palets);
                            resumenesSalida2.Add(resumenAux);
                            itinerarios.Add(new CadenaResumen(resumenAux.id, listaItinerario.palets + " PALETS"));
                            contadorIdFalso--;
                            resumenSalida.precioFinal -= (double)listaItinerario.palets;
                        }
                    }
                    direccion2 = ((resumen.referencia == null || resumen.referencia.Equals("")) ? direccion2.Substring(0, direccion2.Length - 2) : (direccion2 + " Ref. " + resumen.referencia));
                    resumenSalida.id = contadorIdFalso;
                    resumenesSalida2.Add(resumenSalida);
                    itinerarios.Add(new CadenaResumen(contadorIdFalso, direccion2.ToUpper()));
                    contadorIdFalso--;
                    this.componentesFactura.Add(new ComponenteFactura(this.idFactura, resumen.id));
                    this.baseImponible += resumen.precioFinal;
                }
                this.importeIva = this.baseImponible * 21.0 / 100.0;
                this.importeTotal = this.baseImponible + this.importeIva;
                this.baseImponible = Math.Round(this.baseImponible, 2);
                this.importeIva = Math.Round(this.importeIva, 2);
                this.importeTotal = Math.Round(this.importeTotal, 2);
                resumenesSalida2 = new List<Resumen>(from r in resumenesSalida2
                                                     orderby r.id
                                                     select r);
                string warning = this.comprobarResumenesFacturados(resumenesSalida2);
                using (ObjectReader reader2 = ObjectReader.Create(this._empresa))
                {
                    tablaEmpresa.Load(reader2);
                }
                using (ObjectReader reader3 = ObjectReader.Create(resumenesSalida2))
                {
                    tablaResumen.Load(reader3);
                }
                using (ObjectReader reader = ObjectReader.Create(itinerarios))
                {
                    tablaItinerario.Load(reader);
                }
                ReportDataSource dsEmpresa = new ReportDataSource("DataSet1", tablaEmpresa);
                ReportDataSource dsResumenes = new ReportDataSource("DataSet2", tablaResumen);
                ReportDataSource dsItinerario = new ReportDataSource("DataSet3", tablaItinerario);
                this._reportViewer.LocalReport.DataSources.Add(dsEmpresa);
                this._reportViewer.LocalReport.DataSources.Add(dsResumenes);
                this._reportViewer.LocalReport.DataSources.Add(dsItinerario);
                string exeDir = AppDomain.CurrentDomain.BaseDirectory;
                string reportDir = Path.Combine(exeDir, "Reportes\\Report1.rdlc");
                this._reportViewer.LocalReport.ReportPath = reportDir;
                ReportParameter[] parameters = new ReportParameter[6]
                {
                new ReportParameter("idFactura", this.idFactura.ToString()),
                null,
                null,
                null,
                null,
                null
                };
                char letraTipoFactura = this.busqueda.letraTipoFactura;
                if (letraTipoFactura.Equals('F'))
                {
                    parameters[1] = new ReportParameter("baseImponible", this.baseImponible.ToString("f2"));
                    parameters[2] = new ReportParameter("importeIva", this.importeIva.ToString("f2"));
                    parameters[3] = new ReportParameter("importeTotal", this.importeTotal.ToString("f2"));
                }
                else
                {
                    this.baseImponible = 0.0 - this.baseImponible;
                    this.importeIva = 0.0 - this.importeIva;
                    this.importeTotal = 0.0 - this.importeTotal;
                    parameters[1] = new ReportParameter("baseImponible", this.baseImponible.ToString("f2"));
                    parameters[2] = new ReportParameter("importeIva", this.importeIva.ToString("f2"));
                    parameters[3] = new ReportParameter("importeTotal", this.importeTotal.ToString("f2"));
                }
                ReportParameter[] array = parameters;
                letraTipoFactura = this.busqueda.letraTipoFactura;
                array[4] = new ReportParameter("tipoReporte", "- " + letraTipoFactura.ToString());
                parameters[5] = new ReportParameter("fechaEmisionFactura", this.busqueda.fechaEmisionFactura.ToShortDateString());
                this._reportViewer.ShowParameterPrompts = true;
                this._reportViewer.LocalReport.SetParameters(parameters);
                this._reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                this._reportViewer.RefreshReport();
                if (!warning.Equals(""))
                {
                    MessageBox.Show(warning, "AVISO", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        private string comprobarResumenesFacturados(List<Resumen> lista)
        {
            List<int> resumenesFacturados = new List<int>(FacturasCRUD.comprobarResumenesFacturados(lista));
            if (resumenesFacturados.Count > 0)
            {
                string msjTipo22 = "¡NOTA! ESTA FACTURA YA HA SIDO FACTURADA EN OTRA OCASIÓN. POR FAVOR, NO IMPRIMA O EXPORTE ÉSTA FACTURA.";
                string msjTipo21 = "¡NOTA! LAS SIGUIENTES ENTRADAS DE LA FACTURA YA HAN SIDO EMITIDAS EN UNA ANTERIOR FACTURA:";
                string msjTipop22 = "\nPOR FAVOR, NO IMPRIMA O EXPORTE ÉSTA FACTURA.";
                string texto4 = "";
                if (resumenesFacturados.Count == lista.Count)
                {
                    texto4 = msjTipo22;
                }
                else
                {
                    texto4 = msjTipo21;
                    texto4 += "\n\n";
                    foreach (int item in resumenesFacturados)
                    {
                        texto4 = texto4 + "ENTRADA " + item + "\n";
                    }
                    texto4 += msjTipop22;
                }
                return texto4;
            }
            return "";
        }

        private void generarFacturaIva()
        {
            TrimestreAño ta = new TrimestreAño(this.trimestre, this.año);
            List<GastoNormal> gastos2 = GastosCRUD.cogerTodosGastosNormalesPorTrimestreAño(ta).Cast<GastoNormal>().ToList();
            if (gastos2.Count > 0)
            {
                base.Show();

                DataTable tablaGasto = new DataTable();
                using (ObjectReader reader = ObjectReader.Create(gastos2))
                {
                    tablaGasto.Load(reader);
                }
                ReportDataSource dsGasto = new ReportDataSource("DataSet4", tablaGasto);
                
                List<ResumenGasto> rGasto = gastos2.GroupBy(g => g.iva).Select(a => new ResumenGasto(Math.Round(a.Sum(x => x.importeBase),2), a.Key, Math.Round(a.Sum(x => x.importeBase * a.Key / 100),2), Math.Round(a.Sum(x => -(x.importeBase * x.irpf / 100)),2), Math.Round(a.Sum(x => x.tasas),2), Math.Round(a.Sum(x => x.cuotaDeducible),2))).OrderBy(x => x.iva).ToList();
                ResumenGasto totales = rGasto.Aggregate((a, b) => new ResumenGasto(a.importeBaseTotal + b.importeBaseTotal,0,a.ivaTotal + b.ivaTotal, a.irpfTotal + b.irpfTotal,a.tasasTotales + b.tasasTotales, a.cuotaDeducibleTotal + b.cuotaDeducibleTotal));
                 
                double importeBaseTotal = totales.importeBaseTotal;
                double irpfTotal = totales.irpfTotal;
                double ivaTotal = totales.ivaTotal;
                double cuotaDeducibleTotal = totales.cuotaDeducibleTotal;
                double tasasTotal = totales.tasasTotales;
                
                DataTable tablaResumenGasto = new DataTable();
                using (ObjectReader reader = ObjectReader.Create(rGasto))
                {
                    tablaResumenGasto.Load(reader);
                }
                ReportDataSource dsResumenGasto = new ReportDataSource("DataSet7", tablaResumenGasto);

                this._reportViewer.LocalReport.DataSources.Add(dsGasto);
                this._reportViewer.LocalReport.DataSources.Add(dsResumenGasto);
                string exeDir = AppDomain.CurrentDomain.BaseDirectory;
                string reportDir = Path.Combine(exeDir, "Reportes\\Report99.rdlc");
                this._reportViewer.LocalReport.ReportPath = reportDir;

                string tituloCabecera;
                if (this.trimestre != 0)
                    tituloCabecera = "EJERCICIO: TRIMESTRE " + this.trimestre + ", " + this.año;
                else
                    tituloCabecera = "EJERCICIO: AÑO " + this.año;
                ReportParameter[] parameters = new ReportParameter[6]
                {
                    new ReportParameter("importeBaseTotal", importeBaseTotal.ToString()),
                    new ReportParameter("irpfTotal", irpfTotal.ToString()),
                    new ReportParameter("ivaTotal", ivaTotal.ToString()),
                    new ReportParameter("cuotaDeducibleTotal", cuotaDeducibleTotal.ToString()),
                    new ReportParameter("tasasTotales", tasasTotal.ToString()),
                    new ReportParameter("tituloCabecera", tituloCabecera)
                };

                this._reportViewer.ShowParameterPrompts = true;
                this._reportViewer.LocalReport.SetParameters(parameters);
                this._reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                this._reportViewer.RefreshReport();
            }
            else
            {
                MessageBox.Show("No existen gastos para las fechas dadas.", "Error factura", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                base.Close();
            }
        }

        private void generarFacturaIvaBienes()
        {
            TrimestreAño ta = new TrimestreAño(this.trimestre, this.año);
            List<GastoBienInversion> gastos2 = GastosCRUD.cogerTodosGastosBienInversionPorTrimestreAño(ta).Cast<GastoBienInversion>().ToList();
            if (gastos2.Count > 0)
            {
                base.Show();
                DataTable tablaGasto = new DataTable();
                using (ObjectReader reader = ObjectReader.Create(gastos2))
                {
                    tablaGasto.Load(reader);
                }
                ReportDataSource dsGasto = new ReportDataSource("DataSet6", tablaGasto);

                List<ResumenGasto> rGasto = gastos2.GroupBy(g => g.iva).Select(a => new ResumenGasto(Math.Round(a.Sum(x => x.amortizacion), 2), a.Key, Math.Round(a.Sum(x => (x.amortizacion + x.intereses)*x.iva/100), 2), Math.Round(a.Sum(x => x.intereses),2), Math.Round(a.Sum(x => x.tasas), 2), Math.Round(a.Sum(x => x.cuotaDeducible), 2))).OrderBy(x => x.iva).ToList();
                ResumenGasto totales = rGasto.Aggregate((a, b) => new ResumenGasto(a.importeBaseTotal + b.importeBaseTotal, 0, a.ivaTotal + b.ivaTotal, a.irpfTotal + b.irpfTotal, a.tasasTotales + b.tasasTotales, a.cuotaDeducibleTotal + b.cuotaDeducibleTotal));
                
                double amortizacionTotal = totales.importeBaseTotal;
                double interesTotal = totales.irpfTotal;
                double ivaTotal = totales.ivaTotal;
                double cuotaDeducibleTotal = totales.cuotaDeducibleTotal;
                double tasasTotal = totales.tasasTotales;

                DataTable tablaResumenGasto = new DataTable();
                using (ObjectReader reader = ObjectReader.Create(rGasto))
                {
                    tablaResumenGasto.Load(reader);
                }
                ReportDataSource dsResumenGasto = new ReportDataSource("DataSet7", tablaResumenGasto);

                this._reportViewer.LocalReport.DataSources.Add(dsGasto);
                this._reportViewer.LocalReport.DataSources.Add(dsResumenGasto);

                string exeDir = AppDomain.CurrentDomain.BaseDirectory;
                string reportDir = Path.Combine(exeDir, "Reportes\\Report5.rdlc");
                this._reportViewer.LocalReport.ReportPath = reportDir;

                string tituloCabecera;
                if (this.trimestre != 0)
                    tituloCabecera = "EJERCICIO: TRIMESTRE " + this.trimestre + ", " + this.año;
                else
                    tituloCabecera = "EJERCICIO: AÑO " + this.año;
                ReportParameter[] parameters = new ReportParameter[6]
                {
                    new ReportParameter("amortizacionTotal", amortizacionTotal.ToString()),
                    new ReportParameter("interesTotal", interesTotal.ToString()),
                    new ReportParameter("ivaTotal", ivaTotal.ToString()),
                    new ReportParameter("cuotaDeducibleTotal", cuotaDeducibleTotal.ToString()),
                    new ReportParameter("tasasTotal", tasasTotal.ToString()),
                    new ReportParameter("tituloCabecera", tituloCabecera)
                };

                this._reportViewer.ShowParameterPrompts = true;
                this._reportViewer.LocalReport.SetParameters(parameters);
                this._reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                this._reportViewer.RefreshReport();
            }
            else
            {
                MessageBox.Show("No existen gastos para las fechas dadas.", "Error factura", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                base.Close();
            }
        }

        private void _reportViewer_PrintingBegin(object sender, ReportPrintEventArgs e)
        {
            if (!this.facturaImpimida && this.tipo.Equals("factura"))
            {
                FacturasCRUD.añadirFactura(new Factura(this.idFactura, this.busqueda.empresa.nombre, this.busqueda.fechaEmisionFactura, this.baseImponible, this.importeIva, this.importeTotal, this.componentesFactura, this.busqueda.letraTipoFactura));
                this.facturaImpimida = true;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!this.facturaImpimida && this.tipo.Equals("factura"))
            {
                ResumenesCRUD.CancelarNumeroFactura();
            }
            new VentanaMenuPrincipal().Show();
        }

        private void _reportViewer_ReportExport(object sender, ReportExportEventArgs e)
        {
            if (!this.facturaImpimida && this.tipo.Equals("factura"))
            {
                if (MessageBox.Show("¿Desea exportar el resumen? Contará como una nueva factura generada.", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    FacturasCRUD.añadirFactura(new Factura(this.idFactura, this.busqueda.empresa.nombre, this.busqueda.fechaEmisionFactura, this.baseImponible, this.importeIva, this.importeTotal, this.componentesFactura, this.busqueda.letraTipoFactura));
                    if (this.busqueda.tipo.Equals("factura"))
                    {
                        this.facturaImpimida = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
