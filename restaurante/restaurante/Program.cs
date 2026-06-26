using System;
using System.Collections.Generic;
using Restaurante.Core;

namespace restaurante
{
    internal class Program
    {
        //Usamos las clases de la biblioteca Restaurante.Core para manejar la lógica del restaurante
        private static GestorRestaurante gestor = new GestorRestaurante();
        private static Usuario usuarioLogueado = null;

        // Punto de entrada del programa
        static void Main(string[] args)
        {
            //Cargamos la configuración inicial del restaurante desde archivos de texto
            gestor.CargarDatosConfiguracion();

            // Si no hay usuarios registrados, creamos un usuario administrador por defecto
            if (gestor.ListaUsuarios.Count == 0)
                gestor.RegistrarUsuario("admin", "1234", "Administrador");

           //Primero se muestra la interfaz de login,si el login es exitoso, se muestra el menú principal
            bool ejecucionSistema = true;
            while (ejecucionSistema)
            {
              //si el usuario ingresa credenciales válidas, se muestra el menú principal; si no, se sale del sistema
                if (InterfazLogin())
                    MostrarMenuPrincipal();
                else
                    ejecucionSistema = false;
            }

            // Mensaje de despedida al salir del sistema
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("  SISTEMA APAGADO - ¡HASTA LUEGO!       ");
            Console.WriteLine("========================================");
            System.Threading.Thread.Sleep(1200); // Pausa de 1.2 segundos antes de cerrar la consola
        }

        //Interfaz de login: solicita al usuario su nombre de usuario y contraseña, y valida las credenciales
        private static bool InterfazLogin()
        {
            while (true)
            {
                // Limpiamos la consola y mostramos el menú de login
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("        SISTEMA RESTAURANTE - LOGIN     ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Iniciar Sesión");
                Console.WriteLine("2. Apagar y Salir del Programa");
                Console.WriteLine("========================================");
                int op = LeerEntero("Seleccione una opción: ", 1, 2);

                if (op == 2) return false;
                
                // Limpiamos la consola y mostramos el menú de control de acceso
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("        CONTROL DE ACCESO - CREDENCIALES ");
                Console.WriteLine("========================================");

                // Solicitamos al usuario su nombre de usuario y contraseña
                string user = LeerCadena("Usuario: ");
                string pass = LeerCadena("Contraseña: ");

                // Buscamos el usuario en la lista de usuarios registrados
                Usuario u = gestor.ListaUsuarios
                    .Find(x => x.Username == user && x.Password == pass);

                // Si encontramos un usuario válido, lo asignamos a la variable global usuarioLogueado y mostramos un
                // mensaje de bienvenida; de lo contrario, mostramos un mensaje de error
                if (u != null)
                {
                    // Guardamos el usuario logueado para usarlo en el menú principal
                    usuarioLogueado = u;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n¡Acceso concedido! Bienvenido {u.Username} ({u.Rol})");
                    Console.ResetColor();
                    System.Threading.Thread.Sleep(1000);
                    return true;
                }
                else
                {   // Si las credenciales son incorrectas, mostramos un mensaje de error y pedimos al usuario que intente nuevamente
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nCredenciales incorrectas. Intente nuevamente.");
                    Console.ResetColor();
                    Pausar();
                }
            }
        }

