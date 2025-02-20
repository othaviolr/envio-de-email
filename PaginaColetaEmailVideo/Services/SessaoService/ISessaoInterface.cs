using PaginaColetaEmailVideo.Models;

namespace PaginaColetaEmailVideo.Services.SessaoService
{
    public interface ISessaoInterface
    {
        UsuarioModel BuscarSessao();
        void CriarSessao(UsuarioModel usuario);
        void RemoverSessao();
    }
}
