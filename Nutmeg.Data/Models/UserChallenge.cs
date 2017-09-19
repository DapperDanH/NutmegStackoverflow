using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class UserChallenge
    {
        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public Guid UserId { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid ChallengeLevelId { get; set; }
        public int PointsTotal { get; set; }
        public int PointsThisWeek { get; set; }

        public virtual Challenge Challenge { get; set; }
        public virtual ChallengeLevel ChallengeLevel { get; set; }
        public virtual User User { get; set; }
    }
}
