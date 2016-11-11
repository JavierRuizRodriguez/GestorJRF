using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Mapas;
using GestorJRF.Utilidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Resumenes
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionResumenesPrevio.xaml
    /// </summary>
    public partial class VentanaGestionResumenesPrevio : Window
    {
        private IList listaEmpleados;
        private IList listaVehículos;
        public ObservableCollection<Itinerario> listaItinerarios { get; set; }
        public ObservableCollection<Comision> listaComisiones { get; set; }
        public Resumen resumen { get; set; }

        private string valorAntiguo;
        private string valorAntiguoPorcentaje;
        private double comisionTotal;

        public VentanaGestionResumenesPrevio()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(1, this);
            listaEmpleados = EmpleadosCRUD.cogerTodosEmpleados();
            listaVehículos = CamionesCRUD.cogerTodosCamiones();
            listaItinerarios = new ObservableCollection<Itinerario>();
            listaComisiones = new ObservableCollection<Comision>();

            foreach (Empleado empleado in listaEmpleados)
            {
                cConductor.Items.Add(empleado.getNombreApellidos());
                cConductorComision.Items.Add(empleado.getNombreApellidos());
            }
            cConductor.SelectedIndex = 0;
            cConductorComision.SelectedIndex = 0;

            foreach (Camion camion in listaVehículos)
                cVehiculo.Items.Add(camion.matricula);
            cVehiculo.SelectedIndex = 0;

            comisionTotal = 0;
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

            e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }

        private void bNuevoResumenFinal_Click(object sender, RoutedEventArgs e)
        {
            if (comprobarDniItinerarios())
            {
                if (comprobarMatriculaItinerarios())
                {
                    if (comprobarKilometrosVehiculoItinerarios())
                    {
                        if(listaComisiones.Count > 0 && comisionTotal == 100)
                        {
                            resumen.listaItinerarios = new List<Itinerario>(listaItinerarios);
                            resumen.listaComisiones = new List<Comision>(listaComisiones);
                            if (!tReferencia.Text.Equals(""))
                                resumen.referencia = tReferencia.Text;

                            int salida = ResumenesCRUD.añadirResumenFinal(resumen);
                            if (salida == 1)
                            {
                                UtilidadesVentana.LimpiarCampos(gridPrincipal);
                                listaItinerarios.Clear();
                                listaComisiones.Clear();
                                comisionTotal = 0;
                            }
                        }
                        else
                            MessageBox.Show("Debe asignar las comisiones del resumen correctamente.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);                        
                    }
                    else
                        MessageBox.Show("Debe introducir los kilometros del vehículo para cada itinerario que no sea ETAPA.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Debe introducir un vehículo para cada itinerario que no sea ETAPA.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Debe introducir un conductor para cada itinerario que no sea ETAPA.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool comprobarKilometrosVehiculoItinerarios()
        {
            foreach (Itinerario i in listaItinerarios)
            {
                if (!i.esEtapa && i.kilometrosVehiculo == 0)
                    return false;
            }
            return true;
        }

        private bool comprobarMatriculaItinerarios()
        {
            foreach (Itinerario i in listaItinerarios)
            {
                if (!i.esEtapa && (i.matricula == null || i.matricula.Equals("")))
                    return false;
            }
            return true;
        }

        private bool comprobarDniItinerarios()
        {
            foreach (Itinerario i in listaItinerarios)
            {
                if (!i.esEtapa && i.dni == null)
                    return false;
            }
            return true;
        }

        private void bNuevoConductorItinerario_Click(object sender, RoutedEventArgs e)
        {
            if (tablaItinerarios.SelectedIndex != -1)
                listaItinerarios[tablaItinerarios.SelectedIndex].dni = ((Empleado)listaEmpleados[cConductor.SelectedIndex]).dni;
            else
                MessageBox.Show("Debe seleccionar un itinerario para añadir el conductor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "resumen previo").Show();
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
                tKmIda.Text = Convert.ToString(resumen.kilometrosIda);
                tKmVuelta.Text = Convert.ToString(resumen.kilometrosVuelta);
                tFecha.Text = resumen.fechaPorte.Date.ToString("dd/MM/yyyy");
                tTipoCamion.Text = resumen.tipoCamion;
                tPrecio.Text = Convert.ToString(resumen.precioFinal);
                foreach (Itinerario itinerario in resumen.listaItinerarios)
                    listaItinerarios.Add(itinerario);
            }
        }

        private void tablaItinerarios_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!e.Column.Header.ToString().Equals("DNI") && !e.Column.Header.ToString().Equals("MATRICULA") && !e.Column.Header.ToString().Equals("KILOMETROS VEHICULO"))
                e.Cancel = true;
            else {
                if (e.EditingEventArgs.Source is TextBlock)
                    valorAntiguo = ((TextBlock)e.EditingEventArgs.Source).Text;
                else
                    valorAntiguo = ((DataGridCell)e.EditingEventArgs.Source).DataContext as string;
            }
        }

        private void bBorrarResumenPrevio_Click(object sender, RoutedEventArgs e)
        {
            if (resumen != null)
            {
                if (MessageBox.Show("¿Desea borrar el resumen previo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    int salida = ResumenesCRUD.borrarResumenPrevio(resumen.id);
                    if (salida == 1)
                    {
                        resumen = null;
                        UtilidadesVentana.LimpiarCampos(gridPrincipal);
                        listaItinerarios.Clear();
                    }
                }
            }
            else
                MessageBox.Show("Debe seleccionar un resumen para borrarlo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void tablaItinerarios_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!e.Column.Header.ToString().Equals("KILOMETROS VEHICULO") && !((TextBox)e.EditingElement).Text.Equals("") && !((TextBox)e.EditingElement).Text.Equals(valorAntiguo))
            {
                ((TextBox)e.EditingElement).Text = "";
                MessageBox.Show("Únicamente se puede borrar el valor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void bNuevoVehiculo_Click(object sender, RoutedEventArgs e)
        {
            if (tablaItinerarios.SelectedIndex != -1)
                listaItinerarios[tablaItinerarios.SelectedIndex].matricula = ((Camion)listaVehículos[cVehiculo.SelectedIndex]).matricula;
            else
                MessageBox.Show("Debe seleccionar un itinerario para añadir el vehículo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("idResumenFinal") || e.Column.Header.ToString().Equals("id"))
                e.Cancel = true;

            e.Column.Width = e.Column.Header.ToString().Equals("porcentaje") ? new DataGridLength(90, DataGridLengthUnitType.Pixel) : new DataGridLength(1, DataGridLengthUnitType.Star);

            e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }

        private void dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!e.Column.Header.ToString().Equals("PORCENTAJE"))
                e.Cancel = true;
            else {
                if (e.EditingEventArgs.Source is TextBlock)
                    valorAntiguoPorcentaje = ((TextBlock)e.EditingEventArgs.Source).Text;
                else
                    valorAntiguoPorcentaje = ((DataGridCell)e.EditingEventArgs.Source).DataContext as string;
            }
        }

        private void bNuevoEmpleadoComision_Click(object sender, RoutedEventArgs e)
        {
            var empleado = (Empleado)listaEmpleados[cConductorComision.SelectedIndex];
            if (comisionTotal == 0)
            {
                listaComisiones.Add(new Comision(empleado.dni, 100));
                comisionTotal = 100;
            }
            else
            {
                if (!estaEnComisiones(empleado))
                    listaComisiones.Add(new Comision(empleado.dni, 0));
                else
                    MessageBox.Show("El conductor seleccionado ya está en la tabla de comisiones.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool estaEnComisiones(Empleado e)
        {
            foreach (Comision c in listaComisiones)
            {
                if (c.dni.Equals(e.dni))
                    return true;
            }
            return false;
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("PORCENTAJE") && !((TextBox)e.EditingElement).Text.Equals(""))
            {
                double comisionActual = Convert.ToDouble(((TextBox)e.EditingElement).Text);
                if (comisionActual + comisionTotal - Convert.ToDouble(valorAntiguoPorcentaje) <= 100 && comisionActual + comisionTotal - Convert.ToDouble(valorAntiguoPorcentaje) >= 0)
                    comisionTotal += comisionActual - Convert.ToDouble(valorAntiguoPorcentaje);
                else
                {
                    ((TextBox)e.EditingElement).Text = valorAntiguoPorcentaje;
                    MessageBox.Show("La comision total debe ser del 100%.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void dataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                comisionTotal -= ((Comision)dataGrid.SelectedItem).porcentaje;
            }
        }
    }
}
