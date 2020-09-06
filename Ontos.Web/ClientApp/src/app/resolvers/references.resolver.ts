import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { GraphService } from "../services/graph.service";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { NotificationService } from "../services/notification.service";
import { Reference } from "../models/graph";


@Injectable()
export class ReferencesResolver implements Resolve<Reference[]> {

  constructor(
    private notificationService: NotificationService,
    private graphService: GraphService) {
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Reference[]> {
    const pageId = route.paramMap.get("id");
    return this.graphService.getReferences(pageId).pipe(
      catchError(e => {
        this.notificationService.notifyError(e);
        return throwError(e);  // todo: error handling
      })
    );
  }
}
