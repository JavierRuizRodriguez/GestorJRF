using GestorJRF.Utilidades;
using GestorJRF.Ventanas.Estadisticas;
using GestorJRF.Ventanas.Facturas;
using GestorJRF.Ventanas.GestionDatosGenericos;
using GestorJRF.Ventanas.Mapas;
using System.Windows;

namespace GestorJRF.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaMenuPrincipal.xaml
    /// </summary>
    public partial class VentanaMenuPrincipal : Window
    {
        private bool xBotonPulsado;
        public VentanaMenuPrincipal()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            xBotonPulsado = true;
        }

        private void bGestionDatosGenerales_Click(object sender, RoutedEventArgs e)
        {
            xBotonPulsado = false;
            new VentanaMenuGestionDatos().Show();
            Close();
        }

        private void bGestionGastos_Click(object sender, RoutedEventArgs e)
        {
            xBotonPulsado = false;
            new VentanaSeleccionGestion("gastos").Show();
            Close();
        }

        private void bMapas_Click(object sender, RoutedEventArgs e)
        {
            xBotonPulsado = false;
            new VentanaMapa().Show();
            Close();
        }

        private void bGestionFacturas_Click(object sender, RoutedEventArgs e)
        {
            xBotonPulsado = false;
            new VentanaSeleccionFacturacion().Show();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (xBotonPulsado)
                Application.Current.Shutdown();
        }

        private void bEstadisticas_Click(object sender, RoutedEventArgs e)
        {
            xBotonPulsado = false;
            new VentanaEstadisticas().Show();
            Close();
        }
    }
}
