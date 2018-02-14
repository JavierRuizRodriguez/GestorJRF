using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Alertas
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionAlertas.xaml
    /// </summary>
    public partial class VentanaGestionAlertas : Window
    {
        private bool esAlta;

        private bool tipoAvisoFecha;

        private bool esAvisoGenerico;

        private IList listaCamiones;

        private Alerta alerta;


        public VentanaGestionAlertas()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.esAlta = true;
            this.listaCamiones = CamionesCRUD.cogerTodosCamiones();
            foreach (Camion listaCamione in this.listaCamiones)
            {
                ItemCollection items = this.cCamion.Items;
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                object newItem = comboBoxItem.Content = listaCamione.matricula;
                items.Add(newItem);
            }
            this.cCamion.SelectedIndex = -1;
        }

        public VentanaGestionAlertas(Alerta alerta)
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.alerta = alerta;
            this.esAlta = false;
            this.listaCamiones = CamionesCRUD.cogerTodosCamiones();
            foreach (Camion listaCamione in this.listaCamiones)
            {
                ItemCollection items = this.cCamion.Items;
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                object newItem = comboBoxItem.Content = listaCamione.matricula;
                items.Add(newItem);
            }
            this.cambiarParaModificar();
        }

        private void cambiarParaModificar()
        {
            this.bNuevoAviso.Content = "MODIFICAR ALERTA";
            this.cCamion.SelectedIndex = ((this.alerta.matricula != null) ? this.buscarIncideCamion() : 0);
            if (this.alerta.matricula == null)
            {
                this.checkAvisoGenerico.IsChecked = true;
            }
            this.tDescripcion.Text = this.alerta.descripcion;
            if (this.alerta.tipoAviso.Equals("fecha"))
            {
                this.cAviso.SelectedIndex = -1;
                AlertaFecha alertaFecha = (AlertaFecha)this.alerta;
                this.tAntelacion.Text = Convert.ToString(alertaFecha.diasAntelacion);
                TextBox textBox = this.tLimite;
                DateTime dateTime = alertaFecha.fechaLimite;
                dateTime = dateTime.Date;
                textBox.Text = dateTime.ToString("dd/MM/yyyy");
            }
            else
            {
                this.cAviso.SelectedIndex = 1;
                this.lAntelacion.Content = "KM ANTELACIÓN AVISO";
                this.lLimite.Content = "KM PARA AVISO";
                AlertaKM alertaKM = (AlertaKM)this.alerta;
                this.tAntelacion.Text = Convert.ToString(alertaKM.kmAntelacion);
                this.tLimite.Text = Convert.ToString(alertaKM.kmLimite);
            }
        }

        private int buscarIncideCamion()
        {
            int salida = 0;
            foreach (Camion listaCamione in this.listaCamiones)
            {
                if (listaCamione.matricula.Equals(this.alerta.matricula))
                {
                    return salida;
                }
                salida++;
            }
            return salida;
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            if (this.esAlta)
            {
                new VentanaMenuGestionDatos().Show();
            }
        }

        private void checkAvisoGenerico_Checked(object sender, RoutedEventArgs e)
        {
            this.cCamion.IsEnabled = false;
            this.esAvisoGenerico = true;
        }

        private void checkAvisoGenerico_Unchecked(object sender, RoutedEventArgs e)
        {
            this.cCamion.IsEnabled = true;
            this.esAvisoGenerico = false;
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
        }

        private void bNuevoAviso_Click(object sender, RoutedEventArgs e)
        {
            bool camposValidos;
            if (((ComboBoxItem)this.cAviso.SelectedItem).Content.Equals("FECHA"))
            {
                this.tipoAvisoFecha = true;
                camposValidos = (UtilidadesVerificacion.validadorFechas(this.tLimite.Text) && UtilidadesVerificacion.validadorNumeroEntero(this.tAntelacion.Text));
            }
            else
            {
                this.tipoAvisoFecha = false;
                camposValidos = (UtilidadesVerificacion.validadorNumeroEntero(this.tLimite.Text) && UtilidadesVerificacion.validadorNumeroEntero(this.tAntelacion.Text));
            }
            if (UtilidadesVentana.ComprobarCampos(this.gridPrincipal))
            {
                if (camposValidos)
                {
                    if (this.esAlta)
                    {
                        this.añadirAlerta();
                    }
                    else
                    {
                        this.modificarAlerta();
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void modificarAlerta()
        {
            string matricula = (!this.esAvisoGenerico) ? ((Camion)this.listaCamiones[this.cCamion.SelectedIndex]).matricula : null;
            if (this.tipoAvisoFecha)
            {
                AlertaFecha alertaFecha = new AlertaFecha(this.alerta.id, matricula, this.tDescripcion.Text, Convert.ToInt32(this.tAntelacion.Text), Convert.ToDateTime(this.tLimite.Text));
                AlertasCRUD.modificarAlertaFecha(alertaFecha);
                base.Close();
            }
            else
            {
                AlertaKM alertaKM = new AlertaKM(this.alerta.id, matricula, this.tDescripcion.Text, Convert.ToInt64(this.tAntelacion.Text), Convert.ToInt64(this.tLimite.Text));
                AlertasCRUD.modificarAlertaKM(alertaKM);
                base.Close();
            }
        }

        private void añadirAlerta()
        {
            string matricula = (!this.esAvisoGenerico) ? ((Camion)this.listaCamiones[this.cCamion.SelectedIndex]).matricula : null;
            int salida;
            if (this.tipoAvisoFecha)
            {
                Alerta a2 = new AlertaFecha(matricula, this.tDescripcion.Text, Convert.ToInt32(this.tAntelacion.Text), Convert.ToDateTime(this.tLimite.Text));
                salida = AlertasCRUD.añadirAlerta(a2, 0);
            }
            else
            {
                Alerta a2 = new AlertaKM(matricula, this.tDescripcion.Text, Convert.ToInt64(this.tAntelacion.Text), Convert.ToInt64(this.tLimite.Text));
                salida = AlertasCRUD.añadirAlerta(a2, 1);
            }
            if (salida == 1)
            {
                UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
            }
        }

        private void cAviso_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)this.cAviso.SelectedItem).Content.Equals("FECHA"))
            {
                this.lAntelacion.Content = "DÍAS ANTELACIÓN AVISO";
                this.lLimite.Content = "FECHA DE AVISO";
                this.checkAvisoGenerico.Visibility = Visibility.Visible;
            }
            else
            {
                this.lAntelacion.Content = "KM ANTELACIÓN AVISO";
                this.lLimite.Content = "KM PARA AVISO";
                this.checkAvisoGenerico.Visibility = Visibility.Hidden;
                this.checkAvisoGenerico.IsChecked = false;
            }
        }
    }
}
