using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Users.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Users
{
  public class UpdateUserEndpoint : Endpoint<UpdateUserRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public UpdateUserEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Put("/users/{id}");
      Roles("SystemAdmin");
      AllowFormData();
    }

    public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(UpdateUserRequest req, CancellationToken ct)
    {
      // Verificar si el email ya está en uso por otro usuario
      var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == req.Email, ct);
      if (existingUser != null && existingUser.Id != req.Id)
      {
        return TypedResults.Conflict();
      }

      // Buscar el usuario por Id
      var user = await _dbContext.Users.FindAsync(new object?[] { req.Id }, ct);
      if (user is null)
        return TypedResults.NotFound();

      // Si se actualiza la imagen, almacenarla en Minio y actualizar la ruta
      if (req.ProfilePicture != null)
      {
        string fileCode = Guid.NewGuid().ToString();
        string objectPath = await _blobService.UploadObject(req.ProfilePicture, fileCode, ct);
        user.ProfilePicture = objectPath;
      }

      // Actualizar propiedades del usuario
      user.FirstName = req.FirstName;
      user.LastName = req.LastName;
      user.Email = req.Email;
      user.Phone = req.Phone;
      user.Password = BCrypt.Net.BCrypt.HashPassword(req.Password);
      user.IsActive = req.IsActive;
      user.Role = req.Role;
      user.IsConfirmed = req.IsConfirmed;

      await _dbContext.SaveChangesAsync(ct);
      return TypedResults.Ok();
    }
  }
}
