using GestorJRF.CRUD;
using GestorJRF.POJOS.Pagos;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionPagos;
using GestorJRF.Ventanas.Login;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace GestorJRF.Ventanas.GestionPagos
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionPagos.xaml
    /// </summary>
    public partial class VentanaGestionPagos : Window
    {
            private Pago pago;

            private bool esAlta;

        public VentanaGestionPagos()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.esAlta = true;
        }

        public VentanaGestionPagos(Pago pago)
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.esAlta = false;
            this.pago = pago;
            this.checkEsPagare.IsEnabled = false;
            this.tFechaPagare.IsEnabled = false;
            this.tNumFactura.IsEnabled = false;
            this.listaFacturas.IsEnabled = false;
            this.bAñadirFactura.IsEnabled = false;
            this.bValidarPago.Content = "COMPLETAR PAGO";
            this.tFechaPagare.Text = pago.fechaPagare.ToString("dd/MM/yyyy");
            foreach (ComponentePago factura in pago.facturas)
            {
                this.listaFacturas.Items.Add(factura.numeroFactura);
            }
        }

        private void bAñadirFactura_Click(object sender, RoutedEventArgs e)
        {
            if (!this.tNumFactura.Text.Equals("") && UtilidadesVerificacion.validadorNumeroEntero(this.tNumFactura.Text) && !this.listaFacturas.Items.Contains(this.tNumFactura.Text))
            {
                if (FacturasCRUD.cogerFacturaPorNumero(Convert.ToInt64(this.tNumFactura.Text)) != null)
                {
                    this.listaFacturas.Items.Add(this.tNumFactura.Text);
                }
                else
                {
                    MessageBox.Show("El número de factura introducido no existe en la BBDD.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                this.tNumFactura.Text = "";
            }
        }

        private void listaFacturas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                while (this.listaFacturas.SelectedItems.Count > 0)
                {
                    this.listaFacturas.Items.Remove(this.listaFacturas.SelectedItems[0]);
                }
            }
        }

        private void tFechaPagare_GotFocus(object sender, RoutedEventArgs e)
        {
            this.tFechaPagare.Text = "";
            this.tFechaPagare.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void tFechaPagare_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.tFechaPagare.Text.Equals(""))
            {
                this.tFechaPagare.Text = "fecha del pagaré... (dd/mm/yyyy)";
                this.tFechaPagare.Foreground = new SolidColorBrush(Colors.Silver);
            }
        }

        private void bValidarPago_Click(object sender, RoutedEventArgs e)
        {
            List<ComponentePago> facturas = new List<ComponentePago>();
            foreach (object item in (IEnumerable)this.listaFacturas.Items)
            {
                facturas.Add(new ComponentePago(0L, Convert.ToInt64(item)));
            }
            if (this.esAlta)
            {
                if (this.listaFacturas.Items.Count > 0)
                {
                    if (UtilidadesVerificacion.validadorFechas(this.tFechaPagare.Text))
                    {
                        Pago p2 = (!this.checkEsPagare.IsChecked.Value) ? new Pago(this.tBanco.Text, facturas) : new Pago(Convert.ToDateTime(this.tFechaPagare.Text), facturas);
                        int salida2 = PagosCRUD.añadirPago(p2);
                        if (salida2 == 1)
                        {
                            UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                            this.listaFacturas.Items.Clear();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Debe introducir alguna factura para asociar el pago a ella.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            else
            {
                Pago p2 = new Pago(this.pago.id, Convert.ToDateTime(this.tFechaPagare.Text), this.tBanco.Text, facturas);
                int salida = PagosCRUD.modificarPago(p2);
                if (salida == 1)
                {
                    base.Close();
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            new VentanaMenuPrincipal().Show();
        }
    }
}
