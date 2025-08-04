using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Context
{
	[MetadataType(typeof(UserMetadata))]
	public partial class User
	{
        [NotMapped]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserMetadata
    {
        [Required(ErrorMessage = "Please enter name.")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter email address.")]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }
        [Display(Name = "Last Login Time")]
        public DateTime? LastLoginTime { get; set; }
        [Display(Name = "Registration Time")]
        public DateTime RegistrationTime { get; set; }
        [Required]
        public string Status { get; set; }
    }
}