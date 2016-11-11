using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Mapas;
using GestorJRF.Utilidades;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GestorJRF.Ventanas.Genericas
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class VistaCompletaDosTablas : Window
    {
        private ObservableCollection<Resumen> listaResumenes;
        private ObservableCollection<Itinerario> listaItinerarios;

        private ObservableCollection<Empresa> listaEmpresas;
        private ObservableCollection<PersonaContacto> listaPersonalContacto;

        private ObservableCollection<Tarifa> listaTarifas;
        private ObservableCollection<ComponenteTarifa> listaComponentesTarifa;

        private string tipo;

        public VistaCompletaDosTablas(string tipo)
        {
            InitializeComponent();
            this.tipo = tipo;

            switch (tipo)
            {
                case "resumen previo":
                    listaResumenes = new ObservableCollection<Resumen>();
                    listaItinerarios = new ObservableCollection<Itinerario>();
                    crearColumnasResumen();
                    crearColumnasItinerario();
                    IList resumenesPrevios = ResumenesCRUD.cogerTodosResumenesPrevios();
                    if (resumenesPrevios != null)
                    {
                        foreach (Resumen resumen in resumenesPrevios)
                            listaResumenes.Add(resumen);
                    }
                    break;
                case "resumen final":
                    listaResumenes = new ObservableCollection<Resumen>();
                    listaItinerarios = new ObservableCollection<Itinerario>();
                    crearColumnasResumen();
                    crearColumnasItinerario();
                    IList resumenesFinales = ResumenesCRUD.cogerTodosResumenesFinales();
                    if (resumenesFinales != null)
                    {
                        foreach (Resumen resumen in resumenesFinales)
                            listaResumenes.Add(resumen);
                    }
                    break;
                case "tarifa":
                    listaTarifas = new ObservableCollection<Tarifa>();
                    listaComponentesTarifa = new ObservableCollection<ComponenteTarifa>();
                    crearColumnasTarifas();
                    crearColumnasComponentesTarifa();
                    IList tarifas = TarifasCRUD.cogerTodasTarifas();
                    if (tarifas != null)
                    {
                        foreach (Tarifa tarifa in tarifas)
                            listaTarifas.Add(tarifa);
                    }
                    break;
                case "empresa":
                    listaEmpresas = new ObservableCollection<Empresa>();
                    listaPersonalContacto = new ObservableCollection<PersonaContacto>();
                    crearColumnasEmpresa();
                    crearColumnasPersonalContacto();
                    IList empresas = EmpresasCRUD.cogerTodasEmpresas();
                    if (empresas != null)
                    {
                        foreach (Empresa empresa in empresas)
                            listaEmpresas.Add(empresa);
                    }
                    break;
                default:
                    break;
            }
        }

        private void tabla1_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            var currentCell = e.ClipboardRowContent[tabla1.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }

        private void tabla2_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            var currentCell = e.ClipboardRowContent[tabla2.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }

        private void crearColumnasResumen()
        {
            tabla1.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaResumenes });
            tabla1.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void crearColumnasItinerario()
        {
            tabla2.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaItinerarios });
            tabla2.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void crearColumnasTarifas()
        {
            tabla1.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaTarifas });
            tabla2.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void crearColumnasComponentesTarifa()
        {
            tabla2.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaComponentesTarifa });
            tabla2.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void crearColumnasEmpresa()
        {
            tabla1.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaEmpresas });
            tabla1.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void crearColumnasPersonalContacto()
        {
            tabla2.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaPersonalContacto });
            tabla2.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void tabla1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Resumen resumenSeleccionado;
            Empresa empresaSeleccionada;
            Tarifa tarifaSeleccionada;

            switch (tipo)
            {
                case "resumen previo":
                    listaItinerarios.Clear();
                    resumenSeleccionado = ((Resumen)((DataGrid)sender).SelectedItem);
                    if (resumenSeleccionado != null)
                    {
                        IList itinerariosPrevios = ResumenesCRUD.cogerTodosItinerariosPorIdResumenPrevio(resumenSeleccionado.id);
                        if (itinerariosPrevios != null)
                        {
                            foreach (Itinerario itinerario in itinerariosPrevios)
                                listaItinerarios.Add(itinerario);
                        }
                    }
                    break;
                case "resumen final":
                    listaItinerarios.Clear();
                    resumenSeleccionado = ((Resumen)((DataGrid)sender).SelectedItem);
                    if (resumenSeleccionado != null)
                    {
                        IList itinerariosFinal = ResumenesCRUD.cogerTodosItinerariosPorIdResumenFinal(resumenSeleccionado.id);
                        if (itinerariosFinal != null)
                        {
                            foreach (Itinerario itinerario in itinerariosFinal)
                                listaItinerarios.Add(itinerario);
                        }
                    }
                    break;
                case "tarifa":
                    listaComponentesTarifa.Clear();
                    tarifaSeleccionada = ((Tarifa)((DataGrid)sender).SelectedItem);
                    if (tarifaSeleccionada != null)
                    {
                        IList componentesTarifa = TarifasCRUD.cogerTodosComponentes(tarifaSeleccionada.nombreTarifa);
                        if (componentesTarifa != null)
                        {
                            foreach (ComponenteTarifa c in componentesTarifa)
                                listaComponentesTarifa.Add(c);
                        }
                    }
                    break;
                case "empresa":
                    listaPersonalContacto.Clear();
                    empresaSeleccionada = ((Empresa)((DataGrid)sender).SelectedItem);
                    if (empresaSeleccionada != null)
                    {
                        IList personasContacto = EmpresasCRUD.cogerTodoPersonalContacto(empresaSeleccionada.cif);
                        if (personasContacto != null)
                        {
                            foreach (PersonaContacto p in personasContacto)
                                listaPersonalContacto.Add(p);
                        }
                    }
                    break;
                default:
                    break;
            }

        }

        private void tabla1_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (tipo.Contains("resumen"))
            {
                if (e.Column.Header.ToString() == "idAntiguo" || e.Column.Header.ToString() == "listaItinerarios"
                    || e.Column.Header.ToString() == "cif" || (e.Column.Header.ToString() == "referencia" && tipo.Equals("resumen previo")))
                    e.Cancel = true;
                else
                {
                    if (e.Column.Header.ToString() == "fechaPorte")
                    {
                        DataGridTextColumn columnaNueva = new DataGridTextColumn();
                        columnaNueva.Header = e.Column.Header.ToString();
                        Binding b = new Binding(e.PropertyName);
                        b.StringFormat = "dd/MM/yyyy";
                        columnaNueva.Binding = b;
                        e.Column = columnaNueva;
                    }
                }
            }
            else if (tipo.Equals("empresa"))
            {
                if (e.Column.Header.ToString() == "cifAntiguo")
                    e.Cancel = true;
            }
            else if (tipo.Equals("tarifa"))
            {
                if (e.Column.Header.ToString() == "nombreTarifaAntiguo")
                    e.Cancel = true;
            }

            e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }

        private void tabla2_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (tipo.Contains("resumen"))
            {
                if (e.Column.Header.ToString() == "id" || e.Column.Header.ToString() == "latitud"
                    || (e.Column.Header.ToString() == "dni" && tipo.Equals("resumen previo"))
                    || e.Column.Header.ToString() == "longitud" || e.Column.Header.ToString() == "idResumen")
                    e.Cancel = true;
                else
                {
                    if (e.Column.Header.ToString() == "fechaPorte")
                    {
                        DataGridTextColumn columnaNueva = new DataGridTextColumn();
                        columnaNueva.Header = e.Column.Header.ToString();
                        Binding b = new Binding(e.PropertyName);
                        b.StringFormat = "dd/MM/yyyy";
                        columnaNueva.Binding = b;
                        e.Column = columnaNueva;
                    }
                }
            }
            else if (tipo.Equals("empresa"))
            {
                if (e.Column.Header.ToString() == "cif" )
                    e.Cancel = true;
            }

            e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }
    }
}
