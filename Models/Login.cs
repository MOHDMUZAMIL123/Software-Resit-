using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyTech.Models
{
    public class Login
    {
        [Required(ErrorMessage ="Please enter the username.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter the password.")]
        public string Password { get; set; }
    }
}