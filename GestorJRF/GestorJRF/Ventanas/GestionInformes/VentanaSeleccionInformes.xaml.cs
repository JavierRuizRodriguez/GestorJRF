using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionInformes;
using GestorJRF.Ventanas.Login;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionInformes
{
    /// <summary>
    /// Lógica de interacción para VentanaSeleccionInformes.xaml
    /// </summary>
    public partial class VentanaSeleccionInformes : Window
    {
        private bool xBotonPulsado;

        public VentanaSeleccionInformes()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.xBotonPulsado = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.xBotonPulsado)
            {
                new VentanaMenuPrincipal().Show();
            }
        }

        private void bInformeEmpleado_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaInformeEmpleado(0).Show();
            base.Close();
        }

        private void bInformeFacturacion_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaInformeEmpleado(1).Show();
            base.Close();
        }

        private void bInformeComisiones_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaVisualizacionComisiones().Show();
            base.Close();
        }

        private void bInformeFacturasEmitidas_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaInformeEmpleado(2).Show();
            base.Close();
        }
    }
}
