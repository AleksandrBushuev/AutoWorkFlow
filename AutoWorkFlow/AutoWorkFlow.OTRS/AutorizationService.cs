using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoWorkFlow.OTRS
{
    /// <summary>
    /// Служба аторизации OTRS
    /// </summary>
    public class AutorizationService : IAutorizationService
    {        
        public string Address { get; }

        public AutorizationService(string address)
        {
            this.Address = address;           
        }

        /// <summary>
        /// Авторизация в OTRS
        /// </summary>
        /// <returns></returns>
        public async Task<CookieCollection> LoginAsync(UserCredential credential)
        {
            using (var clientHandler = new HttpClientHandler())
            {
                using (var client = new HttpClient(clientHandler))
                {
                    var uri = new Uri(Address);
                    var response = await client.PostAsync(uri, CreateBody(credential.Login, credential.Password));
                    response.EnsureSuccessStatusCode();
                    return clientHandler.CookieContainer.GetCookies(uri);
                }

            }
        }

        public void Logout()
        {

        }

      
      
        /// <summary>
        /// Создать параметры авторизации в теле POST запроса
        /// </summary>
        /// <returns></returns>
        private HttpContent CreateBody(string login, string password)
        {
            return new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string,string>("Action","Login"),
                    new KeyValuePair<string,string>("RequestedURL",""),
                    new KeyValuePair<string,string>("Lang","ru"),
                    new KeyValuePair<string,string>("TimeOffset","-300"),
                    new KeyValuePair<string,string>("User", login),
                    new KeyValuePair<string,string>("Password", password),
            });
        }




        
    }
}
