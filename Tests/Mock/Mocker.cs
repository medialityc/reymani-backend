using Faker;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using MockQueryable.Moq;

using reymani_web_api.Data;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Services.EmailServices;
using reymani_web_api.Utils.Options;
using reymani_web_api.Utils.Tokens;

using ReymaniWebApi.Data.Models;

namespace Tests.Mock;

public static class Mocker
{
  public static AppDbContext BuildMockAppDbContext()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: "TestDatabase")
      .Options;
    
    var context = new AppDbContext(options);
    context.Database.EnsureCreated();
    
    var users = FakerGenerator.GetFakeUsers();
    var productCategories = FakerGenerator.GetFakeProductCategories();
    var userAdresses = FakerGenerator.GetFakeUserAddresses();
    var vehicleTypes = FakerGenerator.GetFakeVehicleTypes();
    var forgotPasswordNumbers = FakerGenerator.GetFakeForgotPasswordNumbers();
    var confirmationNumbers = FakerGenerator.GetFakeConfirmationNumbers();
    var provinces = FakerGenerator.GetFakeProvinces();
    var municipalities = FakerGenerator.GetFakeMunicipalities();
    var vehicles = FakerGenerator.GetFakeVehicles();
    var businesses = FakerGenerator.GetFakeBusinesses();
    var shippingCosts = FakerGenerator.GetFakeShippingCosts();
    
    context.Users.AddRange(users);
    context.ProductCategories.AddRange(productCategories);
    context.UserAddresses.AddRange(userAdresses);
    context.VehicleTypes.AddRange(vehicleTypes);
    context.ForgotPasswordNumbers.AddRange(forgotPasswordNumbers);
    context.ConfirmationNumbers.AddRange(confirmationNumbers);
    context.Provinces.AddRange(provinces);
    context.Municipalities.AddRange(municipalities);
    context.Vehicles.AddRange(vehicles);
    context.Businesses.AddRange(businesses);
    context.ShippingCosts.AddRange(shippingCosts);
    
    context.SaveChanges();

    return context;
  }

  public static IBlobService BuildMockIBlobService()
  {
    var blobServiceMock = new Mock<IBlobService>();

    blobServiceMock.Setup(x =>
        x.UploadObject(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(Identification.MedicareBeneficiaryIdentifier()));

    blobServiceMock.Setup(x => x.PresignedGetUrl(It.IsAny<string>(), It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(Internet.SecureUrl()));

    blobServiceMock.Setup(x => x.ValidateExistanceObject(It.IsAny<string>(), It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(true));

    return blobServiceMock.Object;
  }

  public static IEmailSender BuildMockEmailSender()
  {
    var emailSenderMock = new Mock<IEmailSender>();

    emailSenderMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
      .Returns(Task.CompletedTask);
    
    return emailSenderMock.Object;
  }

  public static IEmailTemplateService BuildMockEmailTemplateService()
  {
    var emailTemplateServiceMock = new Mock<IEmailTemplateService>();

    emailTemplateServiceMock.Setup(x => x.GetTemplateAsync(It.IsAny<string>(), It.IsAny<object>()))
      .Returns(Task.FromResult("Email Template Body Test"));
    
    return emailTemplateServiceMock.Object;
  }

  public static TokenGenerator BuildMockTokenGenerator(IConfiguration configuration)
  {
    var authOptionsMock = new Mock<IOptions<AuthOptions>>();
    authOptionsMock.Setup(x => x.Value).Returns(new AuthOptions
    {
      JwtToken = configuration["AuthOptions:JwtToken"] ?? string.Empty,
      SystemAdminEmail = "",
      SystemAdminPassword = "",
      BusinessAdminEmail = "",
      BusinessAdminPassword = "",
      CourierEmail = "",
      CourierPassword = "",
      CustomerEmail = "",
      CustomerPassword = ""
    });
    
    return new TokenGenerator(authOptionsMock.Object);
  }
}