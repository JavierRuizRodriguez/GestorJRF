using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            esAlta = true;
        }

        public VentanaGestionEmpleados(Empleado empleado)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.empleado = empleado;
            esAlta = false;

            bNuevoEmpleado.Content = "MODIFICAR EMPLEADO";
            tNombre.Text = empleado.nombre;
            tApellidos.Text = empleado.apellidos;
            tDNI.Text = empleado.dni;
            tFechaNacimiento.Text = empleado.fechaNacimiento.Date.ToString("dd/MM/yyyy");
            tFechaAlta.Text = empleado.fechaAlta.Date.ToString("dd/MM/yyyy");
            tSueldo.Text = Convert.ToString(empleado.sueldoBruto);
            tTelefono.Text = Convert.ToString(empleado.telefono);
            tMail.Text = Convert.ToString(empleado.email);
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (esAlta)
                new VentanaMenuGestionDatos().Show();
        }

        private void bNuevoEmpleado_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(gridPrincipal))
            {
                if (sonCamposValidos())
                {
                    if (esAlta)
                        crearEmpresa();
                    else
                        modificarEmpresa();
                }   
            }
            else
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool sonCamposValidos()
        {
            if (UtilidadesVerificacion.validadorMail(tMail.Text) || UtilidadesVerificacion.validadorMail(tFechaAlta.Text)
                    || UtilidadesVerificacion.validadorMail(tFechaNacimiento.Text) || UtilidadesVerificacion.validadorNumeroDecimal(tSueldo.Text)
                    || UtilidadesVerificacion.validadorNumeroEntero(tTelefono.Text))
                return true;
            else
                return false;
        }

        private void modificarEmpresa()
        {
            Empleado em = new Empleado(tNombre.Text, tApellidos.Text, tDNI.Text, empleado.dni, Convert.ToDateTime(tFechaNacimiento.Text), Convert.ToDateTime(tFechaAlta.Text), Convert.ToInt32(tSueldo.Text), Convert.ToInt32(tTelefono.Text), tMail.Text);
            EmpleadosCRUD.modificarEmpleado(em);
        }

        private void crearEmpresa()
        {
            Empleado em = new Empleado(tNombre.Text, tApellidos.Text, tDNI.Text, Convert.ToDateTime(tFechaNacimiento.Text), Convert.ToDateTime(tFechaAlta.Text), Convert.ToInt32(tSueldo.Text), Convert.ToInt32(tTelefono.Text), tMail.Text);
            int salida = EmpleadosCRUD.insertarEmpleado(em);

            if (salida == 1)
                UtilidadesVentana.LimpiarCampos(gridPrincipal);
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(gridPrincipal);
        }
    }
}
