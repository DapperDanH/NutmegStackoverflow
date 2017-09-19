using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class Team
    {
        public Team()
        {
            Coach = new HashSet<Coach>();
            Player = new HashSet<Player>();
        }

        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public Guid ClubId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public byte Type { get; set; }
        public Guid CreatedById { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid LastModifiedById { get; set; }
        public DateTimeOffset LastModifiedOn { get; set; }

        public virtual ICollection<Coach> Coach { get; set; }
        public virtual ICollection<Player> Player { get; set; }
        public virtual Club Club { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User LastModifiedBy { get; set; }
    }
}
