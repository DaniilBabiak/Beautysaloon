import {AfterViewInit, Directive, ElementRef, Input} from '@angular/core';
import { gsap } from 'gsap';
import ScrollTrigger from "gsap/ScrollTrigger";

@Directive({
  selector: '[scroll-animator]'
})
export class ScrollAnimatorDirective implements AfterViewInit{
  @Input() options: any = {};
  constructor(private el: ElementRef) {
    gsap.registerPlugin(ScrollTrigger);
  }

  ngAfterViewInit(): void {
    gsap.from(this.el.nativeElement, {
      x: this.options.x,
      opacity: this.options.opacity,
      scrollTrigger: {
        trigger: this.el.nativeElement,
        start: "top 100%",
        end: "bottom 50%",
        scrub: true,
        
      }
    });
  }

}
