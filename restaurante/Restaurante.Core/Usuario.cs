using System;

namespace Restaurante.Core
{
    public class Usuario
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; } // "Administrador" o "Trabajador"

        public Usuario(string username, string password, string rol)
        {
            Username = username;
            Password = password;
            Rol = rol;
        }
    }
}