import { Component } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
@Component({
  selector: 'app-front-page',
  templateUrl: './front-page.component.html',
  styleUrls: ['./front-page.component.css'],


})
export class FrontPageComponent {
  imageUrl: SafeResourceUrl;

  constructor(private sanitizer: DomSanitizer) {
    // Путь к изображению в папке assets
    const imagePath = 'assets/images/Erie-Lighthouse.jpg';

    // Преобразование пути в безопасный ресурс
    this.imageUrl = this.sanitizer.bypassSecurityTrustResourceUrl(imagePath);

  }
}