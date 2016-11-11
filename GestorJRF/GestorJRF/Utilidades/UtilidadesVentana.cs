using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Utilidades
{
    class UtilidadesVentana
    {
        internal static void SituarVentana(int tipo, Window ventana)
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = ventana.Width;
            double windowHeight = ventana.Height;

            if (tipo == 0)
            {
                ventana.Left = (screenWidth / 2) - (windowWidth / 2);
                ventana.Top = (screenHeight / 3) - (windowHeight / 2);
            }
            else if(tipo == 1)
            {
                ventana.Left = (screenWidth / 2) - (windowWidth / 2);
                ventana.Top = (screenHeight / 2.5) - (windowHeight / 2);
            }
            else if(tipo == 2)
            {
                ventana.Left = 0;
                ventana.Top = 0;
            }
            else
            {
                ventana.Left = screenWidth - ventana.Width;
                ventana.Top = screenHeight - ventana.Height - 30;
            }
        }

        internal static bool ComprobarCampos(Grid grid)
        {
            var todosRellenos = true;

            foreach (UIElement elemento in grid.Children)
            {
                if (elemento.GetType() == typeof(TextBox))
                {
                    TextBox txt = (TextBox)elemento;
                    if (txt.Text.Equals("") && txt.IsEnabled)
                        todosRellenos = false;
                }
            }

            return todosRellenos;
        }

        internal static void LimpiarCampos(Grid grid)
        {
            foreach (UIElement elemento in grid.Children)
            {
                if (elemento.GetType() == typeof(TextBox) || elemento.GetType() == typeof(TextBlock))
                {
                    TextBox txt = (TextBox)elemento;
                    txt.Text = "";
                }
            }
        }

        internal static void LimpiarCamposHabilitados(Grid grid)
        {
            foreach (UIElement elemento in grid.Children)
            {
                if (elemento.GetType() == typeof(TextBox))
                {
                    TextBox txt = (TextBox)elemento;
                    if(txt.IsEnabled)
                        txt.Text = "";
                }
            }
        }

        internal static object generarEtiquetaFormatoColumna(string etiqueta)
        {
            List<int> idMayusclas = new List<int>();
            for(int x=0;x<=etiqueta.Length - 1;x++)
            {
                if (Char.IsUpper(etiqueta[x]))
                    idMayusclas.Add(x);
            }

            int contador = 0;
            foreach(int indice in idMayusclas)
            {
                var p1 = etiqueta.Substring(0, indice + contador) + " ";
                var p2 = etiqueta.Substring(indice + contador);
                etiqueta = p1 + p2; 
                contador++;
            }

            return etiqueta.ToUpper();
        }
    }
}
