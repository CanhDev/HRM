import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeListComponent } from './components/main/employee-list/employee-list.component';
import { EmployeeCreateComponent } from './components/main/employee-create/employee-create.component';
import { EmployeeUpdateComponent } from './components/main/employee-update/employee-update.component';
import { NotfoundComponent } from '../../shared/components/notfound/notfound.component';

const routes: Routes = [
  {
    path: 'employees',
    component: EmployeeListComponent
  },
  {
    path: 'employees/create',
    component: EmployeeCreateComponent
  },
  {
    path: 'employees/:id',
    component: EmployeeUpdateComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeRoutingModule {}