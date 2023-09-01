using MLPerguntas.Models.Dto.MercadoLivre;
using RestSharp;

namespace MLPerguntas.Services
{
    public class MlApiService
    {
        private readonly IConfiguration _configuration;
        private IRestClient _mlClient;

        public MlApiService(IConfiguration configuration)
        {
            _configuration = configuration;
            _mlClient = new RestClient("https://api.mercadolibre.com");
        }

        public async Task<(int status, AccessTokenDto? data)> ExchangeCodeForFirstLogin(string code)
        {
            RestRequest exchangeRequest = new RestRequest("oauth/token")
                .AddJsonBody(new
                {
                    grant_type = "authorization_code",
                    client_id = _configuration.GetSection("SuperSecretSettings")["ClientId"],
                    client_secret = _configuration.GetSection("SuperSecretSettings")["ClientSecret"],
                    code,
                    redirect_uri = _configuration.GetSection("SuperSecretSettings")["RedirectUrl"]
                },
                    ContentType.Json);

            RestResponse<AccessTokenDto> response = await
                _mlClient.ExecutePostAsync<AccessTokenDto>(exchangeRequest);

            if (!response.IsSuccessful)
            {
                return ((int)response.StatusCode, response.Data ?? null);
            }

            else
            {
                return ((int)response.StatusCode, response.Data);
            }
        }

        public async Task<(int status, AccessTokenDto? data)> RefreshAccessToken(string refreshToken)
        {
            RestRequest refreshAccessTokenRequest = new RestRequest("oauth/token")
                .AddJsonBody(new
                {
                    grant_type = "refresh_token",
                    client_id = _configuration.GetSection("SuperSecretSettings")["ClientId"],
                    client_secret = _configuration.GetSection("SuperSecretSettings")["ClientSecret"],
                    refresh_token = refreshToken
                },
                    ContentType.Json);

            RestResponse<AccessTokenDto> response = await
                _mlClient.ExecutePostAsync<AccessTokenDto>(refreshAccessTokenRequest);

            if (!response.IsSuccessful)
            {
                return ((int)response.StatusCode, response.Data ?? null);
            }

            else
            {
                return ((int)response.StatusCode, response.Data);
            }

        }

        public async Task<(int status, QuestionRootDto? data)> GetQuestionById(string accessToken, string questionId)
        {
            RestRequest getQuestionRequest = new RestRequest($"/questions/{questionId}")
                .AddHeader("Authorization", $"Bearer {accessToken}")
                .AddQueryParameter("api_version", "4")
                ;

            RestResponse<QuestionRootDto> response = await
                _mlClient.ExecuteGetAsync<QuestionRootDto>(getQuestionRequest);

            if (!response.IsSuccessful)
            {
                return ((int)response.StatusCode, response.Data ?? null);
            }

            else
            {
                return ((int)response.StatusCode, response.Data);
            }
        }

    }
}
