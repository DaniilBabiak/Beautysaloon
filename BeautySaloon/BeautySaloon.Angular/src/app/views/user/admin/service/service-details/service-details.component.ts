import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CategoryModel } from 'src/app/shared/models/category/category-model';
import { ServiceDetailedModel } from 'src/app/shared/models/service/service-detailed-model';
import { CategoryService } from 'src/app/shared/services/category.service';
import { ServiceService } from 'src/app/shared/services/service.service';

@Component({
  selector: 'app-service-details',
  templateUrl: './service-details.component.html',
  styleUrls: ['./service-details.component.css']
})
export class ServiceDetailsComponent {
  @Input() serviceId: number = 0;
  isCloseAttempted: boolean = false;
  service: ServiceDetailedModel;
  categories: CategoryModel[] = [];

  constructor(
    private categoryService: CategoryService,
    private serviceService: ServiceService,
    public activeModal: NgbActiveModal) {
    this.service = this.createEmptyService();
  }

  initModal(): void {
    if (this.serviceId == 0) {
      this.service = this.createEmptyService();
    }
    else {
      this.loadService();
    }
  }

  loadService() {
    this.serviceService.getService(this.serviceId).then(service => {
      this.service = service as ServiceDetailedModel;
    })
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe(result => {
      this.categories = result;
    })
  }

  saveService() {
    if (this.serviceId == 0) {
      this.serviceService.createService(this.service).subscribe(result => {
        this.closeModal();
      });
    }
    else {
      this.serviceService.updateService(this.service).subscribe(result => {
        this.closeModal();
      });
    }
  }

  closeModal() {
    this.activeModal.close();
  }

  createEmptyService(): ServiceDetailedModel {
    var newService: ServiceDetailedModel = {
      id: 0,
      categoryId: 0,
      duration: '00:00:00',
      masterIds: [],
      name: '',
      price: 0,
      reservationIds: []
    };

    return newService;
  }
}
