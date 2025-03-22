namespace reymani_web_api.Services.EmailServices.Templates;

public class ConfirmationEmailTemplate : IEmailTemplate
{
  public string GetEmailTemplate()
  {
    return
      """
      <!DOCTYPE html>
      <html>
        <head>
          <meta charset="UTF-8" />
          <title>Confirma tu cuenta</title>
          <style>
            body {
              font-family: Arial, sans-serif;
              background-color: #f4f4f4;
              text-align: center;
              padding: 20px;
            }
            .container {
              background-color: #ffffff;
              padding: 20px;
              border-radius: 8px;
              box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
              max-width: 500px;
              margin: 0 auto;
            }
            h1 {
              color: #333;
            }
            p {
              font-size: 16px;
              color: #555;
            }
            .code {
              font-size: 24px;
              font-weight: bold;
              color: #007bff;
              background-color: #e9ecef;
              padding: 10px;
              display: inline-block;
              border-radius: 5px;
              margin: 15px 0;
            }
            .footer {
              font-size: 12px;
              color: #777;
              margin-top: 20px;
            }
          </style>
        </head>
        <body>
          <div class="container">
            <h1>Bienvenido a Reymani!</h1>
            <p>
              Gracias por registrarte. Para completar tu registro, por favor utiliza el
              código de confirmación a continuación:
            </p>
            <div class="code">{{confirmationCode}}</div>
            <p>Si no solicitaste esto, por favor ignora este correo electrónico.</p>
            <p class="footer">© 2025 Reymani. Todos los derechos reservados.</p>
          </div>
        </body>
      </html>
      """;
  }
}