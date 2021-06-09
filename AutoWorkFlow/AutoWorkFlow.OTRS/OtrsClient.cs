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
        private CookieCollection _cookie;

        public string Address { get; private set; }       
        public bool IsLogin { get; private set; }       

        public OtrsClient(string address, IAutorizationService autorizationService)
        {
            this.Address = address;
            _service = autorizationService;         
        }



        public bool Login(UserCredential credential) {
            IsLogin = false;
            _cookie = _service.LoginAsync(credential).Result;
            if (validateCookie(_cookie)) {
                IsLogin = true;                
            }
            return IsLogin;
        }
        

        /// <summary>
        /// Получить информацию по тикету
        /// </summary>       
        /// <param name="ticketId">Идентификатор тикета</param>
        /// <returns></returns>
        public async Task<OtrsTicketInfo> GetTicketInfoAsync(string ticketId)
        {
            var result = new OtrsTicketInfo();
            if (!IsLogin)
            {
                return null;
            }
            Uri uri = new Uri(string.Format("{0}?Action=AgentTicketZoom;TicketID={1}", this.Address, ticketId));
            string sitePage = await GetAsync(uri);
            OtrsTicketInfoParser.parse(sitePage, out result);
            return result;

        }

        /// <summary>
        /// Получить информацию по тикетам
        /// </summary>              
        /// <returns></returns>
        public async Task<List<OtrsTicketInfo>> GetTisketsInfoAsync()
        {
            List<int> idTiskets = await GetIdTicketAll();
            List<OtrsTicketInfo> ticketInfos = new List<OtrsTicketInfo>();
            ticketInfos = await GetTisketsInfoAsync(idTiskets);
            return ticketInfos;
        }

        /// <summary>
        /// Получить информацию по тикетам
        /// </summary>      
        /// <param name="idTiskets">Идентификаторы тикетов</param>
        /// <returns></returns>
        public async Task<List<OtrsTicketInfo>> GetTisketsInfoAsync( List<int> idTiskets)
        {
            if (!IsLogin)
            {
                return null;
            }
            List<OtrsTicketInfo> ticketInfos = new List<OtrsTicketInfo>();
            var tasks = new List<Task>();//
            foreach (int id in idTiskets)
            {
                tasks.Add(Task.Run(()=>{
                    var info = GetTicketInfoAsync(id.ToString()).Result;
                    ticketInfos.Add(info);
                }));                
            }
            await Task.WhenAll(tasks.ToArray());
            return ticketInfos;
        }


        /// <summary>
        /// Получить информацию по тикету
        /// </summary>            
        /// <returns></returns>
        public async Task<List<int>> GetIdTicketAll()
        {
            List<int> idTiskets = new List<int>();  
            if (!IsLogin)
            {
                return idTiskets;
            }
            OtrsTicketAgentParser agentParser = new OtrsTicketAgentParser();

            int startHit = 0;
            int count = 0;
            do
            {
                string page = await LoadPageAgentTisketAsync(this.Address, startHit);
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

        #region Private

        private async Task<string> GetAsync(Uri uri)
        {
            using (var client = CreateClient())
            {
                try
                {                  
                    var response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();
                    var page = await response.Content.ReadAsStringAsync();
                    return page;
                }
                catch (Exception ex)
                {                    
                    return string.Empty;
                }
            }
        }

        private bool validateCookie(CookieCollection cookie)
        {
            if (cookie.Count != 0
                && cookie[0].Name.Equals("OTRSAgentInterface")
                && !string.IsNullOrEmpty(cookie[0].Value))
            {
                return true;
            }
            return false;
        }

        private async Task<string> LoadPageAgentTisketAsync(string baseAddress, int startHit)
        {
            Uri uri = new Uri($"{baseAddress}?Action=AgentTicketResponsibleView;Filter=All;View=;SortBy=Age;OrderBy=Down;StartWindow=0;StartHit={startHit}");
            string sitePage =  await GetAsync(uri);
            return sitePage;
        }


        /// <summary>
        /// Создать HttpClienta для отправки запросов в OTRS
        /// </summary>       
        /// <returns></returns>
        private HttpClient CreateClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler()
            {
                CookieContainer = CreateCookieContainer()
            };
            HttpClient client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri(this.Address);
            return client;

        }


        private CookieContainer CreateCookieContainer()
        {
            CookieContainer container = new CookieContainer();
            container.Add(_cookie);
            return container;
        }

        

        #endregion
    }
}
