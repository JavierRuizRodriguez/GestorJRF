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
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.ventanaPadre = ventanaPadre;
            this.tipo = tipo;
            this.tAño.Text = DateTime.Now.Year.ToString();
            if (tipo.Contains("gasto"))
            {
                if (this.tipo.Contains("Proveedor"))
                {
                    this.lNombre.Content = "Proveedor";
                    List<Proveedor> lista4 = ProveedoresCRUD.cogerTodosProveedores().Cast<Proveedor>().ToList();
                    this.proveedores = new List<Proveedor>(from p in lista4
                                                           orderby p.nombre
                                                           select p);
                    foreach (Proveedor proveedore in this.proveedores)
                    {
                        ItemCollection items = this.cNombres.Items;
                        ComboBoxItem comboBoxItem = new ComboBoxItem();
                        object newItem = comboBoxItem.Content = proveedore.nombre;
                        items.Add(newItem);
                    }
                }
                else
                {
                    this.lNombre.Content = "Empleado";
                    List<Empleado> lista3 = EmpleadosCRUD.cogerTodosEmpleados().Cast<Empleado>().ToList();
                    this.empleados = new List<Empleado>(from e in lista3
                                                        orderby e.nombre
                                                        select e);
                    foreach (Empleado empleado in this.empleados)
                    {
                        ItemCollection items2 = this.cNombres.Items;
                        ComboBoxItem comboBoxItem2 = new ComboBoxItem();
                        object newItem = comboBoxItem2.Content = empleado.getNombreApellidos();
                        items2.Add(newItem);
                    }
                }
            }
            else if (!tipo.Contains("Empleado"))
            {
                List<Empresa> lista2 = EmpresasCRUD.cogerTodasEmpresas().Cast<Empresa>().ToList();
                this.empresas = new List<Empresa>(from e in lista2
                                                  orderby e.nombre
                                                  select e);
                foreach (Empresa empresa in this.empresas)
                {
                    ItemCollection items3 = this.cNombres.Items;
                    ComboBoxItem comboBoxItem3 = new ComboBoxItem();
                    object newItem = comboBoxItem3.Content = empresa.nombre;
                    items3.Add(newItem);
                }
            }
            else
            {
                this.lNombre.Content = "Empleado";
                List<Empleado> lista = EmpleadosCRUD.cogerTodosEmpleados().Cast<Empleado>().ToList();
                this.empleados = new List<Empleado>(from e in lista
                                                    orderby e.nombre
                                                    select e);
                foreach (Empleado empleado2 in this.empleados)
                {
                    ItemCollection items4 = this.cNombres.Items;
                    ComboBoxItem comboBoxItem4 = new ComboBoxItem();
                    object newItem = comboBoxItem4.Content = empleado2.getNombreApellidos();
                    items4.Add(newItem);
                }
            }
            this.cNombres.SelectedIndex = -1;
            if (tipo.Contains("Anual"))
            {
                this.lTrimestre.Visibility = Visibility.Hidden;
                this.cTrimestre.Visibility = Visibility.Hidden;
                this.lAño.Margin = new Thickness(151.0, 42.0, 0.0, 0.0);
                this.tAño.Margin = new Thickness(188.0, 46.0, 0.0, 0.0);
            }
        }

        private void bGenerarEstadistica_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVerificacion.validadorNumeroEntero(this.tAño.Text))
            {
                int año = Convert.ToInt32(this.tAño.Text);
                DateTime inicio;
                DateTime final;
                if (!this.tipo.Contains("Anual"))
                {
                    switch (this.cTrimestre.SelectedIndex)
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
                if (!this.tipo.Contains("Empleado"))
                {
                    this.ventanaPadre.opciones = new BusquedaEstadisticas(inicio, final, this.tipo, this.proveedores[this.cNombres.SelectedIndex].cif);
                }
                else
                {
                    this.ventanaPadre.opciones = new BusquedaEstadisticas(this.empleados[this.cNombres.SelectedIndex].dni, inicio, final, this.tipo);
                }
                this.ventanaPadre.generarGraficaLineal();
                base.Close();
            }
        }
    }
}
