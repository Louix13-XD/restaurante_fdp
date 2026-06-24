using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante.Core
{
    public abstract class Plato // Superclase abstracta
    {
        //Atributos
        public double Precio { get; protected set; }
        public abstract string ObtenerDescripcion();
    }
}
