import { Injectable } from '@angular/core';
import { ConfigService } from './config.service';
import { AuthService } from './auth.service';
import { BestWork } from '../models/best-work';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BestWorkService {

  constructor(private config: ConfigService, private auth: AuthService, private http: HttpClient) { }

  getBestWorks(): Observable<BestWork[]> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.get<BestWork[]>(url + '/api/BestWork', options);

  }

  createBestWork(bestWork: BestWork): Observable<BestWork> {
    const bestWorkWithoutImage: BestWork = {
      id: bestWork.id,
      imageBucket: bestWork.imageBucket,
      imageFileName: bestWork.imageFileName,
      image: null // Указываем, что image равен null
    };

    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.post<BestWork>(`${url}/api/BestWork`, bestWorkWithoutImage, options);
  }
  deleteBestWork(id: number): Observable<any> {
    var url = this.config.resourceApiURI;
    var options = this.getOptions();
    return this.http.delete(`${url}/api/BestWork/${id}`, options);
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
