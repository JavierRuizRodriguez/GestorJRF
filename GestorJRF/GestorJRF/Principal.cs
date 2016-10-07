using System.Windows;

namespace GestorJRF
{
    public partial class Principal : Application
    {
        private VentanaLogin vLogin;

        void Principal_startUp(object sender, StartupEventArgs e)
        {
            vLogin = VentanaLogin.getInstancia();
            vLogin.Show();
        }
    }
}
