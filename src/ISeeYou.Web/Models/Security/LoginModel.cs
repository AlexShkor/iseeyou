namespace ISeeYou.Web.Models.Security
{
   public class LoginModel
   {
       public string Email { get; set; }
     
       public string Password { get; set; }

       public bool RememberMe { get; set; }

       public string ReturnUrl { get; set; }
   }
}