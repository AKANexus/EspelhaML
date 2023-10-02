using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MlSuite.App.DTO;
using MlSuite.Domain;
using MlSuite.Domain.Enums;

namespace MlSuite.App.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthoriseAttribute : Attribute, IAuthorizationFilter
{
    private readonly Role[] _allowedRoles;

    public AuthoriseAttribute(params Role[]? allowedRoles)
    {
        _allowedRoles = allowedRoles ?? Array.Empty<Role>();
    }

    //Chamado no ínício do pipeline de filtro para confirmar se a solicitação está autorizada.
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //Pula a autorização se a ação tiver o atributo [Anonymous]
        if (context.ActionDescriptor.EndpointMetadata.OfType<AnonymousAttribute>().Any())
            return; //Segue o pipeline

        //Procura se existe uma account no context. Isso virá do middleware JWT que leu a token no header.
        //Isso indica se o usuário está logado ou não.
        AccountBase? account = (AccountBase?)context.HttpContext.Items["Account"];

        //Verifica se a conta existe, e se a role informada bate com as allowedRoles
        if (account is null || _allowedRoles.Any() && !_allowedRoles.Contains(account.Role))
        {
            //Caso negativo, altera o resultado para Unauthorized
            //context.Result = new UnauthorizedObjectResult(new ResponseError("Não autorizado", "A autorização falhou", ResponseErrorCode.Unauthorised));
            context.Result = new RedirectToActionResult("Login", "Auth", new {status = "NotLoggedIn" });
        }
        //Caso positivo, segue o pipeline
    }
}