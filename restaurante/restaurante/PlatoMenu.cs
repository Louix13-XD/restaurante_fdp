using System;
using System.Collections.Generic;
using System.Text;

namespace restaurante
{
    public class PlatoMenu : Plato // Subclase que hereda e implementa los métodos de su clase padre
    {
        public string Entrada { get; set; }
        public string Segundo { get; set; }

        public static string[] Entradas =
            { "Papa Huancaina", "Sopa", "Ceviche" };

        public static string[] Segundos =
            { "Tallarines Rojos", "Tallarines Verdes", "Arroz Verde",
              "Arroz Cubana", "Menestra con Arroz", "Cabrito",
              "Aji de Gallina", "Olluco" };

        public PlatoMenu(string entrada, string segundo) // Constructor
        {
            Entrada = entrada;
            Segundo = segundo;
            Precio = 10.0;
        }

        public override string ObtenerDescripcion() // Implementamos el método abstracto de la clase padre
        {
            return "Menú (Entrada: " + Entrada + " + " + Segundo + ") - S/" + Precio.ToString("0.00");
        }
    }
}
