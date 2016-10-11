using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Utilidades
{
    class UtilidadesVentana
    {
        internal static void SituarVentana(Window ventana)
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = ventana.Width;
            double windowHeight = ventana.Height;
            ventana.Left = (screenWidth / 2) - (windowWidth / 2);
            ventana.Top = (screenHeight / 3) - (windowHeight / 2);
        }

        internal static bool ComprobarCampos(Grid grid)
        {
            var todosRellenos = true;

            foreach (UIElement elemento in grid.Children)
            {
                if (elemento.GetType() == typeof(TextBox))
                {
                    TextBox txt = (TextBox)elemento;
                    if (txt.Text.Equals("")) todosRellenos = false;
                }
            }

            return todosRellenos;
        }

        internal static void LimpiarCampos(Grid grid)
        {
            foreach (UIElement elemento in grid.Children)
            {
                if (elemento.GetType() == typeof(TextBox))
                {
                    TextBox txt = (TextBox)elemento;
                    txt.Text = "";
                }
            }
        }


    }
}
