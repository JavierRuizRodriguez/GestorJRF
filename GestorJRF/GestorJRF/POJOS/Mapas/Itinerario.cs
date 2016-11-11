using System.ComponentModel;

namespace GestorJRF.POJOS.Mapas
{
    public class Itinerario : INotifyPropertyChanged
    {
        public long id { get; set; }
        private char _punto;
        public char punto
        {
            get { return _punto; }
            set
            {
                if (value != _punto)
                {
                    _punto = value;
                    OnPropertyChanged("Punto");
                }
            }
        }
        public string direccion { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public long idResumen { get; set; }
        public bool esEtapa { get; set; }
        private string _dni { get; set; }
        public string dni
        {
            get
            {
                return _dni;
            }
            set
            {
                if (value != _dni)
                {
                    _dni = value;
                    OnPropertyChanged("Dni");
                }
            }
        }
        private string _matricula { get; set; }
        public string matricula
        {
            get
            {
                return _matricula;
            }
            set
            {
                if (value != _matricula)
                {
                    _matricula = value;
                    OnPropertyChanged("Matricula");
                }
            }
        }
        private long _kilometrosVehiculo { get; set; }
        public long kilometrosVehiculo
        {
            get
            {
                return _kilometrosVehiculo;
            }
            set
            {
                if (value != _kilometrosVehiculo)
                {
                    _kilometrosVehiculo = value;
                    OnPropertyChanged("KilometrosVehiculo");
                }
            }
        }

        public Itinerario() { }

        public Itinerario(char punto, string direccion, double latitud, double longitud, bool esEtapa)
        {
            this.punto = punto;
            this.direccion = direccion;
            this.latitud = latitud;
            this.longitud = longitud;
            this.esEtapa = esEtapa;
        }

        public Itinerario(char punto, string direccion, double latitud, double longitud, bool esEtapa, string dni, string matricula, long kilometrosVehiculo)
        {
            this.punto = punto;
            this.direccion = direccion;
            this.latitud = latitud;
            this.longitud = longitud;
            this.esEtapa = esEtapa;
            this.dni = dni;
            this.matricula = matricula;
            this.kilometrosVehiculo = kilometrosVehiculo;
        }

        public Itinerario(long id, char punto, string direccion, long idResumen, bool esEtapa, string dni, string matricula, long kilometrosVehiculo)
        {
            this.id = id;
            this.punto = punto;
            this.direccion = direccion;
            this.idResumen = idResumen;
            this.esEtapa = esEtapa;
            this.dni = dni;
            this.matricula = matricula;
            this.kilometrosVehiculo = kilometrosVehiculo;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string punto)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(punto));
            }
        }
    }
}
