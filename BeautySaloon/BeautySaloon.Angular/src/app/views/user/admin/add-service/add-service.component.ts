import { Component, EventEmitter, OnInit } from '@angular/core';
import { ServiceCategory } from '../../../../shared/models/service-category';
import { CategoryService } from '../../../../shared/services/category.service';
import { ImageService } from '../../../../shared/services/image.service';
import { AuthService } from '../../../../shared/services/auth.service';
import { Service } from 'src/app/shared/models/service';
import { ServiceService } from '../../../../shared/services/service.service';
import { NgbTimeStringAdapter } from '../../../../shared/helpers/ngb-time-string-adapter'
import { NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';

@Component({
  selector: 'app-add-service',
  templateUrl: './add-service.component.html',
  styleUrls: ['./add-service.component.css'],
})
export class AddServiceComponent implements OnInit {
  totalServicesPossible: number = 0;
  services: Service[] | null = null;
  serviceCategories: ServiceCategory[] | null = null;
  newService: Service = {
    id: null,
    name: null,
    duration: "00:00:00",
    endTime: "00:00:00",
    startTime: "00:00:00",
    category: null,
    reservations: null,
    price: null,
    categoryId: null,
  };
  showAddServiceForm = false;

  constructor(
    private categoryService: CategoryService,
    private imageService: ImageService,
    private authService: AuthService,
    private service: ServiceService,
    private NgbTimeStringAdapter: NgbTimeStringAdapter
  ) { }

  ngOnInit(): void {
    this.authService.loadUser()?.then(() => {
      this.loadCategories();
    });
  }
  ngDoCheck() {
    this.updateTimes();
  }
  deleteCategory(id: number | null) {
    if (id) {
      this.categoryService.deleteCategory(id).subscribe((result) => {
        this.loadCategories();
      });
    }
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe((result) => {
      this.serviceCategories = result;
      this.loadImages();
    });
  }

  loadImages() {
    this.serviceCategories?.forEach((element) => {
      if (element.imageBucket && element.imageFileName) {
        this.imageService
          .getImage(element.imageBucket, element.imageFileName)
          .then((data) => {
            element.image = data;
          });
      }
    });
  }
  selectCategory(category: ServiceCategory) {
    this.newService.categoryId = category.id;
    this.newService.category = category;
  }
  saveService() {
    this.service.createService(this.newService).subscribe(result => {
      this.newService = {
        id: null,
        name: null,
        duration: "00:00:00",
        endTime: "00:00:00",
        startTime: "00:00:00",
        category: null,
        reservations: null,
        price: null,
        categoryId: null,
      }
    })
  }

  private updateTimes() {
    var startTimeModel = this.NgbTimeStringAdapter.fromModel(this.newService.startTime) as NgbTimeStruct;
    var durationModel = this.NgbTimeStringAdapter.fromModel(this.newService.duration) as NgbTimeStruct;
    var endTimeModel = this.NgbTimeStringAdapter.fromModel(this.newService.endTime) as NgbTimeStruct;

    const startTimeMinutes = startTimeModel.hour * 60 + startTimeModel.minute;
    const durationMinutes = durationModel.hour * 60 + durationModel.minute;
    const endTimeMinutes = endTimeModel.hour * 60 + endTimeModel.minute;

    if (startTimeMinutes == 0 || endTimeMinutes == 0) {
      return;
    }

    if (endTimeMinutes < startTimeMinutes + durationMinutes) {
      var endTimeCorrectMinutes = startTimeMinutes + durationMinutes;
    }
    else {
      var totalServicePossible = Math.floor((endTimeMinutes - startTimeMinutes) / durationMinutes);
      var remainingTimeAfterServices = endTimeMinutes % (startTimeMinutes + durationMinutes * totalServicePossible)

      var endTimeCorrectMinutes = endTimeMinutes - remainingTimeAfterServices;
    }

    endTimeModel = {
      hour: Math.floor(endTimeCorrectMinutes / 60),
      minute: endTimeCorrectMinutes % 60,
      second: 0
    }

    this.totalServicesPossible = (endTimeCorrectMinutes - startTimeMinutes) / durationMinutes;

    this.newService.endTime = this.NgbTimeStringAdapter.toModel(endTimeModel);
  }
}
