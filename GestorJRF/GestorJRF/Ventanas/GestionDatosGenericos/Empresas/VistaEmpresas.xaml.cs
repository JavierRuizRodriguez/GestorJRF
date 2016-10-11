using GestorJRF.CRUD.Empresas;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace GestorJRF.Ventanas.GestionDatosGenericos
{
    /// <summary>
    /// Lógica de interacción para VistaEmpresas.xaml
    /// </summary>
    public partial class VistaEmpresas : Window
    {
        public Empresa empresa { get; set; }
        public ObservableCollection<PersonaContacto> listaPersonasContacto { get; set; }

        public VistaEmpresas()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(this);
            listaPersonasContacto = new ObservableCollection<PersonaContacto>();
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }

        internal void MostrarCamionBuscado()
        {
            tNombre.Text = empresa.nombre;
            tNIF.Text = empresa.cif;
            tDomicilio.Text = empresa.domicilio;
            tLocalidad.Text = empresa.localidad;
            tProvincia.Text = empresa.provincia;
            tCP.Text = Convert.ToString(empresa.cp);
            tTelefono.Text = Convert.ToString(empresa.telefono);
            tMail.Text = empresa.email;
            foreach (PersonaContacto empresa in empresa.personasContacto)
                listaPersonasContacto.Add(empresa);
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "empresa").Show();
        }

        private void bModificar_Click(object sender, RoutedEventArgs e)
        {
            if (empresa != null)
            {
                new VentanaGestionEmpresas(empresa).Show();
                UtilidadesVentana.LimpiarCampos(gridPrincipal);
                listaPersonasContacto.Clear();
            }
            else
                MessageBox.Show("Debe seleccionar una empresa para modificarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void tablaPC_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "cif")
                e.Cancel = true;
            else
                e.Column.Header = e.Column.Header.ToString().ToUpper();
        }

        private void bEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (empresa != null)
            {
                if (MessageBox.Show("¿Desea borrar la empresa?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    int salida = EmpresasCRUD.borrarEmpresa(empresa.cif);

                    if (salida == 1)
                    {
                        UtilidadesVentana.LimpiarCampos(gridPrincipal);
                        listaPersonasContacto.Clear();
                    }
                }
            }
            else
                MessageBox.Show("Debe seleccionar una empresa para borrarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);

        }
    }
}
