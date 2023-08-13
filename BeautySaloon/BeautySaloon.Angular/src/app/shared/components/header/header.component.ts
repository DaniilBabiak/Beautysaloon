import { Component, OnInit } from '@angular/core';
import { AuthService } from "../../services/auth.service";
import { Observable } from 'rxjs';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})

export class HeaderComponent implements OnInit {
  isAuthenticated: boolean = false;
  userName: string | undefined;
  isAdmin: boolean = false;
  constructor(private authService: AuthService) {
  }

  ngOnInit(): void {
    this.authService.loadUser()?.then(() => {
      if (this.authService.isAuthenticated) {
        this.isAuthenticated = true;
        this.userName = this.authService.name;
        this.isAdmin = this.authService.isAdmin;
      } else {
        this.userName = '';
        this.isAdmin = false;
      }
    });

  }


  login() {
    this.authService.login()?.then(()=>{
      if (this.authService.isAuthenticated) {
        this.isAuthenticated = true;
        this.userName = this.authService.name;
        this.isAdmin = this.authService.isAdmin;
      } else {
        this.userName = '';
        this.isAdmin = false;
      }
    });
  }

  async signout() {
    await this.authService.signout();
  }
}
