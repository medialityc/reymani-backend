using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Users.Requests;
using reymani_web_api.Services.BlobServices;

using ReymaniWebApi.Data.Models;
using reymani_web_api.Endpoints.Users.Responses;

namespace reymani_web_api.Endpoints.Users
{
  public class CreateUserEndpoint : Endpoint<CreateUserRequest, Results<Created<UserResponse>, Conflict, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public CreateUserEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Post("/users");
      AllowFormData();
    }

    public override async Task<Results<Created<UserResponse>, Conflict, ProblemDetails>> ExecuteAsync(CreateUserRequest req, CancellationToken ct)
    {
      var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == req.Email, ct);
      if (existingUser != null)
      {
        return TypedResults.Conflict();
      }

      var user = new User
      {
        FirstName = req.FirstName,
        LastName = req.LastName,
        Email = req.Email,
        Phone = req.Phone,
        ProfilePicture = string.Empty,
        Password = BCrypt.Net.BCrypt.HashPassword(req.Password),
        IsActive = req.IsActive,
        Role = req.Role,
        IsConfirmed = req.IsConfirmed
      };

      if (req.ProfilePicture != null)
      {
        string fileCode = Guid.NewGuid().ToString();
        string objectPath = await _blobService.UploadObject(req.ProfilePicture, fileCode, ct);
        user.ProfilePicture = objectPath;
      }

      _dbContext.Users.Add(user);
      await _dbContext.SaveChangesAsync(ct);

      var response = new UserResponse
      {
        Id = user.Id,
        ProfilePicture = user.ProfilePicture,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        Phone = user.Phone,
        IsActive = user.IsActive,
        Role = user.Role,
        IsConfirmed = user.IsConfirmed
      };

      return TypedResults.Created($"/users/{user.Id}", response);
    }
  }
}
