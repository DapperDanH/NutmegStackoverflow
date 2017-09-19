using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class Action
    {
        public Action()
        {
            ChallengeAction = new HashSet<ChallengeAction>();
            UserAction = new HashSet<UserAction>();
        }

        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public byte ActionType { get; set; }
        public int Points { get; set; }
        public int LimitPerDay { get; set; }
        public int ThrottleSeconds { get; set; }
        public bool IsEnabled { get; set; }
        public bool RequiresVerification { get; set; }
        public Guid CreatedById { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid LastModifiedById { get; set; }
        public DateTimeOffset LastModifiedOn { get; set; }

        public virtual ICollection<ChallengeAction> ChallengeAction { get; set; }
        public virtual ICollection<UserAction> UserAction { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User LastModifiedBy { get; set; }
    }
}
