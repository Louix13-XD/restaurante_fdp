using System;
using System.Collections.Generic;

namespace Restaurante.Core
{
    public class Bebida
    {
        //Atributos 
        public string Nombre { get; set; }
        public double Precio { get; set; }

        //Creamos listas dinámicas paralelas de los precios y nombres de la bebida
        public static List<string> Nombres { get; set; } = new List<string> { "Agua", "Gaseosa", "Refresco" };
        public static List<double> Precios { get; set; } = new List<double> { 2.0, 4.0, 3.0 };

        //Constructor
        public Bebida(string nombre, double precio)
        {
            Nombre = nombre;
            Precio = precio;
        }

        //Funcion que lista la bebida y su precio
        public string ObtenerDescripcion()
        {
            return Nombre + " - S/" + Precio.ToString("0.00");
        }
    }
}