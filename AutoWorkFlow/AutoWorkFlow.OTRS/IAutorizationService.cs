﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoWorkFlow.OTRS
{
    public interface IAutorizationService
    {
        CookieCollection Login();

        void Logout();
    }
}

