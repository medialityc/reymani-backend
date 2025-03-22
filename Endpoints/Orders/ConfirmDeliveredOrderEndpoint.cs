using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Orders.Requests;
using reymani_web_api.Services.EmailServices;
using reymani_web_api.Services.EmailServices.Templates;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Orders
{
  public class ConfirmDeliveredOrderEndpoint : Endpoint<ConfirmDeliveredOrderRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateService _emailTemplateService;

    public ConfirmDeliveredOrderEndpoint(AppDbContext dbContext,IEmailTemplateService emailTemplateService, IEmailSender emailSender)
    {
      _dbContext = dbContext;
      _emailTemplateService = emailTemplateService;
      _emailSender = emailSender;
    }

    public override void Configure()
    {
      Put("/orders/delivered/{id}");
      Summary(s =>
      {
        s.Summary = "Confirm delivered order";
        s.Description = "Confirm delivered order.";
      });
      Roles("Courier");
    }

    public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(ConfirmDeliveredOrderRequest req, CancellationToken ct)
    {
      var order = await _dbContext.Orders.FindAsync(req.Id, ct);

      if (order is null)
        return TypedResults.NotFound();

      if (order?.Status != Data.Models.OrderStatus.OnTheWay)
        return TypedResults.Conflict();

      order.Status = Data.Models.OrderStatus.Delivered;
      await _dbContext.SaveChangesAsync(ct);

      var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == order.CustomerId);

      var emailBody = _emailTemplateService.GetTemplateAsync(TemplateName.OrderStatus, new
      {
        orderStatus = " en camino"
      });

      await _emailSender.SendEmailAsync(user!.Email, "Tu pedido esta en camino!", emailBody);

      return TypedResults.Ok();
    }
  }
}
