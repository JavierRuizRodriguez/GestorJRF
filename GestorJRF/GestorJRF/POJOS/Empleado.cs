using System;

namespace GestorJRF.POJOS
{
    public class Empleado
    {
        private string nombre { get; set; }
        private string apellidos { get; set; }
        private string dni { get; set; }
        private DateTime fechaNacimiento { get; set; }
        private DateTime fechaAlta { get; set; }
        private double sueldoBruto { get; set; }
        private string telefono { get; set; }
        private string mail { get; set; }

        public Empleado() { }

        public Empleado(string nombre, string apellidos, string dni, DateTime fechaNacimiento, DateTime fechaAlta, double sueldoBruto, string telefono, string mail)
        {
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.dni = dni;
            this.fechaNacimiento = fechaNacimiento;
            this.fechaAlta = fechaAlta;
            this.sueldoBruto = sueldoBruto;
            this.telefono = telefono;
            this.mail = mail;
        }
    }
}
