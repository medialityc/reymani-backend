using System;

namespace reymani_web_api.Api.Endpoints.CategoriaNegocio
{
  public class AssignCategoriaNegocioToNegocioEndpoint : Endpoint<AssignCategoriaNegocioToNegocioRequest>
  {
    private readonly INegocioService _negocioService;
    private readonly ICategoriaNegocioService _categoriaNegocioService;
    private readonly IAuthorizationService _authorizationService;

    public AssignCategoriaNegocioToNegocioEndpoint(INegocioService negocioService, ICategoriaNegocioService categoriaNegocioService, IAuthorizationService authorizationService)
    {
      _negocioService = negocioService;
      _categoriaNegocioService = categoriaNegocioService;
      _authorizationService = authorizationService;
    }

    public override void Configure()
    {
      Verbs(Http.POST);
      Routes("/categoria-negocio/assign_categoria_negocio_to_negocio");
      Summary(s =>
      {
        s.Summary = "Asignar Categorías de Negocio a Negocio";
        s.Description = "Asigna una lista de Categorías de Negocio a un Negocio";
        s.ExampleRequest = new AssignCategoriaNegocioToNegocioRequest
        {
          NegocioId = Guid.NewGuid(),
          CategoriaNegocioIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };
      });
    }

    public override async Task HandleAsync(AssignCategoriaNegocioToNegocioRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Asignar_Categorias_Negocio_A_Negocio"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var negocio = await _negocioService.GetByIdAsync(req.NegocioId);
      if (negocio == null)
      {
        AddError(r => r.NegocioId, "Negocio no encontrado");
      }

      foreach (var categoriaNegocioId in req.CategoriaNegocioIds)
      {
        var categoriaNegocio = await _categoriaNegocioService.GetByIdAsync(categoriaNegocioId);
        if (categoriaNegocio == null)
        {
          AddError(r => r.CategoriaNegocioIds, $"Categoría de Negocio con ID {categoriaNegocioId} no encontrada");
        }
      }

      ThrowIfAnyErrors();
      await _negocioService.AssignCategoriasNegocioToNegocioAsync(req.NegocioId, req.CategoriaNegocioIds);
      await SendOkAsync(ct);
    }
  }
}
