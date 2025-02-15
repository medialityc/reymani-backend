using Faker;

using reymani_web_api.Data.Models;

using ReymaniWebApi.Data.Models;

using Boolean = Faker.Boolean;
using Enum = Faker.Enum;

namespace Tests.Mock;

public static class FakerGenerator
{
  public static List<User> GetFakeUsers()
  {
    var currentTime = DateTimeOffset.Now;

    return new List<User>
    {
      new()
      {
        Id = 1,
        Email = "active_confirmed@example.com",
        Password = BCrypt.Net.BCrypt.HashPassword("password123"),
        IsActive = true,
        IsConfirmed = true,
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
        Phone = "0123456789",
        FirstName = "John",
        Role = UserRole.Courier,
        LastName = "Doe",
        ProfilePicture = null
      },
      new()
      {
        Id = 2,
        Email = "admin@example.com",
        Password = BCrypt.Net.BCrypt.HashPassword("password123"),
        IsActive = true,
        IsConfirmed = true,
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
        Phone = "0123456789",
        FirstName = "John",
        Role = UserRole.SystemAdmin,
        LastName = "Doe",
        ProfilePicture = null
      },
      new()
      {
        Id = 3,
        Email = "business@example.com",
        Password = BCrypt.Net.BCrypt.HashPassword("password123"),
        IsActive = true,
        IsConfirmed = true,
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
        Phone = "0123456789",
        FirstName = "John",
        Role = UserRole.BusinessAdmin,
        LastName = "Doe",
        ProfilePicture = null
      },
      new()
      {
        Id = 1,
        Email = "customer@example.com",
        Password = BCrypt.Net.BCrypt.HashPassword("password123"),
        IsActive = true,
        IsConfirmed = true,
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
        Phone = "0123456789",
        FirstName = "John",
        Role = UserRole.Customer,
        LastName = "Doe",
        ProfilePicture = null
      },
      new()
      {
        Id = 5,
        Email = "inactive@example.com",
        Password = BCrypt.Net.BCrypt.HashPassword("password123"),
        IsActive = false,
        IsConfirmed = true,
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
        Phone = "0123456789",
        FirstName = "John",
        Role = UserRole.Courier,
        LastName = "Doe",
        ProfilePicture = null
      },
      new()
      {
        Id = 6,
        Email = "unconfirmed@example.com",
        Password = BCrypt.Net.BCrypt.HashPassword("password123"),
        IsActive = true,
        IsConfirmed = false,
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
        Phone = "0123456789",
        FirstName = "John",
        Role = UserRole.Courier,
        LastName = "Doe",
        ProfilePicture = null
      }
    };
  }

  public static List<ProductCategory> GetFakeProductCategories(
    int amount = 10,
    bool? isActive = null)
  {
    var currentTime = DateTimeOffset.Now;

    return Enumerable.Range(0, amount)
      .Select(i => new ProductCategory
      {
        Id = i,
        Name = Company.Name(),
        Logo = RandomNumber.Next(2) == 0 ? null : Internet.Url(),
        IsActive = isActive ?? Boolean.Random(),
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
      })
      .ToList();
  }

  public static List<UserAddress> GetFakeUserAddresses(
    int amount = 10,
    bool? isActive = null,
    int? userId = null,
    int? municipalityId = null
  )
  {
    var currentTime = DateTimeOffset.Now;

    return Enumerable.Range(0, amount)
      .Select(i => new UserAddress
      {
        Id = i,
        Address = Address.StreetAddress(),
        Notes = RandomNumber.Next(2) == 0 ? null : Lorem.Sentence(),
        Name = Company.Name(),
        IsActive = isActive ?? Boolean.Random(),
        UserId = userId ?? RandomNumber.Next(10),
        MunicipalityId = municipalityId ?? RandomNumber.Next(10),
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
      })
      .ToList();
  }

  public static List<VehicleType> GetFakeVehicleTypes(
    int amount = 10,
    bool? isActive = null)
  {
    var currentTime = DateTimeOffset.Now;

    return Enumerable.Range(0, amount)
      .Select(i => new VehicleType
      {
        Id = i,
        Name = Company.Name(),
        IsActive = isActive ?? Boolean.Random(),
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
        TotalCapacity = RandomNumber.Next(10) + 1,
        Logo = RandomNumber.Next(2) == 0 ? null : Internet.Url(),
      })
      .ToList();
  }

  public static List<ConfirmationNumber> GetFakeConfirmationNumbers(
    int amount = 10,
    int? userId = null,
    string? number = null)
  {
    var currentTime = DateTimeOffset.Now;

    return Enumerable.Range(0, amount)
      .Select(i => new ConfirmationNumber
      {
        Id = i,
        UserId = userId ?? RandomNumber.Next(10) + 1,
        Number = number ?? Identification.SocialSecurityNumber(),
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
      })
      .ToList();
  }

