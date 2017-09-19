using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class UserAction
    {
        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public Guid UserId { get; set; }
        public Guid ActionId { get; set; }
        public double Score { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        public virtual Action Action { get; set; }
        public virtual User User { get; set; }
    }
}
