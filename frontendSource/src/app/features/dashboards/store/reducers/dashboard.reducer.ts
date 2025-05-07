import { createFeature, createReducer, on } from '@ngrx/store';
import * as DashboardActions from '../actions/dashboard.actions';

export const dashboardFeatureKey = 'dashboard';

export interface State {

}

export const initialState: State = {

};

export const reducer = createReducer(
  initialState,
  on(DashboardActions.loadDashboards, state => state),
  on(DashboardActions.loadDashboardsSuccess, (state, action) => state),
  on(DashboardActions.loadDashboardsFailure, (state, action) => state),
);

export const dashboardFeature = createFeature({
  name: dashboardFeatureKey,
  reducer,
});

