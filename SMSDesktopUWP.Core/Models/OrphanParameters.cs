using System;

namespace SMSDesktopUWP.Core.Models 
{
    public class OrphanParametes 
    {
        public int PageSize { get; set; } = 20;

        public int PageNumber { get; set; } = 1;

        public string SearchQuery { get; set; }
    } 
}