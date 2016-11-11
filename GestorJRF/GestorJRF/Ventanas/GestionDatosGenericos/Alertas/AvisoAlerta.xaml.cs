using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Alertas
{
    /// <summary>
    /// Lógica de interacción para AvisoAlerta.xaml
    /// </summary>
    public partial class AvisoAlerta : Window
    {
        private List<AlertaKM> alertasKM;
        private List<AlertaFecha> alertasFechas;

        public AvisoAlerta()
        {
            InitializeComponent();

            UtilidadesVentana.SituarVentana(3, this);

            alertasKM = AlertasCRUD.cogerTodasAlertasKMenKMs().Cast<AlertaKM>().ToList();
            alertasFechas = AlertasCRUD.cogerTodasAlertasFechaEnFecha().Cast<AlertaFecha>().ToList();

            hipervinculo.Inlines.Clear();
            hipervinculo.Inlines.Add("Hay " + (alertasKM.Count + alertasFechas.Count) + " aviso/s");
        }

        private void hipervinculo_Click(object sender, RoutedEventArgs e)
        {
            if ((alertasKM.Count + alertasFechas.Count) > 0)
            {
                new VistaAvisoAlerta(alertasFechas, alertasKM).Show();
                Close();
            }
        }
    }
}
