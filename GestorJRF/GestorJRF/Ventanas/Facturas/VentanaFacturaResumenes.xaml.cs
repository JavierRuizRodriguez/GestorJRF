using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Facturas;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.Facturas;
using GestorJRF.Ventanas.Genericas;
using GestorJRF.Ventanas.Login;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.Facturas
{
    /// <summary>
    /// Lógica de interacción para VentanaFactura.xaml
    /// </summary>
    public partial class VentanaFacturaResumenes : Window
    {
        private BusquedaFactura busqueda;

        private List<Empresa> empresas;

        private bool pulsadoBotonX;

        private char letraTipoFactura;


        public ObservableCollection<Resumen> listaResumenesFactura
        {
            get;
            set;
        }

        public ObservableCollection<Resumen> listaResumenesAbono
        {
            get;
            set;
        }

        public VentanaFacturaResumenes()
        {
            this.InitializeComponent();
            this.listaResumenesFactura = new ObservableCollection<Resumen>();
            this.listaResumenesAbono = new ObservableCollection<Resumen>();
            this.pulsadoBotonX = true;
            UtilidadesVentana.SituarVentana(0, this);
            this.empresas = EmpresasCRUD.cogerTodasEmpresas().Cast<Empresa>().ToList();
            this.empresas = new List<Empresa>(from e in this.empresas
                                              orderby e.nombre
                                              select e);
            foreach (Empresa empresa in this.empresas)
            {
                ItemCollection items = this.cCliente.Items;
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                object newItem = comboBoxItem.Content = empresa.nombre;
                items.Add(newItem);
            }
            this.tAño.Text = DateTime.Now.ToString("yyyy");
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
            if (this.tablaResumenesFacturas.SelectedItems.Count > 0)
            {
                base.Hide();
                this.busqueda = new BusquedaFactura(this.letraTipoFactura, this.empresas[this.cCliente.SelectedIndex], this.tablaResumenesFacturas.SelectedItems.Cast<Resumen>().ToList(), "factura", this.calendarioResumen.SelectedDate.Value);
                new VentanaImpresionFactura(this.busqueda);
                this.pulsadoBotonX = false;
                base.Close();
            }
            else
            {
                MessageBox.Show("Debe seleccionar algún resumen para emitir la factura.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void bGenerarAbono_Click(object sender, RoutedEventArgs e)
        {
            if (this.tablaResumenesAbono.SelectedItems.Count > 0)
            {
                base.Hide();
                this.letraTipoFactura = 'A';
                this.busqueda = new BusquedaFactura(this.tNumFactura.Text, this.letraTipoFactura, EmpresasCRUD.cogerEmpresa("cif", ((Resumen)this.tablaResumenesAbono.SelectedItems[0]).cif), this.tablaResumenesAbono.SelectedItems.Cast<Resumen>().ToList(), "factura", this.calendarioAbono.SelectedDate.Value);
                new VentanaImpresionFactura(this.busqueda);
                this.pulsadoBotonX = false;
                base.Close();
            }
            else
            {
                MessageBox.Show("Debe seleccionar algun resumen para emitir el abono", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void tablaResumenes_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void tablaResumenes_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("idAntiguo") || e.Column.Header.ToString().Equals("cif") || e.Column.Header.ToString().Equals("kilometrosIda") || e.Column.Header.ToString().Equals("kilometrosVuelta") || e.Column.Header.ToString().Equals("nombreTarifa") || e.Column.Header.ToString().Equals("etiqueta") || e.Column.Header.ToString().Equals("listaItinerarios") || e.Column.Header.ToString().Equals("listaComisiones") || e.Column.Header.ToString().Equals("id"))
            {
                e.Cancel = true;
            }
            if (e.Column.Header.Equals("fechaPorte"))
            {
                DataGridTextColumn columnaNueva = new DataGridTextColumn();
                columnaNueva.Header = e.Column.Header.ToString();
                Binding b = new Binding(e.PropertyName);
                b.StringFormat = "dd/MM/yyyy";
                columnaNueva.Binding = b;
                e.Column = columnaNueva;
            }
            e.Column.Width = (e.Column.Header.ToString().Equals("nombreCliente") ? new DataGridLength(110.0, DataGridLengthUnitType.Pixel) : new DataGridLength(1.0, DataGridLengthUnitType.Star));
            e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }

        private void bListadoCompleto_Click(object sender, RoutedEventArgs e)
        {
            new VistaCompletaDosTablas("factura").Show();
        }

        private void bBuscarResumenesAbono_Click(object sender, RoutedEventArgs e)
        {
            if (this.tNumFactura.Text != "" && UtilidadesVerificacion.validadorNumeroEntero(this.tNumFactura.Text))
            {
                Factura factura = FacturasCRUD.cogerFacturaPorNumeroParaAbono(new Factura(Convert.ToInt64(this.tNumFactura.Text), "", DateTime.Now, 0.0, 0.0, 0.0, new List<ComponenteFactura>(), 'F'));
                if (factura != null)
                {
                    this.listaResumenesAbono.Clear();
                    foreach (ComponenteFactura resumene in factura.resumenes)
                    {
                        this.listaResumenesAbono.Add(ResumenesCRUD.cogerResumenFinal(resumene.idResumenFinal));
                    }
                }
                else
                {
                    MessageBox.Show("La factura introducida no existe en la base de datos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            else
            {
                MessageBox.Show("Debe introducir un número de factua para realizar la búsqueda.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void bBuscarResumenesFactura_Click(object sender, RoutedEventArgs e)
        {
            DateTime inicio = new DateTime(Convert.ToInt32(this.tAño.Text), this.cMes.SelectedIndex + 1, 1);
            DateTime final = new DateTime(Convert.ToInt32(this.tAño.Text), this.cMes.SelectedIndex + 1, DateTime.DaysInMonth(Convert.ToInt32(this.tAño.Text), this.cMes.SelectedIndex + 1));
            this.letraTipoFactura = 'F';
            this.busqueda = new BusquedaFactura(this.letraTipoFactura, this.empresas[this.cCliente.SelectedIndex], inicio, final, this.tReferencia.Text, "factura");
            List<Resumen> resumenes = ResumenesCRUD.cogerResumenesParaFactura(this.busqueda).Cast<Resumen>().ToList();
            if (resumenes.Count > 0)
            {
                this.listaResumenesFactura.Clear();
                foreach (Resumen item in resumenes)
                {
                    this.listaResumenesFactura.Add(item);
                }
            }
            else
            {
                MessageBox.Show("No hay resumenes en las fechas dadas para el empresa seleccionada.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private IEnumerable<DataGridRow> cogerFilasDataGrid(DataGrid tabla)
        {
            IEnumerable items = tabla.ItemsSource;
            if (items == null)
            {
                yield return (DataGridRow)null;
            }
            foreach (object item in items)
            {
                DataGridRow fila = tabla.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (fila != null)
                {
                    yield return fila;
                }
            }
        }

        private void bSeleccionarTodosAbono_Click(object sender, RoutedEventArgs e)
        {
            if (this.tablaResumenesAbono.SelectedItems.Count < this.tablaResumenesAbono.Items.Count)
            {
                this.tablaResumenesAbono.SelectAll();
            }
        }

        private void bInvertirSeleccionAbono_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<DataGridRow> columnas = this.cogerFilasDataGrid(this.tablaResumenesAbono);
            foreach (DataGridRow item in columnas)
            {
                if (item.IsSelected)
                {
                    item.IsSelected = false;
                }
                else
                {
                    item.IsSelected = true;
                }
            }
        }

        private void bSeleccionarTodosFactura_Click(object sender, RoutedEventArgs e)
        {
            if (this.tablaResumenesFacturas.SelectedItems.Count < this.tablaResumenesFacturas.Items.Count)
            {
                this.tablaResumenesFacturas.SelectAll();
            }
        }

        private void bInvertirSeleccionFactura_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<DataGridRow> columnas = this.cogerFilasDataGrid(this.tablaResumenesFacturas);
            foreach (DataGridRow item in columnas)
            {
                if (item.IsSelected)
                {
                    item.IsSelected = false;
                }
                else
                {
                    item.IsSelected = true;
                }
            }
        }
    }
}
