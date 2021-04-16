import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../../environments/environment';
import { InputParameterDefinition } from '../models/parameter';
import { ParameterValue } from '../models/parameter-value';
import { ProcessCall } from '../models/process-call';

@Injectable({
  providedIn: 'root'
})
export class ProcessService {

  private parametersSubject: BehaviorSubject<any>;  

  constructor(
    private http: HttpClient
  ) {
    this.parametersSubject = new BehaviorSubject<any>({});
  }

  getParameters(){
    return this.http.get<any>(`${environment.apiUrl}/grot/parameters`, { withCredentials: false })
      .pipe(map((parameters: any) => {
        this.parametersSubject.next(parameters);
        return parameters;
      }));
  }

  process(inputParameters: ParameterValue[], canvasDataUrl: string){
    const call = new ProcessCall();
    call.inputParameters = inputParameters;
    call.image = canvasDataUrl;
    return this.http.post<any>(`${environment.apiUrl}/grot/process`, call, { withCredentials: true })
    .pipe(map((response: any) => {
      return response;
    }));
  }
}
