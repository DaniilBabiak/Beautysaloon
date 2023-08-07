import { Component } from '@angular/core';

import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent{

  aboutTitlePhoto: SafeResourceUrl;
  aboutTitlePhoto1: SafeResourceUrl
  aboutTitlePhoto2: SafeResourceUrl
  aboutTitlePhoto3: SafeResourceUrl
  aboutTitlePhoto4: SafeResourceUrl
  aboutTitlePhoto5: SafeResourceUrl
  aboutTitlePhoto6: SafeResourceUrl

  constructor(private sanitizer: DomSanitizer) {
    const imagePath = 'assets/images/front-woman-about-title2.png';
    const imagePathAboutTitle1 = 'assets/images/about-page1.png';
    const imagePathAboutTitle2 = 'assets/images/about-page2.png';
    const imagePathAboutTitle3 = 'assets/images/about-page3.png';
    const imagePathAboutTitle4 = 'assets/images/about-page4.png';
    const imagePathAboutTitle5 = 'assets/images/about-page5.png';
    const imagePathAboutTitle6 = 'assets/images/about-page6.png';

    this.aboutTitlePhoto = this.sanitizer.bypassSecurityTrustResourceUrl(imagePath);
    this.aboutTitlePhoto1 = this.sanitizer.bypassSecurityTrustResourceUrl(imagePathAboutTitle1);
    this.aboutTitlePhoto2 = this.sanitizer.bypassSecurityTrustResourceUrl(imagePathAboutTitle2);
    this.aboutTitlePhoto3 = this.sanitizer.bypassSecurityTrustResourceUrl(imagePathAboutTitle3);
    this.aboutTitlePhoto4 = this.sanitizer.bypassSecurityTrustResourceUrl(imagePathAboutTitle4);
    this.aboutTitlePhoto5 = this.sanitizer.bypassSecurityTrustResourceUrl(imagePathAboutTitle5);
    this.aboutTitlePhoto6 = this.sanitizer.bypassSecurityTrustResourceUrl(imagePathAboutTitle6);


  }
 
  

}
