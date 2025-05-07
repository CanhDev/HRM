// employee.module.ts
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

import { EmployeeListComponent } from './components/main/employee-list/employee-list.component';
import { EmployeeFilterComponent } from './components/main/employee-filter/employee-filter.component';
import { EmployeeCreateComponent } from './components/main/employee-create/employee-create.component';
import { EmployeeServiceService } from './services/employeeService/employeeService.service';
import { employeeReducer } from './store/reducers/employee-doc.reducer';
import { EmployeeEffects } from './store/effects/employee-doc.effects';
import { EmployeeUpdateComponent } from './components/main/employee-update/employee-update.component';
import { EmployeeRoutingModule } from './employeeDoc.routing';

@NgModule({
  declarations: [
    EmployeeListComponent,
    EmployeeUpdateComponent,
    EmployeeFilterComponent,
    EmployeeCreateComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    EmployeeRoutingModule,
    StoreModule.forFeature('employee', employeeReducer),
    EffectsModule.forFeature([EmployeeEffects])
  ],
  providers: [
    EmployeeServiceService
  ]
})
export class EmployeeModule { }