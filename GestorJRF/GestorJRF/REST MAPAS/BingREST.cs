using GestorJRF.POJOS.Mapas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;

namespace GestorJRF.REST_MAPAS
{
    public class BingREST
    {
        private const string CABECERA_RUTA_REST_1 = @"http://dev.virtualearth.net/REST/V1/Routes/Driving?";
        private const string CABECERA_RUTA_REST_2 = @"&maxSolns=3&rpo=Points&key=";
        
        public static void Route(List<Itinerario> itinerario, List<string> opcionesCarretera, string key, Action<Response> callback)
        {
            Uri requestURI = new Uri(generarURL(itinerario, opcionesCarretera, key));
            GetResponse(requestURI, callback);
        }

        public static void Location(string lugar, string key, Action<Response> callback)
        {
            //Uri requestURI = new Uri(string.Format("http://dev.virtualearth.net/REST/V1/Locations?countryRegion=Espa%C3%B1a&locality={0}&maxResults=1&key={1}", Uri.EscapeDataString(lugar), key));
            Uri requestURI = new Uri(string.Format("http://dev.virtualearth.net/REST/V1/Locations?q={0}?&maxRes=1&c=es&key={1}", Uri.EscapeDataString(lugar), key));
            GetResponse(requestURI, callback);
        }

        private static string generarURL(List<Itinerario> itinerario, List<string> opcionesCarretera, string key)
        {
            string url = CABECERA_RUTA_REST_1;
            int contador = 0;
            foreach (Itinerario lugar in itinerario)
            {
                if (contador != 0)
                    url += @"&";
                url += @"wp." + contador + "=" + Convert.ToString(lugar.latitud).Replace(",",".") + "," + Convert.ToString(lugar.longitud).Replace(",", ".");
                contador++;
            }

            if (opcionesCarretera.Count > 0)
                url += @"&avoid=" + opcionesCarretera.Aggregate((i, j) => i + ',' + j);

            url += CABECERA_RUTA_REST_2 + key;
            return url;
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
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));

                        if (callback != null)
                        {
                            callback(ser.ReadObject(stream) as Response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Alguna de las direcciones no se encuentra.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
            }
        }
    }
}
