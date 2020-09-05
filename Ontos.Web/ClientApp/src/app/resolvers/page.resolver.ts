import { Injectable } from "@angular/core";
import { Resolve, ActivatedRoute, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { GraphService } from "../services/graph.service";
import { Page } from "../models/graph";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { NotificationService } from "../services/notification.service";


@Injectable()
export class PageResolver implements Resolve<Page> {

  constructor(
    private notificationService: NotificationService,
    private graphService: GraphService) {
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Page> {
    const pageId = route.paramMap.get("id");
    return this.graphService.getPage(pageId).pipe(
      catchError(e => {
        this.notificationService.notifyError(e);
        return throwError(e);  // todo: error handling
      })
    );
  }
}
