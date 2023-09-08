import { Injectable } from '@angular/core';
import { ConfigService } from './config.service';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BestWorkModel } from '../models/bestWork/best-work-model';

@Injectable({
  providedIn: 'root'
})
export class BestWorkService {

  constructor(private config: ConfigService, private auth: AuthService, private http: HttpClient) { }

  getBestWorks(): Observable<BestWorkModel[]> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.get<BestWorkModel[]>(url + '/api/BestWork', options);

  }

  createBestWork(bestWork: BestWorkModel): Observable<BestWorkModel> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.post<BestWorkModel>(`${url}/api/admin/BestWork`, bestWork, options);
  }
  deleteBestWork(id: number): Observable<any> {
    var url = this.config.resourceApiURI;
    var options = this.getOptions();
    return this.http.delete(`${url}/api/admin/BestWork/${id}`, options);
  }
  private getOptions() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': this.auth.authorizationHeaderValue
      })
    };
  }
}
