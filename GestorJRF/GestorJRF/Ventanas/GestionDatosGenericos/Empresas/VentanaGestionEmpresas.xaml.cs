using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace GestorJRF.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionEmpleados.xaml
    /// </summary>
    public partial class VentanaGestionEmpresas : Window
    {
        private bool esAlta;
        private bool esAltaEnMapa;
        public ObservableCollection<PersonaContacto> listaPersonasContacto { get; set; }
        public Empresa empresa;

        public VentanaGestionEmpresas(Empresa empresa)
        {
            InitializeComponent();

            UtilidadesVentana.SituarVentana(0, this);
            esAlta = false;
            this.empresa = empresa;

            bNuevaEmpresa.Content = "MODIFICAR EMPRESA";
            listaPersonasContacto = new ObservableCollection<PersonaContacto>(empresa.personasContacto);

            CambiarParaModificacion();
        }

        public VentanaGestionEmpresas(bool esAltaEnMapa)
        {
            InitializeComponent();

            listaPersonasContacto = new ObservableCollection<PersonaContacto>();
            UtilidadesVentana.SituarVentana(0, this);
            esAlta = true;
            this.esAltaEnMapa = esAltaEnMapa;
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (esAlta && !esAltaEnMapa)
                new VentanaMenuGestionDatos().Show();
        }

        private void bNuevoContacto_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(gridPersonalContacto))
            {
                if (UtilidadesVerificacion.validadorNumeroEntero(tTelefonoPC.Text) && UtilidadesVerificacion.validadorMail(tMailPC.Text))
                    listaPersonasContacto.Add(new PersonaContacto(tNombrePC.Text, Convert.ToInt32(tTelefonoPC.Text), tMailPC.Text));
            }
            else
                MessageBox.Show("Debe introducir todos los campos para rellenar la tabla", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(gridPrincipal);
            UtilidadesVentana.LimpiarCampos(gridPersonalContacto);
            listaPersonasContacto.Clear();
        }

        private void bNuevaEmpresa_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(gridPrincipal) && listaPersonasContacto.Count > 0)
            {
                if (sonCamposValidos())
                {
                    if (esAlta)
                        añadirEmpresa();
                    else
                        modificarEmpresa();
                }
            }
            else
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool sonCamposValidos()
        {
            if (UtilidadesVerificacion.validadorMail(tMail.Text) && UtilidadesVerificacion.validadorNumeroEntero(tCP.Text)
                && UtilidadesVerificacion.validadorNumeroEntero(tTelefono.Text))
                return true;
            else
                return false;
        }

        private void modificarEmpresa()
        {

            Empresa em = new Empresa(tNombre.Text, tNIF.Text, empresa.cif, tDomicilio.Text, tLocalidad.Text, tProvincia.Text, Convert.ToInt32(tCP.Text), Convert.ToInt32(tTelefono.Text), tMail.Text, listaPersonasContacto);
            EmpresasCRUD.modificarPersonasContacto(em, empresa.cif);

        }

        private void añadirEmpresa()
        {
            Empresa em = new Empresa(tNombre.Text, tNIF.Text, tDomicilio.Text, tLocalidad.Text, tProvincia.Text, Convert.ToInt32(tCP.Text), Convert.ToInt32(tTelefono.Text), tMail.Text, listaPersonasContacto);
            var codigo = EmpresasCRUD.insertarEmpresa(em);
            if (codigo == 1)
            {
                MessageBox.Show("Camion almacenado correctamente.", "Nueva empresa", MessageBoxButton.OK, MessageBoxImage.Information);
                bLimpiarCampos_Click(null, null);
            }
        }

        private void CambiarParaModificacion()
        {
            tNombre.Text = empresa.nombre;
            tNIF.Text = empresa.cif;
            tDomicilio.Text = empresa.domicilio;
            tLocalidad.Text = empresa.localidad;
            tProvincia.Text = empresa.provincia;
            tCP.Text = Convert.ToString(empresa.cp);
            tTelefono.Text = Convert.ToString(empresa.telefono);
            tMail.Text = empresa.email;
        }

        private void tablaPC_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "cif")
                e.Cancel = true;
            else
                e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }
    }
}
