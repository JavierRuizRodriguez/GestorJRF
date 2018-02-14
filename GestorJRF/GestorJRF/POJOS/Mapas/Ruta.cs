using System;

namespace GestorJRF.POJOS.Mapas
{
    public class Ruta
    {
        public int id
        {
            get;
            set;
        }

        public double kilometros
        {
            get;
            set;
        }

        public string tiempo
        {
            get;
            set;
        }

        public Ruta(int id, double kilometros, double tiempoSegundos)
        {
            this.id = id;
            this.kilometros = kilometros;
            this.tiempo = TimeSpan.FromSeconds(tiempoSegundos).ToString("h\\h\\ m\\m");
        }
    }
}
