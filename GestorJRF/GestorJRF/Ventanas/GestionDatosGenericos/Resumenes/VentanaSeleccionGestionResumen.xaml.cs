using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos;
using GestorJRF.Ventanas.GestionDatosGenericos.Resumenes;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

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
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            base.Title = "Gestión resumenes";
            this.botonPulsadoX = true;
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            if (this.botonPulsadoX)
            {
                new VentanaMenuGestionDatos().Show();
            }
        }

        private void bGestionResumenesPrevios_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaGestionResumenesPrevio(false).Show();
            base.Close();
        }

        private void bGestionResumenesFinales_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaGestionResumenesFinal().Show();
            base.Close();
        }
    }
}
