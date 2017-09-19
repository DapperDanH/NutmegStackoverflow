using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class Role
    {
        public Role()
        {
            GroupRole = new HashSet<GroupRole>();
        }

        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public string Name { get; set; }
        public Guid CreatedById { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid LastModifiedById { get; set; }
        public DateTimeOffset LastModifiedOn { get; set; }

        public virtual ICollection<GroupRole> GroupRole { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User LastModifiedBy { get; set; }
    }
}
