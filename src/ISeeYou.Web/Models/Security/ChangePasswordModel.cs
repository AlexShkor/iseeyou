using System.ComponentModel.DataAnnotations;

namespace ISeeYou.Web.Models.Security
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Please enter current password")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [Required(ErrorMessage = "Please enter new password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Required(ErrorMessage = "Please confirm your password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string NewPasswordConfirm { get; set; }
    }
}