import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit{
  userName: string = '';
  phoneNumber: string = '';
  showChangePhoneNumberForm: boolean = false;
  constructor() {
  }


  ngOnInit(): void {
  }
}
