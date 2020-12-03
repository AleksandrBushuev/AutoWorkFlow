using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AutoWorkFlow.TFS
{
    /// <summary>
    /// Клиент TFS
    /// </summary>
    public class TfsClient
    {
        /// <summary>
        /// Получить информацию по рабочему элементу TFS
        /// </summary>
        /// <param name="baseAdsressProject"> Базовый адрес проекта TFS, например https://host/tfs/company/_git/Project</>
        /// <param name="personalaccesstoken">Личный маркер доступа</param>
        /// <param name="workIteam">Номер рабочего элемента</param>
        /// <returns></returns>
        public async Task<TfsWorkIteamInfo> GetWorkIteamInfoAsync(string baseAdsressProject, string personalaccesstoken, string workIteam)
        {
            using (HttpClient client = CreateClient(personalaccesstoken))
            {
                string content;
                using (HttpResponseMessage response = await client.GetAsync($"{ baseAdsressProject}/_apis/wit/workItems/{workIteam}"))
                {
                    response.EnsureSuccessStatusCode();
                    content = await response.Content.ReadAsStringAsync();
                }
                return TfsWorkIteamInfoParser.ConvertToTfsWorkIteamInfo(content);
            }
        }
        /// <summary>
        /// Создать HttpClient для отправки запросов в TFS
        /// </summary>     
        private HttpClient CreateClient(string personalaccesstoken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));
            return client;

        }




    }
}
