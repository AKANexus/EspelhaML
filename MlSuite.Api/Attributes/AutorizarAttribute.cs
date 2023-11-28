using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MlSuite.Api.DTOs;
using MlSuite.Domain;

namespace MlSuite.Api.Attributes
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class AutorizarAttribute : Attribute, IAuthorizationFilter
	{
		//public AutorizarAttribute()
		//{

		//}

		public void OnAuthorization(AuthorizationFilterContext authContext)
		{
			//Pula a autorização se a ação tiver o atributo [Anonymous]
			if (authContext.ActionDescriptor.EndpointMetadata.OfType<AnônimoAttribute>().Any())
				return; //Segue o pipeline

			UserInfo? userInfo = (UserInfo?)authContext.HttpContext.Items["userInfo"];

			if (userInfo == null)
			{
				authContext.Result = new UnauthorizedObjectResult(new RetornoDto("Não Autorizado",
					"Access token não autorizada para acessar esse endpoint"));
			}
		}
	}
}
