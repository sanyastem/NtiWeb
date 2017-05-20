using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public class AgreementConfiguration : EntityConfiguration<Agreement>
    {
        public AgreementConfiguration()
        {
            ToTable("Agreement");

            HasRequired(e => e.Document).WithOptional();
        }
    }
}