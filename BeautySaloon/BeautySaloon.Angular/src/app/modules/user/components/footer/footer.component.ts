import { Component } from '@angular/core';
import{ faCoffee } from '@fortawesome/free-solid-svg-icons'
import { faInstagram } from '@fortawesome/free-brands-svg-icons';
import { faTwitter} from '@fortawesome/free-brands-svg-icons'
import { faLinkedin } from '@fortawesome/free-brands-svg-icons'
import { faTelegram } from '@fortawesome/free-brands-svg-icons'
import { faFacebook } from '@fortawesome/free-brands-svg-icons'

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent {
 faCoffee = faCoffee;
 faInstagram = faInstagram;
 faTwitter = faTwitter; 
 faLinkedIn = faLinkedin;
 faTelegram = faTelegram;
 faFacebook = faFacebook;
}
