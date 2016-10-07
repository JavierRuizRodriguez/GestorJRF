﻿using GestorJRF.Utilidades;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using GestorJRF.Ventanas.GestionDatosGenericos.Camiones;
using GestorJRF.Ventanas.GestionDatosGenericos.Empleados;
using GestorJRF.Ventanas.GestionDatosGenericos.Tarifas;
using System.Windows;

namespace GestorJRF.Ventanas.GestionDatosGenericos
{
    /// <summary>
    /// Lógica de interacción para VentanaSeleccionGestion.xaml
    /// </summary>
    public partial class VentanaSeleccionGestion : Window
    {
        private string tipo;
        private bool botonPulsadoX;

        public VentanaSeleccionGestion(string tipo)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(this);
            this.tipo = tipo;
            Title = "Gestión " + tipo;
            botonPulsadoX = true;
        }

        private void bAlta_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            switch (tipo)
            {
                case "camiones":
                    new VentanaGestionCamiones().Show();
                    Close();
                    break;
                case "empleados":
                    new VentanaGestionEmpleados().Show();
                    Close();
                    break;
                case "empresas":
                    new VentanaGestionEmpresas().Show();
                    Close();
                    break;
                case "tarifas":
                    new VentanaGestionTarifas().Show();
                    Close();
                    break;
                case "alertas":
                    new VentanaGestionAlertas().Show();
                    Close();
                    break;
                default:
                    new VentanaGestionCamiones().Show();
                    Close();
                    break;
            }
        }

        private void bVisualizacion_Click(object sender, RoutedEventArgs e)
        {
            botonPulsadoX = false;
            switch (tipo)
            {
                case "camiones":
                    new VistaCamiones().Show();
                    Close();
                    break;
                case "empleados":
                    new VistaEmpleados().Show();
                    Close();
                    break;
                case "empresas":
                    new VistaEmpresas().Show();
                    Close();
                    break;
                case "tarifas":
                    new VistaTarifas().Show();
                    Close();
                    break;
                case "alertas":
                    new VistaAlertas().Show();
                    Close();
                    break;
                default:
                    new VistaCamiones().Show();
                    Close();
                    break;
            }
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (botonPulsadoX) {
                VentanaMenuGestionDatos vMenu = new VentanaMenuGestionDatos();
                vMenu.Show();
            }
        }
    }
}
