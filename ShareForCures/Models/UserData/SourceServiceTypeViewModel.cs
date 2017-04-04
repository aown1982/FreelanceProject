using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class SourceServiceTypeViewModel: SourceServiceType
    {
        public IEnumerable<SourceService> SourcesList { get; set; }

    }
}