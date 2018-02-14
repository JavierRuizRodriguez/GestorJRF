namespace GestorJRF.POJOS
{
    class DobleCadena
    {
        public string cadena1
        {
            get;
            set;
        }

        public string cadena2
        {
            get;
            set;
        }

        public DobleCadena()
        {
        }

        public DobleCadena(string cadena1, string cadena2)
        {
            this.cadena1 = cadena1;
            this.cadena2 = cadena2;
        }
    }
}
