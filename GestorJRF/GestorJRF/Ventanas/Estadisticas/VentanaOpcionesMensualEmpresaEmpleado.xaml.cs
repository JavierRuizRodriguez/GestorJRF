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

namespace GestorJRF.Ventanas.Estadisticas
{
    /// <summary>
    /// Lógica de interacción para VentanaOpciones.xaml
    /// </summary>
    public partial class VentanaOpcionesMensualEmpresaEmpleado : Window
    {
        private VentanaEstadisticas ventanaPadre;
        private string tipo;
        private List<Empresa> empresas;
        private List<Empleado> empleados;
        private List<Proveedor> proveedores;

        public VentanaOpcionesMensualEmpresaEmpleado(VentanaEstadisticas ventanaPadre, string tipo)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.ventanaPadre = ventanaPadre;
            this.tipo = tipo;
            tAño.Text = DateTime.Now.Year.ToString();

            if (tipo.Contains("gasto"))
            {
                if (!this.tipo.Contains("Proveedor"))
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
                if (!this.tipo.Contains("Empleado"))
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

        }

        private void bGenerarEstadistica_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVerificacion.validadorNumeroEntero(tAño.Text))
            {
                DateTime inicio = new DateTime(Convert.ToInt32(tAño.Text), cMes.SelectedIndex + 1, 1);
                DateTime final = new DateTime(Convert.ToInt32(tAño.Text), cMes.SelectedIndex + 1, DateTime.DaysInMonth(Convert.ToInt32(tAño.Text), cMes.SelectedIndex + 1));

                if (!tipo.Contains("Empleado"))
                {
                    if (tipo.Contains("gasto"))
                        ventanaPadre.opciones = new BusquedaEstadisticas(inicio, final, tipo, proveedores[cNombres.SelectedIndex].cif);
                    else
                        ventanaPadre.opciones = new BusquedaEstadisticas(inicio, final, tipo, empresas[cNombres.SelectedIndex].cif);
                }
                else
                    ventanaPadre.opciones = new BusquedaEstadisticas(empleados[cNombres.SelectedIndex].dni, inicio, final, tipo);

                ventanaPadre.generarGraficaLineal();
                Close();
            }
        }
    }
}
