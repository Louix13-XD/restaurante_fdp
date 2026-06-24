using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante.Core
{
    public class Bebida
    {
        public string Nombre { get; set; }
        public double Precio { get; set; }

        public static string[] Nombres = { "Agua", "Gaseosa", "Refresco" };
        public static double[] Precios = { 2.0, 4.0, 3.0 };

        public Bebida(string nombre, double precio) // Constructor
        {
            Nombre = nombre;
            Precio = precio;
        }

        public string ObtenerDescripcion()
        {
            return Nombre + " - S/" + Precio.ToString("0.00");
        }
    }
}
