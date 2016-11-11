using GestorJRF.Utilidades;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using GestorJRF.Ventanas.GestionDatosGenericos.Camiones;
using GestorJRF.Ventanas.GestionDatosGenericos.Empleados;
using GestorJRF.Ventanas.GestionDatosGenericos.Proveedores;
using GestorJRF.Ventanas.GestionDatosGenericos.Tarifas;
using GestorJRF.Ventanas.GestionGastos;
using System.Windows;

namespace GestorJRF.Ventanas.GestionDatosGenericos
{
    /// <summary>
    /// Lógica de interacción para VentanaSeleccionGestion.xaml
    /// </summary>
    public partial class VentanaSeleccionGestionAlertas : Window
    {
        private bool botonPulsadoX;

        public VentanaSeleccionGestionAlertas()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            botonPulsadoX = true;
        }

        private void bAlta_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            new VentanaGestionAlertas().Show();
            Close();
        }

        private void bVisualizacion_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            new VistaAlertas().Show();
            Close();
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (botonPulsadoX)
                new VentanaMenuGestionDatos().Show();
        }

        private void bAlertasEnFecha_Click(object sender, RoutedEventArgs e)
        {
            new AvisoAlerta().Show();
            Close();
        }
    }
}
