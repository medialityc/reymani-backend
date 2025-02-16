# Reymani Backend

[![Tests](https://github.com/medialityc/reymani-backend/actions/workflows/tests.yml/badge.svg?branch=main)](https://github.com/medialityc/reymani-backend/actions/workflows/tests.yml)

## Requisitos

- .NET 8.0 SDK
- PostgreSQL

## Instalación

1. Clona el repositorio:

   ```sh
   git clone https://github.com/Mdialityc/reymani-backend.git
   cd reymani-backend
   ```

2. Restaura los paquetes NuGet:

   ```sh
   dotnet restore
   ```

3. Configura la base de datos PostgreSQL y actualiza la cadena de conexión en `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=reymani;Username=postgres;Password=tu-contraseña"
   }
   ```

4. Configura la clave secreta JWT en `appsettings.json`:
   ```json
   "JwtSecret": "tu-clave-secreta"
   ```

## Ejecución

1. Aplica las migraciones y ejecuta el proyecto:

   ```sh
   dotnet ef database update
   dotnet run
   ```

## Variables de Entorno

- `ConnectionStrings__DefaultConnection`: Cadena de conexión para la base de datos PostgreSQL.
- `JwtSecret`: Clave secreta para la autenticación JWT.

## Librerías Usadas

- FastEndpoints (v5.32.0)
- FastEndpoints.Security (v5.32.0)
- FastEndpoints.Swagger (v5.32.0)
- Microsoft.EntityFrameworkCore.Design (v9.0.0)
- Microsoft.EntityFrameworkCore.Tools (v9.0.0)
- Npgsql.EntityFrameworkCore.PostgreSQL (v9.0.1)
