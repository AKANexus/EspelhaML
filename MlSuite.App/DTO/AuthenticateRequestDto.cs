using FluentValidation;
using System.Text.Json.Serialization;

namespace MlSuite.App.DTO
{
    public class AuthenticateRequestDto
    {
        public AuthenticateRequestDto(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; set; }
        public string Password { get; set; }
        //public Role Role { get; set; }
        [JsonIgnore]
        public AuthenticateRequestValidator Validator { get; set; } = new();
    }

    public class AuthenticateRequestValidator : AbstractValidator<AuthenticateRequestDto>
    {
        public AuthenticateRequestValidator()
        {
            RuleFor(x => x.Login)
                .NotNull()
                .WithMessage(ErrorMessagesStrings.NotNullMessage)
                .Matches(@"^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+$")
                .WithMessage("O login informado não é válido");

            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage(ErrorMessagesStrings.NotNullMessage);

            //RuleFor(x => x.Role)
            //	.NotNull()
            //	.WithMessage(ErrorMessagesStrings.NotNullMessage)
            //	.IsInEnum()
            //	.WithMessage("Fui à padaria comprar um Bubbaloo");
        }
    }
}
