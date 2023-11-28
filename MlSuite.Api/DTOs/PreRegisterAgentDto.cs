using System.Text.Json.Serialization;
using FluentValidation;

namespace MlSuite.Api.DTOs
{
	public class PreRegisterAgentDto
	{
		public string? Login { get; set; }
		public string? Email { get; set; }

		[JsonIgnore] public PreRegisterAgentDtoValidator Validator { get; set; } = new();

	}

	public class PreRegisterAgentDtoValidator : AbstractValidator<PreRegisterAgentDto>
	{
		public PreRegisterAgentDtoValidator()
        {
            RuleFor(x => x.Login)
                .NotNull().NotEmpty()
                .WithMessage("{PropertyName} não pode estar em branco");

            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("{PropertyName} não pode estar em branco")
                .EmailAddress()
                .WithMessage("{PropertyName} tem que ser um endereço de email válido");
        }
	}
}
