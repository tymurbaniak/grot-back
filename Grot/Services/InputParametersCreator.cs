using Grot.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grot.Services
{

    public class InputParametersCreator
    {
        public string CreateInputParametersString(List<InputParameterValue> inputParameters)
        {
            StringBuilder sb = new StringBuilder();

            foreach(var inputParameter in inputParameters)
            {
                if(CheckValuesAreNull(inputParameter))
                {
                    continue;
                }

                string values = string.Join(" ", inputParameter.Values);
                sb.AppendLine($"{inputParameter.Name} {values}");
            }

            sb.AppendLine($"bmp input.png");

            return sb.ToString();
        }

        private bool CheckValuesAreNull(InputParameterValue inputParameter)
        {
            if(inputParameter.Values.Count == 0)
            {
                return true;
            }

            if(inputParameter.Values.Any(v => !string.IsNullOrEmpty(v))){
                return false;
            }

            return true;
        }
    }
}
