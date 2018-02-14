using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Alertas
{
    /// <summary>
    /// Lógica de interacción para VistaAlertas.xaml
    /// </summary>
    public partial class VistaAlertas : Window
    {
        public Alerta alerta
        {
            get;
            set;
        }

        public VistaAlertas()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
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
                MessageBox.Show("Debe seleccionar una alerta para borrarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
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
                MessageBox.Show("Debe seleccionar una alerta para borrarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}
