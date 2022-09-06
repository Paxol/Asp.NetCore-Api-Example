using MediatR;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Newtonsoft.Json;
using System.Text;

namespace DAMWeb.Client
{
    public class ApiPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly HttpClient _httpClient;

        public ApiPipelineBehavior(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                TypeNameHandling = TypeNameHandling.All
            };

            string json = JsonConvert.SerializeObject(request, settings);
            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpRequestMessage message = new(HttpMethod.Post, "/api")
            {
                Content = jsonContent
            };
            message.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await _httpClient.SendAsync(message, cancellationToken);
            string jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);

            TResponse responseObject = JsonConvert.DeserializeObject<TResponse>(jsonResponse, settings);

            return responseObject;
        }
    }
}
