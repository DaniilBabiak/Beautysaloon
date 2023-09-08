import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { ConfigService } from './config.service';
import { Observable, concatMap, map, of, tap } from 'rxjs';
import { ImageLocation } from '../models/image-location';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  constructor(
    private auth: AuthService,
    private config: ConfigService,
    private http: HttpClient,
    private sanitizer: DomSanitizer) {
  }

  uploadImage(file: File, bucketName: string): Observable<ImageLocation> {
    var url = this.config.imageApiURI;
    var options = this.getOptions();
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<ImageLocation>(url + `/api/image?bucketName=${bucketName}`, formData, options);
  }

  async getImage(bucketName: string, fileName: string): Promise<string> {

    const url = this.config.imageApiURI;

    const headers = this.getOptions().headers;

    const response = await this.http.get(url + `/api/Image?bucketName=${bucketName}&fileName=${fileName}`, {
      headers: headers,
      responseType: 'blob'
    }).toPromise();

    if (!response) {
      throw new Error('Image not found');
    }

    return URL.createObjectURL(response as Blob);
  }

  private getOptions() {
    return {
      headers: new HttpHeaders({
        'Authorization': this.auth.authorizationHeaderValue
      })
    };
  }
}

