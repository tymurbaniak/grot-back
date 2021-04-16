import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {

  public forecast: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get<any>(`${environment.apiUrl}/weatherforecast`).subscribe(result => {
      this.forecast = result;
    });
  }

}
