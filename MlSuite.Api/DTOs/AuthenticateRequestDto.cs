namespace MlSuite.Api.DTOs
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
	}
}
