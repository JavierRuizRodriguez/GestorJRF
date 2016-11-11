using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Mapas;
using GestorJRF.Utilidades;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Resumenes
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionResumenesFinal.xaml
    /// </summary>
    public partial class VentanaGestionResumenesFinal : Window
    {
        public ObservableCollection<Itinerario> listaItinerarios { get; set; }
        public ObservableCollection<Comision> listaComisiones { get; set; }
        public Resumen resumen { get; set; }

        public VentanaGestionResumenesFinal()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(1, this);
            listaItinerarios = new ObservableCollection<Itinerario>();
            listaComisiones = new ObservableCollection<Comision>();
        }

        private void tablaItinerarios_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("id") || e.Column.Header.ToString().Equals("latitud") ||
                e.Column.Header.ToString().Equals("longitud") || e.Column.Header.ToString().Equals("idResumen"))
                e.Cancel = true;

            e.Column.Width = e.Column.Header.ToString().Equals("punto") ? new DataGridLength(50, DataGridLengthUnitType.Pixel) :
                (e.Column.Header.ToString().Equals("esEtapa") ? new DataGridLength(70, DataGridLengthUnitType.Pixel) :
                (e.Column.Header.ToString().Equals("dni") || (e.Column.Header.ToString().Equals("matricula"))) ? new DataGridLength(80, DataGridLengthUnitType.Pixel) :
                e.Column.Header.ToString().Equals("kilometrosVehiculo") ? new DataGridLength(140, DataGridLengthUnitType.Pixel) : new DataGridLength(1, DataGridLengthUnitType.Star));

            e.Column.Header = e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }

        private bool comprobarDniItinerarios()
        {
            foreach (Itinerario i in listaItinerarios)
            {
                if (i.dni == null)
                    return false;
            }
            return true;
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "resumen final").Show();
        }

        public void MostrarResumenBuscado()
        {
            if (resumen != null)
            {
                var nombreEmpresa = EmpresasCRUD.cogerEmpresa("cif", resumen.cif).nombre;
                tEmpresa.Text = nombreEmpresa != null ? nombreEmpresa : "";
                if (resumen.nombreTarifa == null)
                    checkEsGrupaje.IsChecked = true;
                else
                {
                    tNombreTarifa.Text = resumen.nombreTarifa;
                    tEtiquetaTarifa.Text = resumen.etiqueta;
                }
                tReferencia.Text = resumen.referencia;
                tKmIda.Text = Convert.ToString(resumen.kilometrosIda);
                tKmVuelta.Text = Convert.ToString(resumen.kilometrosVuelta);
                tFecha.Text = resumen.fechaPorte.Date.ToString("dd/MM/yyyy");
                tTipoCamion.Text = resumen.tipoCamion;
                tPrecio.Text = Convert.ToString(resumen.precioFinal);
                foreach (Itinerario itinerario in resumen.listaItinerarios)
                    listaItinerarios.Add(itinerario);
                foreach (Comision comision in resumen.listaComisiones)
                    listaComisiones.Add(comision);
            }
        }

        private void bBorrarResumenFinal_Click(object sender, RoutedEventArgs e)
        {
            if (resumen != null)
            {
                if (MessageBox.Show("¿Desea borrar la el resumen final?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    int salida = ResumenesCRUD.borrarResumenFinal(resumen.id);
                    if (salida == 1)
                    {
                        resumen = null;
                        listaItinerarios.Clear();
                        UtilidadesVentana.LimpiarCampos(gridPrincipal);
                    }
                }
            }
            else
                MessageBox.Show("Debe seleccionar un resumen para borrarlo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("idResumenFinal") || e.Column.Header.ToString().Equals("id"))
                e.Cancel = true;

            e.Column.Width = e.Column.Header.ToString().Equals("porcentaje") ? new DataGridLength(90, DataGridLengthUnitType.Pixel) : new DataGridLength(1, DataGridLengthUnitType.Star);

            e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }
    }
}
