using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace GestorJRF
{
    public partial class Principal : Application
    {
        void Principal_startUp(object sender, StartupEventArgs e)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            VentanaLogin.getInstancia().Show();
        }
    }
}
