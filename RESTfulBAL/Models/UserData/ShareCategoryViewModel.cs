using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RESTfulBAL.Models.UserData
{
    public class ShareCategoryViewModel
    {
       
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDateTime { get; set; }

     }
}