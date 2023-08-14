using RestSharp;

namespace EspelhaML.Services
{
    public class MlApiService
    {
        private readonly IConfiguration _configuration;
        private IRestClient _mlClient;

        public MlApiService(IConfiguration configuration)
        {
            _configuration = configuration;
            _mlClient = new RestClient("https://api.mercadolibre.com/");
        }
        public Task<int> ExchangeCodeForFirstLogin(string code)
        {
            RestRequest exchangeRequest = new RestRequest()
                .AddJsonBody(new
                    {
                        grant_type = "authorization_code",
                        client_id = "",
                        client_secret = "",
                        code = code,
                        redirect_uri = _configuration.GetSection("SuperSecretSettings")["RedirectUrl"]
                    },
                    ContentType.Json);

        }
    }
}
