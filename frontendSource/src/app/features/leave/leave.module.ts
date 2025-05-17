import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { HttpClientModule } from '@angular/common/http';


// PrimeNG modules
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { CalendarModule } from 'primeng/calendar';
import { DialogModule } from 'primeng/dialog';
import { CardModule } from 'primeng/card';
import { InputNumberModule } from 'primeng/inputnumber';
import { TabViewModule } from 'primeng/tabview';
import { MessageModule } from 'primeng/message';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

import { LeaveBalenceService } from './leaveBalance/leaveBalence.service';
import { LeaveBalanceFilterComponent } from './leaveBalance/components/leaveBalanceFilter/leaveBalanceFilter.component';
import { LeaveBalanceListComponent } from './leaveBalance/components/leaveBalanceList/leaveBalanceList.component';
import { LeaveBalanceModalComponent } from './leaveBalance/components/leaveBalanceModal/leaveBalanceModal.component';

import { LeaveRequestService } from './leaveRequest/leaveRequest.service';
import { LeaveRequestFilterComponent } from './leaveRequest/components/leaveRequestFilter/leaveRequestFilter.component';
import { LeaveRequestFormComponent } from './leaveRequest/components/leaveRequestForm/leaveRequestForm.component';
import { LeaveRequestListComponent } from './leaveRequest/components/leaveRequestList/leaveRequestList.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    HttpClientModule,
    //// PrimeNG modules
    InputTextModule,
    DropdownModule,
    ButtonModule,
    TableModule,
    CalendarModule,
    DialogModule,
    CardModule,
    InputNumberModule,
    TabViewModule,
    MessageModule,
    ToastModule
  ],
  declarations: [LeaveBalanceFilterComponent, LeaveBalanceListComponent, LeaveBalanceModalComponent, LeaveRequestFilterComponent, LeaveRequestFormComponent, LeaveRequestListComponent],
  providers: [LeaveBalenceService, MessageService, LeaveRequestService]
})
export class LeaveModule { }
