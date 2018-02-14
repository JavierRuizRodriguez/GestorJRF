namespace GestorJRF.REST_MAPAS
{
    using GestorJRF.POJOS.Mapas;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization.Json;
    using System.Windows;

    public class BingREST
    {
        private const string CABECERA_RUTA_REST_1 = "http://dev.virtualearth.net/REST/V1/Routes/Driving?";
        private const string CABECERA_RUTA_REST_2 = "&maxSolns=3&rpo=Points&key=";

        private static string generarURL(List<Itinerario> itinerario, List<string> opcionesCarretera, string key)
        {
            string str = "http://dev.virtualearth.net/REST/V1/Routes/Driving?";
            int num = 0;
            foreach (Itinerario itinerario2 in itinerario)
            {
                if (num > 0)
                {
                    str = str + "&";
                }
                object[] objArray1 = new object[] { str, "wp.", num, "=", Convert.ToString(itinerario2.latitud).Replace(",", "."), ",", Convert.ToString(itinerario2.longitud).Replace(",", ".") };
                str = string.Concat(objArray1);
                num++;
            }
            if (opcionesCarretera.Count > 0)
            {
                str = str + "&avoid=" + opcionesCarretera.Aggregate<string>((i, j) => (i + "," + j));
            }
            return (str + "&maxSolns=3&rpo=Points&key=" + key);
        }

        private static void GetResponse(Uri uri, Action<Response> callback)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Response));
                        if (callback != null)
                        {
                            callback(serializer.ReadObject(stream) as Response);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Alguna de las direcciones no se encuentra.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(exception.ToString());
            }
        }

        public static void Location(string lugar, string key, Action<Response> callback)
        {
            Uri uri = new Uri($"http://dev.virtualearth.net/REST/V1/Locations?countryRegion=Espa%C3%B1a&locality={Uri.EscapeDataString(lugar)}&maxResults=1&key={key}");
            GetResponse(uri, callback);
        }

        public static void Route(List<Itinerario> itinerario, List<string> opcionesCarretera, string key, Action<Response> callback)
        {
            Uri uri = new Uri(generarURL(itinerario, opcionesCarretera, key));
            GetResponse(uri, callback);
        }        
    }
}

