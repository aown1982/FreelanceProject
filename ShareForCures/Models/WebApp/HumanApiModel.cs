using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.WebApp
{
    public class HumanApiModel
    {

        public string clientId { get; set; }
        public string humanId { get; set; }
        public string sessionToken { get; set; }
        public string userId { get; set; }
        public string clientSecret { get; set; }

    }
}