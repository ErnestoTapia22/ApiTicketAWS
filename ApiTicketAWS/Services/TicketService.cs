using ApiTicketAWS.Models;
using Microsoft.Net.Http.Headers;

namespace ApiTicketAWS.Services
{
    public class TicketService : ITicketService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TicketService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetResponseModel> getRequest(string accessToken)
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt";
            StreamWriter? sw = null;
            sw = new StreamWriter(filepath, true);
            try
            {
               
                GetResponseModel response = new GetResponseModel();
                var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            "https://api-stg.soporte.protectasecuritycloud.pe/jira/v1/issues?limit=50&offset=0&jql=(project=GCS or project=OV) AND key in (OV-5215,GCS-8028)")
                {
                    Headers =
                            {
                                { HeaderNames.Accept, "application/json" },

                            }
                };
                var httpClient = _httpClientFactory.CreateClient("Jira");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{accessToken}");
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var contentStream =
                          await httpResponseMessage.Content.ReadAsStringAsync();

                    response.result = contentStream;
                }
                return response;
            }
            catch (Exception ex)
            {
                sw.WriteLine(DateTime.Now.ToString() + "- GetRequest: " + ex.ToString());
                throw;
            }
            finally
            {
                sw.Flush();
                sw.Close();

            }
        }

    }
}
