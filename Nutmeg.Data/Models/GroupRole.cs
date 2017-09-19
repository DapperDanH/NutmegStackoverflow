using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class GroupRole
    {
        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public Guid GroupId { get; set; }
        public Guid RoleId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Role Role { get; set; }
    }
}
