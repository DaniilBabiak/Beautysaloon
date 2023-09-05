import { Component, HostListener, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { BestWorkModel } from 'src/app/shared/models/bestWork/best-work-model';
import { AuthService } from 'src/app/shared/services/auth.service';
import { BestWorkService } from 'src/app/shared/services/best-work.service';
import { ImageService } from 'src/app/shared/services/image.service';
@Component({
  selector: 'app-works',
  templateUrl: './works.component.html',
  styleUrls: ['./works.component.css']
})
export class WorksComponent implements OnInit {
  // Define the number of pages and images per page
  bestWorks: BestWorkModel[] = [];
  totalPages = 10;
  itemsPerPage = 9;
  // Define the URL of the image you want to repeat
  showModal: boolean = false;
  modalImage: string = '';
  modalAlt: string = '';
  modalBestWorkId: number | null = null;
  currentPage = 1;
  isAdmin:boolean = false;
  constructor(private auth: AuthService, private imageService: ImageService, private bestWorkService: BestWorkService) { }
  ngOnInit(): void {
    this.auth.loadUser()?.then(() => {
      this.isAdmin = this.auth.isAdmin;
      this.bestWorkService.getBestWorks().subscribe(result => {
        this.bestWorks = result;
        this.countTotalPages();
        this.loadImages();
      })
    })
    this.updateItemsPerPage();
  }
  countTotalPages(){
    this.totalPages = this.bestWorks.length % this.itemsPerPage == 0 ? this.bestWorks.length / this.itemsPerPage: this.bestWorks.length / this.itemsPerPage ;
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.updateItemsPerPage(); // Вызов метода обновления itemsPerPage
    this.countTotalPages();
    this.currentPage = 1;
  }

  async loadImages() {
    for (const element of this.bestWorks) {
      if (element.imageBucket && element.imageFileName) {
        const data = await this.imageService.getImage(element.imageBucket, element.imageFileName);
        if (data) {
          console.log("loaded image");
          element.image = data;
        }
      }
    }
  }
  private updateItemsPerPage() {
    if (window.innerWidth <= 768) {
      this.itemsPerPage = 3;
    } else if (window.innerWidth <= 1024) {
      this.itemsPerPage = 6;
    } else {
      this.itemsPerPage = 9; // Значение по умолчанию
    }
  }
  getCurrentPageBestWorks() {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;

    const images = this.bestWorks.slice(startIndex, endIndex);

    return images;
  }
  openModal(alt: string, image: string, id:number) {
    this.modalAlt = alt;
    this.modalImage = image;
    this.showModal = true;
    this.modalBestWorkId = id;
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
