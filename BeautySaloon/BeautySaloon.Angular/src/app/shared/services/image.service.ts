import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { ConfigService } from './config.service';
import { Observable } from 'rxjs';
import { ImageLocation } from '../models/image-location';

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private auth: AuthService, private config: ConfigService, private http: HttpClient) { }

  uploadImage(file: File, bucketName: string): Observable<ImageLocation> {
    var url = this.config.imageApiURI;
    var options = this.getOptions();
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<ImageLocation>(url + `/api/image?bucketName=${bucketName}`, formData, options);
  }

  getImage(bucketName: string, fileName: string): Observable<Blob> {
    var url = this.config.imageApiURI;

    var headers = this.getOptions().headers;

    return this.http.get(url + `/api/Image?bucketName=${bucketName}&fileName=${fileName}`, { headers: headers, responseType: 'blob' });
  }

  private getOptions() {
    return {
      headers: new HttpHeaders({
        'Authorization': this.auth.authorizationHeaderValue
      })
    };
  }
}
