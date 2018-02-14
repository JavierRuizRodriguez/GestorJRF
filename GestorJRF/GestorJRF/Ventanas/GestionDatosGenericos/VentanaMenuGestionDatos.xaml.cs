using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using GestorJRF.Ventanas.GestionDatosGenericos.Camiones;
using GestorJRF.Ventanas.Login;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaMenuGestionDatos.xaml
    /// </summary>
    public partial class VentanaMenuGestionDatos : Window
    {
        private bool botonPulsadoX = true;
        public VentanaMenuGestionDatos()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }

        private void bGestionEmpresas_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaSeleccionGestion("empresas").Show();
            base.Close();
        }

        private void bGestionCamiones_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaSeleccionGestionCamiones().Show();
            base.Close();
        }

        private void bGestionEmpleados_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaSeleccionGestion("empleados").Show();
            base.Close();
        }

        private void bGestionTarifas_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaSeleccionGestion("tarifas").Show();
            base.Close();
        }

        private void bGestionAlertas_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaSeleccionGestionAlertas().Show();
            base.Close();
        }

        private void bGestionResumenes_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaSeleccionGestionResumen().Show();
            base.Close();
        }

        private void bGestionProveedores_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaSeleccionGestion("proveedores").Show();
            base.Close();
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            if (this.botonPulsadoX)
            {
                new VentanaMenuPrincipal().Show();
            }
        }

        private void bGestionGastos_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            new VentanaSeleccionGestion("gastos").Show();
            base.Close();
        }
    }
}
