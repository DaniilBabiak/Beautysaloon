import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from 'src/app/shared/models/constants';
import { MasterModel } from 'src/app/shared/models/master/master-model';
import { ScheduleModel } from 'src/app/shared/models/schedule/schedule-model';
import { WorkingDayModel } from 'src/app/shared/models/schedule/working-day-model';
import { AuthService } from 'src/app/shared/services/auth.service';
import { MasterService } from 'src/app/shared/services/master.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css']
})
export class ScheduleComponent implements OnInit {
  scheduleId: number = 0;
  schedule: ScheduleModel;
  masterId: number = 0;
  availableWorkingDays: string[] | null = null;
  showAddWorkingDayForm = false;
  newWorkingDay: WorkingDayModel = {
    day: '',
    endTime: '00:00:00',
    startTime: '00:00:00'
  }

  constructor(
    public activeModal: NgbActiveModal,
    private auth: AuthService,
    private masterService: MasterService) {
    this.schedule = {
      id: 0,
      masterId: 0,
      workingDays: []
    }
  }

  ngOnInit(): void {

  }

  // initModal() {
  //   if (this.scheduleId != 0){
  //     this.masterService.getSchedule()
  //   }
  // }

  // saveSchedule(): void {
  //   if (this.schedule) {
  //     if (this.master) {
  //       if (this.master.scheduleId == 0 || this.master.scheduleId == null) {
  //         this.masterService.createSchedule(this.schedule).subscribe(() => {
  //           this.activeModal.close();
  //         });
  //       }
  //     }
  //   }
  // }

  // deleteWorkingDay(workingDay: WorkingDay) {
  //   if (this.schedule && this.schedule.workingDays) {
  //     this.schedule.workingDays = this.schedule.workingDays.filter(day => day.workingDayId != workingDay.workingDayId);
  //   }
  // }

  addWorkingDay() {
    this.schedule.workingDays.push(this.newWorkingDay);
    this.showAddWorkingDayForm = false;
    this.newWorkingDay = {
      day: '',
      endTime: '00:00:00',
      startTime: '00:00:00'
    }
  }

  loadAvailableDays() {
    const existingDays = this.schedule?.workingDays?.map(workingDay => workingDay.day) || [];
    this.availableWorkingDays = Constants.DAYS_OF_WEEK.filter(day => !existingDays.includes(day));
    console.log(this.availableWorkingDays);
  }

  // loadSchedule() {
  //   this.auth.loadUser()?.then(() => {
  //     this.masterService.getMaster(this.masterId).subscribe(result => {
  //       this.master = result;
  //       if (result.scheduleId != null) {
  //         // this.schedule = result.schedule;
  //       }
  //       else {
  //         this.schedule = {
  //           masterId: this.masterId,
  //           workingDays: [],
  //         }
  //       }

  //       this.newWorkingDay = {
  //         day: '',
  //         endTime: '00:00:00',
  //         startTime: '00:00:00',
  //       }

  //     });
  //   });
  // }

  selectDay(day: string) {
    this.newWorkingDay.day = day;
  }

  closeModal() {
    this.activeModal.close();
  }
}
