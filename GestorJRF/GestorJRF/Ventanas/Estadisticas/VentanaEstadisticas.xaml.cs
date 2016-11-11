using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Estadisticas;
using GestorJRF.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            InitializeComponent();
            UtilidadesVentana.SituarVentana(2, this);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new VentanaMenuPrincipal().Show();
        }

        private void mostrarGrafico(int tipo)
        {
            if (tipo == 0)
            {
                graficoBarras.Visibility = Visibility.Hidden;
                graficoLineas.Visibility = Visibility.Hidden;
            }
            else if (tipo == 1)
            {
                graficoBarras.Visibility = Visibility.Hidden;
                graficoLineas.Visibility = Visibility.Visible;
            }
            else
            {
                graficoLineas.Visibility = Visibility.Hidden;
                graficoBarras.Visibility = Visibility.Visible;
            }
        }

        public void generarGraficoBarras()
        {
            List<KeyValuePair<string, double>> resultados = new List<KeyValuePair<string, double>>();

            if (opciones.tipo.Contains("Facturacion"))
            {
                if (opciones.tipo.Contains("Empleado"))
                {
                    List<Empleado> empleados = EmpleadosCRUD.cogerTodosEmpleados().Cast<Empleado>().ToList();
                    if (empleados != null)
                    {
                        foreach (Empleado e in empleados)
                        {
                            opciones.dni = e.dni;
                            var sumartorio = ResumenesCRUD.cogerResumenesFinalesPorDni(opciones);
                            if (sumartorio > 0)
                                resultados.Add(new KeyValuePair<string, double>(e.getNombreApellidos(), sumartorio));
                        }
                    }
                }
                else
                {
                    List<Empresa> empresas = EmpresasCRUD.cogerTodasEmpresas().Cast<Empresa>().ToList();
                    if (empresas != null)
                    {
                        foreach (Empresa e in empresas)
                        {
                            opciones.cif = e.cif;
                            var sumartorio = ResumenesCRUD.cogerResumenesFinalesPorCif(opciones);
                            if (sumartorio > 0)
                                resultados.Add(new KeyValuePair<string, double>(e.nombre, sumartorio));
                        }
                    }
                }
            }
            else
            {
                if (opciones.tipo.Contains("Empleado"))
                {
                    List<Empleado> empleados = EmpleadosCRUD.cogerTodosEmpleados().Cast<Empleado>().ToList();
                    if (empleados != null)
                    {
                        foreach (Empleado e in empleados)
                        {
                            opciones.dni = e.dni;
                            var sumartorio = GastosCRUD.cogerSumatorioGastosParaEstadisticaPorEmpleado(opciones);
                            if (sumartorio > 0)
                                resultados.Add(new KeyValuePair<string, double>(e.getNombreApellidos(), sumartorio));
                        }
                    }
                }
                else
                {
                    List<Proveedor> proveedores = ProveedoresCRUD.cogerTodosProveedores().Cast<Proveedor>().ToList();
                    if (proveedores != null)
                    {
                        foreach (Proveedor p in proveedores)
                        {
                            opciones.cif = p.cif;
                            var sumartorio = GastosCRUD.cogerSumatorioGastosParaEstadisticaPorProveedor(opciones);
                            if (sumartorio > 0)
                                resultados.Add(new KeyValuePair<string, double>(p.nombre, sumartorio));
                        }
                    }
                }
            }
            
            if (resultados.Count > 0)
            {
                ((ColumnSeries)graficoBarras.Series[0]).ItemsSource = resultados;
                mostrarGrafico(2);
            }
            else
                MessageBox.Show("No hay datos para generar la gráfica", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void generarGraficaLineal()
        {
            bool hayDatos = false;

            List<ResumenEstadistica> resumenes;
            List<GastoEstadistica> gastos;

            if (opciones.tipo.Contains("facturacion"))
            {
                if (opciones.tipo.Contains("Empleado"))
                    resumenes = ResumenesCRUD.cogerResumenesParaEstadisticaPorEmpleado(opciones).Cast<ResumenEstadistica>().ToList();
                else if (opciones.tipo.Contains("Empresa"))
                    resumenes = ResumenesCRUD.cogerResumenesParaEstadisticaPorEmpresa(opciones).Cast<ResumenEstadistica>().ToList();
                else
                    resumenes = ResumenesCRUD.cogerResumenesParaEstadistica(opciones).Cast<ResumenEstadistica>().ToList();

                gastos = null;

                if (resumenes.Count > 0)
                    hayDatos = true;
            }
            else
            {
                if (opciones.tipo.Contains("Empleado"))
                    gastos = GastosCRUD.cogerTodosGastosParaEstadisticaPorEmpleado(opciones).Cast<GastoEstadistica>().ToList();
                else if (opciones.tipo.Contains("Empresa"))
                    gastos = GastosCRUD.cogerTodosGastosParaEstadisticaPorProveedor(opciones).Cast<GastoEstadistica>().ToList();
                else
                    gastos = GastosCRUD.cogerTodosGastosParaEstadistica(opciones).Cast<GastoEstadistica>().ToList();

                resumenes = null;

                if (gastos.Count > 0)
                    hayDatos = true;
            }

            if (hayDatos)
            {
                List<KeyValuePair<DateTime, double>> listaPares = new List<KeyValuePair<DateTime, double>>();
                Style estilo = new Style(typeof(DateTimeAxisLabel));

                DateTimeAxis axisX = new DateTimeAxis();
                axisX.Orientation = AxisOrientation.X;
                axisX.ShowGridLines = true;

                double sumatorio;

                //CAMBIAR TITULO DEL GRAFICO DEPENDIENDO DE EL TIPO

                switch (opciones.tipo)
                {
                    case "gastoMensualEmpleado":
                    case "gastoMensualProveedor":
                    case "gastoMensual":
                    case "facturacionMensualEmpleado":
                    case "facturacionMensualEmpresa":
                    case "facturacionMensual":
                        estilo.Setters.Add(new Setter(DateTimeAxisLabel.StringFormatProperty, "{0:dd}"));

                        axisX.Title = "DÍAS DEL MES";
                        axisX.Interval = 1;
                        axisX.IntervalType = DateTimeIntervalType.Days;
                        axisX.AxisLabelStyle = estilo;

                        break;

                    case "gastoTrimestralEmpleado":
                    case "gastoTrimestralProveedor":
                    case "gastoTrimestral":
                    case "facturacionTrimestralEmpleado":
                    case "facturacionTrimestralEmpresa":
                    case "facturacionTrimestral":
                        estilo.Setters.Add(new Setter(DateTimeAxisLabel.StringFormatProperty, "{0:dd-MMMM}"));

                        axisX.Title = "DÍAS DEL TRIMESTRE";
                        axisX.Interval = 7;
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
                        estilo.Setters.Add(new Setter(DateTimeAxisLabel.StringFormatProperty, "{0:MMMM-yyyy}"));

                        axisX.Title = "MESES AÑO";
                        axisX.Interval = 1;
                        axisX.IntervalType = DateTimeIntervalType.Months;
                        break;
                    default:
                        break;
                }

                sumatorio = 0;
                if (opciones.tipo.Contains("facturacion"))
                {
                    foreach (ResumenEstadistica r in resumenes)
                    {
                        sumatorio += r.sumaDiaria;
                        listaPares.Add(new KeyValuePair<DateTime, double>(r.fechaPorte, Math.Round(sumatorio, 2)));
                    }
                }
                else
                {
                    foreach (GastoEstadistica g in gastos)
                    {
                        sumatorio += g.sumaImporteBase;
                        listaPares.Add(new KeyValuePair<DateTime, double>(g.fecha, Math.Round(sumatorio, 2)));
                    }
                }



                axisX.Maximum = opciones.fechaFinal.Date;
                axisX.Minimum = opciones.fechaInicio.Date;
                axisX.AxisLabelStyle = estilo;
                _graficoLineas.IndependentAxis = axisX;

                LinearAxis axisY = new LinearAxis();
                axisY.Orientation = AxisOrientation.Y;
                axisY.Title = "CUENTA ACUMULADA";
                axisY.Minimum = 0;
                axisY.Maximum = sumatorio + sumatorio / 3;
                _graficoLineas.DependentRangeAxis = axisY;


                ((LineSeries)graficoLineas.Series[0]).ItemsSource = listaPares;
                mostrarGrafico(1);
            }
            else
                MessageBox.Show("No hay datos para generar la gráfica", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void bFacturacionMensual_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesMensual(this, "facturacionMensual").Show();
        }

        private void bFacturacionTrimestral_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "facturacionTrimestral").Show();
        }

        private void bFacturacionAnual_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "facturacionAnual").Show();
        }

        private void bFacturacionGeneral_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, "facturacionGeneral");
            generarGraficaLineal();
        }

        private void bFacturacionMensualEmpresas_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesMensualEmpresaEmpleado(this, "facturacionMensualEmpresa").Show();
        }

        private void bFacturacionTrimestralEmpresas_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "facturacionTrimestralEmpresa").Show();
        }

        private void bFacturacionAnualEmpresas_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "facturacionAnualEmpresa").Show();
        }

        private void bFacturacionGeneralEmpresas_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesGeneralEmpresaEmpleado(this, "facturacionGeneralEmpresa").Show();
        }

        private void bFacturacionMensualEmpleado_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesMensualEmpresaEmpleado(this, "facturacionMensualEmpleado").Show();
        }

        private void bFacturacionTrimestralEmpleado_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "facturacionTrimestralEmpleado").Show();
        }

        private void bFacturacionAnualEmpleado_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "facturacionAnualEmpleado").Show();
        }

        private void bFacturacionGeneralEmpleado_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesGeneralEmpresaEmpleado(this, "facturacionGeneralEmpleado").Show();
        }

        private void bGastosMensual_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesMensual(this, "gastoMensual").Show();
        }

        private void bGastosTrimestral_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "gastoTrimestral").Show();
        }

        private void bGastosAnual_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "gastoAnual").Show();
        }

        private void bGastosGeneral_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, "gastoGeneral");
            generarGraficaLineal();
        }

        private void bGastosMensualProveedor_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesMensualEmpresaEmpleado(this, "gastoMensualProveedor").Show();
        }

        private void bGastosTrimestralProveedor_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "gastoTrimestralProveedor").Show();
        }

        private void bGastosAnualProveedor_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "gastoAnualProveedor").Show();
        }

        private void bGastosGeneralProveedor_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesGeneralEmpresaEmpleado(this, "gastoGeneralProveedor").Show();
        }

        private void bGastosMensualEmpleado_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesMensualEmpresaEmpleado(this, "gastoMensualEmpleado").Show();
        }

        private void bGastosTrimestralEmpleado_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "gastoTrimestralEmpleado").Show();
        }

        private void bGastosAnualEmpleado_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnualEmpresaEmpleado(this, "gastoAnualEmpleado").Show();
        }

        private void bGastosGeneralEmpleado_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesGeneralEmpresaEmpleado(this, "gastoGeneralEmpleado").Show();
        }

        private void bCompFactMensualEmpresas_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesMensual(this, "comparativaFacturacionEmpresasMenusal").Show();
        }

        private void bCompFactTrimestralEmpresas_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaFacturacionEmpresasTrimestral").Show();
        }

        private void bCompFactAnualEmpresas_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaFacturacionEmpresasAnual").Show();
        }

        private void bCompFactGeneralEmpresas_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, "comparativaFacturacionEmpresasGeneral");
            generarGraficoBarras();
        }

        private void bCompFactMensualEmpleados_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesMensual(this, "comparativaFacturacionEmpleadosMenusal").Show();
        }

        private void bCompFactTrimestralEmpleados_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaFacturacionEmpleadosTrimestral").Show();
        }

        private void bCompFactAnualEmpleados_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaFacturacionEmpleadosAnual").Show();
        }

        private void bCompFactGeneralEmpleados_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, "comparativaFacturacionEmpleadosGeneral");
            generarGraficoBarras();
        }

        private void bCompGastosMensualProveedores_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesMensual(this, "comparativaGastosProveedoresMenusal").Show();
        }

        private void bCompGastosTrimestralProveedores_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaGastosProveedoresTrimestral").Show();
        }

        private void bCompGastosAnualProveedores_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaGastosProveedoresAnual").Show();
        }

        private void bCompGastosGeneralProveedores_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, "comparativaGastosProveedoresGeneral");
            generarGraficoBarras();
        }

        private void bCompGastosMensualEmpleados_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesMensual(this, "comparativaGastosEmpleadosMenusal").Show();
        }

        private void bCompGastosTrimestralEmpleados_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaGastosEmpleadosTrimestral").Show();
        }

        private void bCompGastosAnualEmpleados_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            new VentanaOpcionesTrimestralAnual(this, "comparativaGastosEmpleadosAnual").Show();
        }

        private void bCompGastosGeneralEmpleados_Click(object sender, RoutedEventArgs e)
        {
            mostrarGrafico(0);
            opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, "comparativaGastosEmpleadosGeneral");
            generarGraficoBarras();
        }
    }
}
