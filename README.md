# Sistema de Gestión para Restaurante

Sistema desarrollado en C# (.NET) para la administración básica de un restaurante mediante una aplicación de consola. El proyecto permite gestionar pedidos, productos y usuarios utilizando persistencia en archivos de texto (.txt), sin necesidad de una base de datos.

---

## Descripción

Este sistema fue desarrollado con fines académicos para aplicar los fundamentos de programación en C#, programación estructurada, modular y manejo de archivos.

El sistema permite

- Inicio de sesión con diferentes roles.
- Gestión de productos.
- Registro de pedidos.
- Historial de pedidos.
- Control de ganancias.
- Gestión de usuarios.
- Configuración del sistema.
- Persistencia de datos mediante archivos de texto.

---

# Características

✔ Inicio de sesión

✔ Roles de usuario
- Administrador
- Empleado

✔ Gestión de productos
- Agregar
- Editar
- Eliminar
- Buscar

✔ Registro de pedidos

✔ Historial de pedidos

✔ Cálculo automático de ganancias

✔ Configuración del restaurante

✔ Persistencia mediante archivos TXT

---

# Tecnologías utilizadas

- C#
- .NET
- Programación Orientada a Objetos
- Archivos de Texto (.txt)
- Visual Studio
- Git
- GitHub

---

# Estructura del proyecto

```
restaurante_fdp
│
├── Data
│   ├── Usuarios.txt
│   ├── PlatosMenu.txt
│   ├── PlatosCarta.txt
│   ├── Bebidas.txt
│   ├── Pedidos.txt
│   ├── Ganancias.txt
│   └── Configuracion.txt
│
├── Clases
│
├── Menus
│
├── Utilidades
│
├── Program.cs
│
└── README.md
```

---

# Roles del sistema

## Administrador

Puede realizar todas las operaciones del sistema.

- Registrar pedidos
- Gestionar productos
- Gestionar usuarios
- Consultar historial
- Consultar ganancias
- Configuración
- Cerrar sesión

---

## Empleado

Tiene acceso limitado.

- Registrar pedidos
- Consultar historial
- Cerrar sesión

---

# Persistencia de datos

El sistema NO utiliza bases de datos.

Toda la información se almacena en archivos de texto (.txt).

Ejemplo

```
Usuarios.txt
```

```
usuario;contraseña;rol
```

```
admin;1234;Administrador
```

---

```
Pedidos.txt
```

```
IDPedido
Fecha
Usuario
Productos
Total
```

Cada módulo lee y actualiza su archivo correspondiente.

---

# Flujo del sistema

```
Inicio
   │
   ▼
Cargar archivos TXT
   │
   ▼
¿Existe usuario
   │
   ├── No
   │      │
   │      ▼
   │ Crear Administrador
   │
   ▼
Pantalla de Login
   │
   ▼
Validar credenciales
   │
   ▼
¿Administrador
 ├───────────────┐
 │               │
 ▼               ▼
Menú Admin   Menú Empleado
 │               │
 └──────┬────────┘
        ▼
Registrar procesos
        │
Guardar archivos TXT
        │
Cerrar sesión
        │
       Fin
```

---

# Conceptos de programación aplicados

Este proyecto implementa

- Variables
- Constantes
- Entrada y salida por consola
- Estructuras secuenciales
- Condicionales
- Ciclos
- Funciones
- Parámetros
- Clases
- Objetos
- Encapsulamiento
- Diseño modular
- Listas (`ListT`)
- Manejo de cadenas
- Lectura y escritura de archivos
- Persistencia de información
- Programación orientada a objetos

---

# Cómo ejecutar el proyecto

1. Clonar el repositorio

```bash
git clone httpsgithub.comLouix13-XDrestaurante_fdp.git
```

2. Abrir la solución en Visual Studio.

3. Compilar el proyecto.

4. Ejecutar la aplicación.

---

# Equipo de desarrollo

| Integrantes |
|--------------|
| Cueva Correa Luis Geanpier |
| Chavez Soles Alex Eduardo |
| Geldres Mendez Esthefany Melissa |
| Sare Aranda Alexia Camila |
| Ugas Cherre Ariana Belen |

Estudiantes de Ingeniería de Sistemas.

---

# Información académica

**Proyecto:** Sistema de Gestión para Restaurante

**Curso:** Fundamentos de Programación

**Lenguaje:** C#

**Institución:** Univerdidad Privada del Norte

**Periodo académico:** 2026-I

---

# Licencia

Este proyecto fue desarrollado con fines **académicos y educativos**.

Se permite su uso como material de aprendizaje, consulta y mejora, siempre que se reconozca a los autores originales.

No está destinado para uso comercial.