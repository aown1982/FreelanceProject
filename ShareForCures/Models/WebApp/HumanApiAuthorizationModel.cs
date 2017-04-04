using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.WebApp
{
    public class HumanApiAuthorizationModel
    {

        public string clientId { get; set; }
        public string humanId { get; set; }
        public string accessToken { get; set; }
        public string publicToken { get; set; }
        public string clientUserId { get; set; }

    }
}