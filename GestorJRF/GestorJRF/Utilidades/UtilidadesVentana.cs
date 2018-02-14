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
            switch (tipo)
            {
                case 0:
                    ventana.Left = screenWidth / 2.0 - windowWidth / 2.0;
                    ventana.Top = screenHeight / 3.0 - windowHeight / 2.0;
                    break;
                case 1:
                    ventana.Left = screenWidth / 2.0 - windowWidth / 2.0;
                    ventana.Top = screenHeight / 2.5 - windowHeight / 2.0;
                    break;
                case 2:
                    ventana.Left = 0.0;
                    ventana.Top = 0.0;
                    break;
                default:
                    ventana.Left = screenWidth - ventana.Width;
                    ventana.Top = screenHeight - ventana.Height - 30.0;
                    break;
            }
        }

        internal static bool ComprobarCampos(Grid grid)
        {
            bool todosRellenos = true;
            foreach (UIElement child in grid.Children)
            {
                if (child.GetType() == typeof(TextBox))
                {
                    TextBox txt2 = (TextBox)child;
                    if (txt2.Text.Equals("") && txt2.IsEnabled)
                    {
                        todosRellenos = false;
                    }
                }
                else if (child.GetType() == typeof(PasswordBox))
                {
                    PasswordBox txt = (PasswordBox)child;
                    if (txt.Password.Equals("") && txt.IsEnabled)
                    {
                        todosRellenos = false;
                    }
                }
            }
            return todosRellenos;
        }

        internal static void LimpiarCampos(Grid grid)
        {
            foreach (UIElement child in grid.Children)
            {
                if (child.GetType() == typeof(TextBox) || child.GetType() == typeof(TextBlock))
                {
                    TextBox txt = (TextBox)child;
                    txt.Text = "";
                }
            }
        }

        internal static void LimpiarCamposHabilitados(Grid grid)
        {
            foreach (UIElement child in grid.Children)
            {
                if (child.GetType() == typeof(TextBox))
                {
                    TextBox txt = (TextBox)child;
                    if (txt.IsEnabled)
                    {
                        txt.Text = "";
                    }
                }
            }
        }

        internal static object generarEtiquetaFormatoColumna(string etiqueta)
        {
            List<int> idMayusclas = new List<int>();
            for (int x = 0; x <= etiqueta.Length - 1; x++)
            {
                if (char.IsUpper(etiqueta[x]))
                {
                    idMayusclas.Add(x);
                }
            }
            int contador = 0;
            foreach (int item in idMayusclas)
            {
                string p3 = etiqueta.Substring(0, item + contador) + " ";
                string p2 = etiqueta.Substring(item + contador);
                etiqueta = p3 + p2;
                contador++;
            }
            return etiqueta.ToUpper();
        }
    }
}