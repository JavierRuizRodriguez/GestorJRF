using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Alertas
{
    /// <summary>
    /// Lógica de interacción para VistaAlertas.xaml
    /// </summary>
    public partial class VistaAvisoAlerta : Window
    {
        private List<AlertaFecha> alertasFecha;

        private List<AlertaKM> alertasKm;

        public Alerta alerta
        {
            get;
            set;
        }

        public VistaAvisoAlerta(List<AlertaFecha> alertasFecha, List<AlertaKM> alertasKm)
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.alertasFecha = new List<AlertaFecha>(alertasFecha);
            this.alertasKm = new List<AlertaKM>(alertasKm);
            for (int x = 1; x <= alertasFecha.Count + alertasKm.Count; x++)
            {
                ItemCollection items = this.cAlertas.Items;
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                object newItem = comboBoxItem.Content = x;
                items.Add(newItem);
            }
            this.cAlertas.SelectedIndex = -1;
        }

        internal void MostrarAlertaBuscada()
        {
            this.tCamion.Text = ((this.alerta.matricula == null) ? "" : this.alerta.matricula);
            this.tTipoAviso.Text = this.alerta.tipoAviso;
            this.tDescripcion.Text = this.alerta.descripcion;
            if (this.alerta.tipoAviso.Equals("fecha"))
            {
                AlertaFecha alertaFecha = (AlertaFecha)this.alerta;
                this.tAntelacion.Text = Convert.ToString(alertaFecha.diasAntelacion);
                TextBox textBox = this.tLimite;
                DateTime dateTime = alertaFecha.fechaLimite;
                dateTime = dateTime.Date;
                textBox.Text = dateTime.ToString("dd/MM/yyyy");
            }
            else
            {
                this.lAntelacion.Content = "KM ANTELACIÓN AVISO";
                this.lLimite.Content = "KM PARA AVISO";
                AlertaKM alertaKM = (AlertaKM)this.alerta;
                this.tAntelacion.Text = Convert.ToString(alertaKM.kmAntelacion);
                this.tLimite.Text = Convert.ToString(alertaKM.kmLimite);
            }
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "alerta").Show();
        }

        private void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (this.alerta != null)
            {
                if (MessageBox.Show("¿Desea borrar la alerta?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    int salida = AlertasCRUD.borrarAlerta(this.alerta.id);
                    if (salida == 1)
                    {
                        UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                        this.alerta = null;
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un camión para borrarlo.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void bModificar_Click(object sender, RoutedEventArgs e)
        {
            if (this.alerta != null)
            {
                new VentanaGestionAlertas(this.alerta).Show();
                UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                this.alerta = null;
            }
            else
            {
                MessageBox.Show("Debe seleccionar una alerta para modificarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void tablaAlertas_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void cAlertas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cAlertas.SelectedIndex != -1)
            {
                int id = this.cAlertas.SelectedIndex + 1;
                if (id <= this.alertasFecha.Count)
                {
                    this.alerta = this.alertasFecha[this.cAlertas.SelectedIndex];
                }
                else
                {
                    this.alerta = this.alertasKm[this.cAlertas.SelectedIndex - this.alertasFecha.Count];
                }
                this.MostrarAlertaBuscada();
            }
        }

        private void bEliminarAlerta_Click(object sender, RoutedEventArgs e)
        {
            if (this.alerta != null)
            {
                if (MessageBox.Show("¿Desea borrar la alerta?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    int salida = AlertasCRUD.borrarAlerta(this.alerta.id);
                    if (salida == 1)
                    {
                        this.reconstruirVista(this.alerta);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una alerta para borrarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void reconstruirVista(Alerta alertaEliminada)
        {
            this.cAlertas.Items.Clear();
            if (alertaEliminada.tipoAviso.Equals("fecha"))
            {
                this.alertasFecha.Remove((AlertaFecha)alertaEliminada);
            }
            else
            {
                this.alertasKm.Remove((AlertaKM)alertaEliminada);
            }
            for (int x = 1; x <= this.alertasFecha.Count + this.alertasKm.Count; x++)
            {
                ItemCollection items = this.cAlertas.Items;
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                object newItem = comboBoxItem.Content = x;
                items.Add(newItem);
            }
            this.alerta = null;
            if (this.cAlertas.Items.Count > 0)
            {
                this.cAlertas.SelectedIndex = -1;
            }
            else
            {
                base.Close();
            }
        }
    }
}
