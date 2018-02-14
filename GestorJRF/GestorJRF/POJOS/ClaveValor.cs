using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorJRF.POJOS
{
    public class ClaveValor
    {
        public long clave;

        public string valor;

        public ClaveValor(long clave, string valor)
        {
            this.clave = clave;
            this.valor = valor;
        }
    }
}
