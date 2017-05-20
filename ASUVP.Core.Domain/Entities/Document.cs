using System;
using System.Collections.Generic;

namespace ASUVP.Core.Domain.Entities
{
    public class Document : Entity
    {
        public string DocNumber { get; set; }
        public string RegNumber { get; set; }
        public DateTime DocDate { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsReadonly { get; set; }
        public Guid? DocOwnerId { get; set; }
        public Guid? DocOwnerId2 { get; set; }
        public Guid? TargetCompanyId { get; set; }
        public Guid? CustomerCompanyId { get; set; }
        public Guid? TargetContactId { get; set; }
        public Guid? CustomerContactId { get; set; }
        public virtual Template Template { get; set; }
        public virtual Document DocOwner { get; set; }
        public virtual Document DocOwner2 { get; set; }
        public virtual Company TargetCompany { get; set; }
        public virtual Company CustomerCompany { get; set; }
        public virtual Contact TargetContact { get; set; }
        public virtual Contact CustomerContact { get; set; }
        public virtual ICollection<Document> OwnerDocuments { get; set; }
        public virtual ICollection<Document> Owner2Documents { get; set; }
    }
}