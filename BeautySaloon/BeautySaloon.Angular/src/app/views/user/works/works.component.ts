import { Component, HostListener, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { BestWorkModel } from 'src/app/shared/models/bestWork/best-work-model';
import { BestWorkWithImage } from 'src/app/shared/models/bestWork/best-work-with-image';
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
  bestWorks: BestWorkWithImage[] = [];
  totalPages = 10;
  itemsPerPage = 9;
  // Define the URL of the image you want to repeat
  showModal: boolean = false;
  modalImage: string | SafeResourceUrl = '';
  modalAlt: string = '';
  modalBestWorkId: number | null = null;
  currentPage = 1;
  isAdmin: boolean = false;

  constructor(
    private auth: AuthService,
    private imageService: ImageService,
    private bestWorkService: BestWorkService) { }

  ngOnInit(): void {
    this.auth.loadUser()?.then(() => {
      this.isAdmin = this.auth.isAdmin;
      this.bestWorkService.getBestWorks().subscribe(bestWorks => {
        const bestWorkPromises = bestWorks.map(element => {
          const bestWorkWithImage: BestWorkWithImage = {
            model: element,
            image: ''
          };
          console.log('Loading image for ' + bestWorkWithImage.model.id);
          const imagePromise = this.imageService.getImage(element.imageBucket, element.imageFileName);

          return Promise.all([imagePromise]).then(([image]) => {
            bestWorkWithImage.image = image;
            console.log('Loaded image for ' + bestWorkWithImage.model.id);

            return bestWorkWithImage;
          });
        });

        Promise.all(bestWorkPromises).then(bestWorksWithImage => {
          this.bestWorks = bestWorksWithImage;
          this.updateItemsPerPage();
          this.countTotalPages();
        });
      });
    })
  }
  countTotalPages() {
    this.totalPages = this.bestWorks.length % this.itemsPerPage == 0 ? this.bestWorks.length / this.itemsPerPage : this.bestWorks.length / this.itemsPerPage;
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.updateItemsPerPage(); // Вызов метода обновления itemsPerPage
    this.countTotalPages();
    this.currentPage = 1;
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
  openModalAsync(alt: string, bestWork: BestWorkWithImage, id: number) {
    this.modalAlt = alt;
    this.modalImage = bestWork.image;
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
