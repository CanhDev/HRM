import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UnauthorizedComponent } from './shared/components/unauthorized/unauthorized.component';
import { NotfoundComponent } from './shared/components/notfound/notfound.component';
import { EmployeeDocumentComponent } from './features/document/components/document/document.component';
import { ListComponent } from './features/departments/components/list/list.component';
import { PositionsListComponent } from './features/positions/components/positions-list/positions-list.component';
import { ContractTypeListComponent } from './features/contractType/components/contractType-list/contractType-list.component';
import { ContractListComponent } from './features/contracts/components/contract-list/contract-list.component';
import { ContractFormComponent } from './features/contracts/components/contractForm/contractForm.component';
import { LeaveTypeListComponent } from './features/leaveType/components/leaveTypeList/leaveTypeList.component';
import { LeaveBalanceListComponent } from './features/leave/leaveBalance/components/leaveBalanceList/leaveBalanceList.component';
import { LeaveRequestListComponent } from './features/leave/leaveRequest/components/leaveRequestList/leaveRequestList.component';
import { LeaveRequestFormComponent } from './features/leave/leaveRequest/components/leaveRequestForm/leaveRequestForm.component';

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
  {
    path: 'documents', component: EmployeeDocumentComponent
  },
  {
    path: 'departments', component: ListComponent
  },
  {
    path: 'positions', component: PositionsListComponent
  },
  {
    path: 'contract-types', component: ContractTypeListComponent
  },
  {
    path: 'employee-contracts', component: ContractListComponent
  },
  {
    path: 'employee-contracts/create', component: ContractFormComponent
  },
  {
    path: 'employee-contracts/:id', component: ContractFormComponent
  },
  {
    path: 'leave-types', component: LeaveTypeListComponent
  },
  {
    path: 'leave-days', component: LeaveBalanceListComponent
  },
  {
    path: 'leave-requests', component: LeaveRequestListComponent
  },
  {
    path: 'leave-requests/create', component: LeaveRequestFormComponent
  },
  {
    path: 'leave-requests/:id', component: LeaveRequestFormComponent
  },
  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: '**', component: NotfoundComponent } 
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
