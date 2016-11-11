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
    /// Lógica de interacción para VentanaOpcionesGeneralEmpresaEmpleado.xaml
    /// </summary>
    public partial class VentanaOpcionesGeneralEmpresaEmpleado : Window
    {
        private VentanaEstadisticas ventanaPadre;
        private string tipo;
        private List<Empresa> empresas;
        private List<Empleado> empleados;
        private List<Proveedor> proveedores;

        public VentanaOpcionesGeneralEmpresaEmpleado(VentanaEstadisticas ventanaPadre, string tipo)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.ventanaPadre = ventanaPadre;
            this.tipo = tipo;

            if (this.tipo.Contains("gasto"))
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
                if (this.tipo.Contains("Empresa"))
                {
                    var lista = EmpresasCRUD.cogerTodasEmpresas().Cast<Empresa>().ToList();
                    empresas = new List<Empresa>(lista.OrderBy(e => e.nombre));
                    foreach (Empresa e in empresas)
                        cNombres.Items.Add(new ComboBoxItem().Content = e.nombre);
                    cNombres.SelectedIndex = 0;
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
            if (!tipo.Contains("Empleado"))
            {
                if(tipo.Contains("gasto"))
                    ventanaPadre.opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, tipo, proveedores[cNombres.SelectedIndex].cif);
                else
                    ventanaPadre.opciones = new BusquedaEstadisticas(new DateTime(2016, 1, 1), DateTime.Now, tipo, empresas[cNombres.SelectedIndex].cif);
            }
            else
                ventanaPadre.opciones = new BusquedaEstadisticas(empleados[cNombres.SelectedIndex].dni, new DateTime(2016, 1, 1), DateTime.Now, tipo);

            ventanaPadre.generarGraficaLineal();
            Close();

        }
    }
}
