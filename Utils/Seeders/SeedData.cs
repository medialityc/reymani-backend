using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using reymani_web_api.Data;
using reymani_web_api.Data.Models;
using reymani_web_api.Utils.Options;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Utils.Seeders;

public class SeedData
{
  public static async void Initialize(IServiceProvider serviceProvider)
  {
    using var context = new AppDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

    // Obtén AuthOptions
    var authOptions = serviceProvider.GetRequiredService<IOptions<AuthOptions>>().Value;

    if (!context.Users.Any())
    {
      context.Users.AddRange(
          new User
          {
            FirstName = "Admin",
            LastName = "System",
            ProfilePicture = string.Empty,
            Email = authOptions.SystemAdminEmail,
            Phone = "1234567890",
            Password = BCrypt.Net.BCrypt.HashPassword(authOptions.SystemAdminPassword),
            IsActive = true,
            Role = UserRole.SystemAdmin,
            IsConfirmed = true
          },
          new User
          {
            FirstName = "Admin",
            LastName = "Business",
            ProfilePicture = string.Empty,
            Email = authOptions.BusinessAdminEmail,
            Phone = "1234567891",
            Password = BCrypt.Net.BCrypt.HashPassword(authOptions.BusinessAdminPassword),
            IsActive = true,
            Role = UserRole.BusinessAdmin,
            IsConfirmed = true
          },
          new User
          {
            FirstName = "Courier",
            LastName = "User",
            ProfilePicture = string.Empty,
            Email = authOptions.CourierEmail,
            Phone = "1234567892",
            Password = BCrypt.Net.BCrypt.HashPassword(authOptions.CourierPassword),
            IsActive = true,
            Role = UserRole.Courier,
            IsConfirmed = true
          },
          new User
          {
            FirstName = "Customer",
            LastName = "User",
            ProfilePicture = string.Empty,
            Email = authOptions.CustomerEmail,
            Phone = "1234567893",
            Password = BCrypt.Net.BCrypt.HashPassword(authOptions.CustomerPassword),
            IsActive = true,
            Role = UserRole.Customer,
            IsConfirmed = true
          }
      );
    }

    if (!context.Provinces.Any())
    {
      var provinces = new List<Province>
        {
            new Province { Id = 1, Name = "Pinar del Río" },
            new Province { Id = 2, Name = "Artemisa" },
            new Province { Id = 3, Name = "La Habana" },
            new Province { Id = 4, Name = "Mayabeque" },
            new Province { Id = 5, Name = "Matanzas" },
            new Province { Id = 6, Name = "Cienfuegos" },
            new Province { Id = 7, Name = "Villa Clara" },
            new Province { Id = 8, Name = "Sancti Spíritus" },
            new Province { Id = 9, Name = "Ciego de Ávila" },
            new Province { Id = 10, Name = "Camagüey" },
            new Province { Id = 11, Name = "Las Tunas" },
            new Province { Id = 12, Name = "Holguín" },
            new Province { Id = 13, Name = "Granma" },
            new Province { Id = 14, Name = "Santiago de Cuba" },
            new Province { Id = 15, Name = "Guantánamo" },
            new Province { Id = 16, Name = "Isla de la Juventud" }
        };
      context.Provinces.AddRange(provinces);
    }

    if (!context.Municipalities.Any())
    {
      var municipalities = new List<Municipality>
        {
            // Pinar del Río (Id = 1)
            new Municipality { Name = "Consolación del Sur", ProvinceId = 1 },
            new Municipality { Name = "San Juan y Martínez", ProvinceId = 1 },
            new Municipality { Name = "Los Palacios", ProvinceId = 1 },
            new Municipality { Name = "La Palma", ProvinceId = 1 },
            new Municipality { Name = "Minas de Matahambre", ProvinceId = 1 },
            new Municipality { Name = "Mantua", ProvinceId = 1 },
            new Municipality { Name = "Puerto Esperanza", ProvinceId = 1 },
            new Municipality { Name = "Viñales", ProvinceId = 1 },
            new Municipality { Name = "San Luis (Pinar del Río)", ProvinceId = 1 },

            // Artemisa (Id = 2)
            new Municipality { Name = "Guanajay", ProvinceId = 2 },
            new Municipality { Name = "Caimito", ProvinceId = 2 },
            new Municipality { Name = "Bauta", ProvinceId = 2 },
            new Municipality { Name = "San Antonio de los Baños", ProvinceId = 2 },
            new Municipality { Name = "Güira de Melena", ProvinceId = 2 },
            new Municipality { Name = "Alquízar", ProvinceId = 2 },
            new Municipality { Name = "Bahía Honda", ProvinceId = 2 },
            new Municipality { Name = "Candelaria", ProvinceId = 2 },
            new Municipality { Name = "San Cristóbal", ProvinceId = 2 },
            new Municipality { Name = "Mariel", ProvinceId = 2 },
            new Municipality { Name = "Artemisa", ProvinceId = 2 },

            // La Habana (Id = 3)
            new Municipality { Name = "La Habana Vieja", ProvinceId = 3 },
            new Municipality { Name = "La Habana del Este", ProvinceId = 3 },
            new Municipality { Name = "Cotorro", ProvinceId = 3 },
            new Municipality { Name = "Arroyo Naranjo", ProvinceId = 3 },
            new Municipality { Name = "Centro Habana", ProvinceId = 3 },
            new Municipality { Name = "Cerro", ProvinceId = 3 },
            new Municipality { Name = "Playa", ProvinceId = 3 },
            new Municipality { Name = "Marianao", ProvinceId = 3 },
            new Municipality { Name = "Plaza de la Revolución", ProvinceId = 3 },
            new Municipality { Name = "La Lisa", ProvinceId = 3 },
            new Municipality { Name = "Regla", ProvinceId = 3 },
            new Municipality { Name = "Guanabacoa", ProvinceId = 3 },
            new Municipality { Name = "San Miguel del Padrón", ProvinceId = 3 },
            new Municipality { Name = "Diez de Octubre", ProvinceId = 3 },
            new Municipality { Name = "Boyeros", ProvinceId = 3 },

            // Mayabeque (Id = 4)
            new Municipality { Name = "San José de las Lajas", ProvinceId = 4 },
            new Municipality { Name = "Güines", ProvinceId = 4 },
            new Municipality { Name = "Jaruco", ProvinceId = 4 },
            new Municipality { Name = "Santa Cruz del Norte", ProvinceId = 4 },
            new Municipality { Name = "Madruga", ProvinceId = 4 },
            new Municipality { Name = "Nueva Paz", ProvinceId = 4 },
            new Municipality { Name = "San Nicolás de Bari", ProvinceId = 4 },
            new Municipality { Name = "Melena del Sur", ProvinceId = 4 },
            new Municipality { Name = "Bejucal", ProvinceId = 4 },
            new Municipality { Name = "Batabanó", ProvinceId = 4 },
            new Municipality { Name = "Quivicán", ProvinceId = 4 },

            // Matanzas (Id = 5)
            new Municipality { Name = "Matanzas", ProvinceId = 5 },
            new Municipality { Name = "Cárdenas", ProvinceId = 5 },
            new Municipality { Name = "Varadero", ProvinceId = 5 },
            new Municipality { Name = "Jovellanos", ProvinceId = 5 },
            new Municipality { Name = "Pedro Betancourt", ProvinceId = 5 },
            new Municipality { Name = "Los Arabos", ProvinceId = 5 },
            new Municipality { Name = "Unión de Reyes", ProvinceId = 5 },
            new Municipality { Name = "Ciénaga de Zapata", ProvinceId = 5 },
            new Municipality { Name = "San Antonio de Cabezas", ProvinceId = 5 },
            new Municipality { Name = "Colón", ProvinceId = 5 },
            new Municipality { Name = "Limonar", ProvinceId = 5 },
            new Municipality { Name = "Calimete", ProvinceId = 5 },
            new Municipality { Name = "Encrucijada (Matanzas)", ProvinceId = 5 },

            // Cienfuegos (Id = 6)
            new Municipality { Name = "Cienfuegos", ProvinceId = 6 },
            new Municipality { Name = "Rodas", ProvinceId = 6 },
            new Municipality { Name = "Palmira", ProvinceId = 6 },
            new Municipality { Name = "Cruces", ProvinceId = 6 },
            new Municipality { Name = "Abreus", ProvinceId = 6 },
            new Municipality { Name = "Cumanayagua", ProvinceId = 6 },
            new Municipality { Name = "Aguada de Pasajeros", ProvinceId = 6 },

            // Villa Clara (Id = 7)
            new Municipality { Name = "Santa Clara", ProvinceId = 7 },
            new Municipality { Name = "Placetas", ProvinceId = 7 },
            new Municipality { Name = "Manicaragua", ProvinceId = 7 },
            new Municipality { Name = "Camajuaní", ProvinceId = 7 },
            new Municipality { Name = "Ranchuelo", ProvinceId = 7 },
            new Municipality { Name = "Caibarién", ProvinceId = 7 },
            new Municipality { Name = "Quemado de Güines", ProvinceId = 7 },
            new Municipality { Name = "Sagua la Grande", ProvinceId = 7 },
            new Municipality { Name = "Remedios", ProvinceId = 7 },
            new Municipality { Name = "Cifuentes", ProvinceId = 7 },
            new Municipality { Name = "Encrucijada (Villa Clara)", ProvinceId = 7 },
            new Municipality { Name = "Fomento (Villa Clara)", ProvinceId = 7 },
            new Municipality { Name = "Corralillo", ProvinceId = 7 },

            // Sancti Spíritus (Id = 8)
            new Municipality { Name = "Sancti Spíritus", ProvinceId = 8 },
            new Municipality { Name = "Trinidad", ProvinceId = 8 },
            new Municipality { Name = "Cabaiguán", ProvinceId = 8 },
            new Municipality { Name = "Jatibonico", ProvinceId = 8 },
            new Municipality { Name = "Taguasco", ProvinceId = 8 },
            new Municipality { Name = "Fomento (Sancti Spíritus)", ProvinceId = 8 },
            new Municipality { Name = "La Sierpe", ProvinceId = 8 },

            // Ciego de Ávila (Id = 9)
            new Municipality { Name = "Ciego de Ávila", ProvinceId = 9 },
            new Municipality { Name = "Morón", ProvinceId = 9 },
            new Municipality { Name = "Chambas", ProvinceId = 9 },
            new Municipality { Name = "Ciro Redondo", ProvinceId = 9 },
            new Municipality { Name = "Majagua", ProvinceId = 9 },
            new Municipality { Name = "Florencia", ProvinceId = 9 },
            new Municipality { Name = "Venezuela", ProvinceId = 9 },
            new Municipality { Name = "Baraguá", ProvinceId = 9 },
            new Municipality { Name = "Primero de Enero", ProvinceId = 9 },
            new Municipality { Name = "Bolivia", ProvinceId = 9 },

            // Camagüey (Id = 10)
            new Municipality { Name = "Camagüey", ProvinceId = 10 },
            new Municipality { Name = "Florida", ProvinceId = 10 },
            new Municipality { Name = "Guáimaro", ProvinceId = 10 },
            new Municipality { Name = "Jimaguayú", ProvinceId = 10 },
            new Municipality { Name = "Minas", ProvinceId = 10 },
            new Municipality { Name = "Nuevitas", ProvinceId = 10 },
            new Municipality { Name = "Sibanicú", ProvinceId = 10 },
            new Municipality { Name = "Santa Cruz del Sur", ProvinceId = 10 },
            new Municipality { Name = "Vertientes", ProvinceId = 10 },
            new Municipality { Name = "Carlos Manuel de Céspedes", ProvinceId = 10 },
            new Municipality { Name = "Esmeralda", ProvinceId = 10 },
            new Municipality { Name = "Sierra de Cubitas", ProvinceId = 10 },
            new Municipality { Name = "Ciego de Ávila (Camagüey)", ProvinceId = 10 },

            // Las Tunas (Id = 11)
            new Municipality { Name = "Las Tunas", ProvinceId = 11 },
            new Municipality { Name = "Puerto Padre", ProvinceId = 11 },
            new Municipality { Name = "Manatí", ProvinceId = 11 },
            new Municipality { Name = "Jesús Menéndez", ProvinceId = 11 },
            new Municipality { Name = "Majibacoa", ProvinceId = 11 },
            new Municipality { Name = "Jobabo", ProvinceId = 11 },
            new Municipality { Name = "Colombia", ProvinceId = 11 },
            new Municipality { Name = "Amancio", ProvinceId = 11 },

            // Holguín (Id = 12)
            new Municipality { Name = "Holguín", ProvinceId = 12 },
            new Municipality { Name = "Banes", ProvinceId = 12 },
            new Municipality { Name = "Gibara", ProvinceId = 12 },
            new Municipality { Name = "Calixto García", ProvinceId = 12 },
            new Municipality { Name = "Rafael Freyre", ProvinceId = 12 },
            new Municipality { Name = "Mayarí", ProvinceId = 12 },
            new Municipality { Name = "Moa", ProvinceId = 12 },
            new Municipality { Name = "Cueto", ProvinceId = 12 },
            new Municipality { Name = "Frank País", ProvinceId = 12 },
            new Municipality { Name = "Antilla", ProvinceId = 12 },
            new Municipality { Name = "Báguanos", ProvinceId = 12 },
            new Municipality { Name = "Cacocum", ProvinceId = 12 },
            new Municipality { Name = "Urbano Noris", ProvinceId = 12 },
            new Municipality { Name = "Sagua de Tánamo (Holguín)", ProvinceId = 12 },

            // Granma (Id = 13)
            new Municipality { Name = "Bayamo", ProvinceId = 13 },
            new Municipality { Name = "Manzanillo", ProvinceId = 13 },
            new Municipality { Name = "Pilón", ProvinceId = 13 },
            new Municipality { Name = "Yara", ProvinceId = 13 },
            new Municipality { Name = "Bartolomé Masó", ProvinceId = 13 },
            new Municipality { Name = "Buey Arriba", ProvinceId = 13 },
            new Municipality { Name = "Media Luna", ProvinceId = 13 },
            new Municipality { Name = "Jiguaní", ProvinceId = 13 },
            new Municipality { Name = "Niquero", ProvinceId = 13 },

            // Santiago de Cuba (Id = 14)
            new Municipality { Name = "Santiago de Cuba", ProvinceId = 14 },
            new Municipality { Name = "Guamá", ProvinceId = 14 },
            new Municipality { Name = "Mella", ProvinceId = 14 },
            new Municipality { Name = "Palma Soriano", ProvinceId = 14 },
            new Municipality { Name = "San Luis (Santiago de Cuba)", ProvinceId = 14 },
            new Municipality { Name = "Contramaestre", ProvinceId = 14 },
            new Municipality { Name = "Segundo Frente", ProvinceId = 14 },
            new Municipality { Name = "Songo-La Maya", ProvinceId = 14 },
            new Municipality { Name = "Tercer Frente", ProvinceId = 14 },

            // Guantánamo (Id = 15)
            new Municipality { Name = "Guantánamo", ProvinceId = 15 },
            new Municipality { Name = "Baracoa", ProvinceId = 15 },
            new Municipality { Name = "El Salvador", ProvinceId = 15 },
            new Municipality { Name = "Caimanera", ProvinceId = 15 },
            new Municipality { Name = "Yateras", ProvinceId = 15 },
            new Municipality { Name = "San Antonio del Sur", ProvinceId = 15 },
            new Municipality { Name = "Niceto Pérez", ProvinceId = 15 },
            new Municipality { Name = "Manuel Tames", ProvinceId = 15 },
            new Municipality { Name = "Maisí", ProvinceId = 15 },
            new Municipality { Name = "Sagua de Tánamo (Guantánamo)", ProvinceId = 15 },

            // Isla de la Juventud (Id = 16)
            new Municipality { Name = "Nueva Gerona", ProvinceId = 16 }
        };
      context.Municipalities.AddRange(municipalities);
    }
    await context.SaveChangesAsync();
  }
}
