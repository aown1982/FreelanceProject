using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Models.UserData
{
    public class ConnectedSourceViewModel
    {
        public string Type { get; set; }
        public int Value { get; set; }
        public int UserID { get; set; }
        public int ID { get; set; }
    }
}
