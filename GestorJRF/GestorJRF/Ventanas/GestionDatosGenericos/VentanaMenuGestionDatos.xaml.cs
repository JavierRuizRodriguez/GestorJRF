using GestorJRF.Utilidades;
using GestorJRF.Ventanas.GestionDatosGenericos;
using GestorJRF.Ventanas.GestionDatosGenericos.Camiones;
using GestorJRF.Ventanas.GestionDatosGenericos.Empleados;
using System.Windows;

namespace GestorJRF.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaMenuGestionDatos.xaml
    /// </summary>
    public partial class VentanaMenuGestionDatos : Window
    {
        private bool botonPulsadoX = true;
        private VentanaSeleccionGestion vSeleccion;
        public VentanaMenuGestionDatos()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(this);
        }

        private void bGestionEmpresas_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            vSeleccion = new VentanaSeleccionGestion("empresas");
            vSeleccion.Show();
            Close();

        }

        private void bGestionCamiones_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            vSeleccion = new VentanaSeleccionGestion("camiones");
            vSeleccion.Show();
            Close();
        }

        private void bGestionEmpleados_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            vSeleccion = new VentanaSeleccionGestion("empleados");
            vSeleccion.Show();
            Close();
        }

        private void bGestionTarifas_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            vSeleccion = new VentanaSeleccionGestion("tarifas");
            vSeleccion.Show();
            Close();
        }

        private void bGestionAlertas_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            vSeleccion = new VentanaSeleccionGestion("alertas");
            vSeleccion.Show();
            Close();
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (botonPulsadoX)
            {
                VentanaMenuPrincipal vMenu = new VentanaMenuPrincipal();
                vMenu.Show();
            }
        }
    }
}
