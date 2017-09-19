using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class ChallengeLevel
    {
        public ChallengeLevel()
        {
            UserChallenge = new HashSet<UserChallenge>();
        }

        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public Guid ChallengeId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public byte Type { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public string LockedIconUrl { get; set; }
        public int TriggerPoint { get; set; }
        public string Hyperlink { get; set; }

        public virtual ICollection<UserChallenge> UserChallenge { get; set; }
        public virtual Challenge Challenge { get; set; }
    }
}
