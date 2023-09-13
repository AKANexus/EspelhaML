using MlSuite.MlSynch.DTO;
using RestSharp;

namespace MlSuite.MlSynch.Services
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
                    redirect_uri =
#if DEBUG
                    "https://localhost:7089/v1/Auth/MlRedirect"
#else
                        _configuration.GetSection("SuperSecretSettings")["RedirectUrl"]
#endif
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

        public async Task<(int status, MeDto? data)> GetMeInfo(string accessToken)
        {
            RestRequest getMeInfoRequest = new RestRequest("users/me")
                .AddHeader("Authorization", $"Bearer {accessToken}");

            RestResponse<MeDto> response = await
                _mlClient.ExecuteGetAsync<MeDto>(getMeInfoRequest);

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
                return ((int)response.StatusCode, response.Data ?? new QuestionRootDto() { Error = response.ErrorMessage });
            }

            else
            {
                return ((int)response.StatusCode, response.Data);
            }
        }

        public async Task<(int status, OrderRootDto? data)> GetOrderById(string accessToken, string orderId)
        {
            RestRequest getOrderRequest = new RestRequest($"/orders/{orderId}")
                    .AddHeader("Authorization", $"Bearer {accessToken}")
                ;

            RestResponse<OrderRootDto> response = await
                _mlClient.ExecuteGetAsync<OrderRootDto>(getOrderRequest);

            if (!response.IsSuccessful)
            {
                
                return ((int)response.StatusCode, response.Data ?? new OrderRootDto { Error = response.ErrorMessage });
            }

            else
            {
                return ((int)response.StatusCode, response.Data);
            }
        }

        public async Task<(int status, ShipmentDto? data)> GetShipmentById(string accessToken, string shipmentId)
        {
            RestRequest getOrderRequest = new RestRequest($"/shipments/{shipmentId}")
                    .AddHeader("Authorization", $"Bearer {accessToken}")
                ;

            RestResponse<ShipmentDto> response = await
                _mlClient.ExecuteGetAsync<ShipmentDto>(getOrderRequest);

            if (!response.IsSuccessful)
            {
                return ((int)response.StatusCode, response.Data ?? new ShipmentDto() { Error = response.ErrorMessage });
            }

            else
            {
                return ((int)response.StatusCode, response.Data);
            }
        }

        public async Task<(int status, ItemRootDto? data)> GetItemById(string accessToken, string itemId)
        {
            RestRequest getQuestionRequest = new RestRequest($"/items/{itemId}")
                    .AddHeader("Authorization", $"Bearer {accessToken}")
                    //.AddQueryParameter("api_version", "4")
                ;

            RestResponse<ItemRootDto> response = await
                _mlClient.ExecuteGetAsync<ItemRootDto>(getQuestionRequest);


            if (!response.IsSuccessful)
            {
                return ((int)response.StatusCode, response.Data ?? new ItemRootDto() { Error = response.ErrorMessage });
            }

            else
            {
                return ((int)response.StatusCode, response.Data);
            }
        }

    }
}
