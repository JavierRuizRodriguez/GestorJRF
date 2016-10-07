using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas.Genericas;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using GestorJRF.Ventanas.GestionDatosGenericos.Camiones;
using GestorJRF.Ventanas.GestionDatosGenericos.Empleados;
using GestorJRF.Ventanas.GestionDatosGenericos.Tarifas;
using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Ventanas.GestionDatosGenericos
{
    /// <summary>
    /// Lógica de interacción para VentanaBusqueda.xaml
    /// </summary>
    public partial class VentanaBusqueda : Window
    {
        private VistaCamiones vCamionPadre;
        private VistaEmpresas vEmpresaPadre;
        private VistaEmpleados vEmpleadoPadre;
        private VistaTarifas vTarifaPadre;
        private VistaAlertas vAlertaPadre;
        private string tipo;

        public VentanaBusqueda(Window ventanaPadre, string tipoBusqueda)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(this);
            this.tipo = tipoBusqueda;

            switch (tipo)
            {
                case "camion":
                    vCamionPadre = (VistaCamiones)ventanaPadre;
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "NÚMERO DE MATRICULA");
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "NÚMERO DE BASTIDOR");
                    cBusqueda.SelectedIndex = 0;
                    break;
                case "empleado":
                    vEmpleadoPadre = (VistaEmpleados)ventanaPadre;
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "DNI");
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "APELLIDOS, NOMBRE");
                    cBusqueda.SelectedIndex = 0;
                    break;
                case "empresa":
                    vEmpresaPadre = (VistaEmpresas)ventanaPadre;
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "NOMBRE");
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "CIF/NIF");
                    cBusqueda.SelectedIndex = 0;
                    break;
                case "tarifa":
                    vTarifaPadre = (VistaTarifas)ventanaPadre;
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "TIPO DE TARIFA [CLIENTE/GENERAL]");
                    cBusqueda.SelectedIndex = 0;
                    break;
                case "alerta":
                    vAlertaPadre = (VistaAlertas)ventanaPadre;
                    break;
                default:
                    break;
            }
        }

        private void bListadoCompleto_Click(object sender, RoutedEventArgs e)
        {
            new VistaCompletaTabla(tipo).Show();
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {

            switch (tipo)
            {
                case "camion":
                    Camion camion;
                    if (cBusqueda.SelectedValue.Equals("NÚMERO DE MATRICULA"))
                        camion = (Camion)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerCamionPorMatricula", tBusqueda.Text);
                    else
                        camion = (Camion)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerCamionPorBastidor", tBusqueda.Text);

                    if (camion != null)
                    {
                        vCamionPadre.camion = camion;
                        vCamionPadre.MostrarCamionBuscado();
                        Close();
                    }   
                    else
                        MessageBox.Show("El camión buscado no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;

                case "empleado":
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "DNI");
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "APELLIDOS, NOMBRE");
                    cBusqueda.SelectedIndex = 0;
                    break;

                case "empresa":
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "NOMBRE");
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "CIF/NIF");
                    cBusqueda.SelectedIndex = 0;
                    break;

                case "tarifa":
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "TIPO DE TARIFA [CLIENTE/GENERAL]");
                    cBusqueda.SelectedIndex = 0;
                    break;

                case "alerta":
                    break;

                default:
                    break;
            }
        }
    }
}
