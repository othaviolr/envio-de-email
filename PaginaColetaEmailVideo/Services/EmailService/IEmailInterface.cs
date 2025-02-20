using PaginaColetaEmailVideo.Models;

namespace PaginaColetaEmailVideo.Services.EmailService
{
    public interface IEmailInterface
    {
        Task<EmailModel> SalvarDadosCliente(EmailModel infoRecebida);
        Task<List<EmailModel>> ListarEmails(string pesquisar = null);
        Task<EmailModel> ListarEmailPorId(int id);

        bool EnviarEmail(string email, string mensagem, string assunto);
    }
}
