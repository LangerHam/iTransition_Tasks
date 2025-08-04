using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserManagement.ViewModel
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Please enter your email address.")]
		[EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
		[Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
		public bool RememberMe { get; set; }
    }
}