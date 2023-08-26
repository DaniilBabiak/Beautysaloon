import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Service } from 'src/app/shared/models/service';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ServiceService } from 'src/app/shared/services/service.service';
import { AvailableReservation } from '../../../shared/models/resrvation/available-reservation';
import { ReservationService } from 'src/app/shared/services/reservation.service';
import { ServiceCategory } from 'src/app/shared/models/service-category';
import { Master } from 'src/app/shared/models/master';
import { ProfileService } from 'src/app/shared/services/profile.service';
import { CreateReservationRequest } from 'src/app/shared/models/resrvation/create-reservation-request';

@Component({
  selector: 'app-reservation',
  templateUrl: './add-reservation.component.html',
  styleUrls: ['./add-reservation.component.css']
})
export class ReservationComponent {
  category: ServiceCategory | null = null;
  isLoggedIn: boolean = false;

  services: Service[] = [];

  selectedService: Service | null = null;
  availableReservations: AvailableReservation[] = [];

  availableDays: Date[] = [];
  selectedDay: Date | null = null;
  selectedTime: Date | null = null;

  availableMasters: Master[] = [];
  selectedMaster: Master | null = null;

  customerPhoneNumber: string = '';

  constructor(
    public activeModal: NgbActiveModal,
    private auth: AuthService,
    private profileService: ProfileService,
    private serviceService: ServiceService,
    private reservationService: ReservationService) { }



  init() {
    this.loadUser();
    this.loadServices();
    this.loadPhoneNumber();
  }

  loadPhoneNumber() {
    this.profileService.getProfile().subscribe(result => {
      if (result && result.phoneNumber)
        this.customerPhoneNumber = result.phoneNumber;
    })
  }

  loadServices() {
    if (this.category && this.category.id) {
      this.serviceService.getServices(this.category.id).subscribe(result => {
        this.services = result;
      })
    }
  }

  loadUser() {
    this.auth.loadUser()?.then(() => {
      this.isLoggedIn = this.auth.isAuthenticated;
    })
  }

  loadAvailableReservations(service: Service) {
    this.selectedService = service;
    if (service.id) {
      this.reservationService.getAvailableReservations(service.id).subscribe(result => {
        this.availableReservations = result;
        this.availableReservations.forEach(element => {

          if (!this.availableMasters.some(availableMaster => availableMaster.id === element.master.id)) {
            this.availableMasters.push(element.master);
          }
        });
        console.log(this.availableDays);
        console.log(this.availableMasters);

      });
    }
  }

  showAvailableSlots(day: Date) {
    this.selectedDay = day;
    console.log(this.selectedDay);
  }

  getSlots(): Date[] {
    if (!this.selectedDay) {
      return [];
    }

    const selectedDayKey = this.selectedDay.toISOString().split('T')[0];

    const slots = this.availableReservations
      .filter(entry => entry.availableTime.toString().split('T')[0] === selectedDayKey && entry.master.id === this.selectedMaster?.id)
      .map(entry => entry.availableTime);

    return slots;
  }

  saveSelectedMaster(master: Master) {
    this.selectedMaster = master;
    this.selectedDay = null;
    this.availableDays = [];
    this.availableReservations.forEach(element => {
      if (element.master.id === this.selectedMaster?.id) {
        const day = element.availableTime.toString().split('T')[0];
        const dateOfTheDay = new Date(day);
        if (!this.availableDays.some(existingDay => existingDay.getTime() === dateOfTheDay.getTime())) {
          this.availableDays.push(dateOfTheDay);
        }
      }
    });
  }

  selectTime(time: Date) {
    this.selectedTime = time;
  }

  saveReservation() {
    if (this.selectedMaster && this.selectedMaster.id) {
      if (this.selectedTime) {
        if (this.selectedService && this.selectedService.id) {
          const reservation: CreateReservationRequest = {
            masterId: this.selectedMaster.id,
            dateTime: this.selectedTime,
            phoneNumber: this.customerPhoneNumber,
            serviceId: this.selectedService.id
          }

          this.auth.loadUser()?.then(() =>{
            this.reservationService.createReservation(reservation).subscribe(() =>{
              this.activeModal.close();
            })
          })
        }
      }
    }

  }
}
