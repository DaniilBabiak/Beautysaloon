import { Component, OnInit } from '@angular/core';
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
@Component({
  selector: 'app-service',
  templateUrl: './service.component.html',
  styleUrls: ['./service.component.css']
})
export class ServiceComponent implements OnInit {
  categories: CategoryModel[] = [];
  ngOnInit() {
    this.authService.loadUser()?.then(() => {
      this.loadCategories();
    })

  }

  constructor(
    private imageService: ImageService,
    private categoryService: CategoryService,
    private serviceService: ServiceService,
    private authService: AuthService,
    private modalService: NgbModal) {
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe(result => {
      this.categories = result;
      this.initButtons();
    });
  }

  async loadImageAsync(category: CategoryModel) {
    return this.imageService.getImage(category.imageBucket, category.imageFileName);
  }

  async loadServicesAsync(categoryId: number) {
    var service = await this.serviceService.getServices(categoryId).toPromise();

    return service as ServiceModel[];
  }

  initButtons() {
    console.log("init buttons")
    const Boxlayout = (function () {
      const wrapper = document.body;
      const sgroups = Array.from(document.querySelectorAll(".sgroup")) as HTMLElement[];
      const closeButtons = Array.from(document.querySelectorAll(".close-sgroup")) as HTMLElement[];
      const expandedClass = "is-expanded";
      const hasExpandedClass = "has-expanded-item";

      return { init };

      function init() {
        _initEvents();
      }

      function _initEvents() {
        sgroups.forEach((element: HTMLElement) => {
          element.addEventListener('click', function (this: HTMLElement) {
            _opensgroup(this);
          });
        });

        closeButtons.forEach((element: HTMLElement) => {
          element.addEventListener('click', function (this: HTMLElement, event: Event) {
            event.stopPropagation();
            _closesgroup(this.parentElement as HTMLElement);
          });
        });
      }

      function _opensgroup(element: HTMLElement) {
        if (!element.classList.contains(expandedClass)) {
          element.classList.add(expandedClass);
          wrapper.classList.add(hasExpandedClass);
        }
      }

      function _closesgroup(element: HTMLElement) {
        if (element.classList.contains(expandedClass)) {
          element.classList.remove(expandedClass);
          wrapper.classList.remove(hasExpandedClass);
        }
      }
    })();

    Boxlayout.init();
  }

  async getSectionStyles(index: number, category: CategoryModel): Promise<{ [key: string]: string; }> {
    var image = await this.loadImageAsync(category);
    const totalColumns = 2; // Number of columns
    const categoriesPerRow = Math.ceil(this.categories.length / totalColumns);
    const row = Math.floor(index / categoriesPerRow);
    const col = index % totalColumns;

    const top = (row * 50) + '%';
    const left = (col * 50) + '%';
    const backgroundImage = `url(${image})`;
    return { top, left, backgroundImage };
  }

  makeAppointment(category: CategoryModel) {
    const modalRef = this.modalService.open(ReservationComponent);
    modalRef.componentInstance.category = category;
    modalRef.componentInstance.init();
  }
}
