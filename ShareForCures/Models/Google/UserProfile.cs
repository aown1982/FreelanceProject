using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.Google
{
    public class UserProfile
    {
        [Required(ErrorMessage = "Please tell us your name")]
        [DisplayName("Your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Choose a displayname for you")]
        [DisplayName("Displayname")]
        public string DisplayName { get; set; }

        public string Picture { get; set; }

        [Required(ErrorMessage = "Please enter an E-mail address")]
        [DisplayName("E-Mail-Address")]
        public string Email { get; set; }

        [Key]
        [Required]
        public string UniqueId { get; set; }

        public IdentityProvider Provider { get; set; }
    }
}