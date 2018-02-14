using GestorJRF.POJOS.Facturas;
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
    /// Lógica de interacción para VentanaFactura.xaml
    /// </summary>
    public partial class VentanaFacturaIva : Window
    {
        private bool pulsadoBotonX;
        private string tipo;

        public VentanaFacturaIva()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.tAño.Content = DateTime.Now.Year.ToString();
            this.pulsadoBotonX = true;
        }

        public VentanaFacturaIva(string tipo)
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.tAño.Content = DateTime.Now.Year.ToString();
            this.pulsadoBotonX = true;
            this.cTipo.IsEnabled = false;
            this.isAnual.IsChecked = true;
            this.isAnual.IsEnabled = false;
            this.tipo = tipo;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.pulsadoBotonX)
            {
                new VentanaMenuPrincipal().Show();
            }
        }

        private void bGenerarFactura_Click(object sender, RoutedEventArgs e)
        {
            if (cTipo.SelectedIndex > -1 || this.tipo.Equals("facturacioAnual"))
            {
                if (this.isAnual.IsEnabled)
                {
                    base.Hide();
                    var tipo = ((ComboBoxItem)this.cTipo.SelectedItem).Content.Equals("IVA") ? "ivaNormal" : "ivaBienes";

                    if (this.isAnual.IsChecked == true)
                    {
                        new VentanaImpresionFactura(tipo, 0, Convert.ToInt32(this.tAño.Content));
                    }
                    else
                    {
                        new VentanaImpresionFactura(tipo, this.cTrimestre.SelectedIndex + 1, Convert.ToInt32(this.tAño.Content));
                    }
                }
                else
                {
                    var fechaInicio = new DateTime(Convert.ToInt32(this.tAño.Content),1, 1);
                    var fechaFinal = new DateTime(Convert.ToInt32(this.tAño.Content), 12,31);
                    BusquedaFactura busqueda = new BusquedaFactura(null, fechaInicio, fechaFinal, "facturacionAnual");
                    new VentanaImpresionFactura(busqueda);
                }
                
                this.pulsadoBotonX = false;
                base.Close();
            }
            else
            {
                    MessageBox.Show("Debe seleccionar el tipo de IVA", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void isAnual_Checked(object sender, RoutedEventArgs e)
        {
            this.cTrimestre.IsEnabled = ((CheckBox)sender).IsChecked == false ? true : false;
        }

        private void bArriba_Click(object sender, RoutedEventArgs e)
        {
            tAño.Content = (int.Parse(tAño.Content.ToString()) + 1).ToString();
        }

        private void bAbajo_Click(object sender, RoutedEventArgs e)
        {
            tAño.Content = (int.Parse(tAño.Content.ToString()) - 1).ToString();
        }
    }
}
