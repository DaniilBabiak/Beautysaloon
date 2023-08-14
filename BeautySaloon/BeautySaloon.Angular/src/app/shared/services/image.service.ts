import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { ConfigService } from './config.service';
import { Observable, concatMap, map, of, tap } from 'rxjs';
import { ImageLocation } from '../models/image-location';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  private imageCache: Map<string, { image: Blob, expiration: number }> = new Map<string, { image: Blob, expiration: number }>();
  private cacheDurationSeconds = 3600; //1 час
  constructor(private auth: AuthService, private config: ConfigService, private http: HttpClient) {
  }

  uploadImage(file: File, bucketName: string): Observable<ImageLocation> {
    var url = this.config.imageApiURI;
    var options = this.getOptions();
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<ImageLocation>(url + `/api/image?bucketName=${bucketName}`, formData, options);
  }

  async getImage(bucketName: string, fileName: string): Promise<string> {
    await this.restoreCacheFromLocalStorage(); // Дожидаемся восстановления кеша

    const cacheKey = `${bucketName}/${fileName}`;

    if (this.imageCache.has(cacheKey)) {
      const cachedItem = this.imageCache.get(cacheKey)!;
      if (cachedItem.expiration > Date.now()) {
        return URL.createObjectURL(cachedItem.image);
      } else {
        this.imageCache.delete(cacheKey);
        this.saveCacheToLocalStorage();
      }
    }

    var url = this.config.imageApiURI;

    var headers = this.getOptions().headers;

    var result = await this.http.get(url + `/api/Image?bucketName=${bucketName}&fileName=${fileName}`,
      { headers: headers, responseType: 'blob' }).toPromise();

    const expiration = Date.now() + this.cacheDurationSeconds * 1000;
    this.imageCache.set(cacheKey, { image: result as Blob, expiration });
    await this.saveCacheToLocalStorage();
    return URL.createObjectURL(result as Blob);
  }

  private getOptions() {
    return {
      headers: new HttpHeaders({
        'Authorization': this.auth.authorizationHeaderValue
      })
    };
  }

  private async saveCacheToLocalStorage(): Promise<void> {
    try {
      var dataToCache: Map<string, { imageEncoded: string, expiration: number, type: string }> = new Map<string, { imageEncoded: string, expiration: number, type: string }>();

      for (const [key, element] of this.imageCache.entries()) {
        const blobImage = await this.blobToBase64(element.image);
        dataToCache.set(key, { imageEncoded: blobImage, expiration: element.expiration, type: element.image.type })
      }

      let jsonObject: any[] = [];
      dataToCache.forEach((value, key) => {
        jsonObject.push({ key: key, value: value });
      });

      var json = JSON.stringify(jsonObject);

      localStorage.setItem('imageCache', json);
    } catch (error) {
      console.error('Error saving cache to localStorage:', error);
      throw error;
    }
  }

  private async restoreCacheFromLocalStorage() {
    const cacheDataString = localStorage.getItem('imageCache');
    if (cacheDataString) {
      const cacheData: any[] = JSON.parse(cacheDataString);

      this.imageCache = new Map<string, { image: Blob, expiration: number }>();

      for (const cacheItem of cacheData) {
        const image = await this.base64ToBlob(cacheItem.value.imageEncoded, cacheItem.value.type);
        this.imageCache.set(cacheItem.key, { image: image, expiration: cacheItem.value.expiration });
      }
    }
  }

  blobToBase64(blob: Blob): Promise<string> {
    return new Promise<string>((resolve, reject) => {
      const reader = new FileReader();
  
      reader.onloadend = () => {
        if (reader.result instanceof ArrayBuffer) {
          reject(new Error('Failed to convert blob to base64: Result is an ArrayBuffer'));
          return;
        }
  
        const base64Data = reader.result as string;
        const base64WithoutPrefix = base64Data.split(',')[1]; // Убираем префикс
        resolve(base64WithoutPrefix);
      };
  
      reader.onerror = (event) => {
        reject(new Error(`Error reading blob: ${event}`));
      };
  
      reader.readAsDataURL(blob);
    });
  }

  base64ToBlob(base64Data: string, contentType: string = ''): Blob {
    const binary = atob(base64Data);
    const array = new Uint8Array(binary.length);
    for (let i = 0; i < binary.length; i++) {
      array[i] = binary.charCodeAt(i);
    }
    return new Blob([array], { type: contentType });
  }
}

