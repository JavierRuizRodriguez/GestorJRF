using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Facturas;
using GestorJRF.POJOS.Mapas;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos;
using GestorJRF.Ventanas.GestionDatosGenericos.Resumenes;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Resumenes
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionResumenesFinal.xaml
    /// </summary>
    public partial class VentanaGestionResumenesFinal : Window
    {
        private bool xBotonPulsado;

        public ObservableCollection<Itinerario> listaItinerarios
        {
            get;
            set;
        }

        public ObservableCollection<Comision> listaComisiones
        {
            get;
            set;
        }

        public Resumen resumen
        {
            get;
            set;
        }

        public VentanaGestionResumenesFinal()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(1, this);
            this.listaItinerarios = new ObservableCollection<Itinerario>();
            this.listaComisiones = new ObservableCollection<Comision>();
            this.xBotonPulsado = true;
        }

        private void tablaItinerarios_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("id") || e.Column.Header.ToString().Equals("latitud") || e.Column.Header.ToString().Equals("longitud") || e.Column.Header.ToString().Equals("idResumen"))
            {
                e.Cancel = true;
            }
            e.Column.Width = (e.Column.Header.ToString().Equals("punto") ? new DataGridLength(50.0, DataGridLengthUnitType.Pixel) : (e.Column.Header.ToString().Equals("esEtapa") ? new DataGridLength(70.0, DataGridLengthUnitType.Pixel) : ((e.Column.Header.ToString().Equals("dni") || e.Column.Header.ToString().Equals("matricula")) ? new DataGridLength(80.0, DataGridLengthUnitType.Pixel) : (e.Column.Header.ToString().Equals("kilometrosVehiculo") ? new DataGridLength(140.0, DataGridLengthUnitType.Pixel) : new DataGridLength(1.0, DataGridLengthUnitType.Star)))));
            DataGridColumn column = e.Column;
            DataGridColumn column2 = e.Column;
            object obj3 = column.Header = (column2.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString()));
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.xBotonPulsado)
            {
                new VentanaMenuGestionDatos().Show();
            }
        }

        private bool comprobarDniItinerarios()
        {
            foreach (Itinerario listaItinerario in this.listaItinerarios)
            {
                if (listaItinerario.dni == null)
                {
                    return false;
                }
            }
            return true;
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "resumen final").Show();
        }

        public void MostrarResumenBuscado()
        {
            if (this.resumen != null)
            {
                string nombreEmpresa = EmpresasCRUD.cogerEmpresa("cif", this.resumen.cif).nombre;
                this.tEmpresa.Text = ((nombreEmpresa != null) ? nombreEmpresa : "");
                if (this.resumen.nombreTarifa == null)
                {
                    this.checkEsGrupaje.IsChecked = true;
                }
                else
                {
                    this.tNombreTarifa.Text = this.resumen.nombreTarifa;
                    this.tEtiquetaTarifa.Text = this.resumen.etiqueta;
                }
                this.tReferencia.Text = this.resumen.referencia;
                this.tKmIda.Text = Convert.ToString(this.resumen.kilometrosIda);
                this.tKmVuelta.Text = Convert.ToString(this.resumen.kilometrosVuelta);
                TextBox textBox = this.tFecha;
                DateTime dateTime = this.resumen.fechaPorte;
                dateTime = dateTime.Date;
                textBox.Text = dateTime.ToString("dd/MM/yyyy");
                this.tTipoCamion.Text = this.resumen.tipoCamion;
                this.tPrecio.Text = Convert.ToString(this.resumen.precioFinal);
                this.listaItinerarios.Clear();
                this.listaComisiones.Clear();
                foreach (Itinerario listaItinerario in this.resumen.listaItinerarios)
                {
                    this.listaItinerarios.Add(listaItinerario);
                }
                foreach (Comision listaComisione in this.resumen.listaComisiones)
                {
                    this.listaComisiones.Add(listaComisione);
                }
                Factura factura = FacturasCRUD.cogerFacturaPorIdResumen(this.resumen.id);
                if (factura != null)
                {
                    this.tMensaje.Visibility = Visibility.Visible;
                    this.tMensaje.Text = "ATENCIÓN, EL RESUMEN FINAL MOSTRADO POR PANTALLA ESTA INCLUIDO EN UNA FACTURA YA EMITIDA.";
                }
            }
        }

        private void bBorrarResumenFinal_Click(object sender, RoutedEventArgs e)
        {
            if (this.resumen != null)
            {
                if (MessageBox.Show("¿Desea borrar la el resumen final?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    int salida = ResumenesCRUD.borrarResumenFinal(this.resumen.id);
                    if (salida == 1)
                    {
                        this.resumen = null;
                        this.listaItinerarios.Clear();
                        this.listaComisiones.Clear();
                        UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un resumen para borrarlo.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("idResumenFinal") || e.Column.Header.ToString().Equals("id"))
            {
                e.Cancel = true;
            }
            e.Column.Width = (e.Column.Header.ToString().Equals("porcentaje") ? new DataGridLength(90.0, DataGridLengthUnitType.Pixel) : new DataGridLength(1.0, DataGridLengthUnitType.Star));
            e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }

        private void bModificar_Click(object sender, RoutedEventArgs e)
        {
            if (this.resumen != null)
            {
                VentanaGestionResumenesPrevio v = new VentanaGestionResumenesPrevio(true);
                v.resumen = this.resumen;
                v.MostrarResumenFinalModificacion();
                v.Show();
                UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                this.listaComisiones.Clear();
                this.listaItinerarios.Clear();
                this.resumen = null;
            }
            else
            {
                MessageBox.Show("Debe seleccionar un resumen para borrarlo.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}
