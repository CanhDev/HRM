import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UnauthorizedComponent } from './shared/components/unauthorized/unauthorized.component';
import { NotfoundComponent } from './shared/components/notfound/notfound.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full', 
    loadChildren: () => import('./features/dashboards/dashboards.module').then(m => m.DashboardsModule)
  },
  {
    path: 'employees',
    loadChildren: () =>
      import('./features/employeeDoc/employeeDoc.module').then(m => m.EmployeeModule)
  },
  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: '**', component: NotfoundComponent } 
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
