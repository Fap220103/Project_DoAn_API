using Application.Features.Products.Queries;
using Application.Services.Externals;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Infrastructure.GeneralReview
{
    public class GeneralReview : IGeneralReview
    {
        private readonly HttpClient _httpClient;

        public GeneralReview(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8000");
        }

        public async Task<int> GetGeneralReview(string content)
        {
            var request = new PredictRequest { Text = content };
            var response = await _httpClient.PostAsJsonAsync("/predict", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PredictResponse>();
                return result.Prediction;
            }

            throw new Exception("Lỗi khi gọi API dự đoán từ FastAPI");
        }
    }

}
