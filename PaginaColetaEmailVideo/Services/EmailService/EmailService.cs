using Microsoft.EntityFrameworkCore;
using PaginaColetaEmailVideo.Data;
using PaginaColetaEmailVideo.Models;
using System.Net;
using System.Net.Mail;

namespace PaginaColetaEmailVideo.Services.EmailService
{
    public class EmailService : IEmailInterface
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public EmailService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        public bool EnviarEmail(string email, string mensagem, string assunto)
        {
            try
            {
                // Lendo configurações do appsettings.json
                string host = _configuration["SMTP:Host"];
                string nome = _configuration["SMTP:Nome"];
                string username = _configuration["SMTP:Username"];
                string senha = _configuration["SMTP:Senha"];
                int porta = _configuration.GetValue<int>("SMTP:Porta");

                // Criando a mensagem de e-mail
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(username, nome),
                    Subject = assunto,
                    Body = mensagem,
                    IsBodyHtml = false, // Desabilite o HTML por enquanto
                    Priority = MailPriority.High
                };

                mail.To.Add(email);

                // Configurando o cliente SMTP
                using (SmtpClient smtp = new SmtpClient(host, porta))
                {
                    smtp.Credentials = new NetworkCredential(username, senha);
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true; // SSL é necessário para o Outlook
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    Console.WriteLine("Tentando enviar e-mail...");
                    smtp.Send(mail);
                    Console.WriteLine("E-mail enviado com sucesso!");

                    return true;
                }
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"Erro SMTP: {smtpEx.StatusCode} - {smtpEx.Message}");
                Console.WriteLine($"StackTrace: {smtpEx.StackTrace}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral ao enviar e-mail: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return false;
            }
        }


        public async Task<EmailModel> ListarEmailPorId(int id)
        {
            try
            {

                var registroEmail = await _context.Emails.FirstOrDefaultAsync(email => email.Id == id);

                return registroEmail;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<EmailModel>> ListarEmails(string pesquisar = null)
        {

            List<EmailModel> registrosEmails = new List<EmailModel>();
            try
            {

                if (pesquisar == null)
                {

                    registrosEmails = await _context.Emails.ToListAsync();
                }
                else
                {
                    registrosEmails = await _context.Emails
                        .Where(email => email.Nome.Contains(pesquisar) ||
                               email.Email.Contains(pesquisar))
                        .ToListAsync();
                }

                return registrosEmails;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EmailModel> SalvarDadosCliente(EmailModel infoRecebida)
        {
            try
            {

                _context.Add(infoRecebida);
                await _context.SaveChangesAsync();

                return infoRecebida;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}