﻿using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.AddressUser.Responses;
using reymani_web_api.Endpoints.Mappers;


namespace reymani_web_api.Endpoints.AddressUser;

public class GetAllCustomersEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<UserAddressResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;


  public GetAllCustomersEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/userAddress");
    Summary(s =>
    {
      s.Summary = "Get all active customers";
      s.Description = "Retrieves a list of all customers.";
    });
    Roles("Customer");
  }

  public override async Task<Results<Ok<IEnumerable<UserAddressResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var mapper = new UserAddressMapper();

    var userIdClaim = User.Claims.First(c => c.Type == "Id");
    int userId = int.Parse(userIdClaim.Value);
    
    var users = await _dbContext.UserAddresses
      .Where(p => p.IsActive == true && p.UserId == userId)
      .AsNoTracking()
      .Include(p => p.Municipality)
      .Include(p => p.Municipality.Province)
      .OrderBy(u => u.Id)
      .ToListAsync(ct);


    var response = users.Select(u => mapper.FromEntity(u)).ToList();

    return TypedResults.Ok(response.AsEnumerable());
  }
}
