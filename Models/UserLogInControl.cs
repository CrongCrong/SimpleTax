

using System.ComponentModel.DataAnnotations;

namespace SimpleTax
{
    public class UserLogInControl
    {

        public User user { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter valid username.")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter valid password.")]
        public string Password { get; set; }

        public bool IncorrectLogin { get; set; }

        public string LoginMessage { get; set; }
    }
}