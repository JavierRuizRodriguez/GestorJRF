using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestorJRF.Ventanas.Genericas
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class VistaCompletaTabla : Window
    {
        public VistaCompletaTabla(string tipo)
        {
            InitializeComponent();

            switch (tipo)
            {
                case "camion":
                    crearColumnasCamion();
                    IList camiones = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosCamiones", null);
                    foreach (Camion camion in camiones)
                        tabla.Items.Add(camion);
                    break;
                case "empleado":
                    crearColumnasEmpleado();
                    break;
                case "empresa":
                    crearColumnasEmpresa();
                    break;
                case "tarifa":
                    crearColumnasTarifa();
                    break;
                case "alerta":
                    crearColumnasAlerta();
                    break;
                default:
                    break;
            }
        }

        private void tabla_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            var currentCell = e.ClipboardRowContent[tabla.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }

        private void crearColumnasCamion()
        {
            DataGridTextColumn columna = new DataGridTextColumn();

            columna.Header = "MARCA";
            columna.Binding = new Binding("marca");
            columna.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            tabla.Columns.Add(columna);

            columna = new DataGridTextColumn();
            columna.Header = "MODELO";
            columna.Binding = new Binding("modelo");
            columna.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            tabla.Columns.Add(columna);

            columna = new DataGridTextColumn();
            columna.Header = "MATRICULA";
            columna.Binding = new Binding("matricula");
            columna.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            tabla.Columns.Add(columna);

            columna = new DataGridTextColumn();
            columna.Header = "Nº BASTIDOR";
            columna.Binding = new Binding("nBastidor");
            columna.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            tabla.Columns.Add(columna);

            columna = new DataGridTextColumn();
            columna.Header = "LARGO CAJA (m)";
            columna.Binding = new Binding("largoCaja");
            columna.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            tabla.Columns.Add(columna);

            columna = new DataGridTextColumn();
            columna.Header = "LARGO VEHÍCULO (m)";
            columna.Binding = new Binding("largoVehiculo");
            columna.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            tabla.Columns.Add(columna);

            columna = new DataGridTextColumn();
            columna.Header = "KILOMETRAJE";
            columna.Binding = new Binding("kilometraje");
            columna.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            tabla.Columns.Add(columna);

            columna = new DataGridTextColumn();
            columna.Header = "GÁLIBO";
            columna.Binding = new Binding("galibo");
            columna.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            tabla.Columns.Add(columna);

            columna = new DataGridTextColumn();
            columna.Header = "TIPO COMBUSTIBLE";
            columna.Binding = new Binding("tipoCombustible");
            columna.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            tabla.Columns.Add(columna);
        }

        private void crearColumnasEmpleado()
        {

        }

        private void crearColumnasEmpresa()
        {

        }

        private void crearColumnasTarifa()
        {

        }

        private void crearColumnasAlerta()
        {

        }
    }
}
