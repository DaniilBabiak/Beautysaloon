import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
@Component({
  selector: 'app-works',
  templateUrl: './works.component.html',
  styleUrls: ['./works.component.css']
})
export class WorksComponent implements OnInit{
  // Define the number of pages and images per page
  totalPages = 10;
  itemsPerPage = 10;
  // Define the URL of the image you want to repeat
  imageUrl = this.sanitizer.bypassSecurityTrustResourceUrl('assets/images/front-woman.avif');
  showModal: boolean = false;
  modalImage: SafeResourceUrl = '';
  modalAlt: string = '';
  currentPage = 1;
  constructor(private sanitizer: DomSanitizer) {}
  ngOnInit(): void {}
  getCurrentPageImages(): SafeResourceUrl[] {
    const images = new Array(this.itemsPerPage).fill(this.imageUrl);
    return images;
  }
  openModal(alt: string, image: SafeResourceUrl) {
    this.modalAlt = alt;
    this.modalImage = image;
    this.showModal = true;
  }
  closeModal() {
    this.showModal = false;
  }
  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }
  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }
}
