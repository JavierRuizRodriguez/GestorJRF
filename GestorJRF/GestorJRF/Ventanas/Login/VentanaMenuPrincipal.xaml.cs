using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.Estadisticas;
using GestorJRF.Ventanas.Facturas;
using GestorJRF.Ventanas.GestionInformes;
using GestorJRF.Ventanas.GestionPagos;
using GestorJRF.Ventanas.Mapas;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;


namespace GestorJRF.Ventanas.Login
{
    /// <summary>
    /// Lógica de interacción para VentanaMenuPrincipal.xaml
    /// </summary>
    public partial class VentanaMenuPrincipal : Window
    {
        private bool xBotonPulsado;
        public VentanaMenuPrincipal()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.xBotonPulsado = true;
        }

        private void bGestionDatosGenerales_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaMenuGestionDatos().Show();
            base.Close();
        }

        private void bGestionGastos_Click(object sender, RoutedEventArgs e)
        {
        }

        private void bMapas_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaMapa().Show();
            base.Close();
        }

        private void bGestionFacturas_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaSeleccionFacturacion().Show();
            base.Close();
        }

        private void bGestionPagos_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaSeleccionGestionPagos().Show();
            base.Close();
        }

        private void bInformes_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaSeleccionInformes().Show();
            base.Close();
        }

        private void bEstadisticas_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaEstadisticas().Show();
            base.Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.xBotonPulsado)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
