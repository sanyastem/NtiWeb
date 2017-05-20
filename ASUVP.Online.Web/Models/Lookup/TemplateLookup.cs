using ASUVP.Core.DataAccess.Model;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASUVP.Online.Web.Models.Lookup
{
    public class TemplateLookup
    {
        public static List<Template> GetTemplatesRange(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            using (var context = new ProcData())
            {
                return context.TemplateGet(Core.Web.Security.AuthManager.User.CompanyId).Where(c => c.TemplateName.StartsWith(args.Filter)).OrderBy(c => c.TemplateName).Skip(skip).Take(take).ToList();
            }
        }

        public static List<Template> GetTemplateById(ListEditItemRequestedByValueEventArgs args)
        {
            Guid id;
            if (args.Value == null || !Guid.TryParse(args.Value.ToString(), out id))
                return null;
            using (var context = new ProcData())
            {
                return context.TemplateGet(id).ToList();
            }
        }

        public static List<SupplementaryTemplate> GetSupplementaryTemplatesRange(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            using (var context = new ProcData())
            {
                return context.SupplementaryTemplateGet().Skip(skip).Take(take).ToList();
            }
        }
    }
}