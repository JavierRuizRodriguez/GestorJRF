using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos.Tarifas;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Tarifas
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionTarifas.xaml
    /// </summary>
    public partial class VentanaGestionTarifas : Window
    {
        private string valorAntiguoTipoCamion;

        private bool esAlta;

        private IList listaEmpresas;

        public Tarifa tarifa
        {
            get;
            set;
        }

        public ObservableCollection<ComponenteTarifa> listaComponentesTarifa
        {
            get;
            set;
        }

        public VentanaGestionTarifas()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.listaComponentesTarifa = new ObservableCollection<ComponenteTarifa>();
            this.esAlta = true;
        }

        public VentanaGestionTarifas(Tarifa tarifa)
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.tarifa = tarifa;
            this.listaComponentesTarifa = new ObservableCollection<ComponenteTarifa>(tarifa.listaComponentesTarifa);
            this.esAlta = false;
            this.bNuevaTarifa.Content = "MODIFICAR TARIFA";
            if (tarifa.nombreTarifa.Contains("GENERAL"))
            {
                this.checkCliente.IsChecked = false;
            }
            this.tDescripcionTarifa.Text = tarifa.nombreTarifa;
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            if (this.esAlta)
            {
                new VentanaMenuGestionDatos().Show();
            }
        }

        private void checkCliente_Checked(object sender, RoutedEventArgs e)
        {
            this.listaEmpresas = EmpresasCRUD.cogerTodasEmpresas();
            if (this.listaEmpresas != null)
            {
                foreach (Empresa listaEmpresa in this.listaEmpresas)
                {
                    ItemCollection items = this.cCliente.Items;
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    object newItem = comboBoxItem.Content = listaEmpresa.nombre;
                    items.Add(newItem);
                }
                this.cCliente.IsEnabled = true;
                this.cCliente.SelectedIndex = -1;
            }
            else
            {
                this.checkCliente.IsChecked = false;
                MessageBox.Show("No se encontraron empresas en el sistema.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void checkCliente_Unchecked(object sender, RoutedEventArgs e)
        {
            this.cCliente.IsEnabled = false;
            this.cCliente.Items.Clear();
            this.tDescripcionTarifa.IsEnabled = false;
            this.tDescripcionTarifa.Text = "TARIFA GENERAL";
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
            if (!this.checkCliente.IsChecked.Value)
            {
                UtilidadesVentana.LimpiarCampos(this.gridTabla);
            }
            this.listaComponentesTarifa.Clear();
        }

        private void bNuevoComponenteTarifa_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(this.gridTabla))
            {
                if (UtilidadesVerificacion.validadorNumeroDecimal(this.tPrecio.Text))
                {
                    this.listaComponentesTarifa.Add(new ComponenteTarifa(this.tEtiqueta.Text, Convert.ToDouble(this.tPrecio.Text, UtilidadesVerificacion.cogerProveedorDecimal()), ((ComboBoxItem)this.cTipoCamion.SelectedItem).Content as string));
                }
                UtilidadesVentana.LimpiarCampos(this.gridTabla);
                this.tEtiqueta.Focus();
            }
            else
            {
                MessageBox.Show("Debe introducir todos los campos para rellenar la tabla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void bNuevaTarifa_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(this.gridPrincipal) && this.listaComponentesTarifa.Count > 0)
            {
                if (this.esAlta)
                {
                    this.crearTarifa();
                }
                else
                {
                    this.modificarTarifa();
                }
            }
            else
            {
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void modificarTarifa()
        {
            Tarifa t = new Tarifa(this.tDescripcionTarifa.Text, this.tarifa.nombreTarifa, this.tarifa.nombreEmpresa, this.listaComponentesTarifa);
            if (TarifasCRUD.modificarTarifa(t) == 1)
            {
                base.Close();
            }
        }

        private void crearTarifa()
        {
            string nombreCliente = (!this.checkCliente.IsChecked.Value) ? "GENERAL" : ((Empresa)this.listaEmpresas[this.cCliente.SelectedIndex]).nombre;
            Tarifa t = new Tarifa(this.tDescripcionTarifa.Text, nombreCliente, this.listaComponentesTarifa);
            int salida = TarifasCRUD.añadirTarifa(t);
            if (salida == 1)
            {
                UtilidadesVentana.LimpiarCampos(this.gridTabla);
                this.listaComponentesTarifa.Clear();
                if (!this.checkCliente.IsChecked.Value)
                {
                    this.tDescripcionTarifa.Text = "GENERAL";
                }
            }
        }

        private void tabla_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("nombreTarifa"))
            {
                e.Cancel = true;
            }
            else
            {
                e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
            }
        }

        private void cCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cCliente.Items.Count > 0)
            {
                this.tDescripcionTarifa.Text = "TARIFA " + (this.cCliente.SelectedItem as string).ToUpper();
            }
        }

        private void tabla_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            this.valorAntiguoTipoCamion = ((ComponenteTarifa)e.Row.DataContext).tipoCamion;
        }

        private void tabla_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("TIPO CAMION") && !((TextBox)e.EditingElement).Text.Equals("") && !((TextBox)e.EditingElement).Text.Equals("CAMIÓN GRANDE") && !((TextBox)e.EditingElement).Text.Equals("CAMIÓN PEQUEÑO") && !((TextBox)e.EditingElement).Text.Equals("CAMIÓN MEDIANO"))
            {
                ((TextBox)e.EditingElement).Text = this.valorAntiguoTipoCamion;
                MessageBox.Show("El tipo de camión debe ser 'CAMIÓN GRANDE', 'CAMIÓN MEDIANO' o 'CAMIÓN PEQUEÑO'.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}
