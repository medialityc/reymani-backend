using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Users.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Users
{
  public class UpdateMeEndpoint : Endpoint<UpdateMeRequest, Results<Ok, NotFound, Conflict, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public UpdateMeEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Put("/users/me");
      AllowFormData();
    }

    public override async Task<Results<Ok, NotFound, Conflict, ProblemDetails>> ExecuteAsync(UpdateMeRequest req, CancellationToken ct)
    {
      // Extraer el claim "Id" del usuario a partir del JWT
      var userIdClaim = User.Claims.First(c => c.Type == "Id");
      int userId = int.Parse(userIdClaim.Value);

      // Verificar si el email ya está en uso por otro usuario
      var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == req.Email, ct);
      if (existingUser != null && existingUser.Id != userId)
      {
        return TypedResults.Conflict();
      }

      // Buscar el usuario por Id
      var user = await _dbContext.Users.FindAsync(new object[] { userId }, ct);
      if (user is null)
        return TypedResults.NotFound();

      // Actualizar la imagen si se provee nueva
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

      await _dbContext.SaveChangesAsync(ct);
      return TypedResults.Ok();
    }
  }
}
