using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class ChallengeAction
    {
        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public Guid ChallengeId { get; set; }
        public Guid ActionId { get; set; }

        public virtual Action Action { get; set; }
        public virtual Challenge Challenge { get; set; }
    }
}
