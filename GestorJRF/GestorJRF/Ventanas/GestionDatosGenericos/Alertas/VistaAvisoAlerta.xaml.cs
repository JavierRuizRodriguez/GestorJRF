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
        public Alerta alerta { get; set; }
        private List<AlertaFecha> alertasFecha;
        private List<AlertaKM> alertasKm;

        public VistaAvisoAlerta(List<AlertaFecha> alertasFecha, List<AlertaKM> alertasKm)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.alertasFecha = new List<AlertaFecha>(alertasFecha);
            this.alertasKm = new List<AlertaKM>(alertasKm);

            for (int x = 1; x <= alertasFecha.Count + alertasKm.Count; x++)
                cAlertas.Items.Add(new ComboBoxItem().Content = x);

            cAlertas.SelectedIndex = 0;
        }

        internal void MostrarAlertaBuscada()
        {
            AlertaFecha alertaFecha;
            AlertaKM alertaKM;
            tCamion.Text = (alerta.matricula == null) ? "" : alerta.matricula;
            tTipoAviso.Text = alerta.tipoAviso;
            tDescripcion.Text = alerta.descripcion;
            if (alerta.tipoAviso.Equals("fecha"))
            {
                alertaFecha = (AlertaFecha)alerta;
                tAntelacion.Text = Convert.ToString(alertaFecha.diasAntelacion);
                tLimite.Text = alertaFecha.fechaLimite.Date.ToString("dd/MM/yyyy");
            }
            else
            {
                lAntelacion.Content = "KM ANTELACIÓN AVISO";
                lLimite.Content = "KM PARA AVISO";
                alertaKM = (AlertaKM)alerta;
                tAntelacion.Text = Convert.ToString(alertaKM.kmAntelacion);
                tLimite.Text = Convert.ToString(alertaKM.kmLimite);
            }

        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "alerta").Show();
        }

        private void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (alerta != null)
            {
                if (MessageBox.Show("¿Desea borrar la alerta?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    int salida = AlertasCRUD.borrarAlerta(alerta.id);

                    if (salida == 1)
                    {
                        UtilidadesVentana.LimpiarCampos(gridPrincipal);
                        alerta = null;
                    }
                }

            }
            else
                MessageBox.Show("Debe seleccionar un camión para borrarlo.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void bModificar_Click(object sender, RoutedEventArgs e)
        {
            if (alerta != null)
            {
                new VentanaGestionAlertas(alerta).Show();
                UtilidadesVentana.LimpiarCampos(gridPrincipal);
                alerta = null;
            }
            else
                MessageBox.Show("Debe seleccionar una alerta para modificarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void tablaAlertas_BeginningEdit(object sender, System.Windows.Controls.DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void cAlertas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cAlertas.SelectedIndex != -1)
            {
                var id = cAlertas.SelectedIndex + 1;
                if (id <= alertasFecha.Count)
                    alerta = alertasFecha[cAlertas.SelectedIndex];
                else
                {
                    alerta = alertasKm[cAlertas.SelectedIndex - alertasFecha.Count];
                }
                MostrarAlertaBuscada();
            }
        }

        private void bEliminarAlerta_Click(object sender, RoutedEventArgs e)
        {
            if (alerta != null)
            {
                if (MessageBox.Show("¿Desea borrar la alerta?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    int salida = AlertasCRUD.borrarAlerta(alerta.id);

                    if (salida == 1)
                        reconstruirVista(alerta);
                }

            }
            else
                MessageBox.Show("Debe seleccionar una alerta para borrarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void reconstruirVista(Alerta alertaEliminada)
        {
            cAlertas.Items.Clear();
            if (alertaEliminada.tipoAviso.Equals("fecha"))
                alertasFecha.Remove((AlertaFecha)alertaEliminada);
            else
                alertasKm.Remove((AlertaKM)alertaEliminada);

            for (int x = 1; x <= alertasFecha.Count + alertasKm.Count; x++)
                cAlertas.Items.Add(new ComboBoxItem().Content = x);

            alerta = null;

            if (cAlertas.Items.Count > 0)
                cAlertas.SelectedIndex = 0;
            else
                UtilidadesVentana.LimpiarCampos(gridPrincipal);
        }
    }
}