        // Interfaz del menú principal: muestra las opciones disponibles según el rol del usuario logueado y ejecuta
        // la acción correspondiente según la opción seleccionada
        public static void MostrarMenuPrincipal()
        {
            // Variable para almacenar la opción seleccionada por el usuario
            string opcion = "";
            string opcionSalir = usuarioLogueado.Rol == "Administrador" ? "8" : "3";// La opción de salir depende del rol del usuario

            // Bucle principal del menú: se repite hasta que el usuario seleccione la opción de salir
            while (opcion != opcionSalir)
            {
                // Limpiamos la consola y mostramos el encabezado del menú principal con el nombre de usuario y rol
                Console.ResetColor();
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine($"  SESIÓN ACTIVA: {usuarioLogueado.Username.ToUpper()} ({usuarioLogueado.Rol.ToUpper()})");
                Console.WriteLine("========================================");

                // Mostramos las opciones disponibles según el rol del usuario logueado
                if (usuarioLogueado.Rol == "Administrador")
                {
                    // Opciones para el administrador
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
                    // Opciones para el trabajador
                    Console.WriteLine("1. Registrar Nuevo Pedido");
                    Console.WriteLine("2. Panel de Mantenimiento de Productos");
                    Console.WriteLine("3. Cerrar Sesión");
                }

                // Mostramos una línea divisoria y solicitamos al usuario que seleccione una opción
                Console.WriteLine("========================================");
                opcion = LeerCadena("Seleccione una opción: ");

                // Ejecutamos la acción correspondiente según la opción seleccionada y el rol del usuario logueado
                if (usuarioLogueado.Rol == "Administrador")
                {
                    switch (opcion)
                    {   // Acciones para el administrador
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
                    {   // Acciones para el trabajador
                        case "1": InterfazRegistrarPedido(); Pausar(); break;
                        case "2": InterfazMantenimientoProductos(); break;
                        case "3": Console.WriteLine("Cerrando sesión..."); break;
                        default: Console.WriteLine("Opción no válida."); Pausar(); break;
                    }
                }
            }
        }

        // Interfaz para cancelar un pedido activo: muestra la lista de pedidos registrados y permite al usuario seleccionar uno para cancelarlo
        private static void InterfazCancelarPedido()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("        ANULACIÓN / CANCELACIÓN PEDIDO  ");
            Console.WriteLine("========================================");

            // Si no hay pedidos registrados, mostramos un mensaje y salimos de la función
            if (gestor.ListaPedidos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No existen pedidos registrados en este turno.");
                Console.ResetColor();
                return;
            }

            // Mostramos la lista de pedidos activos con su número, cliente y total
            Console.WriteLine("Pedidos activos actualmente:");
            foreach (var ped in gestor.ListaPedidos)
            {
                Console.WriteLine($"  -> Pedido #{ped.NumeroPedido:00} | Cliente: {ped.Cliente.PadRight(12)} | Total: S/. {ped.Total():0.00}");
            }
            Console.WriteLine("----------------------------------------");

            // Solicitamos al usuario que ingrese el número del pedido que desea cancelar
            int idAnular = LeerEntero("Ingrese el número del pedido que desea CANCELAR: ", 1, 1000);

            // Confirmación de la acción: pedimos al usuario que confirme si realmente desea cancelar el pedido seleccionado
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"\n¿Está completamente seguro de eliminar el Pedido #{idAnular}? (1 = Sí / 0 = No): ");
            Console.ResetColor();
            int confirmar = LeerEntero("", 0, 1);

