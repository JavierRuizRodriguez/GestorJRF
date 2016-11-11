using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System.Windows;
using System;
using System.Collections.ObjectModel;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Tarifas
{
    /// <summary>
    /// Lógica de interacción para VistaTarifas.xaml
    /// </summary>
    public partial class VistaTarifas : Window
    {
        public ObservableCollection<ComponenteTarifa> listaComponentesTarifa { get; set; }
        public Tarifa tarifa { get; set; }
        public VistaTarifas()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            listaComponentesTarifa = new ObservableCollection<ComponenteTarifa>();
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "tarifa").Show();
        }

        private void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (tarifa != null)
            {
                if (MessageBox.Show("¿Desea borrar la empresa?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    TarifasCRUD.borrarTarfia(tarifa.nombreTarifa);
                    listaComponentesTarifa.Clear();
                    tDescripcionTarifa.Text = "";
                    tarifa = null;
                }
            }
            else
                MessageBox.Show("Debe seleccionar una tarifa para borrarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void bModificar_Click(object sender, RoutedEventArgs e)
        {
            if (tarifa != null)
            {
                new VentanaGestionTarifas(tarifa).Show();
                listaComponentesTarifa.Clear();
                tDescripcionTarifa.Text = "";
                tarifa = null;
            }
            else
                MessageBox.Show("Debe seleccionar una tarifa para modificarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        internal void MostrarTarifaBuscada()
        {
            tDescripcionTarifa.Text = tarifa.nombreTarifa;
            foreach (ComponenteTarifa componente in tarifa.listaComponentesTarifa)
                listaComponentesTarifa.Add(componente);
        }

        private void tabla_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "nombreTarifa")
                e.Cancel = true;
        }
    }
}
