using System;
using System.Collections.Generic;
using Restaurante.Core;

namespace restaurante
{
    internal class Program
    {
        private static GestorRestaurante gestor = new GestorRestaurante();
        private static Usuario usuarioLogueado = null;

        static void Main(string[] args)
        {
            gestor.CargarDatosConfiguracion();

            if (gestor.ListaUsuarios.Count == 0)
                gestor.RegistrarUsuario("admin", "1234", "Administrador");

            bool ejecucionSistema = true;
            while (ejecucionSistema)
            {
                if (InterfazLogin())
                    MostrarMenuPrincipal();
                else
                    ejecucionSistema = false;
            }

            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("  SISTEMA APAGADO - ¡HASTA LUEGO!       ");
            Console.WriteLine("========================================");
            System.Threading.Thread.Sleep(1200);
        }

        private static bool InterfazLogin()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("        SISTEMA RESTAURANTE - LOGIN     ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Iniciar Sesión");
                Console.WriteLine("2. Apagar y Salir del Programa");
                Console.WriteLine("========================================");
                int op = LeerEntero("Seleccione una opción: ", 1, 2);

                if (op == 2) return false;

                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("        CONTROL DE ACCESO - CREDENCIALES ");
                Console.WriteLine("========================================");

                string user = LeerCadena("Usuario: ");
                string pass = LeerCadena("Contraseña: ");

                Usuario u = gestor.ListaUsuarios.Find(x => x.Username == user && x.Password == pass);

