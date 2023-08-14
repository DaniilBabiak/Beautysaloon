import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ServiceCategory } from 'src/app/shared/models/service-category';
import { CategoryService } from 'src/app/shared/services/category.service';
import { AuthService } from '../../../shared/services/auth.service';
import { ImageService } from 'src/app/shared/services/image.service';
@Component({
  selector: 'app-service',
  templateUrl: './service.component.html',
  styleUrls: ['./service.component.css']
})
export class ServiceComponent implements OnInit {
  categories: ServiceCategory[] = [];
  ngOnInit() {
    this.authService.loadUser()?.then(() => {
      this.loadCategories();
    })

  }

  constructor(private imageService: ImageService, private categoryService: CategoryService, private authService: AuthService) {
    this.authService.loadUser()?.then(() => {
      this.categoryService.getCategories().subscribe(result => {
        console.log(result);
      })
    });
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe(result => {
      this.categories = result;
      this.loadImages().then(() => {
        this.initButtons();
      });
    });
  }

  async loadImages() {
    for (const element of this.categories) {
      if (element.imageBucket && element.imageUrl) {
        const data = await this.imageService.getImage(element.imageBucket, element.imageUrl).toPromise();
        if (data) {
          element.image = URL.createObjectURL(data);
        }
      }
    }

  }

  initButtons() {
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

  getSectionStyles(index: number, image: string): { [key: string]: string } {
    const totalColumns = 2; // Number of columns
    const categoriesPerRow = Math.ceil(this.categories.length / totalColumns);
    const row = Math.floor(index / categoriesPerRow);
    const col = index % totalColumns;
  
    const top = (row * 50) + '%';
    const left = (col * 50) + '%';
    const backgroundImage = `url(${image})`;
    return { top, left, backgroundImage };
  }
  
}
