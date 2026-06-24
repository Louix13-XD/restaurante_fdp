using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Restaurante.Core
{
    public class GestorRestaurante
    {
        //Lista de Pedidos
        private List<Pedido> listaPedidos = new List<Pedido>();
        private double gananciaTotal = 0; //Variable que guarda la ganancia total
        private int siguienteNumeroPedido = 1; //Evita indices duplicados en los pedidos
        private const string ArchivoNombre = "RegistroVentas.txt"; //Nombre del archivo donde se guardará el registro de ventas 

        //
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

        //SINCRO CARGA DE ARCHIVOS AUTOMÁTICA 
        public void CargarDatosConfiguracion()
        {
            try
            {
                //Si el archivo ya existe
                if (File.Exists("PlatosMenu.txt"))
                {
                    //Guardamos las lineas en un array
                    string[] lineas = File.ReadAllLines("PlatosMenu.txt");
                    if (lineas.Length >= 3)//si las lineas son mayor a 3
                    {
                        //Guardamos en nuevas listas
                        PlatoMenu.Entradas = new List<string>(lineas[0].Split(','));
                        PlatoMenu.Segundos = new List<string>(lineas[1].Split(','));
                        PlatoMenu.PrecioCategoriaMenu = double.Parse(lineas[2]);//convertimos string a double
                    }
                }

                if (File.Exists("PlatosCarta.txt"))
                {
                    string[] lineas = File.ReadAllLines("PlatosCarta.txt");
                    if (lineas.Length >= 2)
                    {
                        PlatoACarta.Platos = new List<string>(lineas[0].Split(','));
                        PlatoACarta.PrecioCategoriaCarta = double.Parse(lineas[1]);
                    }
                }

                if (File.Exists("Bebidas.txt"))
                {
                    Bebida.Nombres.Clear(); Bebida.Precios.Clear();
                    foreach (string l in File.ReadAllLines("Bebidas.txt"))
                    {
                        string[] p = l.Split('-');
                        if (p.Length == 2) { Bebida.Nombres.Add(p[0]); Bebida.Precios.Add(double.Parse(p[1])); }
                    }
                }

                if (File.Exists("Usuarios.txt"))
                {
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

        //MÉTODOS CRUD AUTOMÁTICOS
        public void ActualizarPrecioMenu(double np) { PlatoMenu.PrecioCategoriaMenu = np; GuardarMenuTxt(); }
        public void ActualizarPrecioCarta(double np) { PlatoACarta.PrecioCategoriaCarta = np; GuardarCartaTxt(); }

        public void RegistrarEntrada(string e) { PlatoMenu.Entradas.Add(e); GuardarMenuTxt(); }
        public void RetirarEntrada(int idx) { PlatoMenu.Entradas.RemoveAt(idx); GuardarMenuTxt(); }
        public void RegistrarSegundo(string s) { PlatoMenu.Segundos.Add(s); GuardarMenuTxt(); }
        public void RetirarSegundo(int idx) { PlatoMenu.Segundos.RemoveAt(idx); GuardarMenuTxt(); }

        public void RegistrarPlatoCarta(string p) { PlatoACarta.Platos.Add(p); GuardarCartaTxt(); }
        public void RetirarPlatoCarta(int idx) { PlatoACarta.Platos.RemoveAt(idx); GuardarCartaTxt(); }

        public void RegistrarBebida(string n, double p) { Bebida.Nombres.Add(n); Bebida.Precios.Add(p); GuardarBebidasTxt(); }
        public void ModificarBebida(int idx, string n, double p) { Bebida.Nombres[idx] = n; Bebida.Precios[idx] = p; GuardarBebidasTxt(); }
        public void RetirarBebida(int idx) { Bebida.Nombres.RemoveAt(idx); Bebida.Precios.RemoveAt(idx); GuardarBebidasTxt(); }

        //SINCRONIZADORES OCULTOS (.TXT)
        private void GuardarMenuTxt()
        {
            string e = string.Join(",", PlatoMenu.Entradas);
            string s = string.Join(",", PlatoMenu.Segundos);
            File.WriteAllLines("PlatosMenu.txt", new string[] { e, s, PlatoMenu.PrecioCategoriaMenu.ToString() });
        }
        private void GuardarCartaTxt()
        {
            string p = string.Join(",", PlatoACarta.Platos);
            File.WriteAllLines("PlatosCarta.txt", new string[] { p, PlatoACarta.PrecioCategoriaCarta.ToString() });
        }
        private void GuardarBebidasTxt()
        {
            List<string> l = new List<string>();
            for (int i = 0; i < Bebida.Nombres.Count; i++) l.Add($"{Bebida.Nombres[i]}-{Bebida.Precios[i]}");
            File.WriteAllLines("Bebidas.txt", l);
        }
        private void GuardarUsuariosTxt()
        {
            List<string> l = new List<string>();
            for (int i = 0; i < ListaUsuarios.Count; i++)
            {
                l.Add($"{ListaUsuarios[i].Username}-{ListaUsuarios[i].Password}-{ListaUsuarios[i].Rol}");
            }
            File.WriteAllLines("Usuarios.txt", l);
        }

        //CÓDIGO INSERCIÓN Y PUNTEROS ORIGINAL
        public unsafe void AcumularGananciaConPuntero(double monto)
        {
            fixed (double* ptrGanancia = &gananciaTotal)
            {
                *ptrGanancia = *ptrGanancia + monto;
            }
        }

        public unsafe void RestarGananciaConPuntero(double monto)
        {
            fixed (double* ptrGanancia = &gananciaTotal)
            {
                *ptrGanancia = *ptrGanancia - monto;
            }
        }

        public int ObtenerSiguienteNumeroPedido()
        {
            return siguienteNumeroPedido++;
        }

        public void AgregarPedido(Pedido p)
        {
            listaPedidos.Add(p);
            AcumularGananciaConPuntero(p.Total());
        }

        public bool CancelarPedidoPorNumero(int numeroPedido)
        {
            Pedido pedidoACancelar = listaPedidos.Find(p => p.NumeroPedido == numeroPedido);
            if (pedidoACancelar != null)
            {
                RestarGananciaConPuntero(pedidoACancelar.Total());
                listaPedidos.Remove(pedidoACancelar);
                return true;
            }
            return false;
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