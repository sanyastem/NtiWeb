using System;
using System.Collections.Generic;

namespace ASUVP.Core.Web.Dto
{
    public class EmployeeDto : BaseDto
    {
        public EmployeeDto()
        {
            Roles = new List<Guid>();
        }

        public Guid UserId { get; set; }
        public List<Guid> Roles { get; set; }
    }
}