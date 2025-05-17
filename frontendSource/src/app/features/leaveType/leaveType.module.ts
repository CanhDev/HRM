import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatSelectModule } from '@angular/material/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { HttpClientModule } from '@angular/common/http';

import { LeaveTypeService } from './leaveType.service';
import { LeaveTypeListComponent } from './components/leaveTypeList/leaveTypeList.component';
import { LeaveTypeFilterComponent } from './components/leaveTypeFilter/leaveTypeFilter.component';
import { LeaveTypeModalComponent } from './components/leaveTypeModal/leaveTypeModal.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    HttpClientModule
  ],
  declarations: [LeaveTypeListComponent, LeaveTypeFilterComponent, LeaveTypeModalComponent],
  providers : [LeaveTypeService]
})
export class LeaveTypeModule { }
