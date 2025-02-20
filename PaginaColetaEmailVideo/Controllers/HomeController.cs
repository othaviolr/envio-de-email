using Microsoft.AspNetCore.Mvc;
using PaginaColetaEmailVideo.Dto;
using PaginaColetaEmailVideo.Models;
using PaginaColetaEmailVideo.Services.EmailService;
using PaginaColetaEmailVideo.Services.SessaoService;
using PaginaColetaEmailVideo.Services.UsuarioService;
using System.Diagnostics;

namespace PaginaColetaEmailVideo.Controllers
{
    public class HomeController : Controller
    {

        private readonly IEmailInterface _emailInterface;
        private readonly IUsuarioInterface _usuarioInterface;
        private readonly ISessaoInterface _sessaoInterface;

        public HomeController(IEmailInterface emailInterface, IUsuarioInterface usuarioInterface, ISessaoInterface sessaoInterface)
        {
            _emailInterface = emailInterface;
            _usuarioInterface = usuarioInterface;
            _sessaoInterface = sessaoInterface;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Agradecimento(EmailModel InfoRecebida)
        {
            return View(InfoRecebida);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Sair()
        {

            _sessaoInterface.RemoverSessao();
            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<ActionResult> SalvarDadosCliente(EmailModel InfoRecebida)
        {

            if(ModelState.IsValid)
            {
                var registroFeito = await _emailInterface.SalvarDadosCliente(InfoRecebida);
                return View("Agradecimento", registroFeito);
            }
            else
            {
                return RedirectToAction("Index");
            }

            
        }




        [HttpPost]

        public async Task<ActionResult<UsuarioModel>> Login(LoginDto loginDto)
        {

            if (ModelState.IsValid)
            {
                var usuario = await _usuarioInterface.Login(loginDto);

                if(usuario.Id == 0)
                {
                    TempData["MensagemErro"] = "Credenciais inválidas!";
                    return View(loginDto);
                }
                else
                {
                    TempData["MensagemSucesso"] = "Usuário logado com sucesso!";


                    _sessaoInterface.CriarSessao(usuario);  


                    return RedirectToAction("Index", "Email");
                }

            }
            else
            {
                return View(loginDto);
            }


        }

    }
}
