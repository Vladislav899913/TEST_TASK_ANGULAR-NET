import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class SharedService {
  readonly APIUrl = 'http://localhost:53535/api';

  constructor(private http: HttpClient) {}

  addData(val: any) {
    return this.http.post<any>(this.APIUrl + '/user', val);
  }
}
