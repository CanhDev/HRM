import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
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

import { ContractsService } from './contracts.service';
import { ContractListComponent } from './components/contract-list/contract-list.component';
import { ContractFilterComponent } from './components/contract-filter/contract-filter.component';
import { ContractFormComponent } from './components/contractForm/contractForm.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatAutocompleteModule,
    HttpClientModule,
    // PrimeNG modules
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
  declarations: [ContractListComponent, ContractFilterComponent, ContractFormComponent],
  providers:[ContractsService, MessageService]
})
export class ContractsModule { }
