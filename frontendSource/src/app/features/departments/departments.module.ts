import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { HttpClientModule } from '@angular/common/http';

import { DepartmentService } from './department.service';
import { DepartmentModalComponent } from './components/department-modal/department-modal.component';
import { ListComponent } from './components/list/list.component';
import { FilterComponent } from './components/filter/filter.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    HttpClientModule
  ],
  declarations: [ListComponent, FilterComponent, DepartmentModalComponent],
  providers:[
    DepartmentService
  ]
})
export class DepartmentsModule { }
