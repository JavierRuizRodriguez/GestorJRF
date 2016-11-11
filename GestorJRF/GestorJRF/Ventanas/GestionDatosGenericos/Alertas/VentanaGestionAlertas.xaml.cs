using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

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
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            esAlta = true;
            listaCamiones = CamionesCRUD.cogerTodosCamiones();
            foreach (Camion c in listaCamiones)
                cCamion.Items.Add(new ComboBoxItem().Content = c.matricula);
            cCamion.SelectedIndex = 0;
        }

        public VentanaGestionAlertas(Alerta alerta)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.alerta = alerta;
            esAlta = false;
            listaCamiones = CamionesCRUD.cogerTodosCamiones();
            foreach (Camion c in listaCamiones)
                cCamion.Items.Add(new ComboBoxItem().Content = c.matricula);
            cambiarParaModificar();
        }

        private void cambiarParaModificar()
        {
            bNuevoAviso.Content = "MODIFICAR ALERTA";
            AlertaFecha alertaFecha;
            AlertaKM alertaKM;
            cCamion.SelectedIndex = (alerta.matricula == null) ? 0 : buscarIncideCamion();
            if (alerta.matricula == null)
                checkAvisoGenerico.IsChecked = true;

            tDescripcion.Text = alerta.descripcion;
            if (alerta.tipoAviso.Equals("fecha"))
            {
                cAviso.SelectedIndex = 0;
                alertaFecha = (AlertaFecha)alerta;
                tAntelacion.Text = Convert.ToString(alertaFecha.diasAntelacion);
                tLimite.Text = alertaFecha.fechaLimite.Date.ToString("dd/MM/yyyy");
            }
            else
            {
                cAviso.SelectedIndex = 1;
                lAntelacion.Content = "KM ANTELACIÓN AVISO";
                lLimite.Content = "KM PARA AVISO";
                alertaKM = (AlertaKM)alerta;
                tAntelacion.Text = Convert.ToString(alertaKM.kmAntelacion);
                tLimite.Text = Convert.ToString(alertaKM.kmLimite);
            }
        }

        private int buscarIncideCamion()
        {
            var salida = 0;
            foreach (Camion c in listaCamiones)
            {
                if (c.matricula.Equals(alerta.matricula))
                    return salida;
                else
                    salida++;
            }
            return salida;
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (esAlta)
                new VentanaMenuGestionDatos().Show();
        }

        private void checkAvisoGenerico_Checked(object sender, RoutedEventArgs e)
        {
            cCamion.IsEnabled = false;
            esAvisoGenerico = true;
        }

        private void checkAvisoGenerico_Unchecked(object sender, RoutedEventArgs e)
        {
            cCamion.IsEnabled = true;
            esAvisoGenerico = false;
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(gridPrincipal);
        }

        private void bNuevoAviso_Click(object sender, RoutedEventArgs e)
        {
            bool camposValidos;
            if (((ComboBoxItem)cAviso.SelectedItem).Content.Equals("FECHA"))
            {
                tipoAvisoFecha = true;
                camposValidos = UtilidadesVerificacion.validadorFechas(tLimite.Text) && UtilidadesVerificacion.validadorNumeroEntero(tAntelacion.Text);
            }
            else
            {
                tipoAvisoFecha = false;
                camposValidos = UtilidadesVerificacion.validadorNumeroEntero(tLimite.Text) && UtilidadesVerificacion.validadorNumeroEntero(tAntelacion.Text);
            }

            if (UtilidadesVentana.ComprobarCampos(gridPrincipal))
            {
                if (camposValidos)
                {
                    if (esAlta)
                        añadirAlerta();
                    else
                        modificarAlerta();
                }
            }
            else
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void modificarAlerta()
        {
            string matricula;

            if (esAvisoGenerico)
                matricula = null;
            else
                matricula = ((Camion)listaCamiones[cCamion.SelectedIndex]).matricula;

            if (tipoAvisoFecha)
            {
                AlertaFecha alertaFecha = new AlertaFecha(alerta.id, matricula, tDescripcion.Text, Convert.ToInt32(tAntelacion.Text), Convert.ToDateTime(tLimite.Text));
                AlertasCRUD.modificarAlertaFecha(alertaFecha);
                Close();
            }
            else {
                AlertaKM alertaKM = new AlertaKM(alerta.id, matricula, tDescripcion.Text, Convert.ToInt64(tAntelacion.Text), Convert.ToInt64(tLimite.Text));
                AlertasCRUD.modificarAlertaKM(alertaKM);
                Close();
            }
        }

        private void añadirAlerta()
        {
            string matricula;

            if (esAvisoGenerico)
                matricula = null;
            else
                matricula = ((Camion)listaCamiones[cCamion.SelectedIndex]).matricula;

            Alerta a;
            int salida;
            if (tipoAvisoFecha)
            {
                a = new AlertaFecha(matricula, tDescripcion.Text, Convert.ToInt32(tAntelacion.Text), Convert.ToDateTime(tLimite.Text));
                salida = AlertasCRUD.añadirAlerta(a, 0);
            }
            else {
                a = new AlertaKM(matricula, tDescripcion.Text, Convert.ToInt64(tAntelacion.Text), Convert.ToInt64(tLimite.Text));
                salida = AlertasCRUD.añadirAlerta(a, 1);
            }

            if (salida == 1)
                UtilidadesVentana.LimpiarCampos(gridPrincipal);
        }

        private void cAviso_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)cAviso.SelectedItem).Content.Equals("FECHA"))
            {
                lAntelacion.Content = "DÍAS ANTELACIÓN AVISO";
                lLimite.Content = "FECHA DE AVISO";
                checkAvisoGenerico.Visibility = Visibility.Visible;
            }
            else
            {
                lAntelacion.Content = "KM ANTELACIÓN AVISO";
                lLimite.Content = "KM PARA AVISO";
                checkAvisoGenerico.Visibility = Visibility.Hidden;
                checkAvisoGenerico.IsChecked = false;
            }
        }
    }
}
