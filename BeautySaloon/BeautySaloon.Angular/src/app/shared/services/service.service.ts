import { Injectable } from '@angular/core';
import {AuthService} from "./auth.service";
import {ConfigService} from "./config.service";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {Service} from "../models/service";

@Injectable({
  providedIn: 'root'
})
export class ServiceService {
  constructor(private auth: AuthService, private config: ConfigService, private http: HttpClient) {
  }

  getServices(categoryId:number): Observable<Service[]> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.get<Service[]>(url + '/api/Service?categoryId='+categoryId, options);

  }

  createService(service: Service): Observable<Service> {


    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.post<Service>(`${url}/api/Service`, service, options);
  }
  deleteService(id: number): Observable<any> {
    var url = this.config.resourceApiURI;
    var options = this.getOptions();
    return this.http.delete(`${url}/api/Service/${id}`, options);
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
