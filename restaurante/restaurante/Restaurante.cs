using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace restaurante
{
    public class Restaurante
    {
        // Lista de pedidos (contiene los platos y bebidas)
        private List<Pedido> listaPedidos = new List<Pedido>();
        private double gananciaTotal = 0;
        private const string ArchivoNombre = "RegistroVentas.txt";

        // Función que lista (muestra) los platos y bebidas de las clases
        public string listar(string[] opciones)
        {
            string lista = "";
            for (int i = 0; i < opciones.Length; i++)
                lista += (i + 1) + "." + opciones[i] + " | ";
            return lista;
        }

        public void MostrarMenuPrincipal()
        {
            string opcion = "";
            while (opcion != "5")
            {
                Console.ResetColor();
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("       SISTEMA DE VENTAS - RESTAURANTE  ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Registrar Nuevo Pedido");
                Console.WriteLine("2. Ver Historial de Pedidos");
                Console.WriteLine("3. Ver Resumen de Ganancias");
                Console.WriteLine("4. Guardar Reporte en Archivo");
                Console.WriteLine("5. Salir");
                Console.WriteLine("========================================");
                opcion = LeerCadena("Seleccione una opción: ");

                switch (opcion)
                {
                    case "1": RegistrarPedido(); break;
                    case "2": MostrarHistorial(); break;
                    case "3": MostrarResumen(); break;
                    case "4": GuardarEnArchivo(); break;
                    case "5": Console.WriteLine("Cerrando sistema..."); break;
                    default: Console.WriteLine("Opción no válida."); break;
                }
                if (opcion != "5") { Console.WriteLine("\nPresione cualquier tecla para volver al menú..."); Console.ReadKey(); }
            }
        }

        
        private unsafe void AcumularGananciaConPuntero(double monto)
        {
            // 'fixed' evita que el recolector de basura (Garbage Collector) mueva la variable de sitio
            fixed (double* ptrGanancia = &gananciaTotal)
            {
                // Con el operador '*' accedemos y modificamos el valor apuntado en memoria
                *ptrGanancia = *ptrGanancia + monto;
            }
        }

        private void RegistrarPedido()
        {
            Pedido p = new Pedido { NumeroPedido = listaPedidos.Count + 1 };

            Console.Clear();
            Console.WriteLine("--------- REGISTRO PEDIDO #" + p.NumeroPedido + " -----------");
            p.Cliente = LeerCadena("Nombre del cliente: ");

            int cantPlatos = LeerEntero("Cantidad de platos: ", 1, 50);

            for (int i = 1; i <= cantPlatos; i++)
            {
                Console.WriteLine("\n--------- Plato " + i + " de " + cantPlatos + " --------------");
                Console.WriteLine("1. Menú (S/10) | 2. A la Carta (S/15)");
                int tipo = LeerEntero("Elija el tipo de plato: ", 1, 2);

                if (tipo == 1)
                {
                    Console.WriteLine("\nEntradas: " + listar(PlatoMenu.Entradas));
                    int entrada = LeerEntero("Seleccione entrada: ", 1, PlatoMenu.Entradas.Length);

                    Console.WriteLine("\nSegundos: " + listar(PlatoMenu.Segundos));
                    int segundo = LeerEntero("Seleccione segundo: ", 1, PlatoMenu.Segundos.Length);

                    // Agregamos a la lista de pedidos instancias de platoMenu
                    p.Platos.Add(new PlatoMenu(
                        PlatoMenu.Entradas[entrada - 1],
                        PlatoMenu.Segundos[segundo - 1]
                    ));
                }
                else
                {
                    Console.WriteLine("\nPlatos: " + listar(PlatoACarta.Platos));
                    int carta = LeerEntero("Seleccione plato: ", 1, PlatoACarta.Platos.Length);

                    // Agregamos a la lista de pedidos instancias de plato a la carta
                    p.Platos.Add(new PlatoACarta(PlatoACarta.Platos[carta - 1]));
                }
            }

            Console.WriteLine("\n¿Desea agregar bebidas? 1. Sí | 0. No");
            int quiereBebida = LeerEntero("Opción: ", 0, 1);

            if (quiereBebida == 1)
            {
                int cantBebidas = LeerEntero("¿Cuántas bebidas?: ", 1, 10);
                for (int b = 1; b <= cantBebidas; b++)
                {
                    Console.WriteLine("\nBebida #" + b + ": " + listar(Bebida.Nombres));
                    int tipoB = LeerEntero("Seleccione tipo: ", 1, Bebida.Nombres.Length);
                    // Agregamos a la lista de pedidos instancias de bebidas
                    p.Bebidas.Add(new Bebida(Bebida.Nombres[tipoB - 1], Bebida.Precios[tipoB - 1]));
                }
            }

            Console.WriteLine("\nMétodo de Pago: 1. Yape | 2. Efectivo | 3. Plin");
            int pago = LeerEntero("Seleccione método: ", 1, 3);

            if (pago == 1)
                p.MetodoPago = "Yape";
            else if (pago == 2)
                p.MetodoPago = "Efectivo";
            else
                p.MetodoPago = "Plin";

            // --- CAMBIO APLICADO ---
            // Usamos la función con puntero en lugar del operador tradicional '+='
            AcumularGananciaConPuntero(p.Total());

            listaPedidos.Add(p);
            ImprimirBoleta(p);
        }

        private void ImprimirBoleta(Pedido p)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n------------------------------------------------");
            Console.WriteLine("                MINI BOLETA                     ");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("Pedido N°: " + p.NumeroPedido);
            Console.WriteLine("Cliente:   " + p.Cliente);
            Console.WriteLine("Método:    " + p.MetodoPago);
            Console.WriteLine("Tiempo de espera: 15 minutos");
            Console.WriteLine("\nDetalle:");

            for (int i = 0; i < p.Platos.Count; i++)
                Console.WriteLine("  Plato " + (i + 1) + ": " + p.Platos[i].ObtenerDescripcion());

            foreach (Bebida b in p.Bebidas)
                Console.WriteLine("  Bebida: " + b.ObtenerDescripcion());

            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("TOTAL A PAGAR: S/. " + p.Total().ToString("0.00"));
            Console.WriteLine("------------------------------------------------");
            Console.ResetColor();
        }

        private string LeerCadena(string mensaje)
        {
            string entrada;
            do
            {
                Console.Write(mensaje);
                entrada = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(entrada))
                    Console.WriteLine("Error: El campo no puede estar vacío.");
            }
            while (string.IsNullOrWhiteSpace(entrada));
            return entrada;
        }

        private int LeerEntero(string mensaje, int min, int max)
        {
            int numero;
            bool exito;
            do
            {
                Console.Write(mensaje);
                exito = int.TryParse(Console.ReadLine(), out numero);
                if (!exito || numero < min || numero > max)
                    Console.WriteLine("Entrada inválida. Ingrese un número entre " + min + " y " + max + ".");
            }
            while (!exito || numero < min || numero > max);
            return numero;
        }

        private void MostrarHistorial()
        {
            Console.WriteLine("\n--------------- HISTORIAL DE VENTAS ---------------");
            if (listaPedidos.Count == 0)
            {
                Console.WriteLine("No hay ventas registradas.");
                return;
            }
            foreach (Pedido p in listaPedidos)
            {
                Console.WriteLine("ID: " + p.NumeroPedido.ToString("00") +
                                  " | Cliente: " + p.Cliente.PadRight(15) +
                                  " | Total: S/. " + p.Total().ToString("0.00"));
            }
        }

        private void MostrarResumen()
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine("  Pedidos totales: " + listaPedidos.Count);
            Console.WriteLine("  Ganancia del día: S/. " + gananciaTotal.ToString("0.00"));
            Console.WriteLine("==============================");
        }

        // =========================================================================
        // PERSISTENCIA DE DATOS: OPERACIÓN DE ESCRITURA EN ARCHIVOS
        // =========================================================================
        private void GuardarEnArchivo()
        {
            try
            {
                // El uso de 'using' asegura el correcto cierre del archivo al finalizar (operación cerrar)
                using (StreamWriter sw = new StreamWriter(ArchivoNombre, false, Encoding.UTF8))
                {
                    sw.WriteLine("REPORTE FINAL DEL DÍA");
                    sw.WriteLine("Fecha: " + DateTime.Now);
                    sw.WriteLine("Total Pedidos: " + listaPedidos.Count);
                    sw.WriteLine("Ganancia Total: S/. " + gananciaTotal.ToString("0.00"));
                    sw.WriteLine("------------------------------");

                    foreach (Pedido p in listaPedidos)
                    {
                        sw.WriteLine("Pedido #" + p.NumeroPedido + " - " + p.Cliente +
                                     " - " + p.MetodoPago +
                                     " - S/. " + p.Total().ToString("0.00"));

                        foreach (Plato plato in p.Platos)
                            sw.WriteLine("   Plato: " + plato.ObtenerDescripcion());

                        foreach (Bebida bebida in p.Bebidas)
                            sw.WriteLine("   Bebida: " + bebida.ObtenerDescripcion());
                    }
                }
                Console.WriteLine("Reporte exportado exitosamente a 'RegistroVentas.txt'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar archivo: " + ex.Message);
            }
        }
    }
}
