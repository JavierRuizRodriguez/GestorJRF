using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

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
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(3, this);
            this.calcularAvisos();
        }

        private void calcularAvisos()
        {
            this.alertasKM = new List<AlertaKM>(AlertasCRUD.cogerTodasAlertasKMenKMs());
            this.alertasFechas = AlertasCRUD.cogerTodasAlertasFechaEnFecha().Cast<AlertaFecha>().ToList();
            this.hipervinculo.Inlines.Clear();
            this.hipervinculo.Inlines.Add("Hay " + (this.alertasKM.Count + this.alertasFechas.Count) + " aviso/s");
        }

        private void hipervinculo_Click(object sender, RoutedEventArgs e)
        {
            if (this.alertasKM.Count + this.alertasFechas.Count > 0)
            {
                new VistaAvisoAlerta(this.alertasFechas, this.alertasKM).Show();
                base.Close();
            }
        }

        private void bActualizar_Click(object sender, RoutedEventArgs e)
        {
            this.calcularAvisos();
        }
    }
}
