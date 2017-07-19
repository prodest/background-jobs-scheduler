using BackgroundJobsScheduler.Common.Base;
using System;
using System.Net.Http;

namespace BackgroundJobsScheduler.Jobs
{
    public class RequestApiProcessoEletronico
    {
        private IClientAccessTokenProvider _clientAccessTokenProvider;

        public RequestApiProcessoEletronico(IClientAccessTokenProvider clientAccessTokenProvider)
        {
            _clientAccessTokenProvider = clientAccessTokenProvider;
        }

        public void NotificacoesRequest()
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

                using (HttpClient httpClient = new HttpClient(handler))
                {
                    httpClient.SetBearerToken(_clientAccessTokenProvider.AccessToken);
                    httpClient.PutAsync($"{Environment.GetEnvironmentVariable("UrlApiProcessoEletronico")}/notificacoes", null);
                }
            }
        }
    }
}
