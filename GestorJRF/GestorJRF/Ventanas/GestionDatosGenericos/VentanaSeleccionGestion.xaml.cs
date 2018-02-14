using GestorJRF.Utilidades;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using GestorJRF.Ventanas.GestionDatosGenericos.Camiones;
using GestorJRF.Ventanas.GestionDatosGenericos.Empleados;
using GestorJRF.Ventanas.GestionDatosGenericos.Empresas;
using GestorJRF.Ventanas.GestionDatosGenericos.Gastos;
using GestorJRF.Ventanas.GestionDatosGenericos.Proveedores;
using GestorJRF.Ventanas.GestionDatosGenericos.Tarifas;
using GestorJRF.Ventanas.Login;
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
    public partial class VentanaSeleccionGestion : Window
    {
        private string tipo;
        private bool botonPulsadoX;

        public VentanaSeleccionGestion(string tipo)
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.tipo = tipo;
            base.Title = "Gestión " + tipo;
            this.botonPulsadoX = true;
        }

        private void bAlta_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            switch (this.tipo)
            {
                case "camiones":
                    new VentanaGestionCamiones().Show();
                    base.Close();
                    break;
                case "empleados":
                    new VentanaGestionEmpleados().Show();
                    base.Close();
                    break;
                case "empresas":
                    new VentanaGestionEmpresas(null, false).Show();
                    base.Close();
                    break;
                case "tarifas":
                    new VentanaGestionTarifas().Show();
                    base.Close();
                    break;
                case "alertas":
                    new VentanaGestionAlertas().Show();
                    base.Close();
                    break;
                case "gastos":
                    new VentanaGestionGastos().Show();
                    base.Close();
                    break;
                case "proveedores":
                    new VentanaGestionProveedores().Show();
                    base.Close();
                    break;
                default:
                    new VentanaGestionCamiones().Show();
                    base.Close();
                    break;
            }
        }

        private void bVisualizacion_Click(object sender, RoutedEventArgs e)
        {
            this.botonPulsadoX = false;
            switch (this.tipo)
            {
                case "camiones":
                    new VistaCamiones().Show();
                    base.Close();
                    break;
                case "empleados":
                    new VistaEmpleados().Show();
                    base.Close();
                    break;
                case "empresas":
                    new VistaEmpresas().Show();
                    base.Close();
                    break;
                case "tarifas":
                    new VistaTarifas().Show();
                    base.Close();
                    break;
                case "alertas":
                    new VistaAlertas().Show();
                    base.Close();
                    break;
                case "gastos":
                    new VistaGastos().Show();
                    base.Close();
                    break;
                case "proveedores":
                    new VistaProveedores().Show();
                    base.Close();
                    break;
                default:
                    new VistaCamiones().Show();
                    base.Close();
                    break;
            }
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            if (this.botonPulsadoX)
            {
                if (!this.tipo.Equals("gastos"))
                {
                    new VentanaMenuGestionDatos().Show();
                }
                else
                {
                    new VentanaMenuPrincipal().Show();
                }
            }
        }
    }
}
