using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iDigBio.RESTService.Tests.Controllers
{
    public abstract class ApiTestBase
    {
        public const string ApiUrlRoot = "http://localhost:13514/api";

        public WebClient CreateWebClient()
        {
            var webClient = new WebClient();
            return webClient;
        }

    }
}
