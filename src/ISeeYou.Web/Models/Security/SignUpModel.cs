using System.ComponentModel.DataAnnotations;

namespace ISeeYou.Web.Models.Security
{
    public class SignUpModel
    {
       [Required(ErrorMessage = "Please enter a username")]
       public string Username { get; set; }

       [Required(ErrorMessage = "Please enter an email address")]
        //[EmailAddress]
        public string EmailAddress { get; set; }

       [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Please confirm your password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password confirmation doesn't match the password.")]
        public string ConfirmedPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}