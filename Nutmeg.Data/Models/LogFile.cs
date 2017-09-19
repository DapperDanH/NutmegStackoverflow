using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class LogFile
    {
        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public string SourceName { get; set; }
        public string FileName { get; set; }
        public DateTimeOffset UploadedOn { get; set; }
        public DateTimeOffset? ExpiresOn { get; set; }

        public virtual LogFileData LogFileData { get; set; }
    }
}
