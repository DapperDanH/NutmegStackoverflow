using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class Challenge
    {
        public Challenge()
        {
            ChallengeAction = new HashSet<ChallengeAction>();
            ChallengeLevel = new HashSet<ChallengeLevel>();
            UserChallenge = new HashSet<UserChallenge>();
        }

        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public byte Type { get; set; }
        public Guid CreatedById { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid LastModifiedById { get; set; }
        public DateTimeOffset LastModifiedOn { get; set; }

        public virtual ICollection<ChallengeAction> ChallengeAction { get; set; }
        public virtual ICollection<ChallengeLevel> ChallengeLevel { get; set; }
        public virtual ICollection<UserChallenge> UserChallenge { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User LastModifiedBy { get; set; }
    }
}
