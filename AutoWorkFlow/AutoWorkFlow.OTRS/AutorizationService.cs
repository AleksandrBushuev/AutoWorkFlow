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
        private CookieCollection _cookies;
        private UserCredential _credential;


        public AutorizationService(string address, UserCredential credential)
        {
            this.Address = address;
            this._credential = credential;
            _cookies = new CookieCollection();
        }

        public string Address { get; }


        /// <summary>
        /// Выполнить авторизацию
        /// </summary>
        /// <returns></returns>
        public CookieCollection Login()
        {

            if (_cookies.Count == 0)
            {
                _cookies = AuthorizeAsync().Result;
            }
            return _cookies;
        }


        public void Logout()
        {

        }

        /// <summary>
        /// Авторизация в OTRS
        /// </summary>
        /// <returns></returns>
        private async Task<CookieCollection> AuthorizeAsync()
        {
            using (var clientHandler = new HttpClientHandler())
            {
                using (var client = new HttpClient(clientHandler))
                {
                    var uri = new Uri(Address);
                    var response = await client.PostAsync(uri, CreateBody());
                    response.EnsureSuccessStatusCode();

                    return clientHandler.CookieContainer.GetCookies(uri);
                }

            }
        }
        /// <summary>
        /// Создать параметры авторизации в теле POST запроса
        /// </summary>
        /// <returns></returns>
        private HttpContent CreateBody()
        {
            return new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string,string>("Action","Login"),
                    new KeyValuePair<string,string>("RequestedURL",""),
                    new KeyValuePair<string,string>("Lang","ru"),
                    new KeyValuePair<string,string>("TimeOffset","-300"),
                    new KeyValuePair<string,string>("User", _credential.Login),
                    new KeyValuePair<string,string>("Password",_credential.Password),
            });
        }


    }
}
