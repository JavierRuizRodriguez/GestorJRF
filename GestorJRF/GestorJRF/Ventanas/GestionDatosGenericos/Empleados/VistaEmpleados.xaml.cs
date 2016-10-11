using GestorJRF.Utilidades;
using System.Windows;
using GestorJRF.POJOS;
using System;
using GestorJRF.CRUD.Empresas;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Empleados
{
    /// <summary>
    /// Lógica de interacción para VistaEmpleados.xaml
    /// </summary>
    public partial class VistaEmpleados : Window
    {
        internal Empleado empleado { get; set; }

        public VistaEmpleados()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(this);
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }

        internal void MostrarCamionBuscado()
        {
            tNombre.Text = empleado.nombre;
            tApellidos.Text = empleado.apellidos;
            tDNI.Text = empleado.dni;
            tFechaNacimiento.Text = empleado.fechaNacimiento.Date.ToString("dd/MM/yyyy");
            tFechaAlta.Text = empleado.fechaAlta.Date.ToString("dd/MM/yyyy");
            tSueldo.Text = Convert.ToString(empleado.sueldoBruto);
            tTelefono.Text = Convert.ToString(empleado.telefono);
            tMail.Text = Convert.ToString(empleado.email);
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "empleado").Show();
        }

        private void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (empleado != null)
            {
                if (MessageBox.Show("¿Desea borrar el empleado?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    int salida = EmpleadosCRUD.borrarEmpleado(empleado.dni);

                    if (salida == 1)
                        UtilidadesVentana.LimpiarCampos(gridPrincipal);
                }
            }
            else
                MessageBox.Show("Debe seleccionar un empleado para borrarlo.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void bModificar_Click(object sender, RoutedEventArgs e)
        {
            if (empleado != null)
            {
                new VentanaGestionEmpleados(empleado).Show();
                UtilidadesVentana.LimpiarCampos(gridPrincipal);
            }
            else
                MessageBox.Show("Debe seleccionar un empleado para modificarlo.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
