using System;
using System.Collections.Generic;

namespace Restaurante.Core
{
    //Heredamos de la super clase Plato
    public class PlatoMenu : Plato
    {
        //Atributos
        public string Entrada { get; set; }
        public string Segundo { get; set; }

        //Creamos listas dinámicas editables
        public static List<string> Entradas { get; set; } 
            = new List<string> { "Papa Huancaina", "Sopa", "Ceviche" };
        public static List<string> Segundos { get; set; }
            = new List<string> { "Tallarines Rojos", "Tallarines Verdes", 
                "Arroz Verde", "Arroz Cubana", "Menestra con Arroz", 
                "Cabrito", "Aji de Gallina", "Olluco" };

        //Asigamos un precio fijo para toda la categoría Menú
        public static double PrecioCategoriaMenu { get; set; } = 10.0;

        //Constructor
        public PlatoMenu(string entrada, string segundo)
        {
            Entrada = entrada;
            Segundo = segundo;
            Precio = PrecioCategoriaMenu; // Asigna el precio general de la categoría
        }

        //Método para obtener la descripción del plato
        public override string ObtenerDescripcion()
        {
            return "Menú (Entrada: " + Entrada + " + " + Segundo + ") - S/" + PrecioCategoriaMenu.ToString("0.00");
        }
    }
}
