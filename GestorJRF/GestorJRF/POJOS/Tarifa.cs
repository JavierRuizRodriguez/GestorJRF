using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GestorJRF.POJOS
{
    public class Tarifa
    {
        public string nombreTarifa { get; set; }
        public string nombreTarifaAntiguo { get; set; }
        public string nombreEmpresa { get; set; }
        public List<ComponenteTarifa> listaComponentesTarifa;

        public Tarifa() { }

        public Tarifa(string nombreTarifa, string nombreEmpresa, ObservableCollection<ComponenteTarifa> listaComponentesTarifa)
        {
            this.nombreTarifa = nombreTarifa;
            this.nombreEmpresa = nombreEmpresa;
            this.listaComponentesTarifa = new List<ComponenteTarifa>(listaComponentesTarifa);
        }

        public Tarifa(string nombreTarifa, string nombreTarifaAntiguo, string nombreEmpresa, ObservableCollection<ComponenteTarifa> listaComponentesTarifa)
        {
            this.nombreTarifa = nombreTarifa;
            this.nombreTarifaAntiguo = nombreTarifaAntiguo;
            this.nombreEmpresa = nombreEmpresa;
            this.listaComponentesTarifa = new List<ComponenteTarifa>(listaComponentesTarifa);
        }
    }
}
