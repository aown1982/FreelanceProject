using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.Users
{
    public class ConnectedSourceViewModel
    {
        public string Type { get; set; }
        public int Value { get; set; }
        public int UserID { get; set; }
        public string Color { get; set; }
    }
}