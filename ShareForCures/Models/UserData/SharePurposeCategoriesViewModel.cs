﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ShareForCures.Models.UserData
{
    public class SharePurposeCategoriesViewModel : ShareCategoryViewModel
    {
        public IEnumerable<SharePurposeViewModel> SahrePurposeList { get; set; }
    }
}