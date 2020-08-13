using System;
using System.Collections.Generic;
using System.Text;

namespace SMSDesktopUWP.Core.Models
{
    public enum EditSource
    {
        Orphan,
        Guardian
    }

    public class EditNarrationParams
    {
        public EditNarrationParams() { }

        public EditSource Source { get; set; }  // Set the source (Orphan vs Guardian)

        public object Member { get; set; }    // Use this as the incoming Guardian or Orphan

        public Narration Narration { get; set; }
    }

    public class EditAcademicParams
    {
        public EditAcademicParams() { }

        public EditSource Source { get; set; }  // Set the source (Orphan vs Guardian)

        public object Member { get; set; }    // Use this as the incoming Guardian or Orphan

        public Academic Academic { get; set; }

    }
}
