using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorJRF.POJOS.Facturas
{
    public class OperacionesTerceros
    {        
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string cif { get; set; }
        public double t1 { get; set; }
        public double t2 { get; set; }
        public double t3 { get; set; }
        public double t4 { get; set; }
        public double tTotal { get; set; }

        public OperacionesTerceros(string nombre, string direccion, string cif, double t1, double t2, double t3, double t4, double tTotal)
        {
            this.nombre = nombre;
            this.direccion = direccion;
            this.cif = cif;
            this.t1 = t1;
            this.t2 = t2;
            this.t3 = t3;
            this.t4 = t4;
            this.tTotal = tTotal;
        }
    }
}
