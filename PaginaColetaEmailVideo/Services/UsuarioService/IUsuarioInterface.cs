using PaginaColetaEmailVideo.Dto;
using PaginaColetaEmailVideo.Models;

namespace PaginaColetaEmailVideo.Services.UsuarioService
{
    public interface IUsuarioInterface
    {
        Task<UsuarioModel> Cadastrar(UsuarioCriacaoDto usuarioCriacaoDto);
        Task<UsuarioModel> Login(LoginDto loginDto);
    }
}