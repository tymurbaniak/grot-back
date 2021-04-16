import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { ParameterValue } from '../models/parameter-value';

@Injectable({
  providedIn: 'root'
})
export class ComService {

  private parametersSource = new BehaviorSubject<ParameterValue[]>([]);

  parameters$ = this.parametersSource.asObservable();

  constructor() { }

  public setParameters(parameters: ParameterValue[]){
    this.parametersSource.next(parameters);
  }
}
