using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class NotificationSubscription
    {
        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public short Type { get; set; }
        public Guid UserId { get; set; }
        public bool IncludeApp { get; set; }
        public bool IncludeEmail { get; set; }

        public virtual User User { get; set; }
    }
}
