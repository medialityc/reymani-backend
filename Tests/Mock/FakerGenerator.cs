using Faker;

using reymani_web_api.Data.Models;

using ReymaniWebApi.Data.Models;

using Boolean = Faker.Boolean;

namespace Tests.Mock;

public static class FakerGenerator
{
  public static List<User> GetFakeUsers()
  {
    var currentTime = DateTimeOffset.Now;

    return
    [
      new()
      {
        Email = "active_confirmed@example.com",
        Password = BCrypt.Net.BCrypt.HashPassword("Password123!"),
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
    ];
  }

  public static List<ProductCategory> GetFakeProductCategories(
    int amount = 10,
    bool? isActive = null)
  {
    var currentTime = DateTimeOffset.Now;

    return Enumerable.Range(0, amount)
      .Select(_ => new ProductCategory
      {
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
      .Select(_ => new UserAddress
      {
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
      .Select(_ => new VehicleType
      {
        Name = Company.Name(),
        IsActive = isActive ?? Boolean.Random(),
        CreatedAt = currentTime,
        UpdatedAt = currentTime,
        TotalCapacity = RandomNumber.Next(10) + 1,
        Logo = RandomNumber.Next(2) == 0 ? null : Internet.Url(),
      })
      .ToList();
  }

  public static List<ConfirmationNumber> GetFakeConfirmationNumbers()
  {
    var currentTime = DateTimeOffset.Now;

    return
    [
      new() { CreatedAt = currentTime, UpdatedAt = currentTime, UserId = 6, Number = "1234" },
    ];
  }

  public static List<ForgotPasswordNumber> GetFakeForgotPasswordNumbers()
  {
    var currentTime = DateTimeOffset.Now;

    return
    [
      new(){Number = "1234", CreatedAt = currentTime, UpdatedAt = currentTime, UserId = 1},
      new(){Number = "1234", CreatedAt = currentTime, UpdatedAt = currentTime, UserId = 3},
    ];
  }

  public static List<Province> GetFakeProvinces(
    int amount = 10)
  {
    var currentTime = DateTimeOffset.Now;

    return Enumerable.Range(0, amount)
      .Select(_ => new Province { Name = Company.Name(), CreatedAt = currentTime, UpdatedAt = currentTime, })
      .ToList();
  }

  public static List<Municipality> GetFakeMunicipalities(
    int amount = 10,
    int? provinceId = null)
  {
    var currentTime = DateTimeOffset.Now;
    return Enumerable.Range(0, amount)
      .Select(_ => new Municipality
      {
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
      .Select(_ => new Vehicle
      {
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
      .Select(_ => new Business
      {
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
      .Select(_ => new ShippingCost
      {
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