using DomainModel.Entities;
using DomainModel.Interfaces.Services;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificacaoEmail
{
  public class NotificadorGmail : INotificacaoService
  {
    private const string dominio = "smtp.gmail.com";
    private const int porta = 587;
    private const string conta = "capsuladotempoweb@gmail.com";

    private string senha;
    public NotificadorGmail(string _senha)
    {
      senha = _senha;
    }
    public async Task NotificarCriacaoCapsula(CapsulaDoTempo capsula)

    {
      try
      {
        if (!string.IsNullOrEmpty(capsula.Email))
        {
          MailMessage e = new MailMessage()
          {
            From = new MailAddress(conta, "Cápsula do Tempo")
          };
          e.To.Add(new MailAddress(capsula.Email));

          e.Subject = "Cápsula do Tempo - Cápsula criada";
          e.Body = capsula.Id + " " + System.Net.WebUtility.UrlEncode(capsula.ChaveCapsula);
          e.IsBodyHtml = true;

          using (SmtpClient smtp = new SmtpClient(dominio, porta))
          {
            smtp.Credentials = new NetworkCredential(conta, senha);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(e);
          }
        }
      }
      catch (Exception ex)
      {
        // não posso parar o processo porque não foi possivel notificar
      }
    }
  }
}
