using System.Collections.Generic;

namespace Grot.ViewModels
{
    public class InputParameterDefinition
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public string Default { get; set; }
        public List<Option> Options { get; set; }
    }
}