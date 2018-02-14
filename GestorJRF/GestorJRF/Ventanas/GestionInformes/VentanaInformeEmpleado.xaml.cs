using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Facturas;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionInformes;
using GestorJRF.Ventanas.Login;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace GestorJRF.Ventanas.GestionInformes
{
    /// <summary>
    /// Lógica de interacción para VentanaInformeEmpleado.xaml
    /// </summary>
    public partial class VentanaInformeEmpleado : Window
    {
        private bool pulsadoBotonX;

        private int tipo;

        private List<Empleado> empleados;

        public VentanaInformeEmpleado(int tipo)
        {
            this.InitializeComponent();
            this.tipo = tipo;
            switch (tipo)
            {
                case 1:
                    this.cEmpleado.IsEnabled = false;
                    this.lEmpleado.IsEnabled = false;
                    break;
                case 0:
                    this.empleados = EmpleadosCRUD.cogerTodosEmpleados().Cast<Empleado>().ToList();
                    this.empleados = new List<Empleado>(from e in this.empleados
                                                        orderby e.nombre
                                                        select e);
                    if (this.empleados != null)
                    {
                        foreach (Empleado empleado in this.empleados)
                        {
                            this.cEmpleado.Items.Add(empleado.getNombreApellidos());
                        }
                        this.cEmpleado.SelectedIndex = -1;
                    }
                    break;
                default:
                    this.cEmpleado.IsEnabled = false;
                    this.lEmpleado.IsEnabled = false;
                    this.cMes.Visibility = Visibility.Hidden;
                    this.lMes.Visibility = Visibility.Hidden;
                    this.lCuatrimestre.Visibility = Visibility.Visible;
                    this.cCuatrimestre.Visibility = Visibility.Visible;
                    break;
            }
            this.pulsadoBotonX = true;
            UtilidadesVentana.SituarVentana(0, this);
            this.tAño.Text = DateTime.Now.ToString("yyyy");
        }

        private void bGenerarInforme_Click(object sender, RoutedEventArgs e)
        {
            if (!this.tAño.Text.Equals(""))
            {
                Empleado empleado = null;
                string tipoInforme;
                DateTime fechaInicio;
                DateTime fechaFinal;
                if (this.tipo == 2)
                {
                    tipoInforme = "informeFacturasEmitidas";
                    switch (this.cCuatrimestre.SelectedIndex)
                    {
                        case 0:
                            fechaInicio = new DateTime(Convert.ToInt32(this.tAño.Text), 1, 1);
                            fechaFinal = new DateTime(Convert.ToInt32(this.tAño.Text), 3, 31);
                            break;
                        case 1:
                            fechaInicio = new DateTime(Convert.ToInt32(this.tAño.Text), 4, 1);
                            fechaFinal = new DateTime(Convert.ToInt32(this.tAño.Text), 6, 30);
                            break;
                        case 2:
                            fechaInicio = new DateTime(Convert.ToInt32(this.tAño.Text), 7, 1);
                            fechaFinal = new DateTime(Convert.ToInt32(this.tAño.Text), 9, 30);
                            break;
                        case 3:
                            fechaInicio = new DateTime(Convert.ToInt32(this.tAño.Text), 10, 1);
                            fechaFinal = new DateTime(Convert.ToInt32(this.tAño.Text), 12, 31);
                            break;
                        default:
                            fechaInicio = new DateTime(Convert.ToInt32(this.tAño.Text), 1, 1);
                            fechaFinal = new DateTime(Convert.ToInt32(this.tAño.Text), 3, 31);
                            break;
                    }
                }
                else
                {
                    fechaInicio = new DateTime(Convert.ToInt32(this.tAño.Text), this.cMes.SelectedIndex + 1, 1);
                    fechaFinal = new DateTime(Convert.ToInt32(this.tAño.Text), this.cMes.SelectedIndex + 1, DateTime.DaysInMonth(Convert.ToInt32(this.tAño.Text), this.cMes.SelectedIndex + 1));
                    tipoInforme = "informeGeneral";
                    if (this.tipo == 0)
                    {
                        empleado = this.empleados[this.cEmpleado.SelectedIndex];
                        tipoInforme = "informeEmpleado";
                    }
                }
                BusquedaFactura busqueda = new BusquedaFactura(empleado, fechaInicio, fechaFinal, tipoInforme);
                new VentanaVisualizacionInformes(busqueda);
                this.pulsadoBotonX = false;
                base.Close();
            }
            else
            {
                MessageBox.Show("Debe introducir un año", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.pulsadoBotonX)
            {
                new VentanaMenuPrincipal().Show();
            }
        }
    }
}
