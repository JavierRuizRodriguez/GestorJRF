using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Utilidades
{
    class UtilidadesVerificacion
    {
        private const string PatronMail = "^(([\\w-]+\\.)+[\\w-]+|([a-zA-Z]{1}|[\\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?\r\n\t\t\t\t        [0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?\r\n\t\t\t\t        [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z0-9]+[\\w-]+\\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

        private const string PatronNumeroEntero = "^\\d{1,10}$";

        private const string PatronNumeroDecimal = "^-?[0-9]+([.][0-9]+)?$";

        private const string PatronFecha = "^(?:(?:31(\\/|-|\\.)(?:0?[13578]|1[02]))\\1|(?:(?:29|30)(\\/|-|\\.)(?:0?[1,3-9]|1[0-2])\\2))(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$|^(?:29(\\/|-|\\.)0?2\\3(?:(?:(?:1[6-9]|[2-9]\\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\\d|2[0-8])(\\/|-|\\.)(?:(?:0?[1-9])|(?:1[0-2]))\\4(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$";

        public static NumberFormatInfo cogerProveedorDecimal()
        {
            NumberFormatInfo proveedorDecimal = new NumberFormatInfo();
            proveedorDecimal.NumberDecimalSeparator = ".";
            return proveedorDecimal;
        }

        internal static bool validadorMail(string email)
        {
            if (Regex.IsMatch(email, "^(([\\w-]+\\.)+[\\w-]+|([a-zA-Z]{1}|[\\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?\r\n\t\t\t\t        [0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?\r\n\t\t\t\t        [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z0-9]+[\\w-]+\\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$"))
            {
                return true;
            }
            MessageBox.Show("El email introducido no sigue el patrón especificado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            return false;
        }

        internal static bool validadorNumeroEntero(string entero)
        {
            if (Regex.IsMatch(entero, "^\\d{1,10}$"))
            {
                return true;
            }
            MessageBox.Show("El número entero introducido no sigue el patrón especificado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            return false;
        }

        internal static bool validadorNumeroDecimal(string numeroDecimal)
        {
            if (Regex.IsMatch(numeroDecimal, "^-?[0-9]+([.][0-9]+)?$"))
            {
                return true;
            }
            MessageBox.Show("El número decimal introducido no sigue el patrón especificado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            return false;
        }

        internal static bool validadorFechas(string fecha)
        {
            if (Regex.IsMatch(fecha, "^(?:(?:31(\\/|-|\\.)(?:0?[13578]|1[02]))\\1|(?:(?:29|30)(\\/|-|\\.)(?:0?[1,3-9]|1[0-2])\\2))(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$|^(?:29(\\/|-|\\.)0?2\\3(?:(?:(?:1[6-9]|[2-9]\\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\\d|2[0-8])(\\/|-|\\.)(?:(?:0?[1-9])|(?:1[0-2]))\\4(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$"))
            {
                return true;
            }
            MessageBox.Show("La fecha introducida no sigue el patrón especificado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            return false;
        }

        internal static bool validadorComboBox(ComboBox combo)
        {
            if (combo.SelectedIndex == -1)
            {
                MessageBox.Show("Es necesario rellenar todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }
            return true;
        }
    }
}
