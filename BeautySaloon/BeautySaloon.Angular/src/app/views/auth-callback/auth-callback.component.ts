import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import{AuthService} from "../../shared/services/auth.service";

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.css']
})
export class AuthCallbackComponent implements OnInit{
  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) { }
  error: boolean = false;
  async ngOnInit() {

    // check for error
    if (this.route.snapshot.fragment) {
      if (this.route.snapshot.fragment.indexOf('error') >= 0) {
        this.error = true;
        return;
      }
    }

    await this.authService.completeAuthentication();
    this.router.navigate(['/user']);
  }
}
