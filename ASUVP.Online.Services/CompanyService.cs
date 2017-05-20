using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASUVP.Core.Logging;
using ASUVP.Core.DataAccess.Model;

namespace ASUVP.Online.Services
{
    public interface ICompanyService
    {
        List<Company> GetCompanyList();
        Company GetCompany(Guid id);
    }

    public class CompanyService : BaseHttpService, ICompanyService
    {
        public CompanyService(IEventLogger logger) : base(logger)
        {

        }

        public List<Company> GetCompanyList()
        {
            List<Company> companies = new List<Company>();

            using (var context = new ProcData())
            {
                companies = context.CompanyGet(null).ToList();
            }

            return companies;
        }

        public Company GetCompany(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.CompanyGet(id).FirstOrDefault();
            }
        }
    }
}
