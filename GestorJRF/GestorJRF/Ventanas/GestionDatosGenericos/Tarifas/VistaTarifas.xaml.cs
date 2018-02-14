using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos;
using GestorJRF.Ventanas.GestionDatosGenericos.Tarifas;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Tarifas
{
    /// <summary>
    /// Lógica de interacción para VistaTarifas.xaml
    /// </summary>
    public partial class VistaTarifas : Window
    {
        public ObservableCollection<ComponenteTarifa> listaComponentesTarifa
        {
            get;
            set;
        }

        public Tarifa tarifa
        {
            get;
            set;
        }

        public VistaTarifas()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.listaComponentesTarifa = new ObservableCollection<ComponenteTarifa>();
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "tarifa").Show();
        }

        private void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (this.tarifa != null)
            {
                if (MessageBox.Show("¿Desea borrar la tarifa?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    TarifasCRUD.borrarTarfia(this.tarifa.nombreTarifa);
                    this.listaComponentesTarifa.Clear();
                    this.tDescripcionTarifa.Text = "";
                    this.tarifa = null;
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una tarifa para borrarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void bModificar_Click(object sender, RoutedEventArgs e)
        {
            if (this.tarifa != null)
            {
                new VentanaGestionTarifas(this.tarifa).Show();
                this.listaComponentesTarifa.Clear();
                this.tDescripcionTarifa.Text = "";
                this.tarifa = null;
            }
            else
            {
                MessageBox.Show("Debe seleccionar una tarifa para modificarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        internal void MostrarTarifaBuscada()
        {
            this.tDescripcionTarifa.Text = this.tarifa.nombreTarifa;
            foreach (ComponenteTarifa item in this.tarifa.listaComponentesTarifa)
            {
                this.listaComponentesTarifa.Add(item);
            }
        }

        private void tabla_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("nombreTarifa"))
            {
                e.Cancel = true;
            }
        }

        private void tabla_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
