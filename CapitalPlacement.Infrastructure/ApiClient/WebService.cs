using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CapitalReplacement.Application.DTOS;
using CapitalReplacement.Application.Interfaces.Application;

namespace CapitalReplacement.Infrastructure.ApiClient
{
    public class WebService : IWebService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<WebService> _logger;

        public WebService(IHttpClientFactory httpClient, ILogger<WebService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<CustomerInfoDtos> CallWebServiceEndpoint(WebDto web, string httpClientName = null,Dictionary<string, string> headers = null)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(web.httpMethod, web.url);

                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        requestMessage.Headers.Add(item.Key, item.Value);
                    }
                }

                requestMessage.Content = new StringContent(web.payload, Encoding.UTF8, web.contentType);
                var client = default(HttpClient);

                if (!string.IsNullOrWhiteSpace(httpClientName))
                    client = _httpClient.CreateClient(httpClientName);
                else
                    client = _httpClient.CreateClient();

                var response = await client.SendAsync(requestMessage);
                var result_ = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($" succesfully gotten response {result_}");
                if (result_ == null)
                    return default(CustomerInfoDtos);
                return JsonSerializer.Deserialize<CustomerInfoDtos>(result_);

            }
            catch (Exception ex) 
            {
                _logger.LogInformation($"Error: {ex.Message}");
                return default(CustomerInfoDtos);
            }
        }

    }

}
