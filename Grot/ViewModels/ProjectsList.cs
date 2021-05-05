using System;
using System.Collections.Generic;
using System.Text;

namespace Grot.ViewModels
{    
    public class ProjectContent
    {
        public string Name { get; set; }
        public List<InputParameterValue> Parameters { get; set; }
        public string InputImageUrl { get; set; }
        public List<string> OutputImageUrls { get; set; }
    }
}
