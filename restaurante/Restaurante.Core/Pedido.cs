using System;
using System.Collections.Generic;

namespace Restaurante.Core
{
    public class Pedido
    {
        public int NumeroPedido { get; set; }
        public string Cliente { get; set; }
        public List<Plato> Platos { get; set; } = new List<Plato>();
        public List<Bebida> Bebidas { get; set; } = new List<Bebida>();
        public string MetodoPago { get; set; }

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
