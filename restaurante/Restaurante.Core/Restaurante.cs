using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Restaurante.Core
{
    //Clase que representa el gestor del restaurante
    public class GestorRestaurante
    {
        //Lista de pedidos
        private List<Pedido> listaPedidos = new List<Pedido>();
        private double gananciaTotal = 0; //Variable que guarda la ganancia total
        private int siguienteNumeroPedido = 1; //Evita indices duplicados en los pedidos
        private const string ArchivoNombre = "RegistroVentas.txt"; //Nombre del archivo donde se guardará el registro de ventas 

        //Propiedades de solo lectura para acceder a la lista de pedidos y la ganancia total
        public List<Pedido> ListaPedidos => listaPedidos;
        public double GananciaTotal => gananciaTotal;

        //Lista de usuarios
        public List<Usuario> ListaUsuarios { get; set; } = new List<Usuario>();


        //REGISTRO DE USUARIOS
        public void RegistrarUsuario(string username, string password, string rol)
        {
            //Verificamos que el usuario no exista
            if (!ListaUsuarios.Exists(u => u.Username.ToLower() == username.ToLower()))
            {
                //Guardamos el usuario en la lista
                ListaUsuarios.Add(new Usuario(username, password, rol));
                GuardarUsuariosTxt();
            }
        }

        //CARGA DE DATOS DE CONFIGURACIÓN
        public void CargarDatosConfiguracion()
        {
            try
            {
                //Cargamos los datos de los archivos de texto si existen
                if (File.Exists("PlatosMenu.txt"))
                {
                    //Leemos todas las lineas del archivo
                    string[] lineas = File.ReadAllLines("PlatosMenu.txt");
                    if (lineas.Length >= 3)//Verificamos que haya al menos 3 lineas
                    {
                        //Separamos las entradas y segundos por comas y los guardamos en las listas correspondientes
                        PlatoMenu.Entradas = new List<string>(lineas[0].Split(','));
                        PlatoMenu.Segundos = new List<string>(lineas[1].Split(','));
                        PlatoMenu.PrecioCategoriaMenu = double.Parse(lineas[2]);//Parseamos el precio de la categoría menú
                    }
                }

                //Cargamos los datos de los platos a la carta
                if (File.Exists("PlatosCarta.txt"))
                {
                    //Leemos todas las lineas del archivo
                    string[] lineas = File.ReadAllLines("PlatosCarta.txt");
                    if (lineas.Length >= 2)
                    {
                        //Separamos los platos por comas y los guardamos en la lista correspondiente
                        PlatoACarta.Platos = new List<string>(lineas[0].Split(','));
                        PlatoACarta.PrecioCategoriaCarta = double.Parse(lineas[1]);
                    }
                }

                //Cargamos los datos de las bebidas
                if (File.Exists("Bebidas.txt"))
                {
                    //Limpiamos las listas de nombres y precios de bebidas antes de cargarlas
                    Bebida.Nombres.Clear(); Bebida.Precios.Clear();
                    foreach (string l in File.ReadAllLines("Bebidas.txt"))
                    {
                        string[] p = l.Split('-');
                        if (p.Length == 2) { Bebida.Nombres.Add(p[0]); Bebida.Precios.Add(double.Parse(p[1])); }
                    }
                }

                //Cargamos los datos de los usuarios
                if (File.Exists("Usuarios.txt"))
                {
                    //Limpiamos la lista de usuarios antes de cargarla
                    ListaUsuarios.Clear();
                    foreach (string l in File.ReadAllLines("Usuarios.txt"))
                    {
                        string[] p = l.Split('-');
                        if (p.Length == 3) { ListaUsuarios.Add(new Usuario(p[0], p[1], p[2])); }
                    }
                }
            }
            catch { /* Usar valores iniciales si no existen los archivos */ }
        }

        //ACTUALIZACIÓN DE DATOS DE CONFIGURACIÓN

        //Métodos para actualizar los precios de las categorías de menú y carta
        public void ActualizarPrecioMenu(double np) { 
            PlatoMenu.PrecioCategoriaMenu = np; 
            GuardarMenuTxt(); 
        }
        public void ActualizarPrecioCarta(double np) { 
            PlatoACarta.PrecioCategoriaCarta = np; 
            GuardarCartaTxt(); 
        }

        //Métodos para registrar y retirar entradas, segundos, platos a la carta y bebidas
        public void RegistrarEntrada(string e) { 
            PlatoMenu.Entradas.Add(e); 
            GuardarMenuTxt(); 
        }
        public void RetirarEntrada(int idx) { 
            PlatoMenu.Entradas.RemoveAt(idx); 
            GuardarMenuTxt(); 
        }
        public void RegistrarSegundo(string s) { 
            PlatoMenu.Segundos.Add(s); 
            GuardarMenuTxt(); 
        }
        public void RetirarSegundo(int idx) { 
            PlatoMenu.Segundos.RemoveAt(idx); 
            GuardarMenuTxt(); 
        }

        public void RegistrarPlatoCarta(string p) { 
            PlatoACarta.Platos.Add(p); 
            GuardarCartaTxt(); 
        }
        public void RetirarPlatoCarta(int idx) { 
            PlatoACarta.Platos.RemoveAt(idx);
            GuardarCartaTxt();
        }

        public void RegistrarBebida(string n, double p) { 
            Bebida.Nombres.Add(n); 
            Bebida.Precios.Add(p); 
            GuardarBebidasTxt(); 
        }
        public void ModificarBebida(int idx, string n, double p) { 
            Bebida.Nombres[idx] = n; 
            Bebida.Precios[idx] = p; 
            GuardarBebidasTxt(); 
        }
        public void RetirarBebida(int idx) { 
            Bebida.Nombres.RemoveAt(idx); 
            Bebida.Precios.RemoveAt(idx); 
            GuardarBebidasTxt(); 
        }


        //MÉTODOS PRIVADOS PARA GUARDAR DATOS EN ARCHIVOS DE TEXTO
        private void GuardarMenuTxt()
        {//Guardamos las entradas, segundos y precio de la categoría menú en un archivo de texto
            string e = string.Join(",", PlatoMenu.Entradas);
            string s = string.Join(",", PlatoMenu.Segundos);
            File.WriteAllLines("PlatosMenu.txt", new string[] { e, s, PlatoMenu.PrecioCategoriaMenu.ToString() });
        }
        private void GuardarCartaTxt()
        {//Guardamos los platos a la carta y el precio de la categoría carta en un archivo de texto
            string p = string.Join(",", PlatoACarta.Platos);
            File.WriteAllLines("PlatosCarta.txt", new string[] { p, PlatoACarta.PrecioCategoriaCarta.ToString() });
        }
        private void GuardarBebidasTxt()
        {// Guardamos los nombres y precios de las bebidas en un archivo de texto
            List<string> l = new List<string>();
            for (int i = 0; i < Bebida.Nombres.Count; i++) l.Add($"{Bebida.Nombres[i]}-{Bebida.Precios[i]}");
            File.WriteAllLines("Bebidas.txt", l);
        }
        private void GuardarUsuariosTxt()
        {// Guardamos los usuarios en un archivo de texto
            List<string> l = new List<string>();
            for (int i = 0; i < ListaUsuarios.Count; i++)
            {
                l.Add($"{ListaUsuarios[i].Username}-{ListaUsuarios[i].Password}-{ListaUsuarios[i].Rol}");
            }
            File.WriteAllLines("Usuarios.txt", l);
        }

        //MÉTODOS PARA MANEJAR PEDIDOS Y GANANCIAS
        public unsafe void AcumularGananciaConPuntero(double monto)
        {// Usamos punteros para acumular la ganancia total de manera segura
            fixed (double* ptrGanancia = &gananciaTotal)
            {
                *ptrGanancia = *ptrGanancia + monto;
            }
        }

        // Usamos punteros para restar la ganancia total de manera segura
        public unsafe void RestarGananciaConPuntero(double monto)
        {
            fixed (double* ptrGanancia = &gananciaTotal)
            {// Restamos el monto de la ganancia total usando punteros
                *ptrGanancia = *ptrGanancia - monto;
            }
        }

        // Método para obtener el siguiente número de pedido y actualizar el contador
        public int ObtenerSiguienteNumeroPedido()
        {
            return siguienteNumeroPedido++;
        }

        // Método para agregar un pedido a la lista y acumular la ganancia total
        public void AgregarPedido(Pedido p)
        {
            listaPedidos.Add(p);
            AcumularGananciaConPuntero(p.Total());
        }

        // Método para cancelar un pedido por su número y restar la ganancia correspondiente
        public bool CancelarPedidoPorNumero(int numeroPedido)
        {// Buscamos el pedido en la lista por su número
            Pedido pedidoACancelar = listaPedidos.Find(p => p.NumeroPedido == numeroPedido);
            if (pedidoACancelar != null)
            {
                RestarGananciaConPuntero(pedidoACancelar.Total());
                listaPedidos.Remove(pedidoACancelar);
                return true;
            }
            return false;
        }

        // Método para obtener el historial de pedidos ordenado según un criterio
        public List<Pedido> ObtenerHistorialOrdenado(int criterio)
        {
            List<Pedido> listaCopia = new List<Pedido>(listaPedidos);
            OrdenarInsercion(listaCopia, criterio);
            return listaCopia;
        }

        // Método para guardar el reporte final del día en un archivo de texto
        public void GuardarEnArchivo()
        {
            // Guardamos el reporte final del día en un archivo de texto
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

        // MÉTODOS PRIVADOS PARA ORDENAR PEDIDOS
        private void OrdenarInsercion(List<Pedido> lista, int criterio)
        {
            int n = lista.Count;
            // Implementación del algoritmo de ordenamiento por inserción
            for (int i = 1; i < n; i++)
            {
                Pedido key = lista[i];
                int j = i - 1;
                // Comparamos los elementos según el criterio especificado
                while (j >= 0 && Comparar(lista[j], key, criterio) > 0)
                {
                    lista[j + 1] = lista[j];
                    j--;
                }
                lista[j + 1] = key;
            }
        }

        // Método privado para comparar dos pedidos según un criterio
        private int Comparar(Pedido a, Pedido b, int criterio)
        {
            switch (criterio)
            {// Criterios de comparación: 1 = Total, 2 = Cliente, 3 = Número de pedido
                case 1: return b.Total().CompareTo(a.Total());
                case 2: return string.Compare(a.Cliente, b.Cliente);
                case 3: return a.NumeroPedido.CompareTo(b.NumeroPedido);
                default: return 0;
            }
        }
    }
}