import {Component, HostListener, OnInit} from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
@Component({
  selector: 'app-works',
  templateUrl: './works.component.html',
  styleUrls: ['./works.component.css']
})
export class WorksComponent implements OnInit{
  // Define the number of pages and images per page
  totalPages = 10;
  itemsPerPage = 9;
  // Define the URL of the image you want to repeat
  imageUrl = this.sanitizer.bypassSecurityTrustResourceUrl('assets/images/front-woman.avif');
  showModal: boolean = false;
  modalImage: SafeResourceUrl = '';
  modalAlt: string = '';
  currentPage = 1;
  constructor(private sanitizer: DomSanitizer) {}
  ngOnInit(): void {
    this.updateItemsPerPage();
  }
  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.updateItemsPerPage(); // Вызов метода обновления itemsPerPage
  }

  private updateItemsPerPage() {
    if (window.innerWidth <= 768) {
      this.itemsPerPage = 3;
    } else if (window.innerWidth <= 1024) {
      this.itemsPerPage = 6;
    }  else {
      this.itemsPerPage = 9; // Значение по умолчанию
    }
  }
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
