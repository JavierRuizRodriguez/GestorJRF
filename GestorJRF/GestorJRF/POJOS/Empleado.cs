using System;

namespace GestorJRF.POJOS
{
    public class Empleado
    {
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string dni { get; set; }
        private string dniAntiguo { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public DateTime fechaAlta { get; set; }
        public double sueldoBruto { get; set; }
        public int telefono { get; set; }
        public string email { get; set; }

        public Empleado() { }

        public Empleado(string nombre, string apellidos, string dni, DateTime fechaNacimiento, DateTime fechaAlta, double sueldoBruto, int telefono, string email)
        {
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.dni = dni;
            this.fechaNacimiento = fechaNacimiento.Date;
            this.fechaAlta = fechaAlta.Date;
            this.sueldoBruto = sueldoBruto;
            this.telefono = telefono;
            this.email = email;
        }

        public Empleado(string nombre, string apellidos, string dni, string dniAntiguo, DateTime fechaNacimiento, DateTime fechaAlta, double sueldoBruto, int telefono, string email)
        {
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.dni = dni;
            this.dniAntiguo = dniAntiguo;
            this.fechaNacimiento = fechaNacimiento.Date;
            this.fechaAlta = fechaAlta.Date;
            this.sueldoBruto = sueldoBruto;
            this.telefono = telefono;
            this.email = email;
        }

        public string getNombreApellidos()
        {
            return nombre + " " + apellidos;
        }
    }
}
