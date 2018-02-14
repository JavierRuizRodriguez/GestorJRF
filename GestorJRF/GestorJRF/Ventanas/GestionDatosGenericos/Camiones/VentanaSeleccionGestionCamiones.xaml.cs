using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos.Camiones;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Camiones
{
    /// <summary>
    /// Lógica de interacción para VentanaSeleccionGestionCamiones.xaml
    /// </summary>
    public partial class VentanaSeleccionGestionCamiones : Window
    {
        private bool botonPulsadoX;

        public VentanaSeleccionGestionCamiones()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.botonPulsadoX = true;
        }

        private void bAlta_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaGestionCamiones().Show();
            base.Close();
        }

        private void bVisualizacion_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VistaCamiones().Show();
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
            this.botonPulsadoX = false;
            new VentanaModificacionKM().Show();
            base.Close();
        }
    }
}
