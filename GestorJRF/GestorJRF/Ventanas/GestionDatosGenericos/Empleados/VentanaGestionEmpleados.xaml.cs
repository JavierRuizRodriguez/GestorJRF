using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
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
    /// Lógica de interacción para VentanaGestionEmpleados.xaml
    /// </summary>
    public partial class VentanaGestionEmpleados : Window
    {
        public Empleado empleado { get; set; }
        private bool esAlta;

        public VentanaGestionEmpleados()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.esAlta = true;
        }

        public VentanaGestionEmpleados(Empleado empleado)
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.empleado = empleado;
            this.esAlta = false;
            this.bNuevoEmpleado.Content = "MODIFICAR EMPLEADO";
            this.tNombre.Text = empleado.nombre;
            this.tApellidos.Text = empleado.apellidos;
            this.tDNI.Text = empleado.dni;
            TextBox textBox = this.tFechaNacimiento;
            DateTime dateTime = empleado.fechaNacimiento;
            dateTime = dateTime.Date;
            textBox.Text = dateTime.ToString("dd/MM/yyyy");
            TextBox textBox2 = this.tFechaAlta;
            dateTime = empleado.fechaAlta;
            dateTime = dateTime.Date;
            textBox2.Text = dateTime.ToString("dd/MM/yyyy");
            this.tSueldo.Text = Convert.ToString(empleado.sueldoBase);
            this.tTelefono.Text = Convert.ToString(empleado.telefono);
            this.tMail.Text = Convert.ToString(empleado.email);
            this.tComision.Text = Convert.ToString(empleado.comision);
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            if (this.esAlta)
            {
                new VentanaMenuGestionDatos().Show();
            }
        }

        private void bNuevoEmpleado_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(this.gridPrincipal))
            {
                if (this.sonCamposValidos())
                {
                    if (this.esAlta)
                    {
                        this.crearEmpresa();
                    }
                    else
                    {
                        this.modificarEmpresa();
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private bool sonCamposValidos()
        {
            if (UtilidadesVerificacion.validadorMail(this.tMail.Text) && UtilidadesVerificacion.validadorFechas(this.tFechaAlta.Text) && UtilidadesVerificacion.validadorFechas(this.tFechaNacimiento.Text) && UtilidadesVerificacion.validadorNumeroDecimal(this.tSueldo.Text) && UtilidadesVerificacion.validadorNumeroEntero(this.tTelefono.Text) && UtilidadesVerificacion.validadorNumeroEntero(this.tComision.Text))
            {
                return true;
            }
            return false;
        }

        private void modificarEmpresa()
        {
            Empleado em = new Empleado(this.tNombre.Text, this.tApellidos.Text, this.tDNI.Text, this.empleado.dni, Convert.ToDateTime(this.tFechaNacimiento.Text), Convert.ToDateTime(this.tFechaAlta.Text), Convert.ToDouble(this.tSueldo.Text, UtilidadesVerificacion.cogerProveedorDecimal()), Convert.ToInt32(this.tTelefono.Text), this.tMail.Text, Convert.ToInt32(this.tComision.Text));
            int salida = EmpleadosCRUD.modificarEmpleado(em);
            if (salida == 1)
            {
                base.Close();
            }
        }

        private void crearEmpresa()
        {
            Empleado em = new Empleado(this.tNombre.Text, this.tApellidos.Text, this.tDNI.Text, Convert.ToDateTime(this.tFechaNacimiento.Text), Convert.ToDateTime(this.tFechaAlta.Text), Convert.ToDouble(this.tSueldo.Text, UtilidadesVerificacion.cogerProveedorDecimal()), Convert.ToInt32(this.tTelefono.Text), this.tMail.Text, Convert.ToInt32(this.tComision.Text));
            int salida = EmpleadosCRUD.insertarEmpleado(em);
            if (salida == 1)
            {
                UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
            }
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
        }
    }
}
