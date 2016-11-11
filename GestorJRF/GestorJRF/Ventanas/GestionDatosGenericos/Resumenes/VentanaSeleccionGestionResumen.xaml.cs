using GestorJRF.Utilidades;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using GestorJRF.Ventanas.GestionDatosGenericos.Camiones;
using GestorJRF.Ventanas.GestionDatosGenericos.Empleados;
using GestorJRF.Ventanas.GestionDatosGenericos.Resumenes;
using GestorJRF.Ventanas.GestionDatosGenericos.Tarifas;
using GestorJRF.Ventanas.GestionGastos;
using System.Windows;

namespace GestorJRF.Ventanas.GestionDatosGenericos
{
    /// <summary>
    /// Lógica de interacción para VentanaSeleccionGestion.xaml
    /// </summary>
    public partial class VentanaSeleccionGestionResumen : Window
    {
        private bool botonPulsadoX;

        public VentanaSeleccionGestionResumen()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            Title = "Gestión resumenes";
            botonPulsadoX = true;
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (botonPulsadoX)
                new VentanaMenuGestionDatos().Show();
        }

        private void bGestionResumenesPrevios_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            new VentanaGestionResumenesPrevio().Show();
            Close();
        }

        private void bGestionResumenesFinales_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            new VentanaGestionResumenesFinal().Show();
            Close();
        }
    }
}
