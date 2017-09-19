using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class Parent
    {
        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public Guid ParentUserId { get; set; }
        public Guid ChildUserId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public byte Type { get; set; }
        public Guid CreatedById { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid LastModifiedById { get; set; }
        public DateTimeOffset LastModifiedOn { get; set; }

        public virtual User ChildUser { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User LastModifiedBy { get; set; }
        public virtual User ParentUser { get; set; }
    }
}
