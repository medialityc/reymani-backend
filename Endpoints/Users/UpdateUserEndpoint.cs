using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Users.Requests;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Utils.Mappers;

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
      Summary(s =>
      {
        s.Summary = "Update user";
        s.Description = "Updates details of an existing user.";
      });
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

      // Actualizar el resto de propiedades usando el mapper
      new UserMapper().ToEntity(req, user);

      await _dbContext.SaveChangesAsync(ct);
      return TypedResults.Ok();
    }
  }
}