  public static List<ForgotPasswordNumber> GetFakeForgotPasswordNumbers(
    int amount = 10,
    int? userId = null,
    string? number = null)
  {
    var currentTime = DateTimeOffset.Now;

    return Enumerable.Range(0, amount)
      .Select(i => new ForgotPasswordNumber
      {
        Id = i,
        UserId = userId ?? RandomNumber.Next(10) + 1,
        Number = number ?? Identification.SocialSecurityNumber(),
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
      })
      .ToList();
  }

  public static List<Province> GetFakeProvinces(
    int amount = 10)
  {
    var currentTime = DateTimeOffset.Now;

    return Enumerable.Range(0, amount)
      .Select(i => new Province
      {
        Id = i, Name = Company.Name(), CreatedAt = currentTime, UpdatedAt = currentTime,
      })
      .ToList();
  }

  public static List<Municipality> GetFakeMunicipalities(
    int amount = 10,
    int? provinceId = null)
  {
    var currentTime = DateTimeOffset.Now;
    return Enumerable.Range(0, amount)
      .Select(i => new Municipality
      {
        Id = i,
        Name = Company.Name(),
        ProvinceId = provinceId ?? RandomNumber.Next(10),
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
      })
      .ToList();
  }

  public static List<Vehicle> GetFakeVehicles(
    int amount = 10,
    bool? isActive = null,
    bool? isAvailable = null,
    int? userId = null,
    int? vehicleTypeId = null)
  {
    var currentTime = DateTimeOffset.Now;

    return Enumerable.Range(0, amount)
      .Select(i => new Vehicle
      {
        Id = i,
        Name = Company.Name(),
        Description = RandomNumber.Next(2) == 0 ? null : Lorem.Sentence(),
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
        VehicleTypeId = vehicleTypeId ?? RandomNumber.Next(10),
        UserId = userId ?? RandomNumber.Next(10),
        Picture = RandomNumber.Next(2) == 0 ? null : Internet.Url(),
        IsActive = isActive ?? Boolean.Random(),
        IsAvailable = isAvailable ?? Boolean.Random(),
      })
      .ToList();
  }

  public static List<Business> GetFakeBusinesses(
    int amount = 10,
    bool? isActive = null,
    bool? isAvailable = null,
    int? userId = null,
    int? municipalityId = null
  )
  {
    var currentTime = DateTimeOffset.Now;
    return Enumerable.Range(0, amount)
      .Select(i => new Business
      {
        Id = i,
        Name = Company.Name(),
        Address = Address.StreetAddress(),
        Banner = RandomNumber.Next(2) == 0 ? null : Internet.Url(),
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
        Description = RandomNumber.Next(2) == 0 ? null : Lorem.Sentence(),
        IsActive = isActive ?? Boolean.Random(),
        Logo = RandomNumber.Next(2) == 0 ? null : Internet.Url(),
        IsAvailable = isAvailable ?? Boolean.Random(),
        UserId = userId ?? RandomNumber.Next(10),
        MunicipalityId = municipalityId ?? RandomNumber.Next(10),
      })
      .ToList();
  }

  public static List<ShippingCost> GetFakeShippingCosts(
    int amount = 10,
    int? vehicleTypeId = null,
    int? municipalityId = null,
    decimal? cost = null)
  {
    var currentTime = DateTimeOffset.Now;
    return Enumerable.Range(0, amount)
      .Select(i => new ShippingCost
      {
        Id = i,
        Cost = cost ?? Finance.Coupon(),
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
        MunicipalityId = municipalityId ?? RandomNumber.Next(10),
        VehicleTypeId = vehicleTypeId ?? RandomNumber.Next(10),
      })
      .ToList();
  }

  // public static List<Product> GetFakeProducts(
  //   int amount = 10,
  //   bool? isActive = null,
  //   int? bussinessId = null,
  //   int? categoryId = null
  // )
  // {
  //   var currentTime = DateTimeOffset.Now;
  //   
  //   return Enumerable.Range(0, amount)
  //     .Select(i => new Product
  //     {
  //       Id = i,
  //       Capacity = Enum.Random<Capacity>(),
  //       Name = Company.Name(),
  //       Description = RandomNumber.Next(2) == 0 ? null : Lorem.Sentence(),
  //       IsActive = isActive ?? Boolean.Random(),
  //       CreatedAt = currentTime,
  //       UpdatedAt = currentTime,
  //       Price = Finance.Coupon(),
  //       
  //     })
  //     .ToList();
  // }
  //
}