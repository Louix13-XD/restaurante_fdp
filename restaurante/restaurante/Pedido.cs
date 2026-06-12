using System;
using System.Collections.Generic;
using System.Text;

namespace restaurante
{
    public class Pedido
    {
        public int NumeroPedido { get; set; }
        public string Cliente { get; set; }
        public List<Plato> Platos { get; set; } = new List<Plato>(); // Lista de platos generales (menú o a la carta)
        public List<Bebida> Bebidas { get; set; } = new List<Bebida>(); // Lista de bebidas

        public double Total() // Sumamos el total de platos y bebidas por pedido
        {
            double totalPlatos = 0;
            double totalBebidas = 0;

            foreach (Plato plato in Platos)
                totalPlatos += plato.Precio;

            foreach (Bebida bebida in Bebidas)
                totalBebidas += bebida.Precio;

            return totalPlatos + totalBebidas;
        }
        public string MetodoPago { get; set; }
    }
}