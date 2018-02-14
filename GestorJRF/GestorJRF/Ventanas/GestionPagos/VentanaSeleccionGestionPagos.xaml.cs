using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionPagos;
using GestorJRF.Ventanas.Login;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionPagos
{
    /// <summary>
    /// Lógica de interacción para VentanaSeleccionGestionPagos.xaml
    /// </summary>
    public partial class VentanaSeleccionGestionPagos : Window
    {
        private bool botonPulsadoX;

        public VentanaSeleccionGestionPagos()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.botonPulsadoX = true;
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            if (this.botonPulsadoX)
            {
                new VentanaMenuPrincipal().Show();
            }
        }

        private void bAltaPago_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaGestionPagos().Show();
            base.Close();
        }

        private void bConfirmacionPagare_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaGestionPagares().Show();
            base.Close();
        }
    }
}
