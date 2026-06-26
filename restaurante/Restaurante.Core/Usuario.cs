using System;

namespace Restaurante.Core
{
    //Clase que representa un usuario en el sistema
    public class Usuario
    {
        //Propiedades de la clase Usuario
        public string Username { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; } // "Administrador" o "Trabajador"

        //Constructor
        public Usuario(string username, string password, string rol)
        {
            Username = username;
            Password = password;
            Rol = rol;
        }
    }
}