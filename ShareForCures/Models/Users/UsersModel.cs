using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.Users
{
    public class UsersModel
    {
        public int ID { get; set; }
        public System.Guid ExternalID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<int> LanguageID { get; set; }
        public Nullable<int> LocationID { get; set; }

        public string Email { get; set; }
        public string Username { get; set; }

        [Display(Description = "Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$&_/\*])(?=.*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,128}", ErrorMessage = "The password can only consist of 1 Capital & three lower case letters, 1 Special Character , 1 number")]
        public string PasswordHash { get; set; }

        [DataType(DataType.Password)]
        [CompareAttribute("PasswordHash", ErrorMessage = "Password not match")]
        public string ComparePasswordHash { get; set; }

        public int PHSaltID { get; set; }
        public Nullable<int> SexID { get; set; }
        public Nullable<int> GenderID { get; set; }
        public Nullable<int> RaceID { get; set; }
        public Nullable<int> EthnicityID { get; set; }
        public Nullable<int> AccountStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
        public string InvitationCode { get; set; }
        public string AccessToken { get; set; }
        public string PublicToken { get; set; }
        public string SocialUserId { get; set; }
        public string SocialType { get; set; }
        public string SourceUserIDToken { get; set; }
        public string ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public Nullable<System.DateTime> AccessTokenExpiration { get; set; }

        public virtual ICollection<tUsersAddressHistory> tUsersAddressHistories { get; set; }
        public virtual ICollection<tUserPasswordResets> tUserPasswordResets { get; set; }

    }
}