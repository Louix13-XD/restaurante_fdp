using System;

namespace Restaurante.Core
{
    public class Usuario
    {
        //Atributos
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