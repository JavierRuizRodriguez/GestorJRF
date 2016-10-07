using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System;
using System.Windows;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Camiones
{
    /// <summary>
    /// Lógica de interacción para VistaCamion.xaml
    /// </summary>
    public partial class VistaCamiones : Window
    {
        public Camion camion { get; set; }
        public VistaCamiones()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(this);
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "camion").Show();
        }

        internal void MostrarCamionBuscado()
        {
            tMarca.Text = camion.marca;
            tModelo.Text = camion.modelo;
            tMatricula.Text = camion.matricula;
            tNumBastidor.Text = camion.nBastidor;
            tLargoCaja.Text = Convert.ToString(camion.largoCaja);
            tLargoVehiculo.Text = Convert.ToString(camion.largoVehiculo);
            tKilometraje.Text = Convert.ToString(camion.kilometraje);
            tGalibo.Text = Convert.ToString(camion.galibo);
            tTipoCombustible.Text = camion.tipoCombustible;
        }
    }
}
