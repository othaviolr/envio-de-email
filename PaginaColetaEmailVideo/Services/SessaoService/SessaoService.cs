using Newtonsoft.Json;
using PaginaColetaEmailVideo.Models;

namespace PaginaColetaEmailVideo.Services.SessaoService
{
    public class SessaoService : ISessaoInterface
    {
        private readonly IHttpContextAccessor _httpAcessor;
        public SessaoService(IHttpContextAccessor httpAcessor)
        {
            _httpAcessor = httpAcessor;
        }

        public UsuarioModel BuscarSessao()
        {
            string sessaoUsuario = _httpAcessor.HttpContext.Session.GetString("UsuarioAtivo");
            if (sessaoUsuario == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);
        }

        public void CriarSessao(UsuarioModel usuario)
        {
            string usuarioJson = JsonConvert.SerializeObject(usuario);
            _httpAcessor.HttpContext.Session.SetString("UsuarioAtivo", usuarioJson);
        }

        public void RemoverSessao()
        {
            _httpAcessor.HttpContext.Session.Remove("UsuarioAtivo");
        }
    }
}