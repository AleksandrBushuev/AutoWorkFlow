using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Получить информацию по тикетам
        /// </summary>
        /// <param name="baseAddress">Базовый адрес OTRS</param>
        /// <param name="idTiskets">Идентификаторы тикетов</param>
        /// <returns></returns>
        public async Task<List<OtrsTicketInfo>> GetTisketsInfoAsync(string baseAddress)
        {
            List<int> idTiskets = await GetTicketsID(baseAddress);
            List<OtrsTicketInfo> ticketInfos = new List<OtrsTicketInfo>();
            foreach (int id in idTiskets)
            {
                var info = await GetTicketInfoAsync(baseAddress, id.ToString());
                ticketInfos.Add(info);
            }
            return ticketInfos;
        }

        /// <summary>
        /// Получить информацию по тикетам
        /// </summary>
        /// <param name="baseAddress">Базовый адрес OTRS</param>
        /// <param name="idTiskets">Идентификаторы тикетов</param>
        /// <returns></returns>
        public async Task<List<OtrsTicketInfo>> GetTisketsInfoAsync(string baseAddress, List<int> idTiskets)
        {
            List<OtrsTicketInfo> ticketInfos = new List<OtrsTicketInfo>();
            foreach (int id in idTiskets)
            {
                var info = await GetTicketInfoAsync(baseAddress, id.ToString());
                ticketInfos.Add(info);
            }
            return ticketInfos;
        }


        /// <summary>
        /// Получить информацию по тикету
        /// </summary>
        /// <param name="baseAddress">Базовый адрес OTRS</param>        
        /// <returns></returns>
        public async Task<List<int>> GetTicketsID(string baseAddress)
        {
            OtrsTicketAgentParser agentParser = new OtrsTicketAgentParser();
            List<int> idTiskets = new List<int>();
            int startHit = 0;
            int count = 0;
            do
            {
                string page = await LoadAgentTisketAsync(baseAddress, startHit);
                if (count == 0)
                {
                    count = agentParser.GetTicketCount(page);
                }
                startHit += 10;
                idTiskets.AddRange(agentParser.GetTicketsId(page));
            } while (startHit <= count);

            var result = idTiskets.Distinct().ToList();//удалить дубликаты
            return result;
        }            
       

        /// <summary> 
        /// Загрузить страницу AgentTicketResponsibleView
        /// </summary>
        /// <param name="baseAddress">Базовый адрес OTRS</param>
        /// <param name="startHit">Начальный номер тикета для загрузки таблицы</param>
        /// <returns></returns>
        private async Task<string> LoadAgentTisketAsync(string baseAddress, int startHit)
        {
            Uri uri = new Uri($"{baseAddress}?Action=AgentTicketResponsibleView;Filter=All;View=;SortBy=Age;OrderBy=Down;StartWindow=0;StartHit={startHit}");
            string sitePage;
            using (var client = CreateClient(baseAddress))
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                sitePage = await response.Content.ReadAsStringAsync();
            }
            return sitePage;
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



        private CookieContainer CreateCookieContainer()
        {
            CookieContainer container = new CookieContainer();
            container.Add(Login());
            return container;
        }     
        
       
        private CookieCollection Login()
        {
            return _service.Login();//Получить cookie в службе авторизации
        }

    }
}
