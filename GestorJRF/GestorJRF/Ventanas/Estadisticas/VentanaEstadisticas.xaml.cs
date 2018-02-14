using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Estadisticas;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.Estadisticas;
using GestorJRF.Ventanas.Login;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.Estadisticas
{
    /// <summary>
    /// Lógica de interacción para VentanaEstadisticas.xaml
    /// </summary>
    public partial class VentanaEstadisticas : Window
    {
        public BusquedaEstadisticas opciones { get; set; }

        public VentanaEstadisticas()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(2, this);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            new VentanaMenuPrincipal().Show();
        }

        private void mostrarGrafico(int tipo)
        {
            switch (tipo)
            {
                case 0:
                    this.graficoBarras.Visibility = Visibility.Hidden;
                    this.graficoLineas.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    this.graficoBarras.Visibility = Visibility.Hidden;
                    this.graficoLineas.Visibility = Visibility.Visible;
                    break;
                default:
                    this.graficoLineas.Visibility = Visibility.Hidden;
                    this.graficoBarras.Visibility = Visibility.Visible;
                    break;
            }
        }

        public void generarGraficoBarras()
        {
            List<KeyValuePair<string, double>> resultados2 = new List<KeyValuePair<string, double>>();
            DateTime dateTime;
            if (this.opciones.tipo.Contains("Facturacion"))
            {
                if (this.opciones.tipo.Contains("Empleado"))
                {
                    List<Empleado> empleados2 = EmpleadosCRUD.cogerTodosEmpleados().Cast<Empleado>().ToList();
                    if (empleados2 != null)
                    {
                        foreach (Empleado item in empleados2)
                        {
                            this.opciones.dni = item.dni;
                            double sumartorio4 = ResumenesCRUD.cogerResumenesFinalesPorDni(this.opciones);
                            if (sumartorio4 > 0.0)
                            {
                                resultados2.Add(new KeyValuePair<string, double>(item.getNombreApellidos(), Math.Round(sumartorio4, 2)));
                            }
                        }
                        Chart chart = this.graficoBarras;
                        string[] obj = new string[5]
                        {
                        "FACTURACIÓN EMPLEADO [",
                        null,
                        null,
                        null,
                        null
                        };
                        dateTime = this.opciones.fechaInicio;
                        obj[1] = dateTime.ToString("dd-MM-yyyy");
                        obj[2] = "  ||  ";
                        dateTime = this.opciones.fechaFinal;
                        obj[3] = dateTime.ToString("dd-MM-yyyy");
                        obj[4] = "]";
                        chart.Title = string.Concat(obj);
                    }
                }
                else
                {
                    List<Empresa> empresas = EmpresasCRUD.cogerTodasEmpresas().Cast<Empresa>().ToList();
                    if (empresas != null)
                    {
                        foreach (Empresa item2 in empresas)
                        {
                            this.opciones.cif = item2.cif;
                            double sumartorio3 = ResumenesCRUD.cogerResumenesFinalesPorCif(this.opciones);
                            if (sumartorio3 > 0.0)
                            {
                                resultados2.Add(new KeyValuePair<string, double>(item2.nombre, sumartorio3));
                            }
                        }
                        Chart chart2 = this.graficoBarras;
                        string[] obj2 = new string[5]
                        {
                        "FACTURACIÓN EMPRESA [",
                        null,
                        null,
                        null,
                        null
                        };
                        dateTime = this.opciones.fechaInicio;
                        obj2[1] = dateTime.ToString("dd-MM-yyyy");
                        obj2[2] = "  ||  ";
                        dateTime = this.opciones.fechaFinal;
                        obj2[3] = dateTime.ToString("dd-MM-yyyy");
                        obj2[4] = "]";
                        chart2.Title = string.Concat(obj2);
                    }
                }
            }
            else if (this.opciones.tipo.Contains("Empleado"))
            {
                List<Empleado> empleados = EmpleadosCRUD.cogerTodosEmpleados().Cast<Empleado>().ToList();
                if (empleados != null)
                {
                    foreach (Empleado item3 in empleados)
                    {
                        this.opciones.dni = item3.dni;
                        double sumartorio2 = GastosCRUD.cogerSumatorioGastosParaEstadisticaPorEmpleado(this.opciones);
                        if (sumartorio2 > 0.0)
                        {
                            resultados2.Add(new KeyValuePair<string, double>(item3.getNombreApellidos(), sumartorio2));
                        }
                    }
                    Chart chart3 = this.graficoBarras;
                    string[] obj3 = new string[5]
                    {
                    "GASTO EMPLEADO [",
                    null,
                    null,
                    null,
                    null
                    };
                    dateTime = this.opciones.fechaInicio;
                    obj3[1] = dateTime.ToString("dd-MM-yyyy");
                    obj3[2] = "  ||  ";
                    dateTime = this.opciones.fechaFinal;
                    obj3[3] = dateTime.ToString("dd-MM-yyyy");
                    obj3[4] = "]";
                    chart3.Title = string.Concat(obj3);
                }
            }
            else
            {
                List<Proveedor> proveedores = ProveedoresCRUD.cogerTodosProveedores().Cast<Proveedor>().ToList();
                if (proveedores != null)
                {
                    foreach (Proveedor item4 in proveedores)
                    {
                        this.opciones.cif = item4.cif;
                        double sumartorio = GastosCRUD.cogerSumatorioGastosParaEstadisticaPorProveedor(this.opciones);
                        if (sumartorio > 0.0)
                        {
                            resultados2.Add(new KeyValuePair<string, double>(item4.nombre, sumartorio));
                        }
                    }
                    Chart chart4 = this.graficoBarras;
                    string[] obj4 = new string[5]
                    {
                    "GASTO EMPRESA [",
                    null,
                    null,
                    null,
                    null
                    };
                    dateTime = this.opciones.fechaInicio;
                    obj4[1] = dateTime.ToString("dd-MM-yyyy");
                    obj4[2] = "  ||  ";
                    dateTime = this.opciones.fechaFinal;
                    obj4[3] = dateTime.ToString("dd-MM-yyyy");
                    obj4[4] = "]";
                    chart4.Title = string.Concat(obj4);
                }
            }
            if (resultados2.Count > 0)
            {
                resultados2 = ((resultados2.Count <= 10) ? new List<KeyValuePair<string, double>>(from c in resultados2
                                                                                                  orderby c.Value descending
                                                                                                  select c) : new List<KeyValuePair<string, double>>(this.filtrarResultados(resultados2)));
                ((ColumnSeries)this.graficoBarras.Series[0]).ItemsSource = resultados2;
                this.mostrarGrafico(2);
            }
            else
            {
                MessageBox.Show("No hay datos para generar la gráfica", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        public void generarGraficaLineal()
        {
            bool hayDatos = false;
            List<ResumenEstadistica> resumenes;
            DateTime dateTime;
            List<GastoEstadistica> gastos;
            if (this.opciones.tipo.Contains("facturacion"))
            {
                if (this.opciones.tipo.Contains("Empleado"))
                {
                    resumenes = ResumenesCRUD.cogerResumenesParaEstadisticaPorEmpleado(this.opciones).Cast<ResumenEstadistica>().ToList();
                    Chart chart = this.graficoLineas;
                    string[] obj = new string[5]
                    {
                    "FACTURACIÓN EMPLEADO [",
                    null,
                    null,
                    null,
                    null
                    };
                    dateTime = this.opciones.fechaInicio;
                    obj[1] = dateTime.ToString("dd-MM-yyyy");
                    obj[2] = "  ||  ";
                    dateTime = this.opciones.fechaFinal;
                    obj[3] = dateTime.ToString("dd-MM-yyyy");
                    obj[4] = "]";
                    chart.Title = string.Concat(obj);
                }
                else if (this.opciones.tipo.Contains("Empresa"))
                {
                    resumenes = ResumenesCRUD.cogerResumenesParaEstadisticaPorEmpresa(this.opciones).Cast<ResumenEstadistica>().ToList();
                    Chart chart2 = this.graficoLineas;
                    string[] obj2 = new string[5]
                    {
                    "FACTURACIÓN EMPRESA [",
                    null,
                    null,
                    null,
                    null
                    };
                    dateTime = this.opciones.fechaInicio;
                    obj2[1] = dateTime.ToString("dd-MM-yyyy");
                    obj2[2] = "  ||  ";
                    dateTime = this.opciones.fechaFinal;
                    obj2[3] = dateTime.ToString("dd-MM-yyyy");
                    obj2[4] = "]";
                    chart2.Title = string.Concat(obj2);
                }
                else
                {
                    resumenes = ResumenesCRUD.cogerResumenesParaEstadistica(this.opciones).Cast<ResumenEstadistica>().ToList();
                    Chart chart3 = this.graficoLineas;
                    string[] obj3 = new string[5]
                    {
                    "FACTURACIÓN GENERAL [",
                    null,
                    null,
                    null,
                    null
                    };
                    dateTime = this.opciones.fechaInicio;
                    obj3[1] = dateTime.ToString("dd-MM-yyyy");
                    obj3[2] = "  ||  ";
                    dateTime = this.opciones.fechaFinal;
                    obj3[3] = dateTime.ToString("dd-MM-yyyy");
                    obj3[4] = "]";
                    chart3.Title = string.Concat(obj3);
                }
                gastos = null;
                if (resumenes.Count > 0)
                {
                    hayDatos = true;
                }
            }
            else
            {
                if (this.opciones.tipo.Contains("Empleado"))
                {
                    gastos = GastosCRUD.cogerTodosGastosParaEstadisticaPorEmpleado(this.opciones).Cast<GastoEstadistica>().ToList();
                    Chart chart4 = this.graficoLineas;
                    string[] obj4 = new string[5]
                    {
                    "GASTO EMPLEADO [",
                    null,
                    null,
                    null,
                    null
                    };
                    dateTime = this.opciones.fechaInicio;
                    obj4[1] = dateTime.ToString("dd-MM-yyyy");
                    obj4[2] = "  ||  ";
                    dateTime = this.opciones.fechaFinal;
                    obj4[3] = dateTime.ToString("dd-MM-yyyy");
                    obj4[4] = "]";
                    chart4.Title = string.Concat(obj4);
                }
                else if (this.opciones.tipo.Contains("Empresa"))
                {
                    gastos = GastosCRUD.cogerTodosGastosParaEstadisticaPorProveedor(this.opciones).Cast<GastoEstadistica>().ToList();
                    Chart chart5 = this.graficoLineas;
                    string[] obj5 = new string[5]
                    {
                    "GASTO PROVEEDOR [",
                    null,
                    null,
                    null,
                    null
                    };
                    dateTime = this.opciones.fechaInicio;
                    obj5[1] = dateTime.ToString("dd-MM-yyyy");
                    obj5[2] = "  ||  ";
                    dateTime = this.opciones.fechaFinal;
                    obj5[3] = dateTime.ToString("dd-MM-yyyy");
                    obj5[4] = "]";
                    chart5.Title = string.Concat(obj5);
                }
                else
                {
                    gastos = GastosCRUD.cogerTodosGastosParaEstadistica(this.opciones).Cast<GastoEstadistica>().ToList();
                    Chart chart6 = this.graficoLineas;
                    string[] obj6 = new string[5]
                    {
                    "GASTO GENERAL [",
                    null,
                    null,
                    null,
                    null
                    };
                    dateTime = this.opciones.fechaInicio;
                    obj6[1] = dateTime.ToString("dd-MM-yyyy");
                    obj6[2] = "  ||  ";
                    dateTime = this.opciones.fechaFinal;
                    obj6[3] = dateTime.ToString("dd-MM-yyyy");
                    obj6[4] = "]";
                    chart6.Title = string.Concat(obj6);
                }
                resumenes = null;
                if (gastos.Count > 0)
                {
                    hayDatos = true;
                }
            }
            if (hayDatos)
            {
                List<KeyValuePair<DateTime, double>> listaPares = new List<KeyValuePair<DateTime, double>>();
                Style estilo = new Style(typeof(DateTimeAxisLabel));
                DateTimeAxis axisX = new DateTimeAxis();
                axisX.Orientation = AxisOrientation.X;
                axisX.ShowGridLines = true;
                switch (this.opciones.tipo)
                {
                    case "gastoMensualEmpleado":
                    case "gastoMensualProveedor":
                    case "gastoMensual":
                    case "facturacionMensualEmpleado":
                    case "facturacionMensualEmpresa":
                    case "facturacionMensual":
                        estilo.Setters.Add(new Setter(AxisLabel.StringFormatProperty, "{0:dd}"));
                        axisX.Title = "DÍAS DEL MES";
                        axisX.Interval = 1.0;
                        axisX.IntervalType = DateTimeIntervalType.Days;
                        axisX.AxisLabelStyle = estilo;
                        break;
                    case "gastoTrimestralEmpleado":
                    case "gastoTrimestralProveedor":
                    case "gastoTrimestral":
                    case "facturacionTrimestralEmpleado":
                    case "facturacionTrimestralEmpresa":
                    case "facturacionTrimestral":
                        estilo.Setters.Add(new Setter(AxisLabel.StringFormatProperty, "{0:dd-MMMM}"));
                        axisX.Title = "DÍAS DEL TRIMESTRE";
                        axisX.Interval = 7.0;
                        axisX.IntervalType = DateTimeIntervalType.Days;
                        break;
                    case "gastoAnualEmpleado":
                    case "gastoAnualProveedor":
                    case "gastoAnual":
                    case "facturacionAnualEmpleado":
                    case "facturacionAnualEmpresa":
                    case "facturacionAnual":
                    case "gastonGeneralEmpleado":
                    case "gastoGeneralProveedor":
                    case "gastoGeneral":
                    case "facturacionGeneralEmpleado":
                    case "facturacionGeneralEmpresa":
                    case "facturacionGeneral":
                        estilo.Setters.Add(new Setter(AxisLabel.StringFormatProperty, "{0:MMMM-yyyy}"));
                        axisX.Title = "MESES AÑO";
                        axisX.Interval = 1.0;
                        axisX.IntervalType = DateTimeIntervalType.Months;
                        break;
                }
                double sumatorio = 0.0;
                if (this.opciones.tipo.Contains("facturacion"))
                {
                    foreach (ResumenEstadistica item in resumenes)
                    {
                        sumatorio += item.sumaDiaria;
                        listaPares.Add(new KeyValuePair<DateTime, double>(item.fechaPorte, Math.Round(sumatorio, 2)));
                    }
                }
                else
                {
                    foreach (GastoEstadistica item2 in gastos)
                    {
                        sumatorio += item2.sumaImporteBase;
                        listaPares.Add(new KeyValuePair<DateTime, double>(item2.fecha, Math.Round(sumatorio, 2)));
                    }
                }
                DateTimeAxis dateTimeAxis = axisX;
                dateTime = this.opciones.fechaFinal;
                dateTimeAxis.Maximum = dateTime.Date;
                DateTimeAxis dateTimeAxis2 = axisX;
                dateTime = this.opciones.fechaInicio;
                dateTimeAxis2.Minimum = dateTime.Date;
                axisX.AxisLabelStyle = estilo;
                this._graficoLineas.IndependentAxis = axisX;
                LinearAxis axisY = new LinearAxis();
                axisY.Orientation = AxisOrientation.Y;
                axisY.Title = "CUENTA ACUMULADA";
                axisY.Minimum = 0.0;
                axisY.Maximum = sumatorio + sumatorio / 3.0;
                this._graficoLineas.DependentRangeAxis = axisY;
                ((LineSeries)this.graficoLineas.Series[0]).ItemsSource = listaPares;
                this.mostrarGrafico(1);
            }
            else
            {
                MessageBox.Show("No hay datos para generar la gráfica", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private List<KeyValuePair<string, double>> filtrarResultados(List<KeyValuePair<string, double>> resultados)
        {
            List<KeyValuePair<string, double>> resultadosAux = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<string, double>> listaOrdenada = new List<KeyValuePair<string, double>>(from c in resultados
                                                                                                      orderby c.Value descending
                                                                                                      select c);
            for (int x2 = 0; x2 <= 9; x2++)
            {
                resultadosAux.Add(listaOrdenada[x2]);
            }
            double sumatorioOtrosValores = 0.0;
            for (int x = 10; x <= resultados.Count - 1; x++)
            {
                sumatorioOtrosValores += listaOrdenada[x].Value;
            }
            resultadosAux.Add(new KeyValuePair<string, double>("OTROS", sumatorioOtrosValores));
            return resultadosAux;
        }

        private void bFacturacionMensual_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesMensual(this, "facturacionMensual").Show();
        }

        private void bFacturacionTrimestral_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "facturacionTrimestral").Show();
        }

        private void bFacturacionAnual_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "facturacionAnual").Show();
        }

        private void bFacturacionGeneral_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            this.opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, "facturacionGeneral");
            this.generarGraficaLineal();
        }

        private void bFacturacionMensualEmpresas_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesMensualEmpresaEmpleado(this, "facturacionMensualEmpresa").Show();
        }

        private void bFacturacionTrimestralEmpresas_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "facturacionTrimestralEmpresa").Show();
        }

        private void bFacturacionAnualEmpresas_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "facturacionAnualEmpresa").Show();
        }

        private void bFacturacionGeneralEmpresas_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesGeneralEmpresaEmpleado(this, "facturacionGeneralEmpresa").Show();
        }

        private void bFacturacionMensualEmpleado_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesMensualEmpresaEmpleado(this, "facturacionMensualEmpleado").Show();
        }

        private void bFacturacionTrimestralEmpleado_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "facturacionTrimestralEmpleado").Show();
        }

        private void bFacturacionAnualEmpleado_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "facturacionAnualEmpleado").Show();
        }

        private void bFacturacionGeneralEmpleado_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesGeneralEmpresaEmpleado(this, "facturacionGeneralEmpleado").Show();
        }

        private void bGastosMensual_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesMensual(this, "gastoMensual").Show();
        }

        private void bGastosTrimestral_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "gastoTrimestral").Show();
        }

        private void bGastosAnual_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "gastoAnual").Show();
        }

        private void bGastosGeneral_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            this.opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, "gastoGeneral");
            this.generarGraficaLineal();
        }

        private void bGastosMensualProveedor_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesMensualEmpresaEmpleado(this, "gastoMensualProveedor").Show();
        }

        private void bGastosTrimestralProveedor_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "gastoTrimestralProveedor").Show();
        }

        private void bGastosAnualProveedor_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "gastoAnualProveedor").Show();
        }

        private void bGastosGeneralProveedor_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesGeneralEmpresaEmpleado(this, "gastoGeneralProveedor").Show();
        }

        private void bGastosMensualEmpleado_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesMensualEmpresaEmpleado(this, "gastoMensualEmpleado").Show();
        }

        private void bGastosTrimestralEmpleado_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "gastoTrimestralEmpleado").Show();
        }

        private void bGastosAnualEmpleado_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "gastoAnualEmpleado").Show();
        }

        private void bGastosGeneralEmpleado_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesGeneralEmpresaEmpleado(this, "gastoGeneralEmpleado").Show();
        }

        private void bCompFactMensualEmpresas_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesMensual(this, "comparativaFacturacionEmpresasMenusal").Show();
        }

        private void bCompFactTrimestralEmpresas_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaFacturacionEmpresasTrimestral").Show();
        }

        private void bCompFactAnualEmpresas_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaFacturacionEmpresasAnual").Show();
        }

        private void bCompFactGeneralEmpresas_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            this.opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, "comparativaFacturacionEmpresasGeneral");
            this.generarGraficoBarras();
        }

        private void bCompFactMensualEmpleados_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesMensual(this, "comparativaFacturacionEmpleadosMenusal").Show();
        }

        private void bCompFactTrimestralEmpleados_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaFacturacionEmpleadosTrimestral").Show();
        }

        private void bCompFactAnualEmpleados_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaFacturacionEmpleadosAnual").Show();
        }

        private void bCompFactGeneralEmpleados_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            this.opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, "comparativaFacturacionEmpleadosGeneral");
            this.generarGraficoBarras();
        }

        private void bCompGastosMensualProveedores_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesMensual(this, "comparativaGastosProveedoresMenusal").Show();
        }

        private void bCompGastosTrimestralProveedores_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaGastosProveedoresTrimestral").Show();
        }

        private void bCompGastosAnualProveedores_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaGastosProveedoresAnual").Show();
        }

        private void bCompGastosGeneralProveedores_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            this.opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, "comparativaGastosProveedoresGeneral");
            this.generarGraficoBarras();
        }

        private void bCompGastosMensualEmpleados_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesMensual(this, "comparativaGastosEmpleadosMenusal").Show();
        }

        private void bCompGastosTrimestralEmpleados_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaGastosEmpleadosTrimestral").Show();
        }

        private void bCompGastosAnualEmpleados_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaGastosEmpleadosAnual").Show();
        }

        private void bCompGastosGeneralEmpleados_Click(object sender, RoutedEventArgs e)
        {
            this.mostrarGrafico(0);
            this.opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, "comparativaGastosEmpleadosGeneral");
            this.generarGraficoBarras();
        }
    }
}
