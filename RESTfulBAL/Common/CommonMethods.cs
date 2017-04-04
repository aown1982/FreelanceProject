using DAL;
using DAL.WebApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Common
{
    public static class CommonMethods
    {
        static private WebApplicationEntities db = new WebApplicationEntities();

        public static bool IsUserExistsInUserFilters(int userFilterId, int userId)
        {
            var isExists = false;
            var sqlQuery = db.tUserFilters.Where(u => u.ID == userFilterId).Select(a => a.FilterQuery).FirstOrDefault();
            var userFiltersList = db.Database.SqlQuery<tUser>(sqlQuery).ToList();

            isExists = (userFiltersList.Count() > 0 && userFiltersList.Any(a => a.ID == userId));

            return isExists;
        }
    }
}