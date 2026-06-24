// ARCHIVO: Program.cs (En tu proyecto de Consola)
using System;
using Restaurante.Core; // Importamos nuestra Biblioteca de Clases

namespace restaurante
{
    internal class Program
    {
        private static GestorRestaurante gestor = new GestorRestaurante();

        static void Main(string[] args)
        {
            MostrarMenuPrincipal();
        }

        public static void MostrarMenuPrincipal()
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
                    case "1": InterfazRegistrarPedido(); break;
                    case "2": InterfazMostrarHistorial(); break;
                    case "3": InterfazMostrarResumen(); break;
                    case "4":
                        gestor.GuardarEnArchivo();
                        Console.WriteLine("Reporte exportado exitosamente.");
                        break;
                    case "5": Console.WriteLine("Cerrando sistema..."); break;
                    default: Console.WriteLine("Opción no válida."); break;
                }
                if (opcion != "5") { Console.WriteLine("\nPresione cualquier tecla para volver al menú..."); Console.ReadKey(); }
            }
        }

        private static void InterfazRegistrarPedido()
        {
            Pedido p = new Pedido { NumeroPedido = gestor.ListaPedidos.Count + 1 };
            Console.Clear();
            Console.WriteLine($"--------- REGISTRO PEDIDO #{p.NumeroPedido} -----------");
            p.Cliente = LeerCadena("Nombre del cliente: ");

            int cantPlatos = LeerEntero("Cantidad de platos: ", 1, 50);
            for (int i = 1; i <= cantPlatos; i++)
            {
                Console.WriteLine($"\n--------- Plato {i} de {cantPlatos} --------------");
                Console.WriteLine("1. Menú (S/10) | 2. A la Carta (S/15)");
                int tipo = LeerEntero("Elija el tipo de plato: ", 1, 2);

                if (tipo == 1)
                {
                    Console.WriteLine("\nEntradas: " + ListarOpciones(PlatoMenu.Entradas));
                    int entrada = LeerEntero("Seleccione entrada: ", 1, PlatoMenu.Entradas.Length);
                    Console.WriteLine("\nSegundos: " + ListarOpciones(PlatoMenu.Segundos));
                    int segundo = LeerEntero("Seleccione segundo: ", 1, PlatoMenu.Segundos.Length);

                    p.Platos.Add(new PlatoMenu(PlatoMenu.Entradas[entrada - 1], PlatoMenu.Segundos[segundo - 1]));
                }
                else
                {
                    Console.WriteLine("\nPlatos: " + ListarOpciones(PlatoACarta.Platos));
                    int carta = LeerEntero("Seleccione plato: ", 1, PlatoACarta.Platos.Length);
                    p.Platos.Add(new PlatoACarta(PlatoACarta.Platos[carta - 1]));
                }
            }

            Console.WriteLine("\n¿Desea agregar bebidas? 1. Sí | 0. No");
            if (LeerEntero("Opción: ", 0, 1) == 1)
            {
                int cantBebidas = LeerEntero("¿Cuántas bebidas?: ", 1, 10);
                for (int b = 1; b <= cantBebidas; b++)
                {
                    Console.WriteLine($"\nBebida #{b}: " + ListarOpciones(Bebida.Nombres));
                    int tipoB = LeerEntero("Seleccione tipo: ", 1, Bebida.Nombres.Length);
                    p.Bebidas.Add(new Bebida(Bebida.Nombres[tipoB - 1], Bebida.Precios[tipoB - 1]));
                }
            }

            Console.WriteLine("\nMétodo de Pago: 1. Yape | 2. Efectivo | 3. Plin");
            int pago = LeerEntero("Seleccione método: ", 1, 3);
            p.MetodoPago = pago == 1 ? "Yape" : (pago == 2 ? "Efectivo" : "Plin");

            gestor.AgregarPedido(p);
            ImprimirBoleta(p);
        }

        private static void InterfazMostrarHistorial()
        {
            Console.WriteLine("\n--------------- HISTORIAL DE VENTAS ---------------");
            if (gestor.ListaPedidos.Count == 0)
            {
                Console.WriteLine("No hay ventas registradas.");
                return;
            }

            Console.WriteLine("\nOrdenar por:\n  1. Total (mayor a menor)\n  2. Cliente (A - Z)\n  3. Número de pedido");
            int criterio = LeerEntero("Criterio: ", 1, 3);

            var listaOrdenada = gestor.ObtenerHistorialOrdenado(criterio);

            string[] nombresCriterio = { "Total", "Cliente", "N° Pedido" };
            Console.WriteLine($"\n--- Ordenado por: {nombresCriterio[criterio - 1]} ---");

            foreach (Pedido p in listaOrdenada)
            {
                Console.WriteLine($"ID: {p.NumeroPedido.ToString("00")} | Cliente: {p.Cliente.PadRight(15)} | Total: S/. {p.Total().ToString("0.00")}");
            }
        }

        private static void InterfazMostrarResumen()
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine("  Pedidos totales: " + gestor.ListaPedidos.Count);
            Console.WriteLine("  Ganancia del día: S/. " + gestor.GananciaTotal.ToString("0.00"));
            Console.WriteLine("==============================");
        }

        private static void ImprimirBoleta(Pedido p)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n------------------------------------------------\n                MINI BOLETA                     \n------------------------------------------------");
            Console.WriteLine($"Pedido N°: {p.NumeroPedido}\nCliente:   {p.Cliente}\nMétodo:    {p.MetodoPago}\nTiempo de espera: 15 minutos\n\nDetalle:");

            for (int i = 0; i < p.Platos.Count; i++)
                Console.WriteLine($"  Plato {i + 1}: {p.Platos[i].ObtenerDescripcion()}");

            foreach (Bebida b in p.Bebidas)
                Console.WriteLine($"  Bebida: {b.ObtenerDescripcion()}");

            Console.WriteLine($"------------------------------------------------\nTOTAL A PAGAR: S/. {p.Total().ToString("0.00")}\n------------------------------------------------");
            Console.ResetColor();
        }

        private static string ListarOpciones(string[] opciones)
        {
            string lista = "";
            for (int i = 0; i < opciones.Length; i++)
                lista += (i + 1) + "." + opciones[i] + " | ";
            return lista;
        }

        private static string LeerCadena(string mensaje)
        {
            string entrada;
            do
            {
                Console.Write(mensaje);
                entrada = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(entrada)) Console.WriteLine("Error: El campo no puede estar vacío.");
            } while (string.IsNullOrWhiteSpace(entrada));
            return entrada;
        }

        private static int LeerEntero(string mensaje, int min, int max)
        {
            int numero; bool exito;
            do
            {
                Console.Write(mensaje);
                exito = int.TryParse(Console.ReadLine(), out numero);
                if (!exito || numero < min || numero > max)
                    Console.WriteLine($"Entrada inválida. Ingrese un número entre {min} y {max}.");
            } while (!exito || numero < min || numero > max);
            return numero;
        }
    }
}
