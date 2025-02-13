using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Users.Requests;
using reymani_web_api.Services.BlobServices;

using reymani_web_api.Endpoints.Users.Responses;
using reymani_web_api.Utils.Mappers;

namespace reymani_web_api.Endpoints.Users
{
  public class CreateUserEndpoint : Endpoint<CreateUserRequest, Results<Created<UserResponse>, Conflict, ProblemDetails>, UserMapper>
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
      Summary(s =>
      {
        s.Summary = "Create user";
        s.Description = "Creates a new user.";
      });
      AllowFormData();
      Roles("SystemAdmin");
    }

    public override async Task<Results<Created<UserResponse>, Conflict, ProblemDetails>> ExecuteAsync(CreateUserRequest req, CancellationToken ct)
    {
      var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == req.Email, ct);
      if (existingUser != null)
      {
        return TypedResults.Conflict();
      }

      var user = Map.ToEntity(req);
      user.Password = BCrypt.Net.BCrypt.HashPassword(req.Password);

      if (req.ProfilePicture != null)
      {
        string fileCode = Guid.NewGuid().ToString();
        string objectPath = await _blobService.UploadObject(req.ProfilePicture, fileCode, ct);
        user.ProfilePicture = objectPath;
      }

      _dbContext.Users.Add(user);
      await _dbContext.SaveChangesAsync(ct);

      var response = Map.FromEntity(user);
      if (!string.IsNullOrEmpty(user.ProfilePicture))
      {
        response.ProfilePicture = await _blobService.PresignedGetUrl(user.ProfilePicture, ct);
      }

      return TypedResults.Created($"/users/{user.Id}", response);
    }
  }
}
