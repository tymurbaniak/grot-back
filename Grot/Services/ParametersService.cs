using Grot.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Grot.Services
{
    public interface IParametersService
    {
        List<InputParameterDefinition> GetParameters();
    }

    public class ParametersService : IParametersService
    {
        public List<InputParameterDefinition> GetParameters()
        {
            List<InputParameterDefinition> parameters;
            using (StreamReader r = new StreamReader("inputParameters.json"))
            {
                string json = r.ReadToEnd();
                parameters = JsonConvert.DeserializeObject<List<InputParameterDefinition>>(json);
            }

            return parameters;
        }
    }
}
