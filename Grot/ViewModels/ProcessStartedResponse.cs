using System;
using System.Collections.Generic;
using System.Text;

namespace Grot.ViewModels
{
    public class ProcessStartedResponse
    {
        public string DateTime { get; set; }
        public bool Started { get; set; }
        public string Id { get; set; }
    }
}
