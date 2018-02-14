using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Alertas
{
    /// <summary>
    /// Lógica de interacción para VentanaSeleccionGestion.xaml
    /// </summary>
    public partial class VentanaSeleccionGestionAlertas : Window
    {
        private bool botonPulsadoX;

        public VentanaSeleccionGestionAlertas()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.botonPulsadoX = true;
        }

        private void bAlta_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaGestionAlertas().Show();
            base.Close();
        }

        private void bVisualizacion_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VistaAlertas().Show();
            base.Close();
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            if (this.botonPulsadoX)
            {
                new VentanaMenuGestionDatos().Show();
            }
        }

        private void bAlertasEnFecha_Click(object sender, RoutedEventArgs e)
        {
            new AvisoAlerta().Show();
            base.Close();
        }
    }
}
