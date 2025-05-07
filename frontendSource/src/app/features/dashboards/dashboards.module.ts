import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import * as fromDashboard from './store/reducers/dashboard.reducer';
import { EffectsModule } from '@ngrx/effects';
import { DashboardEffects } from './store/effects/dashboard.effects';
import { NgChartsModule } from 'ng2-charts';
import { DashboardsRoutes } from './dashboards.routing';
import { SharedModule } from '../../shared/shared.module';
import { Admin_dashboardComponent } from './components/admin_dashboard/admin_dashboard.component';
import { Employee_dashboardComponent } from './components/employee_dashboard/employee_dashboard.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    NgChartsModule,
    DashboardsRoutes,
    StoreModule.forFeature(fromDashboard.dashboardFeatureKey, fromDashboard.reducer),
    EffectsModule.forFeature([DashboardEffects])
  ],
  declarations: [Admin_dashboardComponent, Employee_dashboardComponent]
})
export class DashboardsModule { }
