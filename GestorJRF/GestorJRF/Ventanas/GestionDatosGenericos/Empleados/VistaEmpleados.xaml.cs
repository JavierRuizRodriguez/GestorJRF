using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos;
using GestorJRF.Ventanas.GestionDatosGenericos.Empleados;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Empleados
{
    /// <summary>
    /// Lógica de interacción para VistaEmpleados.xaml
    /// </summary>
    public partial class VistaEmpleados : Window
    {
        internal Empleado empleado
        {
            get;
            set;
        }

        public VistaEmpleados()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }

        internal void MostrarEmpleadoBuscado()
        {
            this.tNombre.Text = this.empleado.nombre;
            this.tApellidos.Text = this.empleado.apellidos;
            this.tDNI.Text = this.empleado.dni;
            TextBox textBox = this.tFechaNacimiento;
            DateTime dateTime = this.empleado.fechaNacimiento;
            dateTime = dateTime.Date;
            textBox.Text = dateTime.ToString("dd/MM/yyyy");
            TextBox textBox2 = this.tFechaAlta;
            dateTime = this.empleado.fechaAlta;
            dateTime = dateTime.Date;
            textBox2.Text = dateTime.ToString("dd/MM/yyyy");
            this.tSueldo.Text = Convert.ToString(this.empleado.sueldoBase);
            this.tTelefono.Text = Convert.ToString(this.empleado.telefono);
            this.tMail.Text = Convert.ToString(this.empleado.email);
            this.tComision.Text = Convert.ToString(this.empleado.comision);
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "empleado").Show();
        }

        private void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (this.empleado != null)
            {
                if (MessageBox.Show("¿Desea borrar el empleado?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    int salida = EmpleadosCRUD.borrarEmpleado(this.empleado.dni);
                    if (salida == 1)
                    {
                        UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                        this.empleado = null;
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un empleado para borrarlo.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void bModificar_Click(object sender, RoutedEventArgs e)
        {
            if (this.empleado != null)
            {
                new VentanaGestionEmpleados(this.empleado).Show();
                UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                this.empleado = null;
            }
            else
            {
                MessageBox.Show("Debe seleccionar un empleado para modificarlo.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}
