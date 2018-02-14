using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.Facturas;
using GestorJRF.Ventanas.Login;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.Facturas
{
    /// <summary>
    /// Lógica de interacción para VentanaSeleccionFacturacion.xaml
    /// </summary>
    public partial class VentanaSeleccionFacturacion : Window
    {
        private bool xBotonPulsado;

        public VentanaSeleccionFacturacion()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.xBotonPulsado = true;
        }

        private void bFacturaResumenes_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaFacturaResumenes().Show();
            base.Close();
        }

        private void bFacturaIva_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaFacturaIva("facturacioAnual").Show();
            base.Close();
        }

        private void bFacturaIvaBienes_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaFacturaIva().Show();
            base.Close();
        }

        private void bTerceros_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            new VentanaOperacionesTerceros().Show();
            base.Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.xBotonPulsado)
            {
                new VentanaMenuPrincipal().Show();
            }
        }
    }
}
