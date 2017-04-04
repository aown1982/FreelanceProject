using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Utilities.ErrorHandling
{
    public class ErrorLogging
    {
        public enum enumErrorDB
        {
            UsersDB = 1,
            UsersDataDB = 2,
            WebApplicationDB = 3
        }

        public enum enumErrorSource
        {
            Database = 1,
            SFCDAL = 2,
            SFCBAL = 3,
            SFCWebSite = 4,
            PollingBatchApp = 5
        }

        public enum enumErrorType
        {
            User = 1,
            Application = 2,
            System = 3,
            Database = 4,
            StoredProcedure = 5,
            Security = 6
        }       
    }
}