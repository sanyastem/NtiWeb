using System;

namespace ASUVP.Core.Domain.Entities
{
    public class Template : Entity
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public DateTime? DateBeg { get; set; }
        public DateTime? DateEnd { get; set; }
        public bool IsGlobal { get; set; }
        public virtual TemplateGroup TemplateGroup { get; set; }
    }
}