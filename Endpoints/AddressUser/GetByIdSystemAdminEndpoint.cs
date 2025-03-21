﻿using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.AddressUser.Requests;
using reymani_web_api.Endpoints.AddressUser.Responses;
using reymani_web_api.Endpoints.Mappers;


namespace reymani_web_api.Endpoints.AddressUser;

public class GetByIdSystemAdminEndpoint : Endpoint<GetByIdCustomerRequest, Results<Ok<UserAddressResponse>, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;


  public GetByIdSystemAdminEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/userAddress/admin/{id}");
    Summary(s =>
    {
      s.Summary = "Get userAddress by Id";
      s.Description = "Retrieves details of a userAddress by their ID.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok<UserAddressResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetByIdCustomerRequest req, CancellationToken ct)
  {
    var userAddress = await _dbContext.UserAddresses
        .Include(p => p.Municipality)
        .Include(p => p.Municipality.Province)
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == req.Id, ct);

    if (userAddress is null)
      return TypedResults.NotFound();


    var mapper = new UserAddressMapper();

    var response = mapper.FromEntity(userAddress);

    return TypedResults.Ok(response);
  }
}
