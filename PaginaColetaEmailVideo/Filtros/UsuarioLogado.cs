using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaginaColetaEmailVideo.Models;

public class UsuarioLogado : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata
            .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

        if (!allowAnonymous)
        {
            string sessaoUsuario = context.HttpContext.Session.GetString("UsuarioAtivo");

            if (string.IsNullOrEmpty(sessaoUsuario))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "Home" },
                    {"Action", "Index"}
                });
            }
            else
            {
                UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);

                if (usuario == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        {"controller", "Home" },
                        {"Action", "Index"}
                    });
                }
            }
        }

        base.OnActionExecuting(context);
    }
}