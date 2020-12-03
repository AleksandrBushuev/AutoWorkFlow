using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWorkFlow.OTRS
{
    /// <summary>
    /// Учетные данные пользователя
    /// </summary>
    public class UserCredential
    {
        public UserCredential(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }
        public string Login { get; }
        public string Password { get; }
    }
}
