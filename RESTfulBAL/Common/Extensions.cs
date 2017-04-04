using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Common
{
    public static class Extensions
    {
        public static MultiResultSetReader MultiResultSetSqlQuery(this DbContext context, string query, params SqlParameter[] parameters)
        {
            return new MultiResultSetReader(context, query, parameters);
        }

        public static string ToMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }
    }
}