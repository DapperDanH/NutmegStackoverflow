using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class VersionHistory
    {
        public string Version { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
