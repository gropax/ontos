import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Router, RouterEvent, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';


export enum NotificationType { Info, Warning, Error };

export class Notification {
  constructor(
    public type: NotificationType,
    public header: string,
    public body: string) {
  }
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  public notifications$ = new BehaviorSubject<Notification[]>([]);

  constructor(private router: Router) {
    // Clear all notifications on page change
    this.router.events.pipe(
      filter(e => e instanceof NavigationEnd)
    ).subscribe(e => {
      this.clear();
    })
  }

  public notifyError(error: Error) {
    var notif = new Notification(NotificationType.Error, error.name, error.message);
    this.show(notif);
  }

  public notifyInfo(header: string, body: string) {
    var notif = new Notification(NotificationType.Info, header, body);
    this.show(notif);
  }

  public clear() {
    this.notifications$.next([]);
  }

  public remove(notification: Notification) {
    var nx = this.notifications$.value;
    nx = nx.filter(n => n != notification);
    this.notifications$.next(nx);
  }


  private show(notification: Notification) {
    var nx = this.notifications$.value;
    nx.push(notification);
    this.notifications$.next(nx);
  }
}
