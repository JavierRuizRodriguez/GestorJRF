using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Facturas;
using GestorJRF.Utilidades;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Ventanas.Facturas
{
    /// <summary>
    /// Lógica de interacción para VentanaFactura.xaml
    /// </summary>
    public partial class VentanaFacturaResumenes : Window
    {
        private IList empresas;
        private bool pulsadoBotonX;

        public VentanaFacturaResumenes()
        {
            InitializeComponent();

            pulsadoBotonX = true;
            UtilidadesVentana.SituarVentana(0, this);
            empresas = EmpresasCRUD.cogerTodasEmpresas();
            foreach (Empresa empresa in empresas)
                cCliente.Items.Add(new ComboBoxItem().Content = empresa.nombre);
            tAño.Text = DateTime.Now.ToString("yyyy");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (pulsadoBotonX)
                new VentanaMenuPrincipal().Show();
        }

        private void bGenerarFactura_Click(object sender, RoutedEventArgs e)
        {
            if (cCliente.SelectedIndex > -1)
            {
                DateTime inicio = new DateTime(Convert.ToInt32(tAño.Text), cMes.SelectedIndex + 1, 1);
                DateTime final = new DateTime(Convert.ToInt32(tAño.Text), cMes.SelectedIndex + 1, DateTime.DaysInMonth(Convert.ToInt32(tAño.Text), cMes.SelectedIndex + 1));
                char letraTipoFactura = cTipoFactura.SelectedIndex == 0 ? 'F' : 'A';
                BusquedaFactura busqueda = new BusquedaFactura(letraTipoFactura, (Empresa)empresas[cCliente.SelectedIndex], inicio, final, tReferencia.Text, "resumenes");
                new VentanaImpresionFactura(busqueda).Show();
                pulsadoBotonX = false;
                Close();
            }
            else
                MessageBox.Show("Debe seleccionar un cliente para emitir la factura.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
