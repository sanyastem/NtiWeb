using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASUVP.Core.Web.Dto
{
    public class RoleDto : BaseDto
    {
        public RoleDto()
        {
            Permissions = new List<Guid>();
        }

        [Required(ErrorMessage = "Введите название роли")]
        public string Name { get; set; }
        public List<Guid> Permissions { get; set; }
    }
}