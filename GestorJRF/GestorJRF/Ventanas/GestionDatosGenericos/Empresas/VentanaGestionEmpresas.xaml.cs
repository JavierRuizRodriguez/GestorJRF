using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos.Empresas;
using GestorJRF.Ventanas.Mapas;
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
    /// Lógica de interacción para VentanaGestionEmpleados.xaml
    /// </summary>
    public partial class VentanaGestionEmpresas : Window
    {
        private bool esAlta;

        private bool esAltaEnMapa;

        private VentanaMapa vPadre;

        public Empresa empresa;

        public ObservableCollection<PersonaContacto> listaPersonasContacto
        {
            get;
            set;
        }

        public VentanaGestionEmpresas(Empresa empresa)
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.esAlta = false;
            this.empresa = empresa;
            this.bNuevaEmpresa.Content = "MODIFICAR EMPRESA";
            this.listaPersonasContacto = new ObservableCollection<PersonaContacto>(empresa.personasContacto);
            this.CambiarParaModificacion();
        }

        public VentanaGestionEmpresas(VentanaMapa vPadre, bool esAltaEnMapa)
        {
            this.InitializeComponent();
            this.listaPersonasContacto = new ObservableCollection<PersonaContacto>();
            UtilidadesVentana.SituarVentana(0, this);
            this.esAlta = true;
            this.esAltaEnMapa = esAltaEnMapa;
            this.vPadre = vPadre;
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            if (this.esAlta && !this.esAltaEnMapa)
            {
                new VentanaMenuGestionDatos().Show();
            }
        }

        private void bNuevoContacto_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(this.gridPersonalContacto))
            {
                if (UtilidadesVerificacion.validadorNumeroEntero(this.tTelefonoPC.Text) && UtilidadesVerificacion.validadorMail(this.tMailPC.Text))
                {
                    this.listaPersonasContacto.Add(new PersonaContacto(this.tNombrePC.Text, Convert.ToInt32(this.tTelefonoPC.Text), this.tMailPC.Text));
                    UtilidadesVentana.LimpiarCampos(this.gridPersonalContacto);
                }
            }
            else
            {
                MessageBox.Show("Debe introducir todos los campos para rellenar la tabla", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
            UtilidadesVentana.LimpiarCampos(this.gridPersonalContacto);
            this.listaPersonasContacto.Clear();
        }

        private void bNuevaEmpresa_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(this.gridPrincipal) && this.listaPersonasContacto.Count > 0)
            {
                if (this.sonCamposValidos())
                {
                    if (this.esAlta)
                    {
                        this.añadirEmpresa();
                    }
                    else
                    {
                        this.modificarEmpresa();
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private bool sonCamposValidos()
        {
            if (UtilidadesVerificacion.validadorNumeroEntero(this.tCP.Text) && UtilidadesVerificacion.validadorNumeroEntero(this.tTelefono.Text))
            {
                return true;
            }
            return false;
        }

        private void modificarEmpresa()
        {
            Empresa em = new Empresa(this.tNombre.Text, this.tNIF.Text, this.empresa.cif, this.tDomicilio.Text, this.tLocalidad.Text, this.tProvincia.Text, this.tCP.Text, Convert.ToInt32(this.tTelefono.Text), this.listaPersonasContacto);
            if (EmpresasCRUD.modificarPersonasContacto(em, this.empresa.cif) == 1)
            {
                base.Close();
            }
        }

        private void añadirEmpresa()
        {
            Empresa em = new Empresa(this.tNombre.Text, this.tNIF.Text, this.tDomicilio.Text, this.tLocalidad.Text, this.tProvincia.Text, this.tCP.Text, Convert.ToInt32(this.tTelefono.Text), this.listaPersonasContacto);
            int codigo = EmpresasCRUD.insertarEmpresa(em);
            if (codigo == 1)
            {
                this.bLimpiarCampos_Click(null, null);
                if (this.esAltaEnMapa)
                {
                    this.vPadre.actualizarEmpresas();
                }
            }
        }

        private void CambiarParaModificacion()
        {
            this.tNombre.Text = this.empresa.nombre;
            this.tNIF.Text = this.empresa.cif;
            this.tDomicilio.Text = this.empresa.domicilio;
            this.tLocalidad.Text = this.empresa.localidad;
            this.tProvincia.Text = this.empresa.provincia;
            this.tCP.Text = this.empresa.cp;
            this.tTelefono.Text = Convert.ToString(this.empresa.telefono);
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
    }
}
