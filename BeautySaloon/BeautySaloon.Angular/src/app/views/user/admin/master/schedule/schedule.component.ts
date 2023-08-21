import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Schedule } from '../../../../../shared/models/schedule';
import { WorkingDay } from 'src/app/shared/models/working-day';
import { Constants } from 'src/app/shared/models/constants';
import { AuthService } from 'src/app/shared/services/auth.service';
import { MasterService } from 'src/app/shared/services/master.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css']
})
export class ScheduleComponent implements OnInit {
  schedule: Schedule | null = null;
  masterId: number = 0;
  availableWorkingDays: string[] | null = null;
  showAddWorkingDayForm = false;
  newWorkingDay: WorkingDay = {
    day: '',
    endTime: '00:00:00',
    startTime: '00:00:00',
    scheduleId: 0,
    workingDayId: 0
  }

  constructor(
    public activeModal: NgbActiveModal,
    private auth: AuthService,
    private masterService: MasterService) {
  }

  ngOnInit(): void {

  }

  saveSchedule(): void {
    console.log(this.schedule);
    if (this.schedule) {
      if (this.schedule.id == 0) {
        this.masterService.createSchedule(this.masterId, this.schedule).subscribe(() => {
          this.activeModal.close();
        });
      }
      else {
        this.masterService.updateSchedule(this.schedule).subscribe(() => {
          this.activeModal.close();
        });
      }
    }

  }

  addWorkingDay() {
    this.schedule?.workingDays?.push(this.newWorkingDay);
    this.showAddWorkingDayForm = false;
    this.newWorkingDay = {
      day: '',
      endTime: '00:00:00',
      startTime: '00:00:00',
      scheduleId: 0,
      workingDayId: 0
    }
  }

  loadAvailableDays() {
    const existingDays = this.schedule?.workingDays?.map(workingDay => workingDay.day) || [];
    this.availableWorkingDays = Constants.DAYS_OF_WEEK.filter(day => !existingDays.includes(day));
    console.log(this.availableWorkingDays);
  }

  loadSchedule() {
    this.auth.loadUser()?.then(() => {
      this.masterService.getMaster(this.masterId).subscribe(result => {
        if (result.schedule) {
          this.schedule = result.schedule;
        }
        else {
          this.schedule = {
            id: 0,
            masterId: this.masterId,
            workingDays: [],
            dayOffs: [],
            master: result
          }
        }

        this.newWorkingDay = {
          day: '',
          endTime: '00:00:00',
          startTime: '00:00:00',
          scheduleId: this.schedule.id,
          workingDayId: 0
        }

      });
    });
  }

  selectDay(day: string) {
    this.newWorkingDay.day = day;
  }

  closeModal() {
    this.activeModal.close();
  }
}
