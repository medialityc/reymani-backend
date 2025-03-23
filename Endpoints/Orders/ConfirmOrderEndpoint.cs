using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Orders.Requests;
using reymani_web_api.Services.EmailServices;
using reymani_web_api.Services.EmailServices.Templates;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Orders;

public class ConfirmOrderEndpoint : Endpoint<ConfirmOrderRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IEmailSender _emailSender;
  private readonly IEmailTemplateService _emailTemplateService;

  public ConfirmOrderEndpoint(AppDbContext dbContext, IEmailTemplateService emailTemplateService, IEmailSender emailSender)
  {
    _dbContext = dbContext;
    _emailTemplateService = emailTemplateService;
    _emailSender = emailSender;
  }

  public override void Configure()
  {
    Put("/orders/{id}");
    Summary(s =>
    {
      s.Summary = "Confirm order";
      s.Description = "Confirm an existing order.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(ConfirmOrderRequest req, CancellationToken ct)
  {
    var order = await _dbContext.Orders.FindAsync(req.OrderId, ct);
    if (order?.Status != Data.Models.OrderStatus.InProcess)
      return TypedResults.Conflict();
    if (order is null)
    {
      return TypedResults.NotFound();
    }

    var userAddress = await _dbContext.UserAddresses.FirstOrDefaultAsync(x=> x.UserId ==order.CustomerAddressId);
    var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(x=> x.UserId == req.CourierId);

    if (vehicle is null || userAddress is null)
    {
      return TypedResults.NotFound();
    }

    // Actualiza la orden
    order.CourierId = req.CourierId;
    order.ShippingCost = await calculateShippingCost(userAddress!,vehicle!);
    order.Status = Data.Models.OrderStatus.InPreparation;
    await _dbContext.SaveChangesAsync(ct);


    var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userAddress.UserId);

    var emailBody = _emailTemplateService.GetTemplateAsync(TemplateName.OrderStatus, new
    {
      orderStatus = " en preparacion"
    });

    await _emailSender.SendEmailAsync(user!.Email, "Tu pedido esta en la cocina!", emailBody);

    return TypedResults.Ok();
  }

  private async Task<decimal> calculateShippingCost(UserAddress address,Vehicle vehicle)
  {
    var shipCost = await _dbContext.ShippingCosts.FirstOrDefaultAsync(x => x.VehicleTypeId == vehicle.VehicleTypeId && x.MunicipalityId == address.MunicipalityId);
    return shipCost?.Cost ?? 0;
  }
}
