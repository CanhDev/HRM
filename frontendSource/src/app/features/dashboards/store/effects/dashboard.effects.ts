import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, concatMap } from 'rxjs/operators';
import { Observable, EMPTY, of } from 'rxjs';
import * as DashboardActions from '../actions/dashboard.actions';


@Injectable()
export class DashboardEffects {

  loadDashboards$ = createEffect(() => {
    return this.actions$.pipe( 

      ofType(DashboardActions.loadDashboards),
      concatMap(() =>
        /** An EMPTY observable only emits completion. Replace with your own observable API request */
        EMPTY.pipe(
          map(data => DashboardActions.loadDashboardsSuccess({ data })),
          catchError(error => of(DashboardActions.loadDashboardsFailure({ error }))))
      )
    );
  });


  constructor(private actions$: Actions) {}
}
