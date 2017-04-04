using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace RESTfulBAL.Models.UserData
{
    public class SharePurposeCategoriesViewModel : ShareCategoryViewModel
    {
        public IEnumerable<SharePurposeViewModel> SahrePurposeList { get; set; }
    }
}