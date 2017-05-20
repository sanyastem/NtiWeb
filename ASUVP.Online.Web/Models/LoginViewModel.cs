using System.ComponentModel.DataAnnotations;

namespace ASUVP.Online.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите Логин")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите Пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}