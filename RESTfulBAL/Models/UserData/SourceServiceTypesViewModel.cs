using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Models.UserData
{
    public class SourceServiceTypesViewModel : tSourceServiceType
    {
        public IEnumerable<SourceServiceViewModel> SourcesList { get; set; }
    }
}