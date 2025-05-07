import { Routes, RouterModule } from '@angular/router';
import { Admin_dashboardComponent } from './components/admin_dashboard/admin_dashboard.component';
import { Employee_dashboardComponent } from './components/employee_dashboard/employee_dashboard.component';

const routes: Routes = [
  {path: '', component: Admin_dashboardComponent  },
  {path: 'employee', component: Employee_dashboardComponent  },
];

export const DashboardsRoutes = RouterModule.forChild(routes);
