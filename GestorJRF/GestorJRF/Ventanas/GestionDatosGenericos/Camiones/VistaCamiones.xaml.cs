using GestorJRF.CRUD.Empresas;
using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using Npgsql;
using System;
using System.Diagnostics;
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

        private void bModificar_Click(object sender, RoutedEventArgs e)
        {
            if (camion != null)
            {
                new VentanaGestionCamiones(camion).Show();
                UtilidadesVentana.LimpiarCampos(gridPrincipal);
            }
            else
                MessageBox.Show("Debe seleccionar un camión para modificarlo.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (camion != null)
            {
                if (MessageBox.Show("¿Desea borrar el camión?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    int salida = CamionesCRUD.borrarCamion(camion.nBastidor);

                    if(salida == 1)
                        UtilidadesVentana.LimpiarCampos(gridPrincipal);                    
                }
                
            }
            else
                MessageBox.Show("Debe seleccionar un camión para borrarlo.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
