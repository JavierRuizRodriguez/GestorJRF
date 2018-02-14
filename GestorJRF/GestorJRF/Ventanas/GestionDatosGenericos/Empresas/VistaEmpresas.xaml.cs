using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos;
using GestorJRF.Ventanas.GestionDatosGenericos.Empresas;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Empresas
{
    /// <summary>
    /// Lógica de interacción para VistaEmpresas.xaml
    /// </summary>
    public partial class VistaEmpresas : Window
    {
        public Empresa empresa
        {
            get;
            set;
        }

        public ObservableCollection<PersonaContacto> listaPersonasContacto
        {
            get;
            set;
        }

        public VistaEmpresas()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.listaPersonasContacto = new ObservableCollection<PersonaContacto>();
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }

        internal void MostrarEmpresaBuscada()
        {
            if (this.listaPersonasContacto.Count > 0)
            {
                this.listaPersonasContacto.Clear();
            }
            this.tNombre.Text = this.empresa.nombre;
            this.tNIF.Text = this.empresa.cif;
            this.tDomicilio.Text = this.empresa.domicilio;
            this.tLocalidad.Text = this.empresa.localidad;
            this.tProvincia.Text = this.empresa.provincia;
            this.tCP.Text = this.empresa.cp;
            this.tTelefono.Text = Convert.ToString(this.empresa.telefono);
            foreach (PersonaContacto item in this.empresa.personasContacto)
            {
                this.listaPersonasContacto.Add(item);
            }
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "empresa").Show();
        }

        private void bModificar_Click(object sender, RoutedEventArgs e)
        {
            if (this.empresa != null)
            {
                if (!this.empresa.nombre.Equals("GENERAL"))
                {
                    new VentanaGestionEmpresas(this.empresa).Show();
                    UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                    this.listaPersonasContacto.Clear();
                    this.empresa = null;
                }
                else
                {
                    MessageBox.Show("La empresa 'GENERAL' no puede ser modificada.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una empresa para modificarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void tablaPC_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("cif"))
            {
                e.Cancel = true;
            }
            else
            {
                e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
            }
        }

        private void bEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (this.empresa != null)
            {
                if (!this.empresa.nombre.Equals("GENERAL"))
                {
                    if (MessageBox.Show("¿Desea borrar la empresa?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        int salida = EmpresasCRUD.borrarEmpresa(this.empresa.cif);
                        if (salida == 1)
                        {
                            UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                            this.listaPersonasContacto.Clear();
                            this.empresa = null;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("La empresa 'GENERAL' no puede ser borrada.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una empresa para borrarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}
