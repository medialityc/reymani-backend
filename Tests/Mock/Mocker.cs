using Faker;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
    var users = FakerGenerator.GetFakeUsers().AsQueryable().BuildMockDbSet().Object;
    var productCategories = FakerGenerator.GetFakeProductCategories().AsQueryable().BuildMockDbSet().Object;
    var userAdresses = FakerGenerator.GetFakeUserAddresses().AsQueryable().BuildMockDbSet().Object;
    var vehicleTypes = FakerGenerator.GetFakeVehicleTypes().AsQueryable().BuildMockDbSet().Object;
    var forgotPasswordNumbers = FakerGenerator.GetFakeForgotPasswordNumbers().AsQueryable().BuildMockDbSet().Object;
    var confirmationNumbers = FakerGenerator.GetFakeConfirmationNumbers().AsQueryable().BuildMockDbSet().Object;
    var provinces = FakerGenerator.GetFakeProvinces().AsQueryable().BuildMockDbSet().Object;
    var municipalities = FakerGenerator.GetFakeMunicipalities().AsQueryable().BuildMockDbSet().Object;
    var vehicles = FakerGenerator.GetFakeVehicles().AsQueryable().BuildMockDbSet().Object;
    var businesses = FakerGenerator.GetFakeBusinesses().AsQueryable().BuildMockDbSet().Object;
    var shippingCosts = FakerGenerator.GetFakeShippingCosts().AsQueryable().BuildMockDbSet().Object;
    
    var dbContextMock = new Mock<AppDbContext>();
    dbContextMock.Setup(m => m.Users).Returns(users);
    dbContextMock.Setup(m => m.ProductCategories).Returns(productCategories);
    dbContextMock.Setup(m => m.UserAddresses).Returns(userAdresses);
    dbContextMock.Setup(m => m.VehicleTypes).Returns(vehicleTypes);
    dbContextMock.Setup(m => m.ForgotPasswordNumbers).Returns(forgotPasswordNumbers);
    dbContextMock.Setup(m => m.ConfirmationNumbers).Returns(confirmationNumbers);
    dbContextMock.Setup(m => m.Provinces).Returns(provinces);
    dbContextMock.Setup(m => m.Municipalities).Returns(municipalities);
    dbContextMock.Setup(m => m.Vehicles).Returns(vehicles);
    dbContextMock.Setup(m => m.Businesses).Returns(businesses);
    dbContextMock.Setup(m => m.ShippingCosts).Returns(shippingCosts);

    return dbContextMock.Object;
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
      .Returns(Task.FromResult(Identification.MedicareBeneficiaryIdentifier()));
    
    return emailTemplateServiceMock.Object;
  }

  public static TokenGenerator BuildMockTokenGenerator()
  {
    var authOptionsMock = new Mock<IOptions<AuthOptions>>();
    authOptionsMock.Setup(x => x.Value).Returns(new AuthOptions
    {
      JwtToken = "12345678",
      SystemAdminEmail = "",
      SystemAdminPassword = "",
      BusinessAdminEmail = "",
      BusinessAdminPassword = "",
      CourierEmail = "",
      CourierPassword = "",
      CustomerEmail = "",
      CustomerPassword = ""
    });
    
    var tokenGeneratorMock = new Mock<TokenGenerator>(authOptionsMock.Object);

    tokenGeneratorMock.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns(Internet.UserName);
    
    return tokenGeneratorMock.Object;
  }
}