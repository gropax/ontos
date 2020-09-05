import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { fadeAnimation } from './animations';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  animations: [fadeAnimation]
})
export class AppComponent {
  title = 'app';

  prepareRoute(outlet: RouterOutlet) {
    return outlet && outlet.activatedRouteData && outlet.activatedRouteData.animation;
  }
}
