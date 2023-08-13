import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { CategoryService } from 'src/app/shared/services/category.service';
@Component({
  selector: 'app-service',
  templateUrl: './service.component.html',
  styleUrls: ['./service.component.css']
})
export class ServiceComponent implements OnInit {
  ngOnInit() {
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
          element.addEventListener('click', function(this: HTMLElement) {
            _opensgroup(this);
          });
        });

        closeButtons.forEach((element: HTMLElement) => {
          element.addEventListener('click', function(this: HTMLElement, event: Event) {
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
  servicesPhoto1: SafeResourceUrl;
  servicesPhoto2: SafeResourceUrl;
  servicesPhoto3: SafeResourceUrl;
  servicesPhoto4: SafeResourceUrl;
  constructor(private sanitizer: DomSanitizer, private categoryService: CategoryService) {
    this.categoryService.getCategories().subscribe(result =>{
      console.log(result);
    })
    const imagePath = 'assets/images/services-image1.jpg';
    const imagePath2 = 'assets/images/services-image2.jpg';
    const imagePath3 = 'assets/images/services-image3.jpg';
    const imagePath4 = 'assets/images/services-image4.jpg';
    this.servicesPhoto1 = this.sanitizer.bypassSecurityTrustResourceUrl(imagePath);
    this.servicesPhoto2 = this.sanitizer.bypassSecurityTrustResourceUrl(imagePath2);
    this.servicesPhoto3 = this.sanitizer.bypassSecurityTrustResourceUrl(imagePath3);
    this.servicesPhoto4 = this.sanitizer.bypassSecurityTrustResourceUrl(imagePath4);
  }
}
