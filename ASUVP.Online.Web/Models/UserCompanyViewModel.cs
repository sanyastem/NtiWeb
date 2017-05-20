using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ASUVP.Online.Web.Models
{
    public class UserCompanyViewModel
    {
        public UserCompanyViewModel()
        {
            Companies = new List<SelectListItem>();
        }

        [Required]
        public Guid CompanyId { get; set; }

        public IList<SelectListItem> Companies { get; set; }
    }
}