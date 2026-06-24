using System;
using System.Collections.Generic;

namespace Restaurante.Core
{
    public class PlatoACarta : Plato
    {
        public string NombrePlato { get; set; }

        // Lista dinámica de platos a la carta
        public static List<string> Platos { get; set; } = new List<string> { "Lomo Saltado", "Churrazco", "Chuleta", "Milanesa", "Ceviche Mixto", "Chicharrón Pescado" };

        // Precio fijo para toda la categoría de platos A la Carta
        public static double PrecioCategoriaCarta { get; set; } = 15.0;

        public PlatoACarta(string nombrePlato)
        {
            NombrePlato = nombrePlato;
            Precio = PrecioCategoriaCarta; // Asigna el precio general de la categoría
        }

        public override string ObtenerDescripcion()
        {
            return "A la Carta (" + NombrePlato + ") - S/" + PrecioCategoriaCarta.ToString("0.00");
        }
    }
}