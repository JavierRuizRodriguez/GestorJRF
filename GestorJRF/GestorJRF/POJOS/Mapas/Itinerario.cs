using System.ComponentModel;

namespace GestorJRF.POJOS.Mapas
{
    public class Itinerario : INotifyPropertyChanged
    {
        private char _punto;

        public long id
        {
            get;
            set;
        }

        public char punto
        {
            get
            {
                return this._punto;
            }
            set
            {
                if (value != this._punto)
                {
                    this._punto = value;
                    this.OnPropertyChanged("Punto");
                }
            }
        }

        public string direccion
        {
            get;
            set;
        }

        public double latitud
        {
            get;
            set;
        }

        public double longitud
        {
            get;
            set;
        }

        public long idResumen
        {
            get;
            set;
        }

        public bool esEtapa
        {
            get;
            set;
        }

        private string _dni
        {
            get;
            set;
        }

        public string dni
        {
            get
            {
                return this._dni;
            }
            set
            {
                if (value != this._dni)
                {
                    this._dni = value;
                    this.OnPropertyChanged("Dni");
                }
            }
        }

        private string _matricula
        {
            get;
            set;
        }

        public string matricula
        {
            get
            {
                return this._matricula;
            }
            set
            {
                if (value != this._matricula)
                {
                    this._matricula = value;
                    this.OnPropertyChanged("Matricula");
                }
            }
        }

        private string _clienteDeCliente
        {
            get;
            set;
        }

        public string clienteDeCliente
        {
            get
            {
                return this._clienteDeCliente;
            }
            set
            {
                if (value != this._clienteDeCliente)
                {
                    this._clienteDeCliente = value;
                    this.OnPropertyChanged("ClienteDeCliente");
                }
            }
        }

        public string poblacion
        {
            get;
            set;
        }

        public int palets
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Itinerario()
        {
        }

        public Itinerario(char punto, string direccion, double latitud, double longitud, bool esEtapa, string poblacion)
        {
            this.punto = punto;
            this.direccion = direccion;
            this.latitud = latitud;
            this.longitud = longitud;
            this.esEtapa = esEtapa;
            this.poblacion = poblacion;
        }

        public Itinerario(char punto, string direccion, double latitud, double longitud, bool esEtapa, string dni, string matricula, string poblacion, string clienteDeCliente, int palets)
        {
            this.punto = punto;
            this.direccion = direccion;
            this.latitud = latitud;
            this.longitud = longitud;
            this.esEtapa = esEtapa;
            this.dni = dni;
            this.matricula = matricula;
            this.clienteDeCliente = clienteDeCliente;
            this.palets = palets;
        }

        public Itinerario(long id, char punto, string direccion, long idResumen, bool esEtapa, string dni, string matricula, string poblacion, string clienteDeCliente, int palets)
        {
            this.id = id;
            this.punto = punto;
            this.direccion = direccion;
            this.idResumen = idResumen;
            this.esEtapa = esEtapa;
            this.dni = dni;
            this.matricula = matricula;
            this.clienteDeCliente = clienteDeCliente;
            this.palets = palets;
        }

        protected void OnPropertyChanged(string punto)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(punto));
            }
        }
    }
}
