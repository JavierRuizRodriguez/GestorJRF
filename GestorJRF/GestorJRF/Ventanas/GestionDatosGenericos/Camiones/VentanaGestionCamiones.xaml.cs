using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Camiones
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionCamiones.xaml
    /// </summary>
    public partial class VentanaGestionCamiones : Window
    {
        private bool esAlta;
        private Camion camion;
        public VentanaGestionCamiones()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            esAlta = true;
        }

        public VentanaGestionCamiones(Camion camion)
        {
            InitializeComponent();
            esAlta = false;

            this.camion = camion;
            tMarca.Text = camion.marca;
            tModelo.Text = camion.modelo;
            tMatricula.Text = camion.matricula;
            tNumBastidor.Text = camion.nBastidor;
            tLargoCaja.Text = Convert.ToString(camion.largoCaja);
            tLargoVehiculo.Text = Convert.ToString(camion.largoVehiculo);
            tKilometraje.Text = Convert.ToString(camion.kilometraje);
            tGalibo.Text = Convert.ToString(camion.galibo);
            cComustible.SelectedItem = Convert.ToString(camion.tipoCombustible);

            bNuevo.Content = "ACTUALIZAR CAMIÓN";

        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (esAlta)
                new VentanaMenuGestionDatos().Show();
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(gridPrincipal);
        }

        private void bNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(gridPrincipal))
            {
                if(sonCamposValidos())
                if (esAlta)
                    añadirCamion();
                else
                    modificarCamion();
            }
            else
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool sonCamposValidos()
        {
            if (UtilidadesVerificacion.validadorNumeroDecimal(tGalibo.Text) || UtilidadesVerificacion.validadorNumeroDecimal(tLargoCaja.Text)
                || UtilidadesVerificacion.validadorNumeroDecimal(tLargoVehiculo.Text) || UtilidadesVerificacion.validadorNumeroEntero(tKilometraje.Text))
                return true;
            else
                return false;
        }

        private void modificarCamion()
        {
            Camion c = new Camion(tMarca.Text, tModelo.Text, tMatricula.Text, tNumBastidor.Text, camion.nBastidor, Convert.ToDouble(tLargoCaja.Text, UtilidadesVerificacion.cogerProveedorDecimal()),
                        Convert.ToDouble(tLargoVehiculo.Text, UtilidadesVerificacion.cogerProveedorDecimal()), Convert.ToInt64(tKilometraje.Text), Convert.ToDouble(tGalibo.Text, UtilidadesVerificacion.cogerProveedorDecimal()),
                        ((ComboBoxItem)cComustible.SelectedItem).Content.ToString());
            CamionesCRUD.modificarCamion(c);
        }

        private void añadirCamion()
        {
            Camion c = new Camion(tMarca.Text, tModelo.Text, tMatricula.Text, tNumBastidor.Text, Convert.ToDouble(tLargoCaja.Text, UtilidadesVerificacion.cogerProveedorDecimal()),
                Convert.ToDouble(tLargoVehiculo.Text, UtilidadesVerificacion.cogerProveedorDecimal()), Convert.ToInt64(tKilometraje.Text), Convert.ToDouble(tGalibo.Text, UtilidadesVerificacion.cogerProveedorDecimal()),
                ((ComboBoxItem)cComustible.SelectedItem).Content.ToString());
            int salida = CamionesCRUD.añadirCamion(c);

            if (salida == 1)
                UtilidadesVentana.LimpiarCampos(gridPrincipal);

        }
    }
}
