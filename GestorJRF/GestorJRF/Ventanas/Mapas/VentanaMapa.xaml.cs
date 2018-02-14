using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Mapas;
using GestorJRF.REST_MAPAS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos.Empresas;
using GestorJRF.Ventanas.GestionDatosGenericos.Resumenes;
using GestorJRF.Ventanas.Login;
using GestorJRF.Ventanas.Mapas;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace GestorJRF.Ventanas.Mapas
{
    /// <summary>
    /// Lógica de interacción para VentanaMapa.xaml
    /// </summary>
    public partial class VentanaMapa : Window
    {
        private const string DIRECCION_NAVE = "Calle La Habana, 28806 Alcalá de Henares";

        private const double LATITUD_NAVE = 40.51272;

        private const double LONGITUD_NAVE = -3.41142;

        private Dispatcher dispacher;

        private char letraItinerario;

        private double kilometrosRutaActual;

        private Tarifa tarifaClienteSeleccionado;

        private ComponenteTarifa componenteTarifaActual;

        private int nCompGrande;

        private int nCompMedio;

        private int nCompPequeño;

        private List<Empresa> listaEmpresas;

        private List<MapPolyline> rutasActuales;

        private List<Pushpin> banderasActuales;

        private List<Route> rutasPosibles;

        private List<Route> rutasRetornoPosibles;

        private Resumen resumenPrevio;

        private bool iniciando;

        public ObservableCollection<Ruta> listaRutas { get; set; }
        public ObservableCollection<Ruta> listaRutasRetorno { get; set; }
        public ObservableCollection<Itinerario> listaItinerarios { get; set; }



        public VentanaMapa()
        {
            this.listaEmpresas = EmpresasCRUD.cogerTodasEmpresas().Cast<Empresa>().ToList();
            this.listaEmpresas = new List<Empresa>(from e in this.listaEmpresas
                                                   orderby e.nombre
                                                   select e);
            this.listaRutas = new ObservableCollection<Ruta>();
            this.listaRutasRetorno = new ObservableCollection<Ruta>();
            this.listaItinerarios = new ObservableCollection<Itinerario>();
            this.listaItinerarios.CollectionChanged += this.OnCollectionChanged;
            this.dispacher = Application.Current.Dispatcher;
            this.letraItinerario = 'A';
            this.iniciando = true;
            this.InitializeComponent();
            this.iniciando = false;
            UtilidadesVentana.SituarVentana(2, this);
            foreach (Empresa listaEmpresa in this.listaEmpresas)
            {
                ItemCollection items = this.cCliente.Items;
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                object newItem = comboBoxItem.Content = listaEmpresa.nombre.ToUpper();
                items.Add(newItem);
            }
            this.banderasActuales = new List<Pushpin>();
            this.rutasPosibles = new List<Route>();
            this.rutasRetornoPosibles = new List<Route>();
            this.rutasActuales = new List<MapPolyline>();
        }

        private char cogerLetraItinerario()
        {
            return this.letraItinerario++;
        }

        private void bCalcularRuta_Click(object sender, RoutedEventArgs e)
        {
            this.reiniciarCampos(1);
            this.dispacher.Invoke(delegate
            {
                this._calcularRuta(this.listaItinerarios);
            });
        }

        private void _calcularRuta(ObservableCollection<Itinerario> _listaItinerarios)
        {
            if (this.listaItinerarios != null && this.listaItinerarios.Count > 1)
            {
                List<Itinerario> itinerarios = new List<Itinerario>(_listaItinerarios);
                this.GetRoute(false, itinerarios);
                List<Itinerario> listaRetorno = new List<Itinerario>();
                listaRetorno.Add(((Collection<Itinerario>)this.listaItinerarios)[this.listaItinerarios.Count - 1]);
                listaRetorno.Add(new Itinerario(this.cogerLetraItinerario(), "Calle La Habana, 28806 Alcalá de Henares", 40.51272, -3.41142, false, "Alcalá de Henares"));
                this.GetRoute(true, listaRetorno);
            }
            else
            {
                MessageBox.Show("Debe haber más de un itinerario seleccionado para el calcula de la ruta.");
            }
        }

        private void GetKey(Action<string> callback)
        {
            if (callback != null)
            {
                this.mapa.CredentialsProvider.GetCredentials(delegate (Credentials c)
                {
                    callback(c.ApplicationId);
                });
            }
        }

        private void GetRoute(bool esRetorno, List<Itinerario> itinerario)
        {
            this.GetKey(delegate (string c)
            {
                List<string> list = new List<string>();
                if (this.checkAutopista.IsChecked == true)
                {
                    list.Add("highways");
                }
                if (this.checkPeaje.IsChecked == true)
                {
                    list.Add("tolls");
                }
                BingREST.Route(itinerario, list, c, delegate (Response r)
                {
                    if (r != null && r.ResourceSets != null && r.ResourceSets.Length != 0 && r.ResourceSets[0].Resources != null && r.ResourceSets[0].Resources.Length != 0)
                    {
                        if (!esRetorno)
                        {
                            this.rutasPosibles.Clear();
                            this.listaRutas.Clear();
                        }
                        else
                        {
                            this.rutasRetornoPosibles.Clear();
                            this.listaRutasRetorno.Clear();
                        }
                        int num = 0;
                        Resource[] resources = r.ResourceSets[0].Resources;
                        for (int i = 0; i < resources.Length; i++)
                        {
                            Route route = (Route)resources[i];
                            if (!esRetorno)
                            {
                                this.rutasPosibles.Add(route);
                                this.listaRutas.Add(new Ruta(num, route.TravelDistance, route.TravelDuration));
                            }
                            else
                            {
                                this.rutasRetornoPosibles.Add(route);
                                this.listaRutasRetorno.Add(new Ruta(num, route.TravelDistance, route.TravelDuration));
                            }
                            num++;
                        }
                        if (!esRetorno)
                        {
                            this.tablaRutasPosibles.SelectedIndex = 0;
                            this.pintarRutas(0);
                            this.tRutaSeleccionada.Text = "0";
                        }
                        else
                        {
                            this.tablaRutasRetornoPosibles.SelectedIndex = 0;
                            this.tRutaRetornoSeleccionada.Text = "0";
                        }
                        this.gridInformacionPorte.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No Results found.");
                    }
                });
            });
        }

        internal void actualizarEmpresas()
        {
            this.cCliente.Items.Clear();
            this.listaEmpresas = EmpresasCRUD.cogerTodasEmpresas().Cast<Empresa>().ToList();
            this.listaEmpresas = new List<Empresa>(from e in this.listaEmpresas
                                                   orderby e.nombre
                                                   select e);
            foreach (Empresa listaEmpresa in this.listaEmpresas)
            {
                ItemCollection items = this.cCliente.Items;
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                object newItem = comboBoxItem.Content = listaEmpresa.nombre.ToUpper();
                items.Add(newItem);
            }
            this.cCliente.SelectedIndex = -1;
        }

        private void GetLocation(string lugar)
        {
            if (!string.IsNullOrWhiteSpace(lugar))
            {
                this.GetKey(delegate (string c)
                {
                    BingREST.Location(lugar, c, delegate (Response r)
                    {
                        if (r != null && r.ResourceSets != null && r.ResourceSets.Length != 0 && r.ResourceSets[0].Resources != null && r.ResourceSets[0].Resources.Length != 0)
                        {
                            GestorJRF.REST_MAPAS.Location location = r.ResourceSets[0].Resources[0] as GestorJRF.REST_MAPAS.Location;
                            if (location.Name.Equals("España"))
                            {
                                MessageBox.Show("La dirección solicitada no se encuentra.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                            }
                            else
                            {
                                this.mapa.SetView(new Microsoft.Maps.MapControl.WPF.Location(location.Point.Coordinates[0], location.Point.Coordinates[1]), 15.0, 0.0);
                                this.listaItinerarios.Add(new Itinerario(this.cogerLetraItinerario(), location.Name, location.Point.Coordinates[0], location.Point.Coordinates[1], false, location.Address.Locality));
                                this.añadirUnaBandera(location.Point.Coordinates[0], location.Point.Coordinates[1]);
                                this.tDireccion.Text = "";
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron resultados.");
                        }
                    });
                });
            }
            else
            {
                MessageBox.Show("Dirección invalida.");
            }
        }

        private void pintarRutas(int idRutaPrincipal)
        {
            this.limpiarRutasAnteriores();
            int contador = 0;
            foreach (Route rutasPosible in this.rutasPosibles)
            {
                if (contador != idRutaPrincipal)
                {
                    this._pintarRuta(false, rutasPosible);
                }
                contador++;
            }
            this._pintarRuta(true, this.rutasPosibles[idRutaPrincipal]);
        }

        private void pintarRutasRetorno(int idRutaPrincipal)
        {
            this.limpiarRutasAnteriores();
            int contador = 0;
            foreach (Route rutasRetornoPosible in this.rutasRetornoPosibles)
            {
                if (contador != idRutaPrincipal)
                {
                    this._pintarRuta(false, rutasRetornoPosible);
                }
                contador++;
            }
            this._pintarRuta(true, this.rutasRetornoPosibles[idRutaPrincipal]);
        }

        private void _pintarRuta(bool esPrincipal, Route r)
        {
            double[][] routePath = r.RoutePath.Line.Coordinates;
            LocationCollection locs = new LocationCollection();
            for (int i = 0; i < routePath.Length; i++)
            {
                if (routePath[i].Length >= 2)
                {
                    locs.Add(new Microsoft.Maps.MapControl.WPF.Location(routePath[i][0], routePath[i][1]));
                }
            }
            MapPolyline routeLine = new MapPolyline
            {
                Locations = locs,
                Stroke = (esPrincipal ? new SolidColorBrush(Colors.Aqua) : new SolidColorBrush(Colors.Red)),
                StrokeThickness = 5.0,
                Opacity = (esPrincipal ? 0.7 : 0.3)
            };
            this.rutasActuales.Add(routeLine);
            this.mapa.Children.Add(routeLine);
            this.mapa.SetView(locs, new Thickness(200.0), 0.0);
            this.añadirBanderas();
        }

        private void limpiarRutasAnteriores()
        {
            foreach (MapPolyline rutasActuale in this.rutasActuales)
            {
                this.mapa.Children.Remove(rutasActuale);
            }
        }

        private void añadirUnaBandera(double latitud, double longitud)
        {
            this.limpiarBanderas();
            Pushpin bandera = new Pushpin();
            bandera.Location = new Microsoft.Maps.MapControl.WPF.Location(latitud, longitud);
            bandera.Background = new SolidColorBrush(Colors.Orange);
            this.mapa.Children.Add(bandera);
            this.banderasActuales.Add(bandera);
        }

        private void añadirBanderas()
        {
            this.limpiarBanderas();
            foreach (Itinerario listaItinerario in this.listaItinerarios)
            {
                Pushpin bandera = new Pushpin();
                bandera.Location = new Microsoft.Maps.MapControl.WPF.Location(listaItinerario.latitud, listaItinerario.longitud);
                bandera.Content = listaItinerario.punto;
                bandera.Background = new SolidColorBrush(Colors.Black);
                this.mapa.Children.Add(bandera);
                this.banderasActuales.Add(bandera);
            }
        }

        private void limpiarBanderas()
        {
            foreach (Pushpin banderasActuale in this.banderasActuales)
            {
                this.mapa.Children.Remove(banderasActuale);
            }
            this.banderasActuales.Clear();
        }

        private void tablaRutasPosibles_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null & e.ClickCount > 1)
            {
                this.tRutaSeleccionada.Text = Convert.ToString(((DataGrid)sender).SelectedIndex);
                this.limpiarBanderas();
                this.pintarRutas(((Ruta)((DataGrid)sender).SelectedItem).id);
                this.calcularTarifacion();
            }
        }

        private void tablaRutasPosibles_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Header = (e.Column.Header.ToString().Equals("id") ? "RUTAS POSIBLES" : UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString()));
        }

        private void esSalidaNave_Checked(object sender, RoutedEventArgs e)
        {
            if (this.listaItinerarios.Count == 0)
            {
                this.listaItinerarios.Add(new Itinerario(this.cogerLetraItinerario(), "Calle La Habana, 28806 Alcalá de Henares", 40.51272, -3.41142, false, "Alcalá de Henares"));
            }
            else
            {
                List<Itinerario> listaItinerarioAux = new List<Itinerario>(this.listaItinerarios);
                this.listaItinerarios.Clear();
                this.listaItinerarios.Add(new Itinerario(this.cogerLetraItinerario(), "Calle La Habana, 28806 Alcalá de Henares", 40.51272, -3.41142, false, "Alcalá de Henares"));
                foreach (Itinerario item in listaItinerarioAux)
                {
                    this.listaItinerarios.Add(new Itinerario(this.cogerLetraItinerario(), item.poblacion, item.latitud, item.longitud, false, item.poblacion));
                }
            }
        }

        private void esSalidaNave_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.listaItinerarios.Count > 0)
            {
                this.listaItinerarios.RemoveAt(0);
            }
        }

        private void nuevoPuntoItinerario_Click(object sender, RoutedEventArgs e)
        {
            this.GetLocation(this.tDireccion.Text);
        }

        private void tablaItinerario_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("direccionBing") || e.Column.Header.ToString().Equals("latitud") || e.Column.Header.ToString().Equals("longitud") || e.Column.Header.ToString().Equals("id") || e.Column.Header.ToString().Equals("idResumen") || e.Column.Header.ToString().Equals("dni") || e.Column.Header.ToString().Equals("kilometrosVehiculo") || e.Column.Header.ToString().Equals("matricula") || e.Column.Header.ToString().Equals("clienteDeCliente") || e.Column.Header.ToString().Equals("poblacion") || e.Column.Header.ToString().Equals("palets"))
            {
                e.Cancel = true;
            }
            e.Column.Width = (e.Column.Header.ToString().Equals("punto") ? new DataGridLength(50.0, DataGridLengthUnitType.Pixel) : (e.Column.Header.ToString().Equals("esEtapa") ? new DataGridLength(70.0, DataGridLengthUnitType.Pixel) : new DataGridLength(1.0, DataGridLengthUnitType.Star)));
            e.Column.Header = (e.Column.Header.ToString().Equals("esEtapa") ? "es etapa" : e.Column.Header);
            e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }

        private void tablaItinerario_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column.GetType() != typeof(DataGridCheckBoxColumn) || (this.checkSalidaNave.IsChecked == true && ((DataGrid)sender).SelectedIndex == 0))
            {
                e.Cancel = true;
            }
        }

        private void tablaItinerario_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Delete == e.Key && ((Itinerario)this.tablaItinerario.SelectedItems[0]).direccion.Equals("Calle La Habana, 28806 Alcalá de Henares"))
            {
                e.Handled = true;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedAction action = e.Action;
            if (action.ToString().Equals("Remove"))
            {
                this.rehacerListaRutas();
            }
            else
            {
                action = e.Action;
                if (action.ToString().Equals("Reset"))
                {
                    this.letraItinerario = 'A';
                }
            }
        }

        private void rehacerListaRutas()
        {
            this.letraItinerario = 'A';
            foreach (Itinerario listaItinerario in this.listaItinerarios)
            {
                listaItinerario.punto = this.cogerLetraItinerario();
            }
        }

        private void cCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.checkGrupaje.IsChecked == false && this.cCliente.Items.Count > 0 && this.gridInformacionPorte.IsEnabled && this.cCliente.SelectedIndex != -1)
            {
                this.tarifaClienteSeleccionado = TarifasCRUD.cogerTarifaPorNombreEmpresa(this.listaEmpresas[this.cCliente.SelectedIndex].nombre);
                if (this.tarifaClienteSeleccionado == null)
                {
                    this.tarifaClienteSeleccionado = TarifasCRUD.cogerTarifaPorNombreEmpresa("GENERAL");
                }
                this.ordenarListaComponentesTarifa(this.tarifaClienteSeleccionado.listaComponentesTarifa);
                if (this.cComponenteTarifa.Items.Count != 0)
                {
                    this.cComponenteTarifa.Items.Clear();
                }
                foreach (ComponenteTarifa item in this.tarifaClienteSeleccionado.listaComponentesTarifa)
                {
                    if (item.tipoCamion.Equals(((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.ToString()))
                    {
                        ItemCollection items = this.cComponenteTarifa.Items;
                        ComboBoxItem comboBoxItem = new ComboBoxItem();
                        object newItem = comboBoxItem.Content = item.etiqueta;
                        items.Add(newItem);
                    }
                }
                this.calcularTarifacion();
            }
        }

        private void ordenarListaComponentesTarifa(List<ComponenteTarifa> listaComponentesTarifa)
        {
            List<ComponenteTarifa> listaAuxiliar = new List<ComponenteTarifa>();
            int contador = 20;
            ComponenteTarifa etiquetaMaxima = this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Contains("-+") && c.tipoCamion.Equals("CAMIÓN MEDIANO"));
            try
            {
                listaAuxiliar.Add(this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("REPARTO") && c.tipoCamion.Equals("CAMIÓN GRANDE")));
                listaAuxiliar.Add(this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("T-URB") && c.tipoCamion.Equals("CAMIÓN GRANDE")));
                for (; contador <= this.cogerNumeroDeEtiqeta(etiquetaMaxima); contador += 10)
                {
                    listaAuxiliar.Add(this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("T-" + contador + "KM") && c.tipoCamion.Equals("CAMIÓN GRANDE")));
                }
                listaAuxiliar.Add(this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Contains("-+") && c.tipoCamion.Equals("CAMIÓN GRANDE")));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            this.nCompGrande = listaAuxiliar.Count();
            listaAuxiliar.Add(this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("REPARTO") && c.tipoCamion.Equals("CAMIÓN MEDIANO")));
            listaAuxiliar.Add(this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("T-URB") && c.tipoCamion.Equals("CAMIÓN MEDIANO")));
            for (contador = 20; contador <= this.cogerNumeroDeEtiqeta(etiquetaMaxima); contador += 10)
            {
                listaAuxiliar.Add(this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("T-" + contador + "KM") && c.tipoCamion.Equals("CAMIÓN MEDIANO")));
            }
            listaAuxiliar.Add(this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Contains("-+") && c.tipoCamion.Equals("CAMIÓN MEDIANO")));
            this.nCompMedio = listaAuxiliar.Count() - this.nCompGrande;
            listaAuxiliar.Add(this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("REPARTO") && c.tipoCamion.Equals("CAMIÓN PEQUEÑO")));
            listaAuxiliar.Add(this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("T-URB") && c.tipoCamion.Equals("CAMIÓN PEQUEÑO")));
            for (contador = 20; contador <= this.cogerNumeroDeEtiqeta(etiquetaMaxima); contador += 10)
            {
                listaAuxiliar.Add(this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("T-" + contador + "KM") && c.tipoCamion.Equals("CAMIÓN PEQUEÑO")));
            }
            listaAuxiliar.Add(this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Contains("-+") && c.tipoCamion.Equals("CAMIÓN PEQUEÑO")));
            this.nCompPequeño = listaAuxiliar.Count() - this.nCompMedio - this.nCompGrande;
            this.tarifaClienteSeleccionado.listaComponentesTarifa = new List<ComponenteTarifa>(listaAuxiliar);
        }

        private int cogerNumeroDeEtiqeta(ComponenteTarifa componenteTarifa)
        {
            return Convert.ToInt32(Regex.Match(componenteTarifa.etiqueta, "\\d+").Value);
        }

        private void calcularTarifacion()
        {
            if (!this.iniciando && this.checkGrupaje.IsChecked == false && this.cCliente.SelectedIndex != -1)
            {
                int kilemetrajeMaximoTarifa = this.cogerNumeroDeEtiqeta(this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Contains("-+") && c.tipoCamion.Equals("CAMIÓN PEQUEÑO")));
                int numeroRepartos = this.cogerNumeroRepartos();
                this.tRepartos.Text = Convert.ToString(numeroRepartos);
                this.kilometrosRutaActual = ((Ruta)this.tablaRutasPosibles.SelectedItem).kilometros + ((Ruta)this.tablaRutasRetornoPosibles.SelectedItem).kilometros;
                bool salidaCorrecta = true;
                double num;
                if (this.kilometrosRutaActual > (double)kilemetrajeMaximoTarifa)
                {
                    try
                    {
                        this.componenteTarifaActual = this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Contains("-+") && c.tipoCamion == ((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.ToString());
                        TextBox textBox = this.tPrecioFinal;
                        num = this.componenteTarifaActual.precio * this.kilometrosRutaActual + (double)numeroRepartos * this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("REPARTO") && c.tipoCamion == ((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.ToString()).precio;
                        textBox.Text = num.ToString("F2");
                        this.tPrecioTarifa.Text = Convert.ToString(this.componenteTarifaActual.precio) + " €/KM";
                    }
                    catch (Exception e3)
                    {
                        Console.WriteLine(e3.ToString());
                        salidaCorrecta = false;
                        MessageBox.Show("DEBE INTRODUCIR TODAS LOS COMPONENTES DE TARIFA DE 'REPARTO' PARA LA TARIFA ACTUAL.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    }
                }
                else if (this.kilometrosRutaActual < 20.0)
                {
                    try
                    {
                        this.componenteTarifaActual = this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("T-URB") && c.tipoCamion == ((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.ToString());
                        TextBox textBox2 = this.tPrecioFinal;
                        num = this.componenteTarifaActual.precio + (double)numeroRepartos * this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("REPARTO") && c.tipoCamion == ((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.ToString()).precio;
                        textBox2.Text = num.ToString("F2");
                        this.tPrecioTarifa.Text = Convert.ToString(this.componenteTarifaActual.precio) + " €";
                    }
                    catch (Exception e2)
                    {
                        Console.WriteLine(e2.ToString());
                        salidaCorrecta = false;
                        MessageBox.Show("DEBE INTRODUCIR TODAS LOS COMPONENTES DE TARIFA DE 'REPARTO' PARA LA TARIFA ACTUAL.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    }
                }
                else
                {
                    try
                    {
                        this.kilometrosRutaActual /= 2.0;
                        if (this.kilometrosRutaActual % 10.0 != 0.0)
                        {
                            this.kilometrosRutaActual = this.kilometrosRutaActual - this.kilometrosRutaActual % 10.0 + 10.0;
                        }
                        string etiquetaBsuqueda = "T-" + this.kilometrosRutaActual + "KM";
                        this.componenteTarifaActual = this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals(etiquetaBsuqueda) && c.tipoCamion == ((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.ToString());
                        TextBox textBox3 = this.tPrecioFinal;
                        num = this.componenteTarifaActual.precio + (double)numeroRepartos * this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("REPARTO") && c.tipoCamion == ((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.ToString()).precio;
                        textBox3.Text = num.ToString("F2");
                        this.tPrecioTarifa.Text = Convert.ToString(this.componenteTarifaActual.precio) + " €";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        salidaCorrecta = false;
                        MessageBox.Show("DEBE INTRODUCIR TODAS LOS COMPONENTES DE TARIFA DE 'REPARTO' PARA LA TARIFA ACTUAL.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    }
                }
                if (salidaCorrecta)
                {
                    int id = this.tarifaClienteSeleccionado.listaComponentesTarifa.IndexOf(this.componenteTarifaActual);
                    if (((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.Equals("CAMIÓN PEQUEÑO"))
                    {
                        id -= this.cogerIdComponentePequeño();
                    }
                    else if (((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.Equals("CAMIÓN MEDIANO"))
                    {
                        id -= this.cogerIdComponenteGrande();
                    }
                    this.cComponenteTarifa.SelectedIndex = id;
                    this.tPrecioFinal.Text = this.tPrecioFinal.Text.Replace(",", ".");
                    this.tPrecioTarifa.Text = this.tPrecioTarifa.Text.Replace(",", ".");
                    this.tNombreTarifa.Text = this.componenteTarifaActual.nombreTarifa;
                }
            }
        }

        private int cogerIdComponenteGrande()
        {
            return this.nCompGrande;
        }

        private int cogerIdComponentePequeño()
        {
            return this.nCompGrande + this.nCompMedio;
        }

        private int cogerNumeroRepartos()
        {
            int repartos = 0;
            foreach (Itinerario listaItinerario in this.listaItinerarios)
            {
                if (!listaItinerario.esEtapa)
                {
                    repartos++;
                }
            }
            if (this.checkSalidaNave.IsChecked == true)
            {
                return repartos - 2;
            }
            return repartos - 1;
        }

        private void tablaRutasRetornoPosibles_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Header = (e.Column.Header.ToString().Equals("id") ? "RETONO POSIBLE" : UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString()));
        }

        private void tablaRutasRetornoPosibles_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null)
            {
                this.tRutaRetornoSeleccionada.Text = Convert.ToString(((DataGrid)sender).SelectedIndex);
                this.limpiarBanderas();
                this.pintarRutasRetorno(((Ruta)((DataGrid)sender).SelectedItem).id);
                this.calcularTarifacion();
            }
        }

        private void cComponenteTarifa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int numeroRepartos = this.cogerNumeroRepartos();
            if (this.cComponenteTarifa.Items.Count > 0 && this.cComponenteTarifa.SelectedIndex != -1)
            {
                int id = this.cComponenteTarifa.SelectedIndex;
                if (((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.Equals("CAMIÓN PEQUEÑO"))
                {
                    id += this.cogerIdComponentePequeño();
                }
                else if (((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.Equals("CAMIÓN MEDIANO"))
                {
                    id += this.cogerIdComponenteGrande();
                }
                this.componenteTarifaActual = this.tarifaClienteSeleccionado.listaComponentesTarifa[id];
                double num;
                if (this.componenteTarifaActual.etiqueta.Contains("-+"))
                {
                    TextBox textBox = this.tPrecioFinal;
                    num = this.componenteTarifaActual.precio * this.kilometrosRutaActual + (double)numeroRepartos * this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("REPARTO") && c.tipoCamion == ((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.ToString()).precio;
                    textBox.Text = num.ToString("F2");
                    this.tPrecioTarifa.Text = Convert.ToString(this.componenteTarifaActual.precio) + " €/KM";
                }
                else
                {
                    TextBox textBox2 = this.tPrecioFinal;
                    num = this.componenteTarifaActual.precio + (double)numeroRepartos * this.tarifaClienteSeleccionado.listaComponentesTarifa.Single((ComponenteTarifa c) => c.etiqueta.Equals("REPARTO") && c.tipoCamion == ((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.ToString()).precio;
                    textBox2.Text = num.ToString("F2");
                    this.tPrecioTarifa.Text = Convert.ToString(this.componenteTarifaActual.precio) + " €";
                }
                this.tPrecioFinal.Text = this.tPrecioFinal.Text.Replace(",", ".");
                this.tPrecioTarifa.Text = this.tPrecioTarifa.Text.Replace(",", ".");
            }
        }

        private void cTipoCamion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.calcularTarifacion();
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            this.reiniciarCampos(0);
        }

        private void reiniciarCampos(int modo)
        {
            if (modo == 0)
            {
                UtilidadesVentana.LimpiarCampos(this.gridItinerario);
                this.listaItinerarios.Clear();
                this.checkSalidaNave.IsChecked = false;
                this.checkSalidaNave.IsChecked = true;
                this.checkGrupaje.IsChecked = false;
            }
            UtilidadesVentana.LimpiarCampos(this.gridInfoRutas);
            UtilidadesVentana.LimpiarCampos(this.gridInfoTransporte);
            this.listaRutas.Clear();
            this.listaRutasRetorno.Clear();
            this.limpiarBanderas();
            this.limpiarRutasAnteriores();
            this.gridInformacionPorte.IsEnabled = false;
            this.cCliente.SelectedIndex = -1;
            this.cComponenteTarifa.Items.Clear();
        }

        private void bNuevaEmpresa_Click(object sender, RoutedEventArgs e)
        {
            new VentanaGestionEmpresas(this, true).Show();
        }

        private long insertarResumenPrevio()
        {
            if (this.checkGrupaje.IsChecked == true)
            {
                double km_ida;
                double km_vuelta;
                if (this.tablaRutasPosibles.Items.Count > 0)
                {
                    km_ida = Convert.ToDouble(((Ruta)this.tablaRutasPosibles.SelectedItem).kilometros, UtilidadesVerificacion.cogerProveedorDecimal());
                    km_vuelta = Convert.ToDouble(((Ruta)this.tablaRutasRetornoPosibles.SelectedItem).kilometros, UtilidadesVerificacion.cogerProveedorDecimal());
                }
                else
                {
                    km_ida = 0.0;
                    km_vuelta = 0.0;
                }
                this.resumenPrevio = new Resumen(this.listaEmpresas[this.cCliente.SelectedIndex].cif, this.listaEmpresas[this.cCliente.SelectedIndex].nombre, km_ida, km_vuelta, null, null, ((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.ToString(), Convert.ToDateTime(this.tFechaTransporte.Text), this.listaItinerarios, Convert.ToDouble(this.tPrecioFinal.Text, UtilidadesVerificacion.cogerProveedorDecimal()));
            }
            else
            {
                this.resumenPrevio = new Resumen(this.listaEmpresas[this.cCliente.SelectedIndex].cif, this.listaEmpresas[this.cCliente.SelectedIndex].nombre, Convert.ToDouble(((Ruta)this.tablaRutasPosibles.SelectedItem).kilometros, UtilidadesVerificacion.cogerProveedorDecimal()), Convert.ToDouble(((Ruta)this.tablaRutasRetornoPosibles.SelectedItem).kilometros, UtilidadesVerificacion.cogerProveedorDecimal()), this.tNombreTarifa.Text, this.cComponenteTarifa.SelectedItem.ToString(), ((ComboBoxItem)this.cTipoCamion.SelectedItem).Content.ToString(), Convert.ToDateTime(this.tFechaTransporte.Text), this.listaItinerarios, Convert.ToDouble(this.tPrecioFinal.Text, UtilidadesVerificacion.cogerProveedorDecimal()));
            }
            return ResumenesCRUD.añadirResumenPrevio(this.resumenPrevio);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            new VentanaMenuPrincipal().Show();
        }

        private void checkGrupaje_Checked(object sender, RoutedEventArgs e)
        {
            this.cComponenteTarifa.IsEnabled = false;
            this.tNombreTarifa.Text = "";
            this.cComponenteTarifa.SelectedIndex = -1;
            this.tPrecioFinal.Text = "";
            this.tPrecioTarifa.Text = "";
            this.tRepartos.Text = "";
            this.gridInformacionPorte.IsEnabled = true;
        }

        private void checkGrupaje_Unchecked(object sender, RoutedEventArgs e)
        {
            this.cComponenteTarifa.IsEnabled = true;
            this.gridInformacionPorte.IsEnabled = false;
        }

        private void selectorTipoMapa_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.selectorTipoMapa.Value == 1.0)
            {
                this.mapa.Mode = new AerialMode(true);
            }
            else
            {
                this.mapa.Mode = new RoadMode();
            }
        }

        private void bGenerarResumenFinal_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(this.gridInfoTransporte))
            {
                if (UtilidadesVerificacion.validadorFechas(this.tFechaTransporte.Text) && UtilidadesVerificacion.validadorNumeroDecimal(this.tPrecioFinal.Text) && UtilidadesVerificacion.validadorComboBox(this.cCliente) && MessageBox.Show("¿Desea añadir el resumen?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    long salidaInsertarResumenPrevio = this.insertarResumenPrevio();
                    if (salidaInsertarResumenPrevio != -1)
                    {
                        VentanaGestionResumenesPrevio vResumenPrevio = new VentanaGestionResumenesPrevio(true);
                        this.resumenPrevio.id = salidaInsertarResumenPrevio;
                        vResumenPrevio.resumen = this.resumenPrevio;
                        vResumenPrevio.Show();
                        vResumenPrevio.MostrarResumenBuscado();
                        this.reiniciarCampos(0);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void bGenerarResumenPrevio_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(this.gridInfoTransporte))
            {
                if (UtilidadesVerificacion.validadorFechas(this.tFechaTransporte.Text) && UtilidadesVerificacion.validadorNumeroDecimal(this.tPrecioFinal.Text) && UtilidadesVerificacion.validadorComboBox(this.cCliente) && MessageBox.Show("¿Desea añadir el resumen?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes && this.insertarResumenPrevio() != -1)
                {
                    this.reiniciarCampos(0);
                }
            }
            else
            {
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void tDireccion_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.GetLocation(this.tDireccion.Text);
            }
        }
    }
}
