
using EvanBackstageApi.Basic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAuthorizeInfo.Controllers
{
    public class AuthorizationController : ControllerBase
    {
        public string AccessDenied()
        {
            return ResultType.Error;
        }
    }
}
