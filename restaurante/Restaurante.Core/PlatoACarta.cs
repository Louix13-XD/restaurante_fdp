using System;
using System.Collections.Generic;

namespace Restaurante.Core
{
    //Heredamos de la superclase Plato  
    public class PlatoACarta : Plato
    {
        //Propiedades de la clase PlatoACarta
        public string NombrePlato { get; set; }

        //Creamos una lista dinámica de platos a la carta
        public static List<string> Platos { get; set; } = 
            new List<string> { "Lomo Saltado", "Churrazco", "Chuleta", "Milanesa", "Ceviche Mixto", "Chicharrón Pescado" };

        //Asigamos un precio fijo para toda la categoría de platos A la Carta
        public static double PrecioCategoriaCarta { get; set; } = 15.0;

        //Constructor
        public PlatoACarta(string nombrePlato)
        {
            NombrePlato = nombrePlato;
            Precio = PrecioCategoriaCarta; //Asigna el precio general de la categoría
        }

        //Lista el nombre y precio del plato a la carta
        public override string ObtenerDescripcion()
        {
            return "A la Carta (" + NombrePlato + ") - S/" + PrecioCategoriaCarta.ToString("0.00");
        }
    }
}