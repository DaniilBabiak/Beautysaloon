import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MasterDetailedModel } from 'src/app/shared/models/master/master-detailed-model';
import { ServiceModel } from 'src/app/shared/models/service/service-model';
import { AuthService } from 'src/app/shared/services/auth.service';
import { MasterService } from 'src/app/shared/services/master.service';
import { ServiceService } from 'src/app/shared/services/service.service';

@Component({
  selector: 'app-details',
  templateUrl: './master-details.component.html',
  styleUrls: ['./master-details.component.css']
})
export class MasterDetailsComponent {
  @Input() masterId: number | undefined;
  isCloseAttempted: boolean = false;
  master: MasterDetailedModel | null = null;

  services: ServiceModel[] = [];

  isServiceHovered: boolean = false; // Переменная для отслеживания наведения на область категории
  divDisplayStyle = 'flex';
  showServiceList() {
    this.divDisplayStyle = 'flex';
  }
  hideServiceList() {
    this.divDisplayStyle = 'none';
  }
  constructor(
    public activeModal: NgbActiveModal,
    private auth: AuthService,
    private masterService: MasterService,
    private serviceService: ServiceService) { }

  ngOnInit(): void {

  }

  initModal(){
    this.loadMaster();
    this.loadServices();
  }

  loadMaster() {
    if (this.masterId == 0) {
      this.master = {
        id: 0,
        name: '',
        reservationIds: [],
        scheduleId: 0,
        serviceIds: []
      }

      this.loadServices();
    }
    else {
      this.auth.loadUser()?.then(() => {
        if (this.masterId) {
          this.masterService.getMaster(this.masterId).subscribe(result => {
            this.master = result;
            this.loadServices();
          });
        }
      });
    }
  }

  getAssignedServices(): ServiceModel[] {
    if (this.master) {
        const assignedServices = this.services.filter(service => this.master?.serviceIds.includes(service.id as number));
        return assignedServices;
    } else {
        // Если master равен null, возвращаем пустой массив
        return [];
    }
}

getUnAssignedServices(): ServiceModel[] {
  if (this.master) {
      const unAssignedServices = this.services.filter(service => !this.master?.serviceIds.includes(service.id as number));
      return unAssignedServices;
  } else {
      // Если master равен null, возвращаем пустой массив
      return [];
  }
}

  loadServices() {
    this.auth.loadUser()?.then(() => {
      this.serviceService.getServices().subscribe(result => {
        this.services = result;
      })
    })
  }

  saveDungeonMaster() {

    this.auth.loadUser()?.then(() => {
      console.log(this.master);
      if (this.master && this.master.id == 0) {
        this.masterService.createMaster(this.master).subscribe(result => {
          this.closeModal()
        });
      }
      else if (this.master && this.master.id != 0) {
        this.masterService.updateMaster(this.master).subscribe(result => {
          this.closeModal();
        })
      }
    });
  }

  closeModal() {
    this.activeModal.close();
  }

  deleteMaster() {
    this.auth.loadUser()?.then(() => {
      if (this.master && this.master.id) {
        this.masterService.deleteMaster(this.master.id).subscribe(result => {
          this.closeModal();
        })
      }
    });
  }


  onDragStart(event: any, service: any) {
    event.dataTransfer.setData('text', JSON.stringify(service)); // Передача данных сервиса для перемещения
  }

  // Обработчик события наведения на область категории
  onDragOver(event: any) {
    event.preventDefault();
    this.isServiceHovered = true;
  }

  // Обработчик события покидания области категории
  onDragLeave() {
    this.isServiceHovered = false;
  }

  // Обработчик события сбрасывания сервиса в область категории
  onDropAddService(event: any) {
    event.preventDefault();
    this.isServiceHovered = false;

    const serviceData = JSON.parse(event.dataTransfer.getData('text')); // Получение данных сервиса

    // Перемещение сервиса в мастера
    if (this.master) {
      var alreadyHasThisService = this.master.serviceIds.findIndex(serviceId => serviceId === serviceData.id) != -1;
      if (!alreadyHasThisService) {
        this.master.serviceIds.push(serviceData.id);
      }
    }
  }

  onDropRemoveService(event: any) {
    event.preventDefault();

    const serviceData = JSON.parse(event.dataTransfer.getData('text')) as ServiceModel; // Получение данных сервиса

    var index = this.master?.serviceIds.findIndex(serviceId => serviceId === serviceData.id) as number;
    if (index != -1){
      this.master?.serviceIds.splice(index, 1);
    }
  }
}
