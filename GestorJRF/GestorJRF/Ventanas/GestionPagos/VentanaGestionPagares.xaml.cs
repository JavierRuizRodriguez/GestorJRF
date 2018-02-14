using GestorJRF.CRUD;
using GestorJRF.POJOS.Pagos;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.Genericas;
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
    /// Lógica de interacción para VentanaGestionPagares.xaml
    /// </summary>
    public partial class VentanaGestionPagares : Window
    {
        private bool bontonPulsadoX;

        public VentanaGestionPagares()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.bontonPulsadoX = true;
        }

        private void bListadoCompleto_Click(object sender, RoutedEventArgs e)
        {
            new VistaCompletaDosTablas("pagare").Show();
        }

        private void bCompletarPago_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVerificacion.validadorNumeroEntero(this.tIdPagare.Text))
            {
                Pago pago = PagosCRUD.cogerPago(Convert.ToInt64(this.tIdPagare.Text));
                if (pago != null)
                {
                    new VentanaGestionPagos(pago).Show();
                    this.bontonPulsadoX = false;
                    base.Close();
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.bontonPulsadoX)
            {
                new VentanaMenuPrincipal().Show();
            }
        }
    }
}
