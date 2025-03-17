namespace reymani_web_api.Services.EmailServices.Templates;

public class PasswordResetEmailTemplate : IEmailTemplate
{
  public string GetEmailTemplate()
  {
    return
      """
      <!DOCTYPE html>
      <html lang="es">
        <head>
          <meta charset="UTF-8" />
          <meta name="viewport" content="width=device-width, initial-scale=1.0" />
          <title>Solicitud de restablecimiento de contraseña</title>
          <style>
            body {
              font-family: Arial, sans-serif;
              background-color: #f4f4f4;
              margin: 0;
              padding: 20px;
            }
            .container {
              max-width: 600px;
              margin: 0 auto;
              background: #ffffff;
              padding: 20px;
              border-radius: 8px;
              box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            }
            h2 {
              color: #333;
            }
            p {
              color: #555;
            }
            .code {
              font-size: 24px;
              font-weight: bold;
              color: #007bff;
              text-align: center;
              margin: 20px 0;
            }
            .footer {
              margin-top: 20px;
              font-size: 12px;
              color: #777;
              text-align: center;
            }
          </style>
        </head>
        <body>
          <div class="container">
            <h2>Solicitud de restablecimiento de contraseña</h2>
            <p>Estimado(a) <strong>{{UserName}}</strong>,</p>
            <p>
              Hemos recibido una solicitud para restablecer tu contraseña. Utiliza el
              siguiente código para completar el proceso:
            </p>
            <div class="code">{{ResetCode}}</div>
            <p>Si no solicitaste esto, por favor ignora este correo electrónico.</p>
            <p>
              Gracias, <br />
              El equipo de soporte
            </p>
            <div class="footer">&copy; 2025 Reymani. Todos los derechos reservados.</div>
          </div>
        </body>
      </html>
      """;
  }
}
