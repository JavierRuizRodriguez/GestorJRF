using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Tarifas
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionTarifas.xaml
    /// </summary>
    public partial class VentanaGestionTarifas : Window
    {
        private bool esAlta;
        public Tarifa tarifa { get; set; }
        private IList listaEmpresas;
        public ObservableCollection<ComponenteTarifa> listaComponentesTarifa { get; set; }
        public VentanaGestionTarifas()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            listaComponentesTarifa = new ObservableCollection<ComponenteTarifa>();
            esAlta = true;
        }

        public VentanaGestionTarifas(Tarifa tarifa)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.tarifa = tarifa;
            listaComponentesTarifa = new ObservableCollection<ComponenteTarifa>(tarifa.listaComponentesTarifa);
            esAlta = false;

            bNuevaTarifa.Content = "MODIFICAR TARIFA";
            if (tarifa.nombreTarifa.Contains("GENERAL"))
                checkCliente.IsChecked = false;
            tDescripcionTarifa.Text = tarifa.nombreTarifa;
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (esAlta)
                new VentanaMenuGestionDatos().Show();
        }

        private void checkCliente_Checked(object sender, RoutedEventArgs e)
        {
            listaEmpresas = EmpresasCRUD.cogerTodasEmpresas();
            if (listaEmpresas != null)
            {
                foreach (Empresa empresa in listaEmpresas)
                    cCliente.Items.Add(new ComboBoxItem().Content = empresa.nombre);

                cCliente.IsEnabled = true;
                cCliente.SelectedIndex = 0;
            }
            else
            {
                checkCliente.IsChecked = false;
                MessageBox.Show("No se encontraron empresas en el sistema.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void checkCliente_Unchecked(object sender, RoutedEventArgs e)
        {
            cCliente.IsEnabled = false;
            cCliente.Items.Clear();
            tDescripcionTarifa.IsEnabled = false;
            tDescripcionTarifa.Text = "TARIFA GENERAL";
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(gridPrincipal);
            if (!checkCliente.IsChecked.Value)
                UtilidadesVentana.LimpiarCampos(gridTabla);
            listaComponentesTarifa.Clear();
        }

        private void bNuevoComponenteTarifa_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(gridTabla))
            {
                if (UtilidadesVerificacion.validadorNumeroDecimal(tPrecio.Text))
                    listaComponentesTarifa.Add(new ComponenteTarifa(tEtiqueta.Text, Convert.ToDouble(tPrecio.Text, UtilidadesVerificacion.cogerProveedorDecimal()), ((ComboBoxItem)cTipoCamion.SelectedItem).Content as string));
                UtilidadesVentana.LimpiarCampos(gridTabla);
            }
            else
                MessageBox.Show("Debe introducir todos los campos para rellenar la tabla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void bNuevaTarifa_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(gridPrincipal) && listaComponentesTarifa.Count > 0)
            {
                if (esAlta)
                        crearTarifa();
                    else
                        modificarTarifa();
            }
            else
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void modificarTarifa()
        {
            string nombreCliente;
            if (checkCliente.IsChecked.Value)
                nombreCliente = ((Empresa)listaEmpresas[cCliente.SelectedIndex]).nombre;
            else
                nombreCliente = "GENERAL";
            Tarifa t = new Tarifa(tDescripcionTarifa.Text, tarifa.nombreTarifa, nombreCliente, listaComponentesTarifa);
            TarifasCRUD.modificarTarifa(t);
            Close();
        }

        private void crearTarifa()
        {
            string nombreCliente;
            if (checkCliente.IsChecked.Value)
                nombreCliente = ((Empresa)listaEmpresas[cCliente.SelectedIndex]).nombre;
            else
                nombreCliente = "GENERAL";

            Tarifa t = new Tarifa(tDescripcionTarifa.Text, nombreCliente, listaComponentesTarifa);
            int salida = TarifasCRUD.añadirTarifa(t);

            if (salida == 1)
            {
                UtilidadesVentana.LimpiarCampos(gridTabla);
                listaComponentesTarifa.Clear();
                if (!checkCliente.IsChecked.Value)
                    tDescripcionTarifa.Text = "GENERAL";
            }
        }

        private void tabla_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "nombreTarifa")
                e.Cancel = true;
        }

        private void cCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tDescripcionTarifa.Text = "TARIFA " + (cCliente.SelectedItem as string).ToUpper();
        }
    }
}
