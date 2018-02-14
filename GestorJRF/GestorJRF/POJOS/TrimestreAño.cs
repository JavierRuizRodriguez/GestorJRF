using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorJRF.POJOS
{
    internal class TrimestreAño
    {
        private int trimestre
        {
            get;
            set;
        }

        private int año
        {
            get;
            set;
        }

        public TrimestreAño(int trimestre, int año)
        {
            this.trimestre = trimestre;
            this.año = año;
        }
    }
}
