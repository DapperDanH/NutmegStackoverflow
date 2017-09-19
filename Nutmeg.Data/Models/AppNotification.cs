using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class AppNotification
    {
        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public short Type { get; set; }
        public Guid UserId { get; set; }
        public string Details { get; set; }
        public bool IsRead { get; set; }

        public virtual User User { get; set; }
    }
}
