import { Component, OnInit, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { InputParameterDefinition } from '../../models/parameter';
import { ParameterValue } from '../../models/parameter-value';
import { ComService } from '../../services/com.service';
import { ProcessService } from '../../services/process.service';

@Component({
  selector: 'app-input-parameters',
  templateUrl: './input-parameters.component.html',
  styleUrls: ['./input-parameters.component.scss']
})
export class InputParametersComponent implements OnInit {

  public parameters: InputParameterDefinition[] = [];
  public formModel: FormArray;

  constructor(
    private comService: ComService,
    private processService: ProcessService,
    private fb: FormBuilder
  ) {
    this.formModel = this.fb.array([]);
  }

  ngOnInit(): void {
    this.processService.getParameters().subscribe((result: InputParameterDefinition[]) => {
      this.parameters = result;

      this.parameters.forEach(parameter => {
        const formControl = this.fb.control(parameter.default);
        this.formModel.push(formControl);
      });
    });

    this.formModel.valueChanges.subscribe(change => {
      const parameterValues: ParameterValue[] = [];

      if (this.formModel.controls.length == this.parameters.length) {
        this.parameters.forEach((parameter, index) => {
          const parameterValue = new ParameterValue();
          parameterValue.name = parameter.name;
          parameterValue.values = [];
          const formValue = this.formModel.controls[index].value;

          switch(parameter.type){
            case 'select': {
              if (formValue) {
                parameterValue.values.push(formValue.value);
              }
            }
            break;
            case 'multiSelect': {
              if (formValue) {
                parameterValue.values = formValue.map((v: any) => v.value);
              }
            }
            break;
            case 'number': {
              if (formValue) {
                parameterValue.values.push((formValue as number).toString());
              }
            }
          }

          parameterValues.push(parameterValue);
        });

        this.comService.setParameters(parameterValues);
      }
    });
  }

  public getControl(index: number): FormControl {
    return this.formModel.controls[index] as FormControl;
  }

}
