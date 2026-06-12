using System;
using System.Collections.Generic;
using System.Text;

namespace restaurante
{
    public class PlatoACarta : Plato
    {
        public string NombrePlato { get; set; }

        public static string[] Platos =
            { "Lomo Saltado", "Churrazco", "Chuleta",
              "Milanesa", "Ceviche Mixto", "Chicharrón Pescado" };

        public PlatoACarta(string nombrePlato) // Constructor
        {
            NombrePlato = nombrePlato;
            Precio = 15.0;
        }

        public override string ObtenerDescripcion() // Implementamos el método abstracto de la clase padre
        {
            return "A la Carta (" + NombrePlato + ") - S/" + Precio.ToString("0.00");
        }
    }
}