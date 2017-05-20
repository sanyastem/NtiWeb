using System.Collections.Generic;

namespace ASUVP.Core.Web.Dto
{
    public class UserInfoDto : BaseDto
    {
        public UserInfoDto()
        {
            Permissions = new List<string>();
        }

        public string UserName { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public List<string> Permissions { get; set; }
    }
}