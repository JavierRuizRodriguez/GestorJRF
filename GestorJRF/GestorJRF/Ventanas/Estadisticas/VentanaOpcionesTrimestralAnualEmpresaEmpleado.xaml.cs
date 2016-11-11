using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Estadisticas;
using GestorJRF.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Ventanas.Estadisticas
{
    /// <summary>
    /// Lógica de interacción para VentanaOpciones.xaml
    /// </summary>
    public partial class VentanaOpcionesTrimestralAnualEmpresaEmpleado : Window
    {
        private VentanaEstadisticas ventanaPadre;
        private string tipo;
        private List<Empresa> empresas;
        private List<Empleado> empleados;
        private List<Proveedor> proveedores;

        public VentanaOpcionesTrimestralAnualEmpresaEmpleado(VentanaEstadisticas ventanaPadre, string tipo)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.ventanaPadre = ventanaPadre;
            this.tipo = tipo;
            tAño.Text = DateTime.Now.Year.ToString();

            if (tipo.Contains("gasto"))
            {
                if (this.tipo.Contains("Proveedor"))
                {
                    lNombre.Content = "Proveedor";
                    var lista = ProveedoresCRUD.cogerTodosProveedores().Cast<Proveedor>().ToList();
                    proveedores = new List<Proveedor>(lista.OrderBy(p => p.nombre));
                    foreach (Proveedor p in proveedores)
                        cNombres.Items.Add(new ComboBoxItem().Content = p.nombre);
                }
                else
                {
                    lNombre.Content = "Empleado";
                    var lista = EmpleadosCRUD.cogerTodosEmpleados().Cast<Empleado>().ToList();
                    empleados = new List<Empleado>(lista.OrderBy(e => e.nombre));
                    foreach (Empleado e in empleados)
                        cNombres.Items.Add(new ComboBoxItem().Content = e.getNombreApellidos());
                }
            }
            else
            {

                if (!tipo.Contains("Empleado"))
                {
                    var lista = EmpresasCRUD.cogerTodasEmpresas().Cast<Empresa>().ToList();
                    empresas = new List<Empresa>(lista.OrderBy(e => e.nombre));
                    foreach (Empresa e in empresas)
                        cNombres.Items.Add(new ComboBoxItem().Content = e.nombre);
                }
                else
                {
                    lNombre.Content = "Empleado";
                    var lista = EmpleadosCRUD.cogerTodosEmpleados().Cast<Empleado>().ToList();
                    empleados = new List<Empleado>(lista.OrderBy(e => e.nombre));
                    foreach (Empleado e in empleados)
                        cNombres.Items.Add(new ComboBoxItem().Content = e.getNombreApellidos());
                }
            }
            cNombres.SelectedIndex = 0;

            if (tipo.Contains("Anual"))
            {
                lTrimestre.Visibility = Visibility.Hidden;
                cTrimestre.Visibility = Visibility.Hidden;
                lAño.Margin = new Thickness(151, 42, 0, 0);
                tAño.Margin = new Thickness(188, 46, 0, 0);
            }
        }

        private void bGenerarEstadistica_Click(object sender, RoutedEventArgs e)
        {
            DateTime inicio;
            DateTime final;

            if (UtilidadesVerificacion.validadorNumeroEntero(tAño.Text))
            {
                int año = Convert.ToInt32(tAño.Text);
                if (!tipo.Contains("Anual"))
                {
                    switch (cTrimestre.SelectedIndex)
                    {
                        case 0:
                            inicio = new DateTime(año, 1, 1);
                            final = new DateTime(año, 3, 31);
                            break;
                        case 1:
                            inicio = new DateTime(año, 4, 1);
                            final = new DateTime(año, 6, 30);
                            break;
                        case 2:
                            inicio = new DateTime(año, 7, 1);
                            final = new DateTime(año, 9, 30);
                            break;
                        case 3:
                            inicio = new DateTime(año, 10, 1);
                            final = new DateTime(año, 12, 31);
                            break;
                        default:
                            inicio = new DateTime(año, 1, 1);
                            final = new DateTime(año, 3, 31);
                            break;
                    }
                }
                else
                {
                    inicio = new DateTime(año, 1, 1);
                    final = new DateTime(año, 12, 31);
                }

                if (!tipo.Contains("Empleado"))
                    ventanaPadre.opciones = new BusquedaEstadisticas(inicio, final, tipo, proveedores[cNombres.SelectedIndex].cif);
                else
                    ventanaPadre.opciones = new BusquedaEstadisticas(empleados[cNombres.SelectedIndex].dni, inicio, final, tipo);
                ventanaPadre.generarGraficaLineal();
                Close();
            }
        }
    }
}
