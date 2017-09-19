using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class ClubManager
    {
        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public Guid ClubId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public byte Type { get; set; }
        public Guid CreatedById { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid LastModifiedById { get; set; }
        public DateTimeOffset LastModifiedOn { get; set; }

        public virtual Club Club { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User LastModifiedBy { get; set; }
        public virtual User User { get; set; }
    }
}
