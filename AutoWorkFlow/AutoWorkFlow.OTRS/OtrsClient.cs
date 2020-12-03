using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoWorkFlow.OTRS
{
    /// <summary>
    /// Клиент сайта OTRS
    /// </summary>
    public class OtrsClient
    {
        private IAutorizationService _service;

        public OtrsClient(IAutorizationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Получить информацию по тикету
        /// </summary>
        /// <param name="baseAddress">Базовый адрес OTRS</param>
        /// <param name="ticketId">Идентификатор тикета</param>
        /// <returns></returns>
        public async Task<OtrsTicketInfo> GetTicketInfoAsync(string baseAddress, string ticketId)
        {
            Uri uri = new Uri(string.Format("{0}?Action=AgentTicketZoom;TicketID={1}", baseAddress, ticketId));
            string sitePage;
            using (var client = CreateClient(baseAddress))
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                sitePage = await response.Content.ReadAsStringAsync();
            }
            return OtrsTicketInfoParser.convertToOtrsTicketInfo(sitePage);

        }


        /// <summary>
        /// Создать HttpClienta для отправки запросов в OTRS
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <returns></returns>
        private HttpClient CreateClient(string baseAddress)
        {
            HttpClientHandler clientHandler = new HttpClientHandler()
            {
                CookieContainer = CreateCookieContainer()
            };
            HttpClient client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri(baseAddress);
            return client;

        }

        /// <summary>
        /// Получить cookie в службе авторизации
        /// </summary>
        /// <returns></returns>
        private CookieCollection Login()
        {
            return _service.Login();
        }

        private CookieContainer CreateCookieContainer()
        {
            CookieContainer container = new CookieContainer();
            container.Add(Login());
            return container;
        }


    }
}
