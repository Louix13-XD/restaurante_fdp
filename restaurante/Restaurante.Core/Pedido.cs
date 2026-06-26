using System;
using System.Collections.Generic;

namespace Restaurante.Core
{
    //Clase que representa un pedido en el restaurante
    public class Pedido
    {
        //Propiedades de la clase Pedido
        public int NumeroPedido { get; set; }
        public string Cliente { get; set; }
        //Listas de platos y bebidas
        public List<Plato> Platos { get; set; } = new List<Plato>();
        public List<Bebida> Bebidas { get; set; } = new List<Bebida>();
        public string MetodoPago { get; set; }

        //Funcion que calcula el monto total de pedidos (precios de bebidas u platos)
        public double Total()
        {
            double totalPlatos = 0;
            double totalBebidas = 0;

            foreach (Plato plato in Platos)
                totalPlatos += plato.Precio;

            foreach (Bebida bebida in Bebidas)
                totalBebidas += bebida.Precio;

            return totalPlatos + totalBebidas;
        }
    }
}
