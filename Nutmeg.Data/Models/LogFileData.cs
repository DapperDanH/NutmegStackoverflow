using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class LogFileData
    {
        public Guid Id { get; set; }
        public long IndexId { get; set; }
        public Guid LogFileId { get; set; }
        public byte[] FileData { get; set; }

        public virtual LogFile LogFile { get; set; }
    }
}