            // Si el usuario confirma la cancelación, llamamos al método CancelarPedidoPorNumero del gestor y mostramos un
            // mensaje de éxito o error según corresponda
            if (confirmar == 1)
            {
                // Intentamos cancelar el pedido y recalcular la ganancia
                bool exito = gestor.CancelarPedidoPorNumero(idAnular);
                if (exito)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[ÉXITO] El Pedido #{idAnular} fue eliminado y la ganancia fue recalculada.");
                }
                else
                {
                    // Si no se encontró el pedido, mostramos un mensaje de error
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[ERROR] No se encontró ningún pedido con el número #{idAnular}.");
                }
                Console.ResetColor();
            }
            else
                Console.WriteLine("\nOperación abortada. El pedido sigue intacto.");
        }

        // Interfaz para crear un nuevo usuario: solicita al administrador el nombre de usuario, contraseña
        // y rol del nuevo usuario, y lo registra en el sistema
        private static void InterfazCrearUsuario()
        {
            // Limpiamos la consola y mostramos el encabezado de registro de nuevo usuario
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("        REGISTRO DE NUEVO USUARIO       ");
            Console.WriteLine("========================================");
            string nuevoUser = LeerCadena("Ingrese nombre de usuario: ");
            string nuevaPass = LeerCadena("Ingrese contraseña: ");

            // Solicitamos al administrador que seleccione el rol del nuevo usuario (Trabajador o Administrador)
            Console.WriteLine("\nSeleccione el Rol:");
            Console.WriteLine("1. Trabajador");
            Console.WriteLine("2. Administrador");
            int tipoRol = LeerEntero("Opción: ", 1, 2);
            string rolString = tipoRol == 1 ? "Trabajador" : "Administrador";

            // Registramos el nuevo usuario en el sistema mediante el método RegistrarUsuario del gestor
            gestor.RegistrarUsuario(nuevoUser, nuevaPass, rolString);

            // Mostramos un mensaje de éxito indicando que el usuario fue creado correctamente
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n¡Usuario '{nuevoUser}' creado con éxito como '{rolString}'!");
            Console.ResetColor();
        }


        // Interfaz para registrar un nuevo pedido: solicita al usuario los datos del cliente, los platos y
        // bebidas seleccionados, y el método de pago, y lo guarda en el sistema
        private static void InterfazRegistrarPedido()
        {
            // Creamos un nuevo objeto Pedido con el siguiente número de pedido disponible
            Pedido p = new Pedido { NumeroPedido = gestor.ObtenerSiguienteNumeroPedido() };
            Console.Clear();
            Console.WriteLine($"--------- REGISTRO PEDIDO #{p.NumeroPedido} -----------");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("(Escriba '0' para cancelar todo el registro en cualquier paso)");
            Console.ResetColor();

            // Solicitamos al usuario que ingrese el nombre del cliente; si ingresa '0', se cancela el flujo de registro
            p.Cliente = LeerCadena("Nombre del cliente: ");
            if (p.Cliente == "0") { CancelarFlujoRegistro(); return; }

            // Solicitamos al usuario que ingrese la cantidad de platos a registrar; si ingresa '0', se cancela el flujo de registro
            int cantPlatos = LeerEntero("Cantidad de platos (0 para salir): ", 0, 50);
            if (cantPlatos == 0) { CancelarFlujoRegistro(); return; }

            // Bucle para registrar cada plato: se repite según la cantidad de platos ingresada por el usuario
            for (int i = 1; i <= cantPlatos; i++)
            {
                // Limpiamos la consola y mostramos el encabezado del registro de plato actual
                Console.Clear();
                Console.WriteLine($"--------- Plato {i} de {cantPlatos} --------------");
                Console.WriteLine($"1. Menú (S/{PlatoMenu.PrecioCategoriaMenu:0.00})");
                Console.WriteLine($"2. A la Carta (S/{PlatoACarta.PrecioCategoriaCarta:0.00})");
                Console.WriteLine("0. Cancelar todo el pedido");
                Console.WriteLine("------------------------------------------");
                int tipo = LeerEntero("Elija el tipo de plato: ", 0, 2);// Solicitamos al usuario que seleccione

                // Si el usuario ingresa '0', se cancela el flujo de registro
                if (tipo == 0) { CancelarFlujoRegistro(); return; }

                // Según el tipo de plato seleccionado, solicitamos al usuario que seleccione
                if (tipo == 1)// la entrada y segundo del menú, o el plato a la carta, y lo agregamos al pedido
                {
                    // Si el usuario selecciona Menú, mostramos las opciones de entradas y segundos disponibles
                    Console.WriteLine("\nEntradas: " + ListarOpciones(PlatoMenu.Entradas));
                    Console.WriteLine("0. Cancelar");
                    // Solicitamos al usuario que seleccione la entrada; si ingresa '0', se cancela el flujo de registro
                    int entrada = LeerEntero("Seleccione entrada: ", 0, PlatoMenu.Entradas.Count);
                    if (entrada == 0) { CancelarFlujoRegistro(); return; }

                    // Mostramos las opciones de segundos disponibles y solicitamos al usuario que seleccione uno;
                    // si ingresa '0', se cancela el flujo de registro
                    Console.WriteLine("\nSegundos: " + ListarOpciones(PlatoMenu.Segundos));
                    Console.WriteLine("0. Cancelar");
                    int segundo = LeerEntero("Seleccione segundo: ", 0, PlatoMenu.Segundos.Count);
                    if (segundo == 0) { CancelarFlujoRegistro(); return; }

                    // Agregamos el plato seleccionado al pedido
                    p.Platos.Add(new PlatoMenu(PlatoMenu.Entradas[entrada - 1], PlatoMenu.Segundos[segundo - 1]));
                }
                else
                {
                    // Si el usuario selecciona A la Carta, mostramos las opciones de platos disponibles
                    // y solicitamos al usuario que seleccione uno; si ingresa '0', se cancela el flujo de registro
                    Console.WriteLine("\nPlatos Disponibles: " + ListarOpciones(PlatoACarta.Platos));
                    Console.WriteLine("0. Cancelar");
                    int carta = LeerEntero("Seleccione plato: ", 0, PlatoACarta.Platos.Count);
                    if (carta == 0) { CancelarFlujoRegistro(); return; }

                    // Agregamos el plato seleccionado a la carta al pedido
                    p.Platos.Add(new PlatoACarta(PlatoACarta.Platos[carta - 1]));
                }
            }

            // Preguntamos al usuario si desea agregar bebidas al pedido; si ingresa '0', se cancela el flujo de registro
            Console.Clear();
            Console.WriteLine("\n¿Desea agregar bebidas? 1. Sí | 2. No | 0. Cancelar Pedido");
            int opBebida = LeerEntero("Opción: ", 0, 2);

            // Si el usuario selecciona '1', solicitamos la cantidad de bebidas a agregar y registramos cada bebida
            // seleccionada; si ingresa '0', se cancela el flujo de registro
            if (opBebida == 0) { CancelarFlujoRegistro(); return; }
            if (opBebida == 1)
            {
                // Solicitamos al usuario la cantidad de bebidas a agregar al pedido
                int cantBebidas = LeerEntero("¿Cuántas bebidas? (0 para cancelar): ", 0, 10);
                if (cantBebidas == 0) { CancelarFlujoRegistro(); return; }

                // Bucle para registrar cada bebida: se repite según la cantidad de bebidas ingresada por el usuario
                for (int b = 1; b <= cantBebidas; b++)
                {
                    // Limpiamos la consola y mostramos el encabezado del registro de bebida actual
                    Console.WriteLine($"\nBebida #{b}: " + ListarOpciones(Bebida.Nombres));
                    Console.WriteLine("0. Cancelar");
                    int tipoB = LeerEntero("Seleccione tipo: ", 0, Bebida.Nombres.Count);
                    if (tipoB == 0) { CancelarFlujoRegistro(); return; }

                    // Agregamos la bebida seleccionada al pedido
                    p.Bebidas.Add(new Bebida(Bebida.Nombres[tipoB - 1], Bebida.Precios[tipoB - 1]));
                }
            }

            // Solicitamos al usuario que seleccione el método de pago para el pedido; si ingresa '0', se cancela el flujo de registro
            Console.Clear();
            Console.WriteLine("\nMétodo de Pago: 1. Yape | 2. Efectivo | 3. Plin | 0. Cancelar Pedido");
            int pago = LeerEntero("Seleccione método: ", 0, 3);
            if (pago == 0) { CancelarFlujoRegistro(); return; }

            // Asignamos el método de pago al pedido según la opción seleccionada por el usuario
            p.MetodoPago = pago == 1 ? "Yape" : (pago == 2 ? "Efectivo" : "Plin");

            // Agregamos el pedido registrado al gestor y mostramos la boleta correspondiente
            gestor.AgregarPedido(p);
            ImprimirBoleta(p);
        }

        // Método auxiliar para cancelar el flujo de registro de un pedido: muestra un mensaje de error y no guarda los platos ni el dinero
        private static void CancelarFlujoRegistro()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[PROCESO ABORTADO] Se canceló el registro. No se guardaron platos ni dinero.");
            Console.ResetColor();
        }

        // Interfaz para el mantenimiento de productos: permite al usuario modificar los precios y listas de platos y bebidas del restaurante
        private static void InterfazMantenimientoProductos()
        {
            int subOp = 0;
            // Bucle del submenú de mantenimiento: se repite hasta que el usuario seleccione la opción de volver al menú principal
            while (subOp != 4)
            {
                // Limpiamos la consola y mostramos el encabezado del submenú de mantenimiento con los precios actuales de las categorías de menú y carta
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


                // Según la opción seleccionada, mostramos el submenú correspondiente
                if (subOp == 1)
                {
                    // Submenú de ajustes de la categoría Menú: permite al usuario modificar el precio
                    // unificado del menú, agregar o eliminar entradas y segundos
                    Console.Clear();
                    Console.WriteLine("--- CONFIGURACIÓN GENERAL DEL MENÚ ---");
                    Console.WriteLine("1. Modificar Precio Unificado de Menú");
                    Console.WriteLine("2. Agregar Entrada");
                    Console.WriteLine("3. Eliminar Entrada");
                    Console.WriteLine("4. Agregar Segundo");
                    Console.WriteLine("5. Eliminar Segundo");
                    Console.WriteLine("6. Volver Atrás");
                    int opMenu = LeerEntero("Seleccione acción: ", 1, 6);

                    // Si el usuario selecciona la opción de volver atrás, continuamos con la siguiente iteración del bucle del submenú
                    if (opMenu == 6) continue;

                    //llamamos al método correspondiente del gestor para modificar el precio del menú, agregar o eliminar entradas y segundos
                    if (opMenu == 1) gestor.ActualizarPrecioMenu(LeerDecimal("Nuevo precio de la Categoría Menú (S/.): ", 1, 100));

                    // Si se selecciona la opción de agregar una entrada, solicitamos el nombre de la entrada y llamamos al método RegistrarEntrada del gestor
                    else if (opMenu == 2) gestor.RegistrarEntrada(LeerCadena("Nombre de la entrada: "));

                    // Si se selecciona la opción de eliminar una entrada, mostramos la lista de entradas registradas y solicitamos al usuario que
                    // seleccione una para eliminar; si no hay entradas registradas, mostramos un mensaje de error
                    else if (opMenu == 3)
                    {
                        // Si no hay entradas registradas, mostramos un mensaje de error y continuamos con la siguiente iteración del bucle del submenú
                        if (PlatoMenu.Entradas.Count == 0) { Console.WriteLine("No hay entradas registradas."); Console.ReadKey(); continue; }

                        // Mostramos la lista de entradas registradas y solicitamos al usuario que seleccione una para eliminar
                        Console.WriteLine(ListarOpciones(PlatoMenu.Entradas));
                        gestor.RetirarEntrada(LeerEntero("Índice a eliminar: ", 1, PlatoMenu.Entradas.Count) - 1);
                    }

                    // Si se selecciona la opción de agregar un segundo, solicitamos el nombre del segundo y llamamos al método RegistrarSegundo del gestor
                    else if (opMenu == 4) gestor.RegistrarSegundo(LeerCadena("Nombre del segundo: "));

                    // Si se selecciona la opción de eliminar un segundo, mostramos la lista de segundos registrados y solicitamos al usuario que
                    // seleccione uno para eliminar; si no hay segundos registrados, mostramos un mensaje de error
                    else if (opMenu == 5)
                    {
                        // Si no hay segundos registrados, mostramos un mensaje de error y continuamos con la siguiente iteración del bucle del submenú
                        if (PlatoMenu.Segundos.Count == 0) { Console.WriteLine("No hay segundos registrados."); Console.ReadKey(); continue; }
                        Console.WriteLine(ListarOpciones(PlatoMenu.Segundos));
                        gestor.RetirarSegundo(LeerEntero("Índice a eliminar: ", 1, PlatoMenu.Segundos.Count) - 1);
                    }
                }
                // Submenú de ajustes de la categoría Carta: permite al usuario modificar el precio unificado de la carta, agregar o eliminar platos
                else if (subOp == 2)
                {
                    // Limpiamos la consola y mostramos el encabezado del submenú de ajustes de la categoría Carta
                    Console.Clear();
                    Console.WriteLine("--- CONFIGURACIÓN GENERAL DE LA CARTA ---");
                    Console.WriteLine("1. Modificar Precio Unificado de Platos a la Carta");
                    Console.WriteLine("2. Agregar Nuevo Plato a la Lista");
                    Console.WriteLine("3. Eliminar Plato de la Lista");
                    Console.WriteLine("4. Volver Atrás");
                    int opCarta = LeerEntero("Seleccione acción: ", 1, 4);

                    if (opCarta == 4) continue;

                    //llamamos al método correspondiente del gestor para modificar el precio de la carta, agregar o eliminar platos
                    if (opCarta == 1) gestor.ActualizarPrecioCarta(LeerDecimal("Nuevo precio de la Categoría Carta (S/.): ", 1, 200));

                    // Si se selecciona la opción de agregar un nuevo plato, solicitamos el nombre del plato y llamamos al método RegistrarPlatoCarta del gestor
                    else if (opCarta == 2) gestor.RegistrarPlatoCarta(LeerCadena("Nombre del plato: "));

                    // Si se selecciona la opción de eliminar un plato, mostramos la lista de platos registrados y solicitamos al usuario que
                    else if (opCarta == 3)
                    {
                        // Si no hay platos registrados, mostramos un mensaje de error y continuamos con la siguiente iteración del bucle del submenú
                        if (PlatoACarta.Platos.Count == 0) { Console.WriteLine("No hay platos en la carta."); Console.ReadKey(); continue; }
                        Console.WriteLine(ListarOpciones(PlatoACarta.Platos));
                        gestor.RetirarPlatoCarta(LeerEntero("Índice a eliminar: ", 1, PlatoACarta.Platos.Count) - 1);
                    }
                }
                // Submenú de ajustes de bebidas individuales: permite al usuario registrar nuevas bebidas,
                // modificar el nombre o precio de una bebida existente, o retirar una bebida
                else if (subOp == 3)
                {
                    // Limpiamos la consola y mostramos el encabezado del submenú de ajustes de bebidas individuales
                    Console.Clear();
                    Console.WriteLine("--- CONFIGURACIÓN DE BEBIDAS ---");
                    Console.WriteLine("1. Registrar Nueva Bebida");
                    Console.WriteLine("2. Modificar Nombre/Precio de Bebida");
                    Console.WriteLine("3. Retirar Bebida");
                    Console.WriteLine("4. Volver Atrás");
                    int opBebida = LeerEntero("Seleccione acción: ", 1, 4);

                    if (opBebida == 4) continue;

                    // Según la opción seleccionada, llamamos al método correspondiente del gestor para registrar, modificar o retirar bebidas
                    if (opBebida == 1) gestor.RegistrarBebida(LeerCadena("Nombre de la bebida: "), LeerDecimal("Precio (S/.): ", 1, 50));

                    // Si el usuario selecciona la opción de modificar una bebida, mostramos la lista de bebidas registradas y solicitamos al usuario que
                    else if (opBebida == 2)
                    {
                        if (Bebida.Nombres.Count == 0) { Console.WriteLine("No hay bebidas registradas."); Console.ReadKey(); continue; }
                        for (int i = 0; i < Bebida.Nombres.Count; i++) Console.WriteLine($"{i + 1}. {Bebida.Nombres[i]} - S/.{Bebida.Precios[i]:0.00}");
                        int idx = LeerEntero("Bebida a modificar: ", 1, Bebida.Nombres.Count) - 1;
                        gestor.ModificarBebida(idx, LeerCadena("Nuevo nombre: "), LeerDecimal("Nuevo precio (S/.): ", 1, 50));
                    }
                    // Si el usuario selecciona la opción de retirar una bebida, mostramos la lista de bebidas registradas y solicitamos al usuario que
                    else if (opBebida == 3)
                    {
                        if (Bebida.Nombres.Count == 0) { Console.WriteLine("No hay bebidas registradas."); Console.ReadKey(); continue; }
                        for (int i = 0; i < Bebida.Nombres.Count; i++) Console.WriteLine($"{i + 1}. {Bebida.Nombres[i]}");
                        gestor.RetirarBebida(LeerEntero("Bebida a eliminar: ", 1, Bebida.Nombres.Count) - 1);
                    }
                }
            }
        }

        // Interfaz para mostrar el historial de ventas: permite al usuario ver los pedidos registrados y ordenarlos según diferentes criterios
        private static void InterfazMostrarHistorial()
        {
            Console.WriteLine("\n--------------- HISTORIAL DE VENTAS ---------------");
            if (gestor.ListaPedidos.Count == 0) { Console.WriteLine("No hay ventas registradas."); return; }

            // Mostramos la lista de pedidos registrados con su número, cliente y total
            Console.WriteLine("\nOrdenar por:\n  1. Total (mayor a menor)\n  2. Cliente (A - Z)\n  3. Número de pedido");
            int criterio = LeerEntero("Criterio: ", 1, 3);

            // Obtenemos la lista de pedidos ordenada según el criterio seleccionado por el usuario
            var listaOrdenada = gestor.ObtenerHistorialOrdenado(criterio);
            string[] nombresCriterio = { "Total", "Cliente", "N° Pedido" };
            Console.WriteLine($"\n--- Ordenado por: {nombresCriterio[criterio - 1]} ---");
            foreach (Pedido p in listaOrdenada)
                Console.WriteLine($"ID: {p.NumeroPedido.ToString("00")} | Cliente: {p.Cliente.PadRight(15)} | Total: S/. {p.Total().ToString("0.00")}");
        }

        // Interfaz para mostrar el resumen de ganancias: muestra el total de pedidos registrados y la ganancia total del día
        private static void InterfazMostrarResumen()
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine("  Pedidos totales: " + gestor.ListaPedidos.Count);
            Console.WriteLine("  Ganancia del día: S/. " + gestor.GananciaTotal.ToString("0.00"));
            Console.WriteLine("==============================");
        }

        // Método para imprimir la boleta de un pedido: muestra los detalles del pedido, incluyendo el número de pedido, cliente, método de pago, tiempo de
        // espera, platos y bebidas seleccionados, y el total a pagar
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

        // Método auxiliar para listar opciones en un formato legible: recibe una lista de cadenas y devuelve una cadena con las opciones numeradas
        private static string ListarOpciones(List<string> opciones)
        {
            string lista = "";
            for (int i = 0; i < opciones.Count; i++) lista += (i + 1) + "." + opciones[i] + " | ";
            return lista;
        }

        // Métodos auxiliares para leer entradas del usuario con validación: LeerCadena, LeerEntero y LeerDecimal
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

        // Método que solicita al usuario un número entero y valida que esté dentro de un rango
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
            double numero;
            bool exito; 
            do
            {//Muestra el mensaje al usuario
                Console.Write(mensaje);

                //Convertimos la entrada a un num decimal usando el punto como separador decimal
                exito = double.TryParse(
                    Console.ReadLine(),
                    System.Globalization.NumberStyles.Any,//Usamos cualquier estilo de número
                    System.Globalization.CultureInfo.InvariantCulture,//Usamos la cultura invariante para que el punto sea el separador decimal
                    out numero
                );

                //Si la conversión falla o el número está fuera del rango permitido, mostramos error
                if (!exito || numero < min || numero > max)
                    Console.WriteLine(
                        $"Entrada inválida. Ingrese un número entre {min} y {max} (use '.' para decimales)."
                    );

            } while (!exito || numero < min || numero > max);
            //Repetimos mientras la entrada sea inválida o esté fuera del rango

            return numero;
        }

        // Método para pausar la ejecución y esperar a que el usuario presione una tecla antes de continuar
        private static void Pausar()
        {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}