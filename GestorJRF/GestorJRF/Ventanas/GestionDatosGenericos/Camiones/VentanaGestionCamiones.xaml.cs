using GestorJRF.CRUD.Empresas;
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
            UtilidadesVentana.SituarVentana(this);
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
            int salida;
            if (esAlta)
                añadirCamion();
            else
                modificarCamion();
        }

        private void modificarCamion()
        {
            Camion c = new Camion(tMarca.Text, tModelo.Text, tMatricula.Text, tNumBastidor.Text, camion.nBastidor, Convert.ToDouble(tLargoCaja.Text),
                        Convert.ToDouble(tLargoVehiculo.Text), Convert.ToInt64(tKilometraje.Text), Convert.ToDouble(tGalibo.Text),
                        ((ComboBoxItem)cComustible.SelectedItem).Content.ToString());
            CamionesCRUD.modificarCamion(c);
        }

        private void añadirCamion()
        {
            if (UtilidadesVentana.ComprobarCampos(gridPrincipal))
            {
                Camion c = new Camion(tMarca.Text, tModelo.Text, tMatricula.Text, tNumBastidor.Text, Convert.ToDouble(tLargoCaja.Text),
                    Convert.ToDouble(tLargoVehiculo.Text), Convert.ToInt64(tKilometraje.Text), Convert.ToDouble(tGalibo.Text),
                    ((ComboBoxItem)cComustible.SelectedItem).Content.ToString());
                int salida = CamionesCRUD.añadirCamion(c);

                if (salida == 1)
                    UtilidadesVentana.LimpiarCampos(gridPrincipal);
            }
            else
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
