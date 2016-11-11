using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System;
using System.Windows;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Alertas
{
    /// <summary>
    /// Lógica de interacción para VistaAlertas.xaml
    /// </summary>
    public partial class VistaAlertas : Window
    {
        public Alerta alerta { get; set; }
        public VistaAlertas()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
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
                MessageBox.Show("Debe seleccionar una alerta para borrarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Debe seleccionar una alerta para borrarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
