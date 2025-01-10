using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.HorarioNegocio
{
  public class GetHorarioNegocioByIdEndpoint : Endpoint<GetHorarioNegocioByIdRequest, HorarioNegocioDto>
  {
    private readonly IHorarioNegocioService _horarioNegocioService;
    private readonly IAuthorizationService _authorizationService;

    public GetHorarioNegocioByIdEndpoint(IHorarioNegocioService horarioNegocioService, IAuthorizationService authorizationService)
    {
      _horarioNegocioService = horarioNegocioService;
      _authorizationService = authorizationService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/horario-negocio/{HorarioNegocioId:guid}");
      Summary(s =>
      {
        s.Summary = "Obtener horario de negocio por ID";
        s.Description = "Obtiene un horario de negocio de la base de datos por su ID";
        s.ExampleRequest = new GetHorarioNegocioByIdRequest
        {
          HorarioNegocioId = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new HorarioNegocioDto
        {
          IdHorario = Guid.NewGuid(),
          IdNegocio = Guid.NewGuid(),
          Dia = 1,
          HoraApertura = new TimeSpan(8, 0, 0),
          HoraCierre = new TimeSpan(20, 0, 0)
        };
        s.Responses[404] = "Horario de negocio no encontrado";
        s.Responses[200] = "Horario de negocio encontrado";
      });
    }

    public override async Task HandleAsync(GetHorarioNegocioByIdRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Horario_Negocio"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var horarioNegocio = await _horarioNegocioService.GetByIdAsync(req.HorarioNegocioId);
      if (horarioNegocio == null)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      var horarioNegocioDto = new HorarioNegocioDto
      {
        IdHorario = horarioNegocio.IdHorario,
        IdNegocio = horarioNegocio.IdNegocio,
        Dia = horarioNegocio.Dia,
        HoraApertura = horarioNegocio.HoraApertura,
        HoraCierre = horarioNegocio.HoraCierre
      };

      await SendOkAsync(horarioNegocioDto, ct);
    }
  }
}
