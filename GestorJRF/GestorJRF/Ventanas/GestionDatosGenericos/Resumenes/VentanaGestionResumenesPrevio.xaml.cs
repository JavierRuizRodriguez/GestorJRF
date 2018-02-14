using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Mapas;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos;
using GestorJRF.Ventanas.GestionDatosGenericos.Resumenes;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Resumenes
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionResumenesPrevio.xaml
    /// </summary>
    public partial class VentanaGestionResumenesPrevio : Window
    {
        private List<Empleado> listaEmpleados;

        private List<Camion> listaVehículos;

        private int nPaletsTotales;

        private string valorAntiguo;

        private double comisionTotal;

        private bool esResumenDirecto;

        private bool esModificacionResumenFinal;

        private bool esConfiguracionVentana;

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

        public VentanaGestionResumenesPrevio(bool esResumenDirecto)
        {
            this.InitializeComponent();
            this.esResumenDirecto = esResumenDirecto;
            UtilidadesVentana.SituarVentana(1, this);
            this.listaEmpleados = EmpleadosCRUD.cogerTodosEmpleados().Cast<Empleado>().ToList();
            this.listaEmpleados = new List<Empleado>(from e in this.listaEmpleados
                                                     orderby e.nombre
                                                     select e);
            this.listaVehículos = CamionesCRUD.cogerTodosCamiones().Cast<Camion>().ToList();
            this.listaVehículos = new List<Camion>(from c in this.listaVehículos
                                                   orderby c.matricula
                                                   select c);
            this.listaItinerarios = new ObservableCollection<Itinerario>();
            this.listaComisiones = new ObservableCollection<Comision>();
            foreach (Empleado listaEmpleado in this.listaEmpleados)
            {
                this.cConductor.Items.Add(listaEmpleado.getNombreApellidos());
                this.cConductorComision.Items.Add(listaEmpleado.getNombreApellidos());
            }
            this.cConductor.SelectedIndex = -1;
            this.cConductorComision.SelectedIndex = -1;
            foreach (Camion listaVehículo in this.listaVehículos)
            {
                this.cVehiculo.Items.Add(listaVehículo.matricula);
            }
            this.cVehiculo.SelectedIndex = -1;
            this.comisionTotal = 0.0;
            this.nPaletsTotales = 0;
        }

        private void tablaItinerarios_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("id") || e.Column.Header.ToString().Equals("latitud") || e.Column.Header.ToString().Equals("longitud") || e.Column.Header.ToString().Equals("idResumen") || e.Column.Header.ToString().Equals("direccion"))
            {
                e.Cancel = true;
            }
            e.Column.Width = (e.Column.Header.ToString().Equals("punto") ? new DataGridLength(50.0, DataGridLengthUnitType.Pixel) : (e.Column.Header.ToString().Equals("esEtapa") ? new DataGridLength(70.0, DataGridLengthUnitType.Pixel) : ((e.Column.Header.ToString().Equals("dni") || e.Column.Header.ToString().Equals("matricula")) ? new DataGridLength(80.0, DataGridLengthUnitType.Pixel) : (e.Column.Header.ToString().Equals("kilometrosVehiculo") ? new DataGridLength(140.0, DataGridLengthUnitType.Pixel) : (e.Column.Header.ToString().Equals("palets") ? new DataGridLength(50.0, DataGridLengthUnitType.Pixel) : new DataGridLength(1.0, DataGridLengthUnitType.Star))))));
            e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!this.esResumenDirecto)
            {
                new VentanaMenuGestionDatos().Show();
            }
        }

        private void bNuevoResumenFinal_Click(object sender, RoutedEventArgs e)
        {
            if (this.comprobarDniItinerarios())
            {
                if (this.comprobarMatriculaItinerarios())
                {
                    if (this.listaComisiones.Count > 0)
                    {
                        double porcentajeTotalComisiones = 0.0;
                        foreach (Comision listaComisione in this.listaComisiones)
                        {
                            porcentajeTotalComisiones += listaComisione.porcentaje;
                        }
                        if (porcentajeTotalComisiones == 100.0)
                        {
                            this.resumen.precioFinal = Convert.ToDouble(this.tPrecioFinal.Text);
                            if (!this.esModificacionResumenFinal)
                            {
                                this.altaResumenFinal();
                            }
                            else
                            {
                                this.modificacionResumenFinal();
                            }
                        }
                        else
                        {
                            MessageBox.Show("El total de las comisiones debe ser del 100%.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe asignar las comisiones del resumen correctamente en la BBDD.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    }
                }
                else
                {
                    MessageBox.Show("Debe introducir un vehículo para cada itinerario que no sea ETAPA.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            else
            {
                MessageBox.Show("Debe introducir un conductor para cada itinerario que no sea ETAPA.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void modificacionResumenFinal()
        {
            this.resumen.idAntiguo = this.resumen.id;
            this.resumen.listaItinerarios = new List<Itinerario>(this.listaItinerarios);
            this.resumen.listaComisiones = new List<Comision>(this.listaComisiones);
            if (!this.tReferencia.Text.Equals(""))
            {
                this.resumen.referencia = this.tReferencia.Text;
            }
            if (UtilidadesVerificacion.validadorFechas(this.tFecha.Text) && UtilidadesVerificacion.validadorNumeroDecimal(this.tPrecioPorte.Text))
            {
                this.resumen.precioFinal = Convert.ToDouble(this.tPrecioFinal.Text.Replace(",", "."), UtilidadesVerificacion.cogerProveedorDecimal());
                this.resumen.fechaPorte = Convert.ToDateTime(this.tFecha.Text);
                int salida = ResumenesCRUD.modificarResumenFinal(this.resumen);
                if (salida == 1)
                {
                    UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                    this.tPrecioFinal.Text = "0";
                    this.tPrecioPalets.Text = "0";
                    this.tPrecioFinal.Text = "0";
                    this.esConfiguracionVentana = true;
                    this.listaItinerarios.Clear();
                    this.listaComisiones.Clear();
                    this.comisionTotal = 0.0;
                    if (this.esResumenDirecto)
                    {
                        base.Close();
                    }
                }
            }
        }

        private void altaResumenFinal()
        {
            this.resumen.listaItinerarios = new List<Itinerario>(this.listaItinerarios);
            this.resumen.listaComisiones = new List<Comision>(this.listaComisiones);
            if (!this.tReferencia.Text.Equals(""))
            {
                this.resumen.referencia = this.tReferencia.Text;
            }
            int salida = ResumenesCRUD.añadirResumenFinal(this.resumen);
            if (salida == 1)
            {
                UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                this.tPrecioFinal.Text = "0";
                this.tPrecioPalets.Text = "0";
                this.tPrecioFinal.Text = "0";
                this.esConfiguracionVentana = true;
                this.listaItinerarios.Clear();
                this.listaComisiones.Clear();
                this.comisionTotal = 0.0;
                if (this.esResumenDirecto)
                {
                    base.Close();
                }
            }
        }

        private bool comprobarMatriculaItinerarios()
        {
            foreach (Itinerario listaItinerario in this.listaItinerarios)
            {
                if (!listaItinerario.esEtapa && (listaItinerario.matricula == null || listaItinerario.matricula.Equals("")))
                {
                    return false;
                }
            }
            return true;
        }

        private bool comprobarDniItinerarios()
        {
            foreach (Itinerario listaItinerario in this.listaItinerarios)
            {
                if (!listaItinerario.esEtapa && listaItinerario.dni == null)
                {
                    return false;
                }
            }
            return true;
        }

        private void bNuevoConductorItinerario_Click(object sender, RoutedEventArgs e)
        {
            if (this.tablaItinerarios.SelectedItems.Count > 0 && this.cConductor.SelectedIndex != -1)
            {
                foreach (object selectedItem in this.tablaItinerarios.SelectedItems)
                {
                    ((Itinerario)selectedItem).dni = this.listaEmpleados[this.cConductor.SelectedIndex].dni;
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un itinerario para añadir el conductor.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "resumen previo").Show();
        }

        public void MostrarResumenBuscado()
        {
            this.esConfiguracionVentana = true;
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
                this.tKmIda.Text = Convert.ToString(this.resumen.kilometrosIda);
                this.tKmVuelta.Text = Convert.ToString(this.resumen.kilometrosVuelta);
                TextBox textBox = this.tFecha;
                DateTime dateTime = this.resumen.fechaPorte;
                dateTime = dateTime.Date;
                textBox.Text = dateTime.ToString("dd/MM/yyyy");
                this.tTipoCamion.Text = this.resumen.tipoCamion;
                this.tPrecioPorte.Text = Convert.ToString(this.resumen.precioFinal);
                this.tPrecioPalets.Text = "0";
                this.tPrecioFinal.Text = Convert.ToString(this.resumen.precioFinal);
                this.listaItinerarios.Clear();
                foreach (Itinerario listaItinerario in this.resumen.listaItinerarios)
                {
                    this.listaItinerarios.Add(listaItinerario);
                }
                this.esModificacionResumenFinal = false;
            }
            this.esConfiguracionVentana = false;
        }

        public void MostrarResumenFinalModificacion()
        {
            this.esConfiguracionVentana = true;
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
                this.tKmIda.Text = Convert.ToString(this.resumen.kilometrosIda);
                this.tKmVuelta.Text = Convert.ToString(this.resumen.kilometrosVuelta);
                TextBox textBox = this.tFecha;
                DateTime dateTime = this.resumen.fechaPorte;
                dateTime = dateTime.Date;
                textBox.Text = dateTime.ToString("dd/MM/yyyy");
                this.tTipoCamion.Text = this.resumen.tipoCamion;
                this.tReferencia.Text = this.resumen.referencia;
                if (this.resumen.listaComisiones.Count > 0)
                {
                    this.listaComisiones.Clear();
                    foreach (Comision listaComisione in this.resumen.listaComisiones)
                    {
                        this.listaComisiones.Add(listaComisione);
                    }
                }
                this.listaItinerarios.Clear();
                int precioPalets = 0;
                foreach (Itinerario listaItinerario in this.resumen.listaItinerarios)
                {
                    this.listaItinerarios.Add(listaItinerario);
                    if (listaItinerario.palets > 0)
                    {
                        precioPalets += listaItinerario.palets;
                    }
                }
                this.tPrecioPorte.Text = Convert.ToString(this.resumen.precioFinal - (double)precioPalets);
                this.tPrecioPalets.Text = Convert.ToString(precioPalets);
                this.tPrecioFinal.Text = Convert.ToString(this.resumen.precioFinal);
                this.esModificacionResumenFinal = true;
                this.bNuevoResumenFinal.Content = "MODIFICAR RESUMEN FINAL";
                this.tFecha.IsEnabled = true;
                this.tPrecioPorte.IsEnabled = true;
                this.bBorrarResumenPrevio.IsEnabled = false;
                this.bBuscar.IsEnabled = false;
                this.tPrecioFinal.Text = this.tPrecioFinal.Text.Replace(",", ".");
                this.tPrecioPorte.Text = this.tPrecioPorte.Text.Replace(",", ".");
            }
            this.esConfiguracionVentana = false;
        }

        private void tablaItinerarios_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!e.Column.Header.ToString().Equals("DNI") && !e.Column.Header.ToString().Equals("MATRICULA") && !e.Column.Header.ToString().Equals("KILOMETROS VEHICULO") && !e.Column.Header.ToString().Equals("CLIENTE DE CLIENTE") && !e.Column.Header.ToString().Equals("PALETS"))
            {
                e.Cancel = true;
            }
            else
            {
                object item = this.tablaItinerarios.SelectedItem;
                string a = e.Column.Header.ToString();
                DataGridCellInfo dataGridCellInfo;
                if (!(a == "CLIENTE DE CLIENTE"))
                {
                    if (a == "PALETS")
                    {
                        dataGridCellInfo = this.tablaItinerarios.SelectedCells[6];
                        this.valorAntiguo = (dataGridCellInfo.Column.GetCellContent(item) as TextBlock).Text;
                    }
                }
                else
                {
                    dataGridCellInfo = this.tablaItinerarios.SelectedCells[5];
                    this.valorAntiguo = (dataGridCellInfo.Column.GetCellContent(item) as TextBlock).Text;
                }
            }
        }

        private void bBorrarResumenPrevio_Click(object sender, RoutedEventArgs e)
        {
            if (this.resumen != null)
            {
                if (MessageBox.Show("¿Desea borrar el resumen previo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    int salida = ResumenesCRUD.borrarResumenPrevio(this.resumen.id);
                    if (salida == 1)
                    {
                        this.resumen = null;
                        UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                        this.esConfiguracionVentana = true;
                        this.listaItinerarios.Clear();
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un resumen para borrarlo.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void tablaItinerarios_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int num;
            if ((e.Column.Header.ToString().Equals("DNI") || e.Column.Header.ToString().Equals("MATRICULA")) && !((TextBox)e.EditingElement).Text.Equals(""))
            {
                num = ((!((TextBox)e.EditingElement).Text.Equals(this.valorAntiguo)) ? 1 : 0);
                goto IL_0076;
            }
            num = 0;
            goto IL_0076;
        IL_0076:
            if (num != 0)
            {
                ((TextBox)e.EditingElement).Text = "";
                MessageBox.Show("Únicamente se puede borrar el valor.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else if (e.Column.Header.ToString().Equals("PALETS"))
            {
                if (((TextBox)e.EditingElement).Text.Equals(""))
                {
                    ((TextBox)e.EditingElement).Text = "0";
                }
                else
                {
                    string valorCampo = ((TextBox)e.EditingElement).Text;
                    if (UtilidadesVerificacion.validadorNumeroEntero(valorCampo))
                    {
                        if (Convert.ToInt32(this.valorAntiguo) > Convert.ToInt32(valorCampo))
                        {
                            this.nPaletsTotales -= Convert.ToInt32(this.valorAntiguo) - Convert.ToInt32(((TextBox)e.EditingElement).Text);
                        }
                        else if (Convert.ToInt32(this.valorAntiguo) < Convert.ToInt32(((TextBox)e.EditingElement).Text))
                        {
                            this.nPaletsTotales += Convert.ToInt32(((TextBox)e.EditingElement).Text) - Convert.ToInt32(this.valorAntiguo);
                        }
                        this.tPrecioPalets.Text = this.nPaletsTotales.ToString() + ".00";
                        this.tPrecioFinal.Text = Convert.ToString((double)this.nPaletsTotales + this.resumen.precioFinal);
                    }
                    else
                    {
                        ((TextBox)e.EditingElement).Text = this.valorAntiguo;
                    }
                }
            }
        }

        private void bNuevoVehiculo_Click(object sender, RoutedEventArgs e)
        {
            if (this.tablaItinerarios.SelectedItems.Count > 0 && this.cVehiculo.SelectedIndex != -1)
            {
                foreach (object selectedItem in this.tablaItinerarios.SelectedItems)
                {
                    ((Itinerario)selectedItem).matricula = this.listaVehículos[this.cVehiculo.SelectedIndex].matricula;
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un itinerario para añadir el vehículo.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
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

        private void dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!e.Column.Header.ToString().Equals("PORCENTAJE"))
            {
                e.Cancel = true;
            }
        }

        private void bNuevoEmpleadoComision_Click(object sender, RoutedEventArgs e)
        {
            if (this.cConductorComision.SelectedIndex != -1)
            {
                Empleado empleado = this.listaEmpleados[this.cConductorComision.SelectedIndex];
                if (this.comisionTotal == 0.0)
                {
                    this.listaComisiones.Add(new Comision(empleado.dni, 100.0));
                    this.comisionTotal = 100.0;
                }
                else if (!this.estaEnComisiones(empleado))
                {
                    this.listaComisiones.Add(new Comision(empleado.dni, 0.0));
                }
                else
                {
                    MessageBox.Show("El conductor seleccionado ya está en la tabla de comisiones.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un conductor.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private bool estaEnComisiones(Empleado e)
        {
            foreach (Comision listaComisione in this.listaComisiones)
            {
                if (listaComisione.dni.Equals(e.dni))
                {
                    return true;
                }
            }
            return false;
        }

        private void tablaItinerarios_AutoGeneratedColumns(object sender, EventArgs e)
        {
            foreach (DataGridColumn column in this.tablaItinerarios.Columns)
            {
                if (column.Header.Equals("POBLACION"))
                {
                    column.DisplayIndex = 1;
                }
            }
        }

        private void tPrecioPorte_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!this.esConfiguracionVentana)
            {
                if (UtilidadesVerificacion.validadorNumeroDecimal(this.tPrecioPorte.Text))
                {
                    this.tPrecioFinal.Text = Convert.ToString(Convert.ToDouble(this.tPrecioPorte.Text, UtilidadesVerificacion.cogerProveedorDecimal()) + Convert.ToDouble(this.tPrecioPalets.Text, UtilidadesVerificacion.cogerProveedorDecimal()));
                }
                else
                {
                    this.tPrecioPorte.Text = "0";
                    this.tPrecioFinal.Text = this.tPrecioPalets.Text;
                }
            }
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            TextBox t = e.EditingElement as TextBox;
            t.Text = t.Text.Replace(".", ",");
        }
    }
}
