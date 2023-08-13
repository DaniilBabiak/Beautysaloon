import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/shared/services/auth.service';


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
    this.authService.loadUser()?.then(() => {
      this.phoneNumber = this.authService.phoneNumber;
      this.userName = this.authService.name;
    });
  }
}
