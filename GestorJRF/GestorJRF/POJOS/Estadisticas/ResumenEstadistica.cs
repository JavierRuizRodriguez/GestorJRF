using GestorJRF.POJOS.Mapas;
using System;
using System.Collections.Generic;

namespace GestorJRF.POJOS.Estadisticas
{
    public class ResumenEstadistica
    {
        public DateTime fechaPorte { get; set; }
        public double sumaDiaria { get; set; }
        public List<Itinerario> itinerarios { get; set; }

        public ResumenEstadistica() { }
    }
}
