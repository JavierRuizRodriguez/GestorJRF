using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorJRF.POJOS.Estadisticas
{
    public class ComisionesInforme
    {
        public string nombre
        {
            get;
            set;
        }

        public double importe
        {
            get;
            set;
        }

        public ComisionesInforme()
        {
        }

        public ComisionesInforme(string nombre, double importe)
        {
            this.nombre = nombre;
            this.importe = importe;
        }
    }
}
