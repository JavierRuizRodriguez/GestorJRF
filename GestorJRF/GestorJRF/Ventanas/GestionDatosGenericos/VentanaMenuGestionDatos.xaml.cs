using GestorJRF.Utilidades;
using GestorJRF.Ventanas.GestionDatosGenericos;
using GestorJRF.Ventanas.GestionDatosGenericos.Proveedores;
using System.Windows;

namespace GestorJRF.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaMenuGestionDatos.xaml
    /// </summary>
    public partial class VentanaMenuGestionDatos : Window
    {
        private bool botonPulsadoX = true;
        public VentanaMenuGestionDatos()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }

        private void bGestionEmpresas_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            new VentanaSeleccionGestion("empresas").Show();
            Close();

        }

        private void bGestionCamiones_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            new VentanaSeleccionGestion("camiones").Show();
            Close();
        }

        private void bGestionEmpleados_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            new VentanaSeleccionGestion("empleados").Show();
            Close();
        }

        private void bGestionTarifas_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            new VentanaSeleccionGestion("tarifas").Show();
            Close();
        }

        private void bGestionAlertas_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            new VentanaSeleccionGestionAlertas().Show();
            Close();
        }

        private void bGestionResumenes_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            new VentanaSeleccionGestionResumen().Show();
            Close();
        }

        private void bGestionProveedores_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            new VentanaSeleccionGestion("proveedores").Show();
            Close();
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (botonPulsadoX)
                new VentanaMenuPrincipal().Show();

        }
    }
}
