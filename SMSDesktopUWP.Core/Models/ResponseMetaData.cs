using System;

namespace SMSDesktopUWP.Core.Models {
    public class ResponseMetaData {
        public int TotalCount { get; set; } 

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public bool HasNext { get; set; }

        public bool HasPrevious { get; set; }
    }
}