using System;
using System.Collections.Generic;
using System.Text;

namespace restaurante
{
    public abstract class Plato // Superclase abstracta
    {
        public double Precio { get; protected set; }
        public abstract string ObtenerDescripcion();
    }
}
