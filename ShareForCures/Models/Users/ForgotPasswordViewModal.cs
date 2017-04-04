using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.Users
{
    public class ForgotPasswordViewModal
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public Guid ExternalId { get; set; }
        public Guid ResetCode { get; set; }
        public string ErrorMesage { get; set; }
    }

}