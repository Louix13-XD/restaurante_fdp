using System;
using System.Collections.Generic;

namespace Restaurante.Core
{
    public class PlatoMenu : Plato
    {
        public string Entrada { get; set; }
        public string Segundo { get; set; }

        // Listas dinámicas editables
        public static List<string> Entradas { get; set; } = new List<string> { "Papa Huancaina", "Sopa", "Ceviche" };
        public static List<string> Segundos { get; set; } = new List<string> { "Tallarines Rojos", "Tallarines Verdes", "Arroz Verde", "Arroz Cubana", "Menestra con Arroz", "Cabrito", "Aji de Gallina", "Olluco" };

        // Precio fijo para toda la categoría Menú
        public static double PrecioCategoriaMenu { get; set; } = 10.0;

        public PlatoMenu(string entrada, string segundo)
        {
            Entrada = entrada;
            Segundo = segundo;
            Precio = PrecioCategoriaMenu; // Asigna el precio general de la categoría
        }

        public override string ObtenerDescripcion()
        {
            return "Menú (Entrada: " + Entrada + " + " + Segundo + ") - S/" + PrecioCategoriaMenu.ToString("0.00");
        }
    }
}
