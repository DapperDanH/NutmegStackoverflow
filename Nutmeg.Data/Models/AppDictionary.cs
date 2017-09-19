using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
    public partial class AppDictionary
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
        public Guid CreatedById { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid LastModifiedById { get; set; }
        public DateTimeOffset LastModifiedOn { get; set; }

        public virtual User CreatedBy { get; set; }
        public virtual User LastModifiedBy { get; set; }
    }
}
