using System;
using System.Collections.Generic;

namespace SMSDesktopUWP.Core.Models {
    public class OrphansResponse 
    {
        public List<Orphan> Orphans { get; set; }

        public ResponseMetaData MetaData { get; set; }
    }
}