import { Directive, Input, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[hover-class]'
})
export class HoverClassDirective {

  constructor(public elementRef: ElementRef) { }
  @Input('hover-class') hoverClass: any;  
  @Input('no-hover-class') noHoverClass: any;  

  @HostListener('mouseenter') onMouseEnter() {
    if (this.noHoverClass)
      this.elementRef.nativeElement.classList.remove(this.noHoverClass);
    this.elementRef.nativeElement.classList.add(this.hoverClass);
  }

  @HostListener('mouseleave') onMouseLeave() {
    if (this.noHoverClass)
      this.elementRef.nativeElement.classList.add(this.noHoverClass);
    this.elementRef.nativeElement.classList.remove(this.hoverClass);
  }

}
