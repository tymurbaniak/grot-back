using System;
using System.Collections.Generic;
using System.Text;

namespace Grot.ViewModels
{
    public class ProjectsListRequest
    {
        public string UserName { get; set; }
    }

    public class ProjectRequest
    {
        public string ProjectName { get; set; }
        public string UserName { get; set; }
    }
}
