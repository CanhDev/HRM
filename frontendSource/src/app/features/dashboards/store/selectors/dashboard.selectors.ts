import { createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromDashboard from '../reducers/dashboard.reducer';

export const selectDashboardState = createFeatureSelector<fromDashboard.State>(
  fromDashboard.dashboardFeatureKey
);
