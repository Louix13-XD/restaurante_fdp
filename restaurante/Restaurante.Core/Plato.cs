using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante.Core
{
    // Clase abstracta Plato que representa un plato en el restaurante
    public abstract class Plato // Superclase abstracta
    {
        //Propiedades de la clase Plato
        public double Precio { get; protected set; }
        public abstract string ObtenerDescripcion();
    }
}
