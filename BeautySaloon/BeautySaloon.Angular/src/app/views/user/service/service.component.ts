import { Component, OnInit, SecurityContext } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { CategoryService } from 'src/app/shared/services/category.service';
import { AuthService } from '../../../shared/services/auth.service';
import { ImageService } from 'src/app/shared/services/image.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReservationComponent } from '../add-reservation/add-reservation.component';
import { CategoryModel } from 'src/app/shared/models/category/category-model';
import { ServiceService } from '../../../shared/services/service.service';
import { ServiceModel } from 'src/app/shared/models/service/service-model';
import { ServiceDetailedModel } from 'src/app/shared/models/service/service-detailed-model';
import { Observable } from 'rxjs';
import { CategoryWithImage } from 'src/app/shared/models/category/category-with-image';
@Component({
  selector: 'app-service',
  templateUrl: './service.component.html',
  styleUrls: ['./service.component.css']
})
export class ServiceComponent implements OnInit {
  categories: CategoryWithImage[] = [];


  constructor(
    private imageService: ImageService,
    private categoryService: CategoryService,
    private serviceService: ServiceService,
    private authService: AuthService,
    private modalService: NgbModal,
    private sanitizer: DomSanitizer) {
  }

  ngOnInit() {
    this.authService.loadUser()?.then(() => {
      this.loadCategories();
    })

  }

  loadCategories() {
    this.categoryService.getCategories().subscribe(categories => {
      const categoryPromises = categories.map(element => {
        const categoryWithImage: CategoryWithImage = {
          model: element,
          image: '',
          services: []
        };

        const imagePromise = this.imageService.getImage(element.imageBucket, element.imageFileName)
          .then(image => {
            return image;
          })
          .catch(error => {
            return 'assets/images/about-page3.png';
          });
        const servicesPromise = this.serviceService.getServices(element.id).toPromise();

        return Promise.all([imagePromise, servicesPromise]).then(([image, services]) => {
          categoryWithImage.image = image;
          categoryWithImage.services = services as ServiceModel[];
          return categoryWithImage;
        });
      });

      Promise.all(categoryPromises).then(categoriesWithImages => {
        this.categories = categoriesWithImages;
      });
    });
  }

  openSgroup(event: Event) {
    const wrapper = document.body;
    const sgroups = Array.from(document.querySelectorAll(".sgroup")) as HTMLElement[];
    const closeButtons = Array.from(document.querySelectorAll(".close-sgroup")) as HTMLElement[];
    const expandedClass = "is-expanded";
    const hasExpandedClass = "has-expanded-item";
    const element = event.currentTarget as HTMLElement;

    if (!element.classList.contains(expandedClass)) {
      element.classList.add(expandedClass);
      wrapper.classList.add(hasExpandedClass);
    }
  }

  closeSgroup(event: Event){
    const expandedClass = "is-expanded";
    const hasExpandedClass = "has-expanded-item";
    event.stopPropagation();
    const button = event.target as HTMLElement;
    const parent = button.parentElement as HTMLElement;
    if (parent.classList.contains(expandedClass)) {
      parent.classList.remove(expandedClass);
      parent.classList.remove(hasExpandedClass);
    }
  }

  getSectionStyles(index: number, category: CategoryWithImage): { [key: string]: string; } {
    const totalColumns = 2; // Number of columns
    const categoriesPerRow = Math.ceil(this.categories.length / totalColumns);
    const row = Math.floor(index / categoriesPerRow);
    const col = index % totalColumns;

    const top = (row * 50) + '%';
    const left = (col * 50) + '%';
    var backgroundImage = '';

    backgroundImage = `url(${category.image})`;

    return { top, left, backgroundImage };
  }

  makeAppointment(category: CategoryModel) {
    const modalRef = this.modalService.open(ReservationComponent);
    modalRef.componentInstance.category = category;
    modalRef.componentInstance.init();
  }
}
