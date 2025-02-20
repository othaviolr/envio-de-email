using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaginaColetaEmailVideo.Dto;
using PaginaColetaEmailVideo.Models;
using PaginaColetaEmailVideo.Services.UsuarioService;

namespace PaginaColetaEmailVideo.Controllers
{
    [UsuarioLogado] // Aplica o filtro a todas as actions da controller
    public class UsuarioController : Controller
    {
        private readonly IUsuarioInterface _usuarioInterface;

        public UsuarioController(IUsuarioInterface usuarioInterface)
        {
            _usuarioInterface = usuarioInterface;
        }

        [AllowAnonymous] // Permite acesso sem autenticação
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous] // Permite acesso sem autenticação
        public async Task<ActionResult<UsuarioModel>> Cadastrar(UsuarioCriacaoDto usuarioCriacaoDto)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _usuarioInterface.Cadastrar(usuarioCriacaoDto);

                if (usuario != null)
                {
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
                    return RedirectToAction("Index", "Email");
                }
                else
                {
                    TempData["MensagemErro"] = "Ocorreu um erro no momento do cadastro!";
                    return View(usuarioCriacaoDto);
                }
            }
            else
            {
                return View(usuarioCriacaoDto);
            }
        }
    }
}