                if (u != null)
                {
                    usuarioLogueado = u;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n¡Acceso concedido! Bienvenido {u.Username} ({u.Rol})");
                    Console.ResetColor();
                    System.Threading.Thread.Sleep(1000);
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nCredenciales incorrectas. Intente nuevamente.");
                    Console.ResetColor();
                    Pausar();
                }
            }
        }

        public static void MostrarMenuPrincipal()
        {
            string opcion = "";
            string opcionSalir = usuarioLogueado.Rol == "Administrador" ? "8" : "3";

            while (opcion != opcionSalir)
            {
                Console.ResetColor();
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine($"  SESIÓN ACTIVA: {usuarioLogueado.Username.ToUpper()} ({usuarioLogueado.Rol.ToUpper()})");
                Console.WriteLine("========================================");

                if (usuarioLogueado.Rol == "Administrador")
                {
                    Console.WriteLine("1. Registrar Nuevo Pedido");
                    Console.WriteLine("2. Panel de Mantenimiento de Productos");
                    Console.WriteLine("3. Ver Historial de Pedidos");
                    Console.WriteLine("4. Ver Resumen de Ganancias");
                    Console.WriteLine("5. Cancelar Pedido Activo");
                    Console.WriteLine("6. Guardar Reporte en Archivo");
                    Console.WriteLine("7. Registrar Nuevo Usuario");
                    Console.WriteLine("8. Cerrar Sesión");
                }
                else
                {
                    Console.WriteLine("1. Registrar Nuevo Pedido");
                    Console.WriteLine("2. Panel de Mantenimiento de Productos");
                    Console.WriteLine("3. Cerrar Sesión");
                }

                Console.WriteLine("========================================");
                opcion = LeerCadena("Seleccione una opción: ");

                if (usuarioLogueado.Rol == "Administrador")
                {
                    switch (opcion)
                    {
                        case "1": InterfazRegistrarPedido(); Pausar(); break;
                        case "2": InterfazMantenimientoProductos(); break;
                        case "3": InterfazMostrarHistorial(); Pausar(); break;
                        case "4": InterfazMostrarResumen(); Pausar(); break;
                        case "5": InterfazCancelarPedido(); Pausar(); break;
                        case "6": gestor.GuardarEnArchivo(); Console.WriteLine("Reporte exportado exitosamente."); Pausar(); break;
                        case "7": InterfazCrearUsuario(); Pausar(); break;
                        case "8": Console.WriteLine("Cerrando sesión..."); break;
                        default: Console.WriteLine("Opción no válida."); Pausar(); break;
                    }
                }
                else
                {
                    switch (opcion)
                    {
                        case "1": InterfazRegistrarPedido(); Pausar(); break;
                        case "2": InterfazMantenimientoProductos(); break;
                        case "3": Console.WriteLine("Cerrando sesión..."); break;
                        default: Console.WriteLine("Opción no válida."); Pausar(); break;
                    }
                }
            }
        }

        private static void InterfazCancelarPedido()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("        ANULACIÓN / CANCELACIÓN PEDIDO  ");
            Console.WriteLine("========================================");

            if (gestor.ListaPedidos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No existen pedidos registrados en este turno.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Pedidos activos actualmente:");
            foreach (var ped in gestor.ListaPedidos)
            {
                Console.WriteLine($"  -> Pedido #{ped.NumeroPedido:00} | Cliente: {ped.Cliente.PadRight(12)} | Total: S/. {ped.Total():0.00}");
            }
            Console.WriteLine("----------------------------------------");

            int idAnular = LeerEntero("Ingrese el número del pedido que desea CANCELAR: ", 1, 1000);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"\n¿Está completamente seguro de eliminar el Pedido #{idAnular}? (1 = Sí / 0 = No): ");
            Console.ResetColor();
            int confirmar = LeerEntero("", 0, 1);

            if (confirmar == 1)
            {
                bool exito = gestor.CancelarPedidoPorNumero(idAnular);
                if (exito)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[ÉXITO] El Pedido #{idAnular} fue eliminado y la ganancia fue recalculada.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[ERROR] No se encontró ningún pedido con el número #{idAnular}.");
                }
                Console.ResetColor();
            }
            else
                Console.WriteLine("\nOperación abortada. El pedido sigue intacto.");
        }

        private static void InterfazCrearUsuario()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("        REGISTRO DE NUEVO USUARIO       ");
            Console.WriteLine("========================================");
            string nuevoUser = LeerCadena("Ingrese nombre de usuario: ");
            string nuevaPass = LeerCadena("Ingrese contraseña: ");

            Console.WriteLine("\nSeleccione el Rol:");
            Console.WriteLine("1. Trabajador");
            Console.WriteLine("2. Administrador");
            int tipoRol = LeerEntero("Opción: ", 1, 2);
            string rolString = tipoRol == 1 ? "Trabajador" : "Administrador";

            gestor.RegistrarUsuario(nuevoUser, nuevaPass, rolString);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n¡Usuario '{nuevoUser}' creado con éxito como '{rolString}'!");
            Console.ResetColor();
        }

        //REGISTRO CON CANCELACIÓN EN VIVO
        private static void InterfazRegistrarPedido()
        {
            Pedido p = new Pedido { NumeroPedido = gestor.ObtenerSiguienteNumeroPedido() };
            Console.Clear();
            Console.WriteLine($"--------- REGISTRO PEDIDO #{p.NumeroPedido} -----------");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("(Escriba '0' para cancelar todo el registro en cualquier paso)");
            Console.ResetColor();

            p.Cliente = LeerCadena("Nombre del cliente: ");
            if (p.Cliente == "0") { CancelarFlujoRegistro(); return; }

            int cantPlatos = LeerEntero("Cantidad de platos (0 para salir): ", 0, 50);
            if (cantPlatos == 0) { CancelarFlujoRegistro(); return; }

            for (int i = 1; i <= cantPlatos; i++)
            {
                Console.Clear();
                Console.WriteLine($"--------- Plato {i} de {cantPlatos} --------------");
                Console.WriteLine($"1. Menú (S/{PlatoMenu.PrecioCategoriaMenu:0.00})");
                Console.WriteLine($"2. A la Carta (S/{PlatoACarta.PrecioCategoriaCarta:0.00})");
                Console.WriteLine("0. Cancelar todo el pedido");
                Console.WriteLine("------------------------------------------");
                int tipo = LeerEntero("Elija el tipo de plato: ", 0, 2);

                if (tipo == 0) { CancelarFlujoRegistro(); return; }

                if (tipo == 1)
                {
                    Console.WriteLine("\nEntradas: " + ListarOpciones(PlatoMenu.Entradas));
                    Console.WriteLine("0. Cancelar");
                    int entrada = LeerEntero("Seleccione entrada: ", 0, PlatoMenu.Entradas.Count);
                    if (entrada == 0) { CancelarFlujoRegistro(); return; }

                    Console.WriteLine("\nSegundos: " + ListarOpciones(PlatoMenu.Segundos));
                    Console.WriteLine("0. Cancelar");
                    int segundo = LeerEntero("Seleccione segundo: ", 0, PlatoMenu.Segundos.Count);
                    if (segundo == 0) { CancelarFlujoRegistro(); return; }

                    p.Platos.Add(new PlatoMenu(PlatoMenu.Entradas[entrada - 1], PlatoMenu.Segundos[segundo - 1]));
                }
                else
                {
                    Console.WriteLine("\nPlatos Disponibles: " + ListarOpciones(PlatoACarta.Platos));
                    Console.WriteLine("0. Cancelar");
                    int carta = LeerEntero("Seleccione plato: ", 0, PlatoACarta.Platos.Count);
                    if (carta == 0) { CancelarFlujoRegistro(); return; }

                    p.Platos.Add(new PlatoACarta(PlatoACarta.Platos[carta - 1]));
                }
            }

            Console.Clear();
            Console.WriteLine("\n¿Desea agregar bebidas? 1. Sí | 2. No | 0. Cancelar Pedido");
            int opBebida = LeerEntero("Opción: ", 0, 2);

            if (opBebida == 0) { CancelarFlujoRegistro(); return; }
            if (opBebida == 1)
            {
                int cantBebidas = LeerEntero("¿Cuántas bebidas? (0 para cancelar): ", 0, 10);
                if (cantBebidas == 0) { CancelarFlujoRegistro(); return; }

                for (int b = 1; b <= cantBebidas; b++)
                {
                    Console.WriteLine($"\nBebida #{b}: " + ListarOpciones(Bebida.Nombres));
                    Console.WriteLine("0. Cancelar");
                    int tipoB = LeerEntero("Seleccione tipo: ", 0, Bebida.Nombres.Count);
                    if (tipoB == 0) { CancelarFlujoRegistro(); return; }

                    p.Bebidas.Add(new Bebida(Bebida.Nombres[tipoB - 1], Bebida.Precios[tipoB - 1]));
                }
            }

            Console.Clear();
            Console.WriteLine("\nMétodo de Pago: 1. Yape | 2. Efectivo | 3. Plin | 0. Cancelar Pedido");
            int pago = LeerEntero("Seleccione método: ", 0, 3);
            if (pago == 0) { CancelarFlujoRegistro(); return; }

            p.MetodoPago = pago == 1 ? "Yape" : (pago == 2 ? "Efectivo" : "Plin");

            // Solo si pasa todos los pasos ilesos, se guarda y procesa
            gestor.AgregarPedido(p);
            ImprimirBoleta(p);
        }

        private static void CancelarFlujoRegistro()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[PROCESO ABORTADO] Se canceló el registro. No se guardaron platos ni dinero.");
            Console.ResetColor();
        }

        private static void InterfazMantenimientoProductos()
        {
            int subOp = 0;
            while (subOp != 4)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("        MANTENIMIENTO DEL RESTAURANTE    ");
                Console.WriteLine("========================================");
                Console.WriteLine($"1. Ajustes Categoría Menú (Hoy: S/.{PlatoMenu.PrecioCategoriaMenu:0.00})");
                Console.WriteLine($"2. Ajustes Categoría Carta (Hoy: S/.{PlatoACarta.PrecioCategoriaCarta:0.00})");
                Console.WriteLine("3. Ajustes de Bebidas Individuales");
                Console.WriteLine("4. Volver al Menú Principal");
                Console.WriteLine("========================================");
                subOp = LeerEntero("Seleccione una opción: ", 1, 4);

                if (subOp == 1)
                {
                    Console.Clear();
                    Console.WriteLine("--- CONFIGURACIÓN GENERAL DEL MENÚ ---");
                    Console.WriteLine("1. Modificar Precio Unificado de Menú");
                    Console.WriteLine("2. Agregar Entrada");
                    Console.WriteLine("3. Eliminar Entrada");
                    Console.WriteLine("4. Agregar Segundo");
                    Console.WriteLine("5. Eliminar Segundo");
                    Console.WriteLine("6. Volver Atrás");
                    int opMenu = LeerEntero("Seleccione acción: ", 1, 6);

                    if (opMenu == 6) continue;

                    if (opMenu == 1) gestor.ActualizarPrecioMenu(LeerDecimal("Nuevo precio de la Categoría Menú (S/.): ", 1, 100));
                    else if (opMenu == 2) gestor.RegistrarEntrada(LeerCadena("Nombre de la entrada: "));
                    else if (opMenu == 3)
                    {
                        if (PlatoMenu.Entradas.Count == 0) { Console.WriteLine("No hay entradas registradas."); Console.ReadKey(); continue; }
                        Console.WriteLine(ListarOpciones(PlatoMenu.Entradas));
                        gestor.RetirarEntrada(LeerEntero("Índice a eliminar: ", 1, PlatoMenu.Entradas.Count) - 1);
                    }
                    else if (opMenu == 4) gestor.RegistrarSegundo(LeerCadena("Nombre del segundo: "));
                    else if (opMenu == 5)
                    {
                        if (PlatoMenu.Segundos.Count == 0) { Console.WriteLine("No hay segundos registrados."); Console.ReadKey(); continue; }
                        Console.WriteLine(ListarOpciones(PlatoMenu.Segundos));
                        gestor.RetirarSegundo(LeerEntero("Índice a eliminar: ", 1, PlatoMenu.Segundos.Count) - 1);
                    }
                }
                else if (subOp == 2)
                {
                    Console.Clear();
                    Console.WriteLine("--- CONFIGURACIÓN GENERAL DE LA CARTA ---");
                    Console.WriteLine("1. Modificar Precio Unificado de Platos a la Carta");
                    Console.WriteLine("2. Agregar Nuevo Plato a la Lista");
                    Console.WriteLine("3. Eliminar Plato de la Lista");
                    Console.WriteLine("4. Volver Atrás");
                    int opCarta = LeerEntero("Seleccione acción: ", 1, 4);

                    if (opCarta == 4) continue;

                    if (opCarta == 1) gestor.ActualizarPrecioCarta(LeerDecimal("Nuevo precio de la Categoría Carta (S/.): ", 1, 200));
                    else if (opCarta == 2) gestor.RegistrarPlatoCarta(LeerCadena("Nombre del plato: "));
                    else if (opCarta == 3)
                    {
                        if (PlatoACarta.Platos.Count == 0) { Console.WriteLine("No hay platos en la carta."); Console.ReadKey(); continue; }
                        Console.WriteLine(ListarOpciones(PlatoACarta.Platos));
                        gestor.RetirarPlatoCarta(LeerEntero("Índice a eliminar: ", 1, PlatoACarta.Platos.Count) - 1);
                    }
                }
                else if (subOp == 3)
                {
                    Console.Clear();
                    Console.WriteLine("--- CONFIGURACIÓN DE BEBIDAS ---");
                    Console.WriteLine("1. Registrar Nueva Bebida");
                    Console.WriteLine("2. Modificar Nombre/Precio de Bebida");
                    Console.WriteLine("3. Retirar Bebida");
                    Console.WriteLine("4. Volver Atrás");
                    int opBebida = LeerEntero("Seleccione acción: ", 1, 4);

                    if (opBebida == 4) continue;

                    if (opBebida == 1) gestor.RegistrarBebida(LeerCadena("Nombre de la bebida: "), LeerDecimal("Precio (S/.): ", 1, 50));
                    else if (opBebida == 2)
                    {
                        if (Bebida.Nombres.Count == 0) { Console.WriteLine("No hay bebidas registradas."); Console.ReadKey(); continue; }
                        for (int i = 0; i < Bebida.Nombres.Count; i++) Console.WriteLine($"{i + 1}. {Bebida.Nombres[i]} - S/.{Bebida.Precios[i]:0.00}");
                        int idx = LeerEntero("Bebida a modificar: ", 1, Bebida.Nombres.Count) - 1;
                        gestor.ModificarBebida(idx, LeerCadena("Nuevo nombre: "), LeerDecimal("Nuevo precio (S/.): ", 1, 50));
                    }
                    else if (opBebida == 3)
                    {
                        if (Bebida.Nombres.Count == 0) { Console.WriteLine("No hay bebidas registradas."); Console.ReadKey(); continue; }
                        for (int i = 0; i < Bebida.Nombres.Count; i++) Console.WriteLine($"{i + 1}. {Bebida.Nombres[i]}");
                        gestor.RetirarBebida(LeerEntero("Bebida a eliminar: ", 1, Bebida.Nombres.Count) - 1);
                    }
                }
            }
        }

        private static void InterfazMostrarHistorial()
        {
            Console.WriteLine("\n--------------- HISTORIAL DE VENTAS ---------------");
            if (gestor.ListaPedidos.Count == 0) { Console.WriteLine("No hay ventas registradas."); return; }
            Console.WriteLine("\nOrdenar por:\n  1. Total (mayor a menor)\n  2. Cliente (A - Z)\n  3. Número de pedido");
            int criterio = LeerEntero("Criterio: ", 1, 3);
            var listaOrdenada = gestor.ObtenerHistorialOrdenado(criterio);
            string[] nombresCriterio = { "Total", "Cliente", "N° Pedido" };
            Console.WriteLine($"\n--- Ordenado por: {nombresCriterio[criterio - 1]} ---");
            foreach (Pedido p in listaOrdenada)
                Console.WriteLine($"ID: {p.NumeroPedido.ToString("00")} | Cliente: {p.Cliente.PadRight(15)} | Total: S/. {p.Total().ToString("0.00")}");
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
            for (int i = 0; i < p.Platos.Count; i++) Console.WriteLine($"  Plato {i + 1}: {p.Platos[i].ObtenerDescripcion()}");
            foreach (Bebida b in p.Bebidas) Console.WriteLine($"  Bebida: {b.ObtenerDescripcion()}");
            Console.WriteLine($"------------------------------------------------\nTOTAL A PAGAR: S/. {p.Total().ToString("0.00")}\n------------------------------------------------");
            Console.ResetColor();
        }

        private static string ListarOpciones(List<string> opciones)
        {
            string lista = "";
            for (int i = 0; i < opciones.Count; i++) lista += (i + 1) + "." + opciones[i] + " | ";
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

        // Método que solicita al usuario un número decimal y valida que esté dentro de un rango
        private static double LeerDecimal(string mensaje, double min, double max)
        {
            double numero; //Almacena el número ingresado
            bool exito; //Indica si la conversión de texto a número fue exitosa
            do
            {//Muestra el mensaje al usuario
                Console.Write(mensaje);

                //Convertimos la entrada a un num decimal usando el punto como separador decimal
                exito = double.TryParse(
                    Console.ReadLine(),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out numero
                );

                //Si la conversión falla o el número está fuera del rango permitido, mostramos error
                if (!exito || numero < min || numero > max)
                    Console.WriteLine(
                        $"Entrada inválida. Ingrese un número entre {min} y {max} (use '.' para decimales)."
                    );

            } while (!exito || numero < min || numero > max);
            //Repetimos mientras la entrada sea inválida o esté fuera del rango

            return numero;//retornamos el valor del num valido
        }


        private static void Pausar()
        {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}