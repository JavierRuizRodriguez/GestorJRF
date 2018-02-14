using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos.Camiones;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Camiones
{
    /// <summary>
    /// Lógica de interacción para VentanaModificacionKm.xaml
    /// </summary>
    public partial class VentanaModificacionKM : Window
    {
        private List<Camion> camiones;

        private bool actualizando;

        public VentanaModificacionKM()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.actualizarDatos();
        }

        private void actualizarDatos()
        {
            this.actualizando = true;
            this.cCamiones.Items.Clear();
            this.camiones = new List<Camion>(CamionesCRUD.cogerTodosCamiones().Cast<Camion>().ToList());
            foreach (Camion camione in this.camiones)
            {
                ItemCollection items = this.cCamiones.Items;
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                object newItem = comboBoxItem.Content = camione.matricula;
                items.Add(newItem);
            }
            this.actualizando = false;
            if (this.cCamiones.Items.Count > 0)
            {
                this.cCamiones.SelectedIndex = -1;
            }
        }

        private void cCamiones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.actualizando)
            {
                this.tDescripcionCamion.Text = this.camiones[this.cCamiones.SelectedIndex].ToString();
            }
        }

        private void bModificacionKilometraje_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVerificacion.validadorNumeroEntero(this.tNuevoKilometraje.Text))
            {
                Camion c = this.camiones[this.cCamiones.SelectedIndex];
                c.nBastidorAntiguo = c.nBastidor;
                c.kilometraje = Convert.ToInt64(this.tNuevoKilometraje.Text);
                CamionesCRUD.modificarCamion(c);
                this.tNuevoKilometraje.Text = "";
                this.tDescripcionCamion.Text = "";
                this.actualizarDatos();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }
    }
}
