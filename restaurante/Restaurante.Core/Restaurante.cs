// ARCHIVO: Restaurante.cs (Dentro de Restaurante.Core)
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Restaurante.Core
{
    public class GestorRestaurante
    {
        private List<Pedido> listaPedidos = new List<Pedido>();
        private double gananciaTotal = 0;
        private const string ArchivoNombre = "RegistroVentas.txt";

        // Exponemos las listas y ganancias para que la interfaz pueda leerlas
        public List<Pedido> ListaPedidos => listaPedidos;
        public double GananciaTotal => gananciaTotal;

        public unsafe void AcumularGananciaConPuntero(double monto)
        {
            fixed (double* ptrGanancia = &gananciaTotal)
            {
                *ptrGanancia = *ptrGanancia + monto;
            }
        }

        public void AgregarPedido(Pedido p)
        {
            listaPedidos.Add(p);
            AcumularGananciaConPuntero(p.Total());
        }

        public List<Pedido> ObtenerHistorialOrdenado(int criterio)
        {
            List<Pedido> listaCopia = new List<Pedido>(listaPedidos);
            OrdenarInsercion(listaCopia, criterio);
            return listaCopia;
        }

        public void GuardarEnArchivo()
        {
            using (StreamWriter sw = new StreamWriter(ArchivoNombre, false, Encoding.UTF8))
            {
                sw.WriteLine("REPORTE FINAL DEL DÍA");
                sw.WriteLine("Fecha: " + DateTime.Now);
                sw.WriteLine("Total Pedidos: " + listaPedidos.Count);
                sw.WriteLine("Ganancia Total: S/. " + gananciaTotal.ToString("0.00"));
                sw.WriteLine("------------------------------");

                foreach (Pedido p in listaPedidos)
                {
                    sw.WriteLine($"Pedido #{p.NumeroPedido} - {p.Cliente} - {p.MetodoPago} - S/. {p.Total().ToString("0.00")}");
                    foreach (Plato plato in p.Platos)
                        sw.WriteLine("   Plato: " + plato.ObtenerDescripcion());
                    foreach (Bebida bebida in p.Bebidas)
                        sw.WriteLine("   Bebida: " + bebida.ObtenerDescripcion());
                }
            }
        }

        private void OrdenarInsercion(List<Pedido> lista, int criterio)
        {
            int n = lista.Count;
            for (int i = 1; i < n; i++)
            {
                Pedido key = lista[i];
                int j = i - 1;
                while (j >= 0 && Comparar(lista[j], key, criterio) > 0)
                {
                    lista[j + 1] = lista[j];
                    j--;
                }
                lista[j + 1] = key;
            }
        }

        private int Comparar(Pedido a, Pedido b, int criterio)
        {
            switch (criterio)
            {
                case 1: return b.Total().CompareTo(a.Total());
                case 2: return string.Compare(a.Cliente, b.Cliente);
                case 3: return a.NumeroPedido.CompareTo(b.NumeroPedido);
                default: return 0;
            }
        }
    }
}