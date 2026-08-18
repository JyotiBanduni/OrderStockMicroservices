using System.Net;
using System.Net.Http.Json;
using OrderService.DTOs;

namespace OrderService.Services
{
    public class ProductServiceClient
        : IProductServiceClient
    {
        private readonly HttpClient _httpClient;

        public ProductServiceClient(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductStockResponseDto?>
            GetProductAsync(Guid productId)
        {
            var response =
                await _httpClient.GetAsync(
                    $"api/product/{productId}");

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<ProductStockResponseDto>();
        }

        public async Task<bool> ReduceStockAsync(
            Guid productId,
            int quantity)
        {
            var request = new
            {
                quantity
            };

            var response =
                await _httpClient.PatchAsJsonAsync(
                    $"api/product/{productId}/stock",
                    request);

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}