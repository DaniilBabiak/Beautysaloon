import { Component } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-works',
  templateUrl: './works.component.html',
  styleUrls: ['./works.component.css']
})
export class WorksComponent {
  imageUrl: SafeResourceUrl;
 
  showModal: boolean = false;
  modalImage: SafeResourceUrl = ''; 
  modalAlt: string = ''; 
  
  constructor(private sanitizer: DomSanitizer) {
   
    const imagePath = 'assets/images/front-woman.avif';
    this.imageUrl = this.sanitizer.bypassSecurityTrustResourceUrl(imagePath);
  }

  
  openModal(alt: string, image: SafeResourceUrl) {
    this.modalAlt = alt;
    this.modalImage = image;
    this.showModal = true;
  }

  
  closeModal() {
    this.showModal = false;
  }
}