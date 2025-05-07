import { createAction, props } from '@ngrx/store';

export const loadDashboards = createAction(
  '[Dashboard] Load Dashboards'
);

export const loadDashboardsSuccess = createAction(
  '[Dashboard] Load Dashboards Success',
  props<{ data: any }>()
);

export const loadDashboardsFailure = createAction(
  '[Dashboard] Load Dashboards Failure',
  props<{ error: any }>()
);
