import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/shared/services/auth.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  profilePhoto:SafeResourceUrl;
  iphone14:SafeResourceUrl
  userName: string = '';
  phoneNumber: string = '';
  showChangePhoneNumberForm: boolean = false;
  constructor(private authService: AuthService,private sanitizer: DomSanitizer) {
    const imagePath = 'assets/images/macbook.png';
    const imagePath2 = 'assets/images/iphone.png';
    this.profilePhoto = this.sanitizer.bypassSecurityTrustResourceUrl(imagePath);
    this.iphone14 = this.sanitizer.bypassSecurityTrustResourceUrl(imagePath2);
  }


  ngOnInit(): void {
    this.authService.loadUser()?.then(() => {
      this.phoneNumber = this.authService.phoneNumber;
      this.userName = this.authService.name;
    });
  }
}
