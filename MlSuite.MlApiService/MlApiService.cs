using MlSuite.Attributes;
using MlSuite.DTOs;
using RestSharp;
using System.Diagnostics;
using Attribute = MlSuite.DTOs.Attribute;

namespace MlSuite.MlApiServiceLib
{
    public class MlApiService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUrl;
        private IRestClient _mlClient;

        public MlApiService(string clientId, string clientSecret, string redirectUrl)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _redirectUrl = redirectUrl;

            _mlClient = new RestClient("https://api.mercadolibre.com");
        }

        public async Task<(int status, AccessTokenDto? data)> ExchangeCodeForFirstLogin(string code)
        {
            RestRequest exchangeRequest = new RestRequest("oauth/token")
                .AddJsonBody(new
                {
                    grant_type = "authorization_code",
                    client_id = _clientId,
                    client_secret = _clientSecret,
                    code,
                    redirect_uri =
#if DEBUG
                    "https://localhost:7089/v1/Auth/MlRedirect"
#else
                        _redirectUrl
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
                    client_id = _clientId,
                    client_secret = _clientSecret,
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

        /// <summary>
        /// Retorna os dados de um item no Mercado Livre, sem autenticação, retornando apenas os dados públicos.
        /// </summary>
        /// <param name="itemId">MLB do item a ser procurado</param>
        /// <returns>MLB sem informações restritas.</returns>
        public async Task<(int status, ItemRootDto? data)> GetItemById(string itemId)
        {
            RestRequest getQuestionRequest = new RestRequest($"/items/{itemId}");

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

        /// <summary>
        /// Retorna os dados de um item no Mercado Livre, com autenticação, incluindo dados sensíveis, disponíveis apenas aos usuários autorizados.
        /// </summary>
        /// <param name="accessToken">Access token de autenticação</param>
        /// <param name="itemId">MLB do item a ser procurado</param>
        /// <returns>MLB com informações restritas.</returns>
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

        public async Task<(int status, ItemRootDto? data)> _PostItem(string accessToken, ItemRootDto item)
        {
            throw new NotImplementedException();
            RestRequest getQuestionRequest = new RestRequest($"/items/{item.Id}")
                    .AddHeader("Authorization", $"Bearer {accessToken}")
                    .AddHeader("content-type", "application/json")
                    .AddJsonBody(item)
                ;

            RestResponse<ItemRootDto> response = await
                _mlClient.ExecutePostAsync<ItemRootDto>(getQuestionRequest);


            if (!response.IsSuccessful)
            {
                return ((int)response.StatusCode, response.Data ?? new ItemRootDto() { Error = response.ErrorMessage });
            }

            else
            {
                return ((int)response.StatusCode, response.Data);
            }
        }

        public async Task<(bool, string?)> AtualizaEstoqueDisponivel(string accessToken, string mlb, int estoqueDisponivel, ulong? variacao = null)
        {
            RestRequest request = new RestRequest();
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("content-type", "application/json");

            if (variacao is null)
            {
                request.Resource = $"items/{mlb}";
                request.AddJsonBody(new { available_quantity = estoqueDisponivel });
            }
            else
            {
                request.Resource = $"items/{mlb}/variations/{variacao}";
                Variation mlvar = new() { Id = (ulong)variacao, AvailableQuantity = estoqueDisponivel };

                request.AddJsonBody(mlvar);
            }

            RestResponse response = await _mlClient.ExecutePutAsync(request);

            if (!response.IsSuccessful)
            {
                return (false, response.ErrorMessage);
            }

            return (true, null);
        }

    }
}