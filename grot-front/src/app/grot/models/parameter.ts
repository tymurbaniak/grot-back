import { Option } from "./option";

export class InputParameterDefinition {
    public name = '';
    public displayName = '';
    public type = '';
    public default = '';
    public options: Option[] = [];
}
