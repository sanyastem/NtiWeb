using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASUVP.Core.Web.Dto
{
    public class UserDto : BaseDto
    {
        public UserDto()
        {
            Companies = new List<Guid>();
        }

        public Guid? ContactId { get; set; }

        [Required(ErrorMessage = "Введите имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [RegularExpression(@"(?=.*[a-zA-Z])((?=.*\d)|(?=.*[^a-zA-Z0-9])).{6,}",
            ErrorMessage = "Введите более сложный пароль.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }

        public bool IsActive => true;
        public List<Guid> Companies { get; set; }
    }
}