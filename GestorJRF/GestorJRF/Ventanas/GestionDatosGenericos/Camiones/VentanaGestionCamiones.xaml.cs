using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using Npgsql;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Camiones
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionCamiones.xaml
    /// </summary>
    public partial class VentanaGestionCamiones : Window
    {
        public VentanaGestionCamiones()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(this);
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(gridPrincipal);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(gridPrincipal))
            {
                Camion camion = new Camion(tMarca.Text, tModelo.Text, tMatricula.Text, tNumBastidor.Text, Convert.ToDouble(tLargoCaja.Text),
                    Convert.ToDouble(tLargoVehiculo.Text), Convert.ToInt64(tKilometraje.Text), Convert.ToDouble(tGalibo.Text),
                    ((ComboBoxItem)cComustible.SelectedItem).Content.ToString());
                try
                {
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarCamion", camion);
                    MessageBox.Show("Camion almacenado correctamente.", "Nuevo camión", MessageBoxButton.OK, MessageBoxImage.Information);
                    UtilidadesVentana.LimpiarCampos(gridPrincipal);
                }
                catch (NpgsqlException ex)
                {
                    if(ex.ErrorCode == -2147467259)
                    {
                        MessageBox.Show("El número de bastidor, o la matrícula, introducido ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Error en la creación del nuevo camión.", "Aviso error fatal", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
