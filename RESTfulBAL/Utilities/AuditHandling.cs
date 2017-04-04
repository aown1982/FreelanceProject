using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Utilities.AuditHandling
{
    public class AuditLogging
    {
        public enum enumChangeDB
        {
            UsersDB = 1,
            UsersDataDB = 2,
            WebApplicationDB = 3
        }

        public enum enumApplication
        {
            Database = 1,
            SFCDAL = 2,
            SFCBAL = 3,
            SFCWebSite = 4,
            PollingBatchApp = 5
        }

        public enum enumEvent
        {
            Security_Login_Success = 1,
            Security_Login_Failed = 2,
            Security_Logoff = 3,
            UserChange = 4,
            UserRead = 5,
            AdminChange = 6,
            AdminRead = 7,
            ReceivedInsert = 8,
            ReceivedUpdate = 9,
            UserInsert = 10
        }

        public enum AuditTypes
        {
            Login=1,
            NewUserEnteredData,
            NewReceivedMedicalData,
            NewReceivedAppData,
            UserModifiedData,
            LoginErr
        }

        public const string ErrMsg_Invalid_Username = "Failed Login: Invalid Username";
        public const string ErrMsg_Invalid_Password = "Failed Login: Invalid Password";
        public const string ErrMsg_NoChangesToSave = "Update: No changes to save.";
        public const string const_Successful_Login = "Successful Login";
    }
}