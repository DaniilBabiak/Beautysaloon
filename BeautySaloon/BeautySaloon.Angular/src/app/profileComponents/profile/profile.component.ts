import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ChangePhoneNumberFormComponent } from '../change-phone-number-form/change-phone-number-form.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  userName: string = '';
  phoneNumber: string = '';
  showChangePhoneNumberForm: boolean = false;
  constructor(private authService: AuthService) {

  }


  ngOnInit(): void {
    this.userName = this.authService.name;
    this.phoneNumber = this.authService.phoneNumber;
  }
}
