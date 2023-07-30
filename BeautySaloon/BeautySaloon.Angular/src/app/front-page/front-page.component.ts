import { Component } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
@Component({
  selector: 'app-front-page',
  templateUrl: './front-page.component.html',
  styleUrls: ['./front-page.component.css'],


})
export class FrontPageComponent {
  imageUrl: SafeResourceUrl;
  titleimgRight: SafeResourceUrl;
  constructor(private sanitizer: DomSanitizer) {
    // Путь к изображению в папке assets
    const imagePath = 'assets/images/front-gradient.jpg';
    const titleimgRightPath = 'assets/images/front-woman.avif';
    // Преобразование пути в безопасный ресурс
    this.imageUrl = this.sanitizer.bypassSecurityTrustResourceUrl(imagePath);
    this.titleimgRight = this.sanitizer.bypassSecurityTrustResourceUrl(titleimgRightPath);
   
  }
}