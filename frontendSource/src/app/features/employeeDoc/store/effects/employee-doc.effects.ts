// effects/dashboard.effects.ts
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { EmployeeServiceService } from '../../services/employeeService/employeeService.service';
import * as EmployeeDocActions from '../actions/employee-doc.actions';
import { catchError, exhaustMap, map, mergeMap, of, switchMap, tap } from 'rxjs';
import { Router } from '@angular/router';
import { ApiResponseBasic, PagedResult, SearchRequest, DateRange } from 'src/app/core/models/base';
import { Employee } from '../../models/entity';

@Injectable()
export class EmployeeEffects {
  constructor(
    private actions$: Actions,
    private employeeService: EmployeeServiceService,
    private router: Router
  ) {}

  loadEmployees$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeDocActions.loadEmployees),
      switchMap(({ searchRequest }) =>
        this.employeeService.getEmployees(searchRequest).pipe(
          tap(response => {
            console.log('API response:', response);
            console.log('Dispatching loadEmployeesSuccess with data:', response.data ?? []);
          }),
          map(apiResponse => {
            const data = apiResponse.data as any;
            const employees = data?.items ?? [];
            const totalCount = data?.totalCount ?? 0;
            const page = data?.page ?? 1;
            const pageSize = data?.pageSize ?? 10;
            const totalPages = Math.ceil(totalCount / pageSize);

            const listResponse: PagedResult<Employee> = {
              items: employees,
              totalCount: totalCount,
              page: page,
              totalPages: totalPages,
              pageSize: pageSize
            };

            return EmployeeDocActions.loadEmployeesSuccess({ employees, listResponse });
          }),
          catchError(error => {
            console.error('API error:', error);
            return of(EmployeeDocActions.loadEmployeesFailure({ error }));
          })
        )
      )
    )
  );

  loadEmployeesOnSearchRequestChange$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeDocActions.setSearchRequest),
      switchMap(({ searchRequest }) =>
        [EmployeeDocActions.loadEmployees({ searchRequest })]
      )
    )
  );

  // Load Employee by ID effect
  loadEmployee$ = createEffect(() => 
    this.actions$.pipe(
      ofType(EmployeeDocActions.loadEmployee),
      mergeMap(action => 
        this.employeeService.getById(action.id).pipe(
          map(employee => EmployeeDocActions.loadEmployeeSuccess({ employee })),
          catchError(error => of(EmployeeDocActions.loadEmployeeFailure({ error })))
        )
      )
    )
  );

  // Create Employee effect
  createEmployee$ = createEffect(() => 
    this.actions$.pipe(
      ofType(EmployeeDocActions.createEmployee),
      exhaustMap(action => 
        this.employeeService.create(action.employee).pipe(
          map(response => {
            // Navigate to employee list after successful creation
            this.router.navigate(['/employees']);
            return EmployeeDocActions.createEmployeeSuccess({ response });
          }),
          catchError(error => of(EmployeeDocActions.createEmployeeFailure({ error })))
        )
      )
    )
  );

  // Update Employee effect
  updateEmployee$ = createEffect(() => 
    this.actions$.pipe(
      ofType(EmployeeDocActions.updateEmployee),
      exhaustMap(action => 
        this.employeeService.update(action.id, action.employee).pipe(
          map(response => {
            // Navigate to employee list after successful update
            this.router.navigate(['/employees']);
            return EmployeeDocActions.updateEmployeeSuccess({ response });
          }),
          catchError(error => of(EmployeeDocActions.updateEmployeeFailure({ error })))
        )
      )
    )
  );

  deleteEmployee$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeDocActions.deleteEmployee),
      switchMap(({ id }) =>
        this.employeeService.deleteEmployee(id).pipe(
          map(() => EmployeeDocActions.deleteEmployeeSuccess({ id })),
          catchError(error => of(EmployeeDocActions.deleteEmployeeFailure({ error })))
        )
      )
    )
  );

  deleteEmployeeSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeDocActions.deleteEmployeeSuccess),
      tap(() => {
        this.router.navigate(['/employees']);
      })
    ),
    { dispatch: false }
  );
}